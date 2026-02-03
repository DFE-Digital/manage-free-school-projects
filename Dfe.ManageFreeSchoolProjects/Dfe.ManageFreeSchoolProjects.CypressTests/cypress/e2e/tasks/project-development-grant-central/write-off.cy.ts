import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";
import writeOff from "cypress/pages/tasks/project-development-grant-central/edit-write-off";
import { GrantManagers } from "cypress/constants/cypressConstants";

describe("Write off Task", () => {
    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login({ role: GrantManagers });

        project = RequestBuilder.createProjectDetailsNonPresumption();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks/`);
            });
    });
    
    it("Should successfully set Write off", () => {
        // The conditional radio buttons break "aria-allowed-attr"
        // This is a gov component so we can't fix it, for now just disable the check
        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(9)
            .summaryShows("Is there any write-off?")
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();

        pdgDashboard.selectChangeWriteOff();

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("All fields are optional");
        writeOff
            .titleIs("Edit Write-off")
            .schoolNameIs(project.schoolName)
            .withIsWriteOff("Yes")
            .clickContinue();

        cy.log("Confirm empty dashboard");
        summaryPage
            .inOrder()
            .skip(9)
            .summaryShows("Is there any write-off?")
            .summaryShows("Write-off reason").IsEmpty()
            .summaryShows("Write-off amount").IsEmpty()
            .summaryShows("Write-off date").IsEmpty()
            .summaryShows("Finance business partner approval received from").IsEmpty()
            .summaryShows("Approval date")
            .isNotMarkedAsComplete();

        pdgDashboard.selectChangeWriteOff();
        
        cy.log("Check Write off date validation");
        writeOff
            .schoolNameIs(project.schoolName)
            .withWriteOffDate("a", "12", "2025")
            .clickContinue()
            .errorForWriteOffDate().showsError("Day must be a number, like 12")
            
        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Check Approval Date validation");
        writeOff
            .schoolNameIs(project.schoolName)
            .withApprovalDate("a", "12", "2025")
            .clickContinue()
            .errorForApprovalDate().showsError("Day must be a number, like 12")

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Check Write off amount validation");
        writeOff
            .schoolNameIs(project.schoolName)
            .withWriteOffAmount("299.045")
            .clickContinue()
            .errorForWriteOffAmount().showsError("Write-off amount must be two decimal places")

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });
        
        cy.log("Check Write off amount validation");
        writeOff
            .schoolNameIs(project.schoolName)
            .withWriteOffReason("NotValid{")
            .clickContinue()
            .errorForWriteOffReason().showsError("Write-off reason must not include special characters other than , ( ) '")

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Check Write off amount validation");
        writeOff
            .schoolNameIs(project.schoolName)
            .withFinanceBusinessPartnerApproval("NotValid{")
            .clickContinue()
            .errorForFinanceBusinessPartnerApproval().showsError("Finance business partner approval received from must not include special characters other than , ( ) '")

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log('Confirm all set')
        writeOff
            .schoolNameIs(project.schoolName)
            .withWriteOffDate("4", "12", "2025")
            .withApprovalDate("5", "12", "2025")
            .withWriteOffAmount("299.04")
            .withWriteOffReason("Reason")
            .withFinanceBusinessPartnerApproval("Sam Smigel")
            .clickContinue()

        summaryPage
            .inOrder()
            .skip(9)
            .summaryShows("Is there any write-off?").IsNotEmpty()
            .summaryShows("Write-off reason").IsNotEmpty()
            .summaryShows("Write-off amount").IsNotEmpty()
            .summaryShows("Write-off date").IsNotEmpty()
            .summaryShows("Finance business partner approval received from").IsNotEmpty()
            .summaryShows("Approval date")       
            .MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("PDG");

    });

});
