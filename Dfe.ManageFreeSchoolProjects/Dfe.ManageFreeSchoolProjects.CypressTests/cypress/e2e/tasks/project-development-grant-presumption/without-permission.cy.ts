import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import root from "cypress/pages/root";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-presumption/dashboard";

describe("Payment Schedule Task", () => {
    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks/`);
            });
    });

    it("Should successfully set payment schedule", () => {

        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusHidden("PDG")
            .selectPDGFromTaskList();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .HasNoMarkAsComplete()
            .hasNoConfirmAndContinue();

        pdgDashboard
            .changePaymentScheduleNotShown()
            .changeTrustLetterNotShown() 
            .changeStopPaymentsNotShown()
            .changeRefundsNotShown() 
            .changeWriteOffNotShown()

            summaryPage.clickBack();

        taskListPage.isTaskStatusHidden("PDG")

        cy.executeAccessibilityTests();

        root.checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/presumption/edit-payment-schedule`)
            .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/presumption/edit-trust-letter`)
            .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/presumption/edit-refunds`)
            .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/presumption/edit-stop-payment`)
            .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/presumption/edit-write-off`)
    });

});
