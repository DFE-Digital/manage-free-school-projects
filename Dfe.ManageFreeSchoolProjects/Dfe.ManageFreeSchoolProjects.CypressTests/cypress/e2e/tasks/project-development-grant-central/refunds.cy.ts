import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";
import refunds from "cypress/pages/tasks/project-development-grant-central/edit-refunds";
import { GrantManagers } from "cypress/constants/cypressConstants";

describe("Refunds Task", () => {
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
    
    it("Should successfully set refunds", () => {
        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(7)
            .summaryShows("Latest refund date").IsEmpty()
            .summaryShows("Total amount").IsEmpty()
            .isNotMarkedAsComplete();

        pdgDashboard.selectChangeRefunds();

        cy.executeAccessibilityTests();

        cy.log("All fields are optional");
        refunds
            .titleIs("Edit Refunds")
            .schoolNameIs(project.schoolName)
            .clickContinue();

        cy.log("Confirm empty dashboard");
        summaryPage
            .inOrder()
            .skip(7)
            .summaryShows("Latest refund date").IsEmpty()
            .summaryShows("Total amount").IsEmpty()

        pdgDashboard.selectChangeRefunds();
        
        cy.log("Check refund date validation");
        refunds
            .withLatestRefundDate("a", "12", "2025")
            .clickContinue()
            .errorForLatestRefundDate().showsError("Day must be a number, like 12")
            
        cy.executeAccessibilityTests();
       
        cy.log("Check refund date validation");
        refunds
            .withTotalAmount("250.254")
            .clickContinue()
            .errorForTotalAmount().showsError("Total amount must be two decimal places")

        cy.log('Confirm all set')
        refunds
            .withLatestRefundDate("5", "12", "2025")
            .withTotalAmount("250.25")
            .clickContinue()
        
        summaryPage
            .inOrder()
            .skip(7)
            .summaryShows("Latest refund date")
            .summaryShows("Total amount")

        summaryPage.MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("PDG");

    });
});
