import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import dataGenerator from "cypress/fixtures/dataGenerator";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import kickOffMeetingEditPage from "../../../pages/tasks/pre-opening/edit-kick-off-meeting.cy";

describe("Testing kick off meeting Task", () => {

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

    it("Should successfully set kick off meeting", () => {

        cy.log("Select kick off meeting");
        taskListPage.isTaskStatusIsNotStarted("KickOffMeeting")
            .selectKickOffMeetingFromTaskList();

        cy.log("Confirm empty kick off meeting");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Kick-off meeting")
            .inOrder()
            .summaryShows("Funding arrangement details agreed between local authority and trust").IsEmpty().HasChangeLink()
            .summaryShows("Comments").IsEmpty().HasChangeLink()
            .summaryShows("Realistic year of opening").IsEmpty().HasChangeLink()
            .summaryShows("Saved documents in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();
        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open kick-off meeting");
        taskListPage.isTaskStatusIsNotStarted("KickOffMeeting")
            .selectKickOffMeetingFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("KickOffMeeting")
            .selectKickOffMeetingFromTaskList();

        cy.log("Check search page");

        summaryPage.clickChange();

        cy.executeAccessibilityTests();

        cy.log("All fields are optional");
        kickOffMeetingEditPage
            .titleIs("Edit Kick-off meeting")
            .schoolNameIs(project.schoolName)
            .clickContinue()

        cy.executeAccessibilityTests()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Kick-off meeting")
            .inOrder()
            .summaryShows("Funding arrangement details agreed between local authority and trust").IsEmpty().HasChangeLink()
            .summaryShows("Comments").IsEmpty().HasChangeLink()
            .summaryShows("Realistic year of opening").IsEmpty().HasChangeLink()
            .summaryShows("Saved documents in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .clickChange();

        cy.log("Comment")

        kickOffMeetingEditPage
            .withComments(dataGenerator.generateAlphaNumeric(101))
            .clickContinue()
            .errorForComments().showsError("Comments must be 100 characters or less")

        kickOffMeetingEditPage
            .withComments("#TaTers")
            .clickContinue()
            .errorForComments().showsError("Comments must not include special characters other than , ( ) '")
            .withComments("comment that's ok")
            .clickContinue();

        summaryPage.SummaryHasValue("Comments", "comment that's ok")
            .clickChange();

        kickOffMeetingEditPage
            .withRealisticYearOfOpeningStartDate("1234")
            .clickContinue()
            .errorForRealisticStartDate("Start year must begin with 20")
            .withRealisticYearOfOpeningStartDate("2050")
            .withRealisticYearOfOpeningEndDate("1234")
            .clickContinue()
            .errorForRealisticStartDate("End year must begin with 20")
            .withRealisticYearOfOpeningStartDate("2049")
            .withRealisticYearOfOpeningEndDate("2050")
            .clickContinue()

        summaryPage.SummaryHasValue("Realistic year of opening", "2049/50")
            .clickChange();

        kickOffMeetingEditPage
            .checkFundingArrangementAgreed()
            .clickContinue()

        summaryPage.SummaryHasValue("Funding arrangement details agreed between local authority and trust", "Yes")
            .clickChange();

        cy.log('Confirm all set')

        kickOffMeetingEditPage
            .checkSavedDocumentsInWorkplacesFolder()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Kick-off meeting")
            .inOrder()
            .summaryShows("Funding arrangement details agreed between local authority and trust").HasValue("Yes").HasChangeLink()
            .summaryShows("Comments").HasValue("comment that's ok").HasChangeLink()
            .summaryShows("Realistic year of opening").HasValue("2049/50").HasChangeLink()
            .summaryShows("Saved documents in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("KickOffMeeting");
    })
})
