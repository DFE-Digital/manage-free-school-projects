import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import educationBriefEditPage from "../../../pages/tasks/sign-off-preparation/edit-education-brief.cy";
import riskPage from "cypress/pages/risk/projectRiskSummaryPage"

describe("Testing education plans and policies", () => {

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

    it("Should successfully set education plans and policies task", () => {

        cy.log("education plans and policies");
        taskListPage.isTaskStatusIsNotStarted("EducationBrief")
            .selectEducationBriefFromList();
        
        cy.log("Confirm empty education plans and policies");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Education plans and policies")
            .inOrder()
            .summaryShows("Trust confirmed it has education plans and policies in place").IsEmpty().HasChangeLink()
            .summaryShows("Commisioned an external expert (EE) to review the safeguarding policy - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Commissioned an EE to review the pupil assessment, recording and reporting policy - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Date the EE reviewed the education brief - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Saved EE specification and advice in Workplaces - if applicable").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();
        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open education plans and policies");
        taskListPage.isTaskStatusIsNotStarted("EducationBrief")
            .selectEducationBriefFromList()

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("EducationBrief")
            .selectEducationBriefFromList()

        cy.log("Check education plans and policies page");

        summaryPage.clickChange();

        cy.executeAccessibilityTests

        educationBriefEditPage
            .withDateEEReviewedEducationBrief("2","ds","2050")
            .clickContinue()
            .errorForDateEEReviewedEducationBrief().showsError("Enter a date in the correct format")
            .withDateEEReviewedEducationBrief("2","2","2090")
            .clickContinue()
            .errorForDateEEReviewedEducationBrief().showsError("Year must be between 2000 and 2050")
            .withDateEEReviewedEducationBrief("2","2","1999")
            .clickContinue()
            .errorForDateEEReviewedEducationBrief().showsError("Year must be between 2000 and 2050")
            .withDateEEReviewedEducationBrief("","","")

        educationBriefEditPage
            .withDateEEReviewedEducationBrief("2", "3", "2049")
            .checkTrustConfirmedPlansAndPoliciesInPlace()
            .checkCommissionedEEToReviewSafeguardingPolicy()
            .checkCommissionedEEToReviewPupilAssessmentRecordingAndReportingPolicy()
            .checkSavedEESpecificationAndAdviceInWorkplaces()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Education plans and policies")
            .inOrder()
            .summaryShows("Trust confirmed it has education plans and policies in place").HasValue("Yes").HasChangeLink()
            .summaryShows("Commisioned an external expert (EE) to review the safeguarding policy - if applicable").HasValue("Yes").HasChangeLink()
            .summaryShows("Commissioned an EE to review the pupil assessment, recording and reporting policy - if applicable").HasValue("Yes").HasChangeLink()
            .summaryShows("Date the EE reviewed the education brief - if applicable").HasValue("2 March 2049").HasChangeLink()
            .summaryShows("Saved EE specification and advice in Workplaces - if applicable").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete();

        cy.log("uncheck education plans and policies page");

        summaryPage.clickChange();

        cy.executeAccessibilityTests

        educationBriefEditPage
            .uncheckTrustConfirmedPlansAndPoliciesInPlace()
            .uncheckCommissionedEEToReviewSafeguardingPolicy()
            .uncheckCommissionedEEToReviewPupilAssessmentRecordingAndReportingPolicy()
            .uncheckSavedEESpecificationAndAdviceInWorkplaces()
            .clickContinue()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage
            .selectEducationBriefFromList()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Education plans and policies")
            .inOrder()
            .summaryShows("Trust confirmed it has education plans and policies in place").IsEmpty().HasChangeLink()
            .summaryShows("Commisioned an external expert (EE) to review the safeguarding policy - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Commissioned an EE to review the pupil assessment, recording and reporting policy - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Date the EE reviewed the education brief - if applicable").IsEmpty().HasChangeLink()
            .summaryShows("Date the EE reviewed the education brief - if applicable").HasValue("2 March 2049").HasChangeLink()
            .summaryShows("Saved EE specification and advice in Workplaces - if applicable").IsEmpty().HasChangeLink()
            .isMarkedAsComplete()

        cy.log("click on risk link");
        
        summaryPage
            .clickChangeRiskRating()

        riskPage
            .hasSchoolName(project.schoolName)
            .hasTitle("Current risk ratings")
            .hasOverallRiskRating(["Empty"])
    })
})