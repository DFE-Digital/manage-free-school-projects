import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import readinessToOpenMeetingEditPage from "../../../pages/tasks/Getting-ready-to-open/edit-readiness-to-open-meeting-cy";

describe("Testing readiness to open meeting task", () => {

    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();
        project = RequestBuilder.createProjectDetailsNonPresumption();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it("Should be able to set readiness to open meeting", () => {
        Logger.log("Select moving to open");
        taskListPage.isTaskStatusIsNotStarted("ReadinessToOpenMeeting")
            .selectReadinessToOpenMeetingFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Readiness to open meeting (ROM)")
            .inOrder()
            .summaryShows("A ROM is expected to happen").IsEmpty().HasChangeLink()
            .summaryShows("Type of meeting held").IsEmpty().HasChangeLink()
            .summaryShows("Principal designate (PD) has provided the checklist").IsEmpty().HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Saved the internal ROM report in Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Saved the external ROM report in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .reviewRiskRating();

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectReadinessToOpenMeetingFromTaskList();
        summaryPage.clickChange();

        Logger.log("Readiness to Open Meeting can save null values");

        readinessToOpenMeetingEditPage
            .clickContinue()

        Logger.log("Readiness to Open Meeting can be edited");

        summaryPage.clickChange();

        readinessToOpenMeetingEditPage
            .selectAROMIsExpectedToHappenYes()
            .expectedDateOfTheMeeting("60", "12", "2050")
            .clickContinue()
            .errorForExpectedMeetingDate("Day must be between 1 and 31")
            .expectedDateOfTheMeeting("30", "12", "1999")
            .clickContinue()
            .errorForExpectedMeetingDate("Year must be between 2000 and 2050")
            .expectedDateOfTheMeeting("5", "6", "2040")
            .selectFormalMeeting()
            .dateOfTheFormalMeeting("60", "12", "2050")
            .clickContinue()
            .errorForFormalMeetingDate("Day must be between 1 and 31")
            .dateOfTheFormalMeeting("30", "12", "1999")
            .clickContinue()
            .errorForFormalMeetingDate("Year must be between 2000 and 2050")
            .dateOfTheFormalMeeting("5", "5", "2040")
            .checkPrincipalDesignate()
            .checkCommissionedAnExternalExpert()
            .checkSavedTheInternalRomReport()
            .checkSavedTheExternalRomReport()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Readiness to open meeting (ROM)")
            .inOrder()
            .summaryShows("A ROM is expected to happen").HasValue("Yes").HasChangeLink()
            .summaryShows("Expected date of the meeting").HasValue("5 June 2040").HasChangeLink()
            .summaryShows("Type of meeting held").HasValue("Formal meeting").HasChangeLink()
            .summaryShows("Date of the meeting").HasValue("5 May 2040").HasChangeLink()
            .summaryShows("Principal designate (PD) has provided the checklist").HasValue("Yes").HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the internal ROM report in Workplaces folder").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the external ROM report in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("ReadinessToOpenMeeting");

        taskListPage.selectReadinessToOpenMeetingFromTaskList();

        Logger.log("Readiness to Open Meeting can be edited again with No ROM held option");

        summaryPage.clickChange();

        readinessToOpenMeetingEditPage
            .selectAROMIsExpectedToHappenNo()
            .selectNoRomHeld()
            .whyMeetingWasNotHeld()
            .uncheckPrincipalDesignate()
            .checkCommissionedAnExternalExpert()
            .uncheckSavedTheInternalRomReport()
            .checkSavedTheExternalRomReport()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Readiness to open meeting (ROM)")
            .inOrder()
            .summaryShows("A ROM is expected to happen").HasValue("No").HasChangeLink()
            .summaryShows("Type of meeting held").HasValue("No meeting held").HasChangeLink()
            .summaryShows("Why a meeting was not held").HasValue("This is the reason why meeting was not held").HasChangeLink()
            .summaryShows("Principal designate (PD) has provided the checklist").IsEmpty().HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the internal ROM report in Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Saved the external ROM report in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("ReadinessToOpenMeeting");

        taskListPage.selectReadinessToOpenMeetingFromTaskList();
        summaryPage.clickChange();

        Logger.log("Should be able to select Informal meeting");

        readinessToOpenMeetingEditPage
            .selectAROMIsExpectedToHappenYes()
            .expectedDateOfTheMeeting("6", "6", "2040")
            .selectInformalMeeting()
            .dateOfTheInformalMeeting("60", "12", "2050")
            .clickContinue()
            .errorForInformalMeetingDate("Day must be between 1 and 31")
            .dateOfTheInformalMeeting("30", "12", "1999")
            .clickContinue()
            .errorForInformalMeetingDate("Year must be between 2000 and 2050")
            .dateOfTheInformalMeeting("23", "5", "2030")
            .checkPrincipalDesignate()
            .checkCommissionedAnExternalExpert()
            .checkSavedTheInternalRomReport()
            .checkSavedTheExternalRomReport()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Readiness to open meeting (ROM)")
            .inOrder()
            .summaryShows("A ROM is expected to happen").HasValue("Yes").HasChangeLink()
            .summaryShows("Expected date of the meeting").HasValue("6 June 2040").HasChangeLink()
            .summaryShows("Type of meeting held").HasValue("Informal meeting").HasChangeLink()
            .summaryShows("Date of the meeting").HasValue("23 May 2030").HasChangeLink()
            .summaryShows("Principal designate (PD) has provided the checklist").HasValue("Yes").HasChangeLink()
            .summaryShows("Commissioned an external expert to attend any meetings (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the internal ROM report in Workplaces folder").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the external ROM report in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusIsCompleted("ReadinessToOpenMeeting");
    });
});
