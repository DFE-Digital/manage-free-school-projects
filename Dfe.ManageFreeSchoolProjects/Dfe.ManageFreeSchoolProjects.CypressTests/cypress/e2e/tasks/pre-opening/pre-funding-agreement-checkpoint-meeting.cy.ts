import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import preFundingAgreementCheckpointMeetingEditPage from "../../../pages/tasks/pre-opening/edit-pre-funding-agreement-checkpoint-meeting-cy";

describe("Testing pre-funding agreement checkpoint meeting task", () => {

    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetailsCentralRoute();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it("Should be able to set pre-funding agreement checkpoint meeting", () => {
        Logger.log("Select pre-funding agreement checkpoint meeting");
        taskListPage.isTaskStatusIsNotStarted("PreFundingAgreementCheckpointMeeting")
            .selectPreFundingAgreementCheckpointMeetingFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pre-funding agreement checkpoint meeting")
            .inOrder()
            .summaryShows("Type of meeting held").IsEmpty().HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Saved the meeting note in Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Sent an email to the trust highlighting any significant concerns and necessary actions (if applicable)").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectPreFundingAgreementCheckpointMeetingFromTaskList();
        summaryPage.clickChange();

        Logger.log("Pre-funding agreement checkpoint meeting can save null values");

        preFundingAgreementCheckpointMeetingEditPage
            .clickContinue()

        Logger.log("Pre-funding agreement checkpoint meeting can be edited");

        summaryPage.clickChange();

        preFundingAgreementCheckpointMeetingEditPage
            .selectFormalCheckpointMeeting()
            .dateOfTheFormalCheckpointMeeting("60", "12", "2050")
            .clickContinue()
            .errorForFormalCheckpointMeetingDate("Day must be between 1 and 31")
            .dateOfTheFormalCheckpointMeeting("30", "12", "1999")
            .clickContinue()
            .errorForFormalCheckpointMeetingDate("Year must be between 2000 and 2050")
            .dateOfTheFormalCheckpointMeeting("5", "5", "2040")
            .checkCommissionedExternalExpert()
            .checkSavedMeetingNote()
            .checkSentAnEmailToTheTrust()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pre-funding agreement checkpoint meeting")
            .inOrder()
            .summaryShows("Type of meeting held").HasValue("Formal checkpoint meeting").HasChangeLink()
            .summaryShows("Date of the meeting").HasValue("5 May 2040").HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the meeting note in Workplaces folder").HasValue("Yes").HasChangeLink()
            .summaryShows("Sent an email to the trust highlighting any significant concerns and necessary actions (if applicable)").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("PreFundingAgreementCheckpointMeeting");

        taskListPage.selectPreFundingAgreementCheckpointMeetingFromTaskList();

        Logger.log("Pre-funding agreement checkpoint meeting can be edited again with No meeting held option");

        summaryPage.clickChange();

        preFundingAgreementCheckpointMeetingEditPage
            .selectNoMeetingHeld()
            .whyMeetingWasNotHeld()
            .checkCommissionedExternalExpert()
            .uncheckSavedMeetingNote()
            .checkSentAnEmailToTheTrust()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pre-funding agreement checkpoint meeting")
            .inOrder()
            .summaryShows("Type of meeting held").HasValue("No meeting held").HasChangeLink()
            .summaryShows("Why a meeting was not held").HasValue("This is the reason why meeting was not held").HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the meeting note in Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Sent an email to the trust highlighting any significant concerns and necessary actions (if applicable)").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("PreFundingAgreementCheckpointMeeting");

        taskListPage.selectPreFundingAgreementCheckpointMeetingFromTaskList();
        summaryPage.clickChange();

        Logger.log("Should be able to select Internal review meeting");

        preFundingAgreementCheckpointMeetingEditPage
            .selectInternalReviewMeeting()
            .dateOfTheInternalReviewMeeting("60", "12", "2050")
            .clickContinue()
            .errorForInternalReviewMeetingDate("Day must be between 1 and 31")
            .dateOfTheInternalReviewMeeting("30", "12", "1999")
            .clickContinue()
            .errorForInternalReviewMeetingDate("Year must be between 2000 and 2050")
            .dateOfTheInternalReviewMeeting("23", "5", "2030")
            .checkCommissionedExternalExpert()
            .checkSavedMeetingNote()
            .checkSentAnEmailToTheTrust()
            .clickContinue()
      
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Pre-funding agreement checkpoint meeting")
            .inOrder()
            .summaryShows("Type of meeting held").HasValue("Internal review / case conference").HasChangeLink()
            .summaryShows("Date of the meeting").HasValue("23 May 2030").HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the meeting note in Workplaces folder").HasValue("Yes").HasChangeLink()
            .summaryShows("Sent an email to the trust highlighting any significant concerns and necessary actions (if applicable)").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusIsCompleted("PreFundingAgreementCheckpointMeeting");
    });
});
