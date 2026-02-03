import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import fundingAgreementSubmissionEditPage from "../../../pages/tasks/pre-opening/edit-funding-agreement-submission.cy";

describe("Testing Funding agreement submission Task", () => {

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

    it("Should successfully set funding agreement submission task", () => {

        cy.log("Select funding agreement heakth check");
        taskListPage.isTaskStatusIsNotStarted("FundingAgreementSubmission")
            .selectFundingAgreementSubmissionFromTaskList();

        cy.log("Confirm empty funding agreement submission");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement submission")
            .inOrder()
            .summaryShows("Drafted a funding agreement (FA) submission").IsEmpty().HasChangeLink()
            .summaryShows("Regional Director has signed-off the FA submission (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Minister has signed-off the FA submission (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Included a signed-off impact assessment in the submission").IsEmpty().HasChangeLink()
            .summaryShows("Saved the submission in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();


        cy.executeAccessibilityTests();
        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open funding agreement submission");
        taskListPage.isTaskStatusIsNotStarted("FundingAgreementSubmission")
            .selectFundingAgreementSubmissionFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("FundingAgreementSubmission")
            .selectFundingAgreementSubmissionFromTaskList();

        cy.log("Check search page");

        summaryPage.clickChange();

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });

        cy.log("Fill in funding arrangement submission")

        fundingAgreementSubmissionEditPage
            .checkDraftedFASubmission()
            .checkMinisterSignedOffFASubmission()
            .checkIncludedSignedOffImpactAssessment()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement submission")
            .inOrder()
            .summaryShows("Drafted a funding agreement (FA) submission").HasValue("Yes").HasChangeLink()
            .summaryShows("Regional Director has signed-off the FA submission (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Minister has signed-off the FA submission (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Included a signed-off impact assessment in the submission").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the submission in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.log("Edit funding agreement submission")
       
        summaryPage.clickChange();
        fundingAgreementSubmissionEditPage
            .uncheckMinisterSignedOffFASubmission()
            .checkRegionalDirectorSignedOffFASubmission()
            .checkSavedFASubmissionInWorkplacesFolder()
            .clickContinue()
            

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement submission")
            .inOrder()
            .summaryShows("Drafted a funding agreement (FA) submission").HasValue("Yes").HasChangeLink()
            .summaryShows("Regional Director has signed-off the FA submission (if applicable)").HasValue("Yes").HasChangeLink()
            .summaryShows("Minister has signed-off the FA submission (if applicable)").IsEmpty().HasChangeLink()
            .summaryShows("Included a signed-off impact assessment in the submission").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the submission in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage
            .selectFundingAgreementSubmissionFromTaskList()
        
        summaryPage.isMarkedAsComplete()
    })
    
    
    
})