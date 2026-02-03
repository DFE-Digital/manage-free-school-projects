import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";
import paymentSchedule from "cypress/pages/tasks/project-development-grant-central/edit-payment-schedule";
import editTotalGrant from "cypress/pages/tasks/project-development-grant-central/edit-total-grant";
import addPayment from "cypress/pages/tasks/project-development-grant-central/add-payment";
import editPayment from "cypress/pages/tasks/project-development-grant-central/edit-payment";
import deletePayment from "cypress/pages/tasks/project-development-grant-central/delete-payment";
import { DateTime } from "../../../../node_modules/luxon";
import { GrantManagers } from "cypress/constants/cypressConstants";

describe("Payment Schedule Task", () => {
    let project: ProjectDetailsRequest;
    let paymentDate: DateTime;

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
    it("Should successfully set payment schedule", () => {

        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.executeAccessibilityTests();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .summaryShows("Total amount").IsEmpty()
            .summaryShows("Amount sent").IsEmpty()
            .summaryShows("Due amount").IsEmpty()
            .summaryShows("Total amount").IsEmpty()
            .summaryShows("Number of scheduled payments").HasValue("0")
            .isNotMarkedAsComplete();

        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("PDG")
            .selectPDGFromTaskList();

        pdgDashboard.selectChangeTotalGrant();

        cy.executeAccessibilityTests();

        editTotalGrant
            .titleIs("Edit Total Grant")
            .schoolNameIs(project.schoolName)
            .withTotalGrantAmount("200000")
            .clickContinue();

        pdgDashboard.selectChangePaymentSchedule();

        cy.executeAccessibilityTests();

        paymentSchedule
            .titleIs("Edit Payment schedule")
            .schoolNameIs(project.schoolName)
            .checkNoPayments()
            .selectAddPayment();

        cy.executeAccessibilityTests();

        addPayment
            .titleIs("Add a payment")
            .schoolNameIs(project.schoolName)
            .clickDiscard()

            paymentSchedule
            .titleIs("Edit Payment schedule")
            .schoolNameIs(project.schoolName)
            .checkNoPayments()
            .selectAddPayment();
        
        paymentDate = DateTime.now().minus({days: 28});

        addPayment
            .titleIs("Add a payment")
            .schoolNameIs(project.schoolName)
            .withPaymentDueDate(paymentDate.day, paymentDate.month, paymentDate.year)
            .withPaymentDueAmount("5000")
            .withPaymentActualDate(paymentDate.day, paymentDate.month, paymentDate.year)
            .withPaymentActualAmount("5000")
            .clickContinue();

        paymentSchedule
            .schoolNameIs(project.schoolName)
            .checkPaymentAdded("1")
            .selectEditPayment("1");
        
        cy.executeAccessibilityTests();

        paymentDate = paymentDate.plus({days: 1})

        editPayment
            .titleIs("Edit Payment 1")
            .schoolNameIs(project.schoolName)

            .withPaymentDueDate("a", "12", "2025")
            .clickContinue()
            .errorForPaymentDueDate().showsError("Day must be a number, like 12")
            .withPaymentDueDate(paymentDate.day, paymentDate.month, paymentDate.year)

            .withPaymentDueAmount("a")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be a number")
            .withPaymentDueAmount("-1")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be between 0 and 640000")
            .withPaymentDueAmount("250.256")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be two decimal places")
            .withPaymentDueAmount("6558767868")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be between 0 and 640000")
            .withPaymentDueAmount("10000")

            .withPaymentActualDate("a", "12", "2025")
            .clickContinue()
            .errorForPaymentActualDate().showsError("Day must be a number, like 12")
            .withPaymentActualDate(paymentDate.day, paymentDate.month, paymentDate.year)

            .withPaymentActualAmount("a")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Actual amount must be a number")
            .withPaymentActualAmount("-1")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Actual amount must be between 0 and 640000")
            .withPaymentActualAmount("250.256")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Actual amount must be two decimal places")
            .withPaymentActualAmount("25000000")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Actual amount must be between 0 and 640000")
            .withPaymentActualAmount("10000")
            .clickContinue();

        paymentSchedule
            .schoolNameIs(project.schoolName)
            .checkPaymentUpdated("1")
            .selectAddPayment();

            paymentDate = paymentDate.plus({days: 26})

        addPayment
            .titleIs("Add a payment")
            .schoolNameIs(project.schoolName)

            .withPaymentDueDate("a", "12", "2025")
            .clickContinue()
            .errorForPaymentDueDate().showsError("Day must be a number, like 12")
            .withPaymentDueDate(paymentDate.day, paymentDate.month, paymentDate.year)

            .withPaymentDueAmount("a")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be a number")
            .withPaymentDueAmount("-1")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be between 0 and 640000")
            .withPaymentDueAmount("78976567899")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be between 0 and 640000")
            .withPaymentDueAmount("250.256")
            .clickContinue()
            .errorForPaymentDueAmount().showsError("Due amount must be two decimal places")
            .withPaymentDueAmount("5000")

            .withPaymentActualDate("a", "12", "2025")
            .clickContinue()
            .errorForPaymentActualDate().showsError("Day must be a number, like 12")
            .withPaymentActualDate(paymentDate.day, paymentDate.month, paymentDate.year)

            .withPaymentActualAmount("a")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Amount sent must be a number")
            .withPaymentActualAmount("-1")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Amount sent must be between 0 and 640000")
            .withPaymentActualAmount("250.256")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Amount sent must be two decimal places")
            .withPaymentActualAmount("5456465444446")
            .clickContinue()
            .errorForPaymentActualAmount().showsError("Amount sent must be between 0 and 640000")
            .withPaymentActualAmount("4000")
            .clickContinue();

        var nextPaymentDate = paymentDate.plus({days: 28})
        
        for (var i = 3; i <= 12; i++) {

            paymentDate = paymentDate.plus({days: 28})

            paymentSchedule
                .selectAddPayment();
            addPayment
                .titleIs("Add a payment")
                .schoolNameIs(project.schoolName)
                .withPaymentDueDate(paymentDate.day, paymentDate.month, paymentDate.year)
                .withPaymentDueAmount("5000")
                .clickContinue();

            paymentSchedule
                .schoolNameIs(project.schoolName)
                .checkPaymentAdded(i.toString())
        }

        paymentSchedule
            .checkAddPaymentDoesNotExist();
            
        paymentSchedule
            .selectEditPayment("1");

        editPayment
            .titleIs("Edit Payment 1")
            .schoolNameIs(project.schoolName)
            .clickDelete();

        cy.executeAccessibilityTests();

        deletePayment
            .clickNo()

        paymentSchedule
            .schoolNameIs(project.schoolName)
            .selectEditPayment("1");

        editPayment
            .titleIs("Edit Payment 1")
            .schoolNameIs(project.schoolName)
            .clickDelete();
        
        deletePayment
            .clickDelete()
        
        paymentSchedule
            .schoolNameIs(project.schoolName)
            .checkPaymentDeleted()
            .checkAddPaymentDoesExist();
            
        paymentSchedule
            .clickBack();
        
        summaryPage
            .startFromRow(4)
            .summaryShows("Number of scheduled payments").HasValue("11")
            .summaryShows("Number of sent payments").HasValue("1")
            .summaryShows("Next payment due date").HasValue(nextPaymentDate.toFormat("d MMMM yyyy"))
            .summaryShows("Next due amount").HasValue("£5,000")
            .summaryShows("Amount sent").HasValue("£4,000");

        summaryPage.MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("PDG");

    });

});