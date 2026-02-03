import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import dueDiligenceChecksEditPage from "../../../pages/tasks/Getting-ready-to-open/edit-due-diligence-checks-cy";

describe("Testing the due diligence checks task", () => {

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

    it("Should be able input due diligence checks", () => {
        Logger.log("Select due diligence checks");

        taskListPage.isTaskStatusIsNotStarted("DueDiligenceChecks")
            .selectDueDiligenceFromTaskList();

            checkSummaryPageHasEmptyValuesAndNotComplete(project.schoolName);

            Logger.log("Go back to task list")
            summaryPage.clickBack();

            taskListPage.selectDueDiligenceFromTaskList();
            summaryPage.clickChange();
            Logger.log("Due diligence checks can save null values");

            dueDiligenceChecksEditPage.clickContinue();

            checkSummaryPageHasEmptyValuesAndNotComplete(project.schoolName);

            cy.executeAccessibilityTests();

            Logger.log("Due diligence checks can be edited");
            
            summaryPage.clickChange();

            dueDiligenceChecksEditPage
            .checkReceivedChairOfTrusteesDbsCountersignedCertificate()
            .checkNonSpecialistChecksDoneOnAllTrustMembersAndTrusteesInLast2Years()
            .requestedCounterExtremismChecks("No")
            .enterDateWhenAllChecksWereCompleted("30", "01", "2050")
            .checkSavedNonSpecialistChecksSpreadsheetInWorkplaces()
            .checkDeletedAnyCopiesOfChairsDBSCertificate()
            .checkDeletedEmailContainingSuitabilityAndDeclarationForms()

            dueDiligenceChecksEditPage.clickContinue()

            summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Due diligence checks")
            .inOrder()
            .summaryShows("Received Chair of Trustee's DBS countersigned certificate").HasValue("Yes").HasChangeLink()
            .summaryShows("Non-specialist checks done on all trust members and trustees in last 2 years").HasValue("Yes").HasChangeLink()
            .summaryShows("Requested counter extremism checks").HasValue("No").HasChangeLink()
            .summaryShows("Date when all checks were completed").HasValue("30 January 2050").HasChangeLink()
            .summaryShows("Saved the non-specialist checks spreadsheet in Workplaces folder").HasValue("Yes").HasChangeLink()
            .summaryShows("Deleted any copies of Chair's DBS certificate").HasValue("Yes").HasChangeLink()
            .summaryShows("Deleted emails containing suitability and declaration forms").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue();
 
            taskListPage.isTaskStatusInProgress("DueDiligenceChecks");
            taskListPage.selectDueDiligenceFromTaskList();

            Logger.log("Due dilligence checks date validation is correct")

            summaryPage.clickChange();

            dueDiligenceChecksEditPage.enterDateWhenAllChecksWereCompleted("30", "01", "2090")
            dueDiligenceChecksEditPage.clickContinue()
            dueDiligenceChecksEditPage
            .errorForDate()
            .showsError("Year must be between 2000 and 2050")
            .enterDateWhenAllChecksWereCompleted("30", "01", "2049")
            .clickContinue()

            summaryPage
            .MarkAsComplete()
            .clickConfirmAndContinue();

            taskListPage.isTaskStatusIsCompleted("DueDiligenceChecks");
    });
});

function checkSummaryPageHasEmptyValuesAndNotComplete(schoolName: string) {
    summaryPage
    .schoolNameIs(schoolName)
    .titleIs("Due diligence checks")
    .inOrder()
    .summaryShows("Received Chair of Trustee's DBS countersigned certificate").IsEmpty().HasChangeLink()
    .summaryShows("Non-specialist checks done on all trust members and trustees in last 2 years").IsEmpty().HasChangeLink()
    .summaryShows("Requested counter extremism checks").IsEmpty().HasChangeLink()
    .summaryShows("Date when all checks were completed").IsEmpty().HasChangeLink()
    .summaryShows("Saved the non-specialist checks spreadsheet in Workplaces folder").IsEmpty().HasChangeLink()
    .summaryShows("Deleted any copies of Chair's DBS certificate").IsEmpty().HasChangeLink()
    .summaryShows("Deleted emails containing suitability and declaration forms").IsEmpty().HasChangeLink()
    .isNotMarkedAsComplete();
}