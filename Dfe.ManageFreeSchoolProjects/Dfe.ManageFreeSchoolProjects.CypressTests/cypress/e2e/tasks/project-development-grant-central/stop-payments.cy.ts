import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";
import stopPayments from "cypress/pages/tasks/project-development-grant-central/edit-stop-payments";
import { GrantManagers } from "cypress/constants/cypressConstants";

describe("Stop Payments Task", () => {
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

    it("Should successfully set Stop Payments", () => {
        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(6)
            .summaryShows("Are you sure you want to stop payments?").IsEmpty()
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();

        pdgDashboard.selectChangeStopPayments();

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("All fields are optional");
        stopPayments
            .titleIs("Edit Stop payments")
            .schoolNameIs(project.schoolName)
            .clickContinue();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(6)
            .summaryShows("Are you sure you want to stop payments?").IsEmpty()
            .isNotMarkedAsComplete();

        pdgDashboard.selectChangeStopPayments();
        
        cy.log("Check Stop payments validation");
        stopPayments
            .titleIs("Edit Stop payments")
            .schoolNameIs(project.schoolName)
            .paymentStoppedDateIsNotVisible()
            .withPaymentStopped("Yes")
            .paymentStoppedDateIsVisible()
            .withPaymentStoppedDate("a", "12", "2025")
            .clickContinue()
            .errorForPaymentStoppedDate().showsError("Day must be a number, like 12")
            
        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });
       
        cy.log('Confirm yes set')

        stopPayments
            .withPaymentStopped("Yes")
            .withPaymentStoppedDate("5", "12", "2025")
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(6)
            .summaryShows("Are you sure you want to stop payments?")
            .summaryShows("Date when payments should stop")
        
        cy.log('Confirm no set')

        pdgDashboard.selectChangeStopPayments();

        stopPayments
            .withPaymentStopped("No")
            .clickContinue()

        pdgDashboard.selectChangeStopPayments();

        stopPayments
            .paymentStoppedDateIsBlank()
            .clickContinue()

        summaryPage.MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("PDG");

    });

});
