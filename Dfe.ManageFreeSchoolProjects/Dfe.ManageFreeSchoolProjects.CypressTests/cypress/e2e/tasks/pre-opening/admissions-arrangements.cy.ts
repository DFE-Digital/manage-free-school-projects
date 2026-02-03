import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import admissionsArrangementsEditPage from "../../../pages/tasks/pre-opening/edit-admissions-arrangements.cy";

describe("Testing the admissions arragements task", () => {

    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it("Should be able to set admissions arrangements", () => {
        Logger.log("Select admissions arrangements");
        taskListPage.isTaskStatusIsNotStarted("AdmissionsArrangements")
            .selectAdmissionsArrangementsFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Admissions arrangements")
            .inOrder()
            .summaryShows("Expected date that trust will confirm arrangements").IsEmpty().HasChangeLink()
            .summaryShows("Trust has confirmed they developed their admissions arrangements using the recommended template").IsEmpty().HasChangeLink()
            .summaryShows("Trust has confirmed their arrangements comply with the School Admissions Code and School Admission Appeals Code").IsEmpty().HasChangeLink()
            .summaryShows("Actual date that trust confirmed arrangements").IsEmpty().HasChangeLink()
            .summaryShows("Saved the confirmation email in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectAdmissionsArrangementsFromTaskList();
        summaryPage.clickChange();

        Logger.log("Edit Admissions Arrangements");

        cy.executeAccessibilityTests();

        cy.executeAccessibilityTests

        admissionsArrangementsEditPage
            .withExpectedDate("15","1","2049")
            .checkTrustConfirmedAdmissionsArrangementsTemplate()
            .checkTrustConfirmedAdmissionsArrangementsPolicies()
            .withConfirmedDate("21","1","2049")
            .checkSavedToWorkplaces()
            .clickContinue()
        
        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Admissions arrangements")
            .inOrder()
            .summaryShows("Expected date that trust will confirm arrangements").HasValue("15 January 2049").HasChangeLink()
            .summaryShows("Trust has confirmed they developed their admissions arrangements using the recommended template").HasValue("Yes").HasChangeLink()
            .summaryShows("Trust has confirmed their arrangements comply with the School Admissions Code and School Admission Appeals Code").HasValue("Yes").HasChangeLink()
            .summaryShows("Actual date that trust confirmed arrangements").HasValue("21 January 2049").HasChangeLink()
            .summaryShows("Saved the confirmation email in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("AdmissionsArrangements");

        taskListPage.selectAdmissionsArrangementsFromTaskList();
        summaryPage.clickChange();
        
        admissionsArrangementsEditPage
            .withExpectedDate("zx","12","2010")
            .clickContinue()
            .errorForExpectedDate().showsError("Day must be a number, like 12")
            .withExpectedDate("15","12","1999")
            .clickContinue()
            .errorForExpectedDate().showsError("Year must be between 2000 and 2050")
            .withExpectedDate("15","4","2051")
            .clickContinue()
            .errorForExpectedDate().showsError("Year must be between 2000 and 2050")
            .withExpectedDate("15","4","2050")
            .withConfirmedDate("aq","12","2010")
            .clickContinue()
            .errorForConfirmedDate().showsError("Day must be a number, like 12")
            .withConfirmedDate("21","12","1999")
            .clickContinue()
            .errorForConfirmedDate().showsError("Year must be between 2000 and 2050")
            .withConfirmedDate("21","12","2051")
            .clickContinue()
            .errorForConfirmedDate().showsError("Year must be between 2000 and 2050")
            .withConfirmedDate("21","4","2050")
            .uncheckTrustConfirmedAdmissionsArrangementsTemplate()
            .uncheckTrustConfirmedAdmissionsArrangementsPolicies()
            .uncheckSavedToWorkplaces()
            .clickContinue()
        
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Admissions arrangements")
            .inOrder()
            .summaryShows("Expected date that trust will confirm arrangements").HasValue("15 April 2050").HasChangeLink()
            .summaryShows("Trust has confirmed they developed their admissions arrangements using the recommended template").HasValue(["Empty"]).HasChangeLink()
            .summaryShows("Trust has confirmed their arrangements comply with the School Admissions Code and School Admission Appeals Code").HasValue(["Empty"]).HasChangeLink()
            .summaryShows("Actual date that trust confirmed arrangements").HasValue("21 April 2050").HasChangeLink()
            .summaryShows("Saved the confirmation email in Workplaces folder").HasValue(["Empty"]).HasChangeLink()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("AdmissionsArrangements");
    });
});