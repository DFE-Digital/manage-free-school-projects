import { CreatePDGGrantLettersRequest, CreatePDGGrantVariationLetterRequest, CreatePDGPaymentScheduleRequest, ProjectDetailsRequest } from "cypress/api/domain";
import grantLettersApi from "cypress/api/grantLettersApi";
import grantVariationLetterApi from "cypress/api/grantVariationLetterApi";
import paymentScheduleApi from "cypress/api/paymentScheduleApi";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import root from "cypress/pages/root";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import editGrantLetter from "cypress/pages/tasks/project-development-grant-central/edit-grant-letter";
import paymentSchedule from "cypress/pages/tasks/project-development-grant-central/edit-payment-schedule";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";

describe("View PDG dashboard", () => {
    describe("with no data", () => {
        let project: ProjectDetailsRequest;

        beforeEach(() => {
            cy.login();

            project = RequestBuilder.createProjectDetailsNonPresumption();

            projectApi
                .post({
                    projects: [project],
                })
                .then(() => {
                    cy.visit(`/projects/${project.projectId}/tasks/`);
                });
        });

        it("Should hide all links", () => {

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
                .changeTotalGrantScheduleNotShown()
                .changeTrustLetterNotShown() 
                .changeStopPaymentsNotShown()
                .changeRefundsNotShown() 
                .changeWriteOffNotShown()

            summaryPage.clickBack();

            taskListPage.isTaskStatusHidden("PDG")

            cy.executeAccessibilityTests();

        });

    });

    describe("with data", () => {
        let project: ProjectDetailsRequest;

        beforeEach(() => {
            cy.login();

            project = RequestBuilder.createProjectDetailsNonPresumption();
                
            const payment : CreatePDGPaymentScheduleRequest =
            {
                paymentScheduleAmount : 100,
                paymentScheduleDate: "2025-01-01T00:00:00.000Z",
                paymentActualAmount: 101,
                paymentActualDate: "2025-01-02T00:00:00.000Z"
            }

            const grantLetter : CreatePDGGrantLettersRequest =
            {
                initialGrantLetterDate: "2024-01-01T00:00:00.000Z",
                finalGrantLetterDate: "2024-01-02T00:00:00.000Z",
                initialGrantLetterSavedToWorkplaces : true,
                finalGrantLetterSavedToWorkplaces : true
            }

            const grantVariationLetter : CreatePDGGrantVariationLetterRequest =
            {
                variation: 1,
                letterDate: "2024-01-01T00:00:00.000Z",
                savedToWorkplacesFolder: true
            }



            projectApi
                .post({
                    projects: [project],
                })
                .then(() => {
                    paymentScheduleApi.post(project.projectId, payment)
                })
                .then(() => 
                {
                    grantLettersApi.put(project.projectId, grantLetter)
                })
                .then(() => {
                    grantVariationLetterApi.put(project.projectId, grantVariationLetter)
                })
                .then(() => {
                    cy.visit(`/projects/${project.projectId}/tasks/`);
                });
        });

        it("Should hide most links but allow Payment schedule and Grant letters to be viewed read-only", () => {

            cy.log("Select Project development grant (PDG)");
            taskListPage.isTaskStatusHidden("PDG")
                .selectPDGFromTaskList();

            cy.log("Confirm empty dashboard");
            summaryPage
                .schoolNameIs(project.schoolName)
                .titleIs("Project development grant (PDG)")
                .HasNoMarkAsComplete()
                .hasNoConfirmAndContinue();

            cy.executeAccessibilityTests();

            pdgDashboard
                .changeTotalGrantScheduleNotShown()
                .changeTrustLetterNotShown() 
                .changeStopPaymentsNotShown()
                .changeRefundsNotShown() 
                .changeWriteOffNotShown()



            pdgDashboard
                .selectViewDetailsPaymentSchedule()

            cy.executeAccessibilityTests();

            paymentSchedule
                .titleIs("Payment schedule")
                .schoolNameIs(project.schoolName)
                .checkAddPaymentDoesNotExist()
                .editPaymentLinkNotShown("1")
                .clickBack()

            pdgDashboard
                .selectViewDetailsGrantLetter()

            cy.executeAccessibilityTests();
                
            editGrantLetter
                .grantTitleIs("Grant letters")
                .schoolNameIs(project.schoolName)
                .addGrantLetterNotShown()
                .changeGrantLetterNotShown()
                .changeVariationLetterNotShown("1")
                .clickBack()
                
            summaryPage.clickBack();

            taskListPage.isTaskStatusHidden("PDG")

            cy.executeAccessibilityTests();

            root.checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/add-grant-letter`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/add-payment`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/add-variation-letter/`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/delete-payment/1`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-grant-letter`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-total-grant`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-payment-schedule/1`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-variation-letter/1`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-refunds`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-stop-payment`)
                .checkAccessToPage(`/projects/${project.projectId}/tasks/pdg/central/edit-write-off`)
       

        });

    });
});