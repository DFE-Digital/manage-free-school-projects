import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pupilNumbersChecksEditPage from "../../../pages/tasks/Getting-ready-to-open/edit-pupil-numbers-checks-cy";

describe("Testing the pupil numbers checks task", () => {

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

    it("Should be able to set pupil numbers checks", () => {
        Logger.log("Select pupil numbers checks");
        taskListPage.isTaskStatusIsNotStarted("PupilNumbersChecks")
            .selectPupilNumbersChecksTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pupil numbers checks")
            .inOrder()
            .summaryShows("School has received enough applications to be viable").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the funding agreement").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the GIAS registration form").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectPupilNumbersChecksTaskList();
        summaryPage.clickChange();

        Logger.log("pupil numbers checks can save null values");

        pupilNumbersChecksEditPage
        .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pupil numbers checks")
            .inOrder()
            .summaryShows("School has received enough applications to be viable").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the funding agreement").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the GIAS registration form").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();

        Logger.log("pupil numbers checks can be edited");

        summaryPage.clickChange();
        
        pupilNumbersChecksEditPage
            .checkReceivedEnoughApplications()
            .checkDataMatchesFundingAgreement()
            .checkDataMatchesGiasRegistration()
            .clickContinue()
        
        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pupil numbers checks")
            .inOrder()
            .summaryShows("School has received enough applications to be viable").HasValue("Yes").HasChangeLink()
            .summaryShows("Capacity data matches what's on the funding agreement").HasValue("Yes").HasChangeLink()
            .summaryShows("Capacity data matches what's on the GIAS registration form").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("PupilNumbersChecks");

        taskListPage.selectPupilNumbersChecksTaskList();
        summaryPage.clickChange();

        Logger.log("Should be able to clear all values");

        pupilNumbersChecksEditPage
            .uncheckReceivedEnoughApplications()
            .uncheckDataMatchesFundingAgreement()
            .uncheckDataMatchesGiasRegistration()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pupil numbers checks")
            .inOrder()
            .summaryShows("School has received enough applications to be viable").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the funding agreement").IsEmpty().HasChangeLink()
            .summaryShows("Capacity data matches what's on the GIAS registration form").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()
        
        taskListPage.isTaskStatusIsCompleted("PupilNumbersChecks");
    });
});