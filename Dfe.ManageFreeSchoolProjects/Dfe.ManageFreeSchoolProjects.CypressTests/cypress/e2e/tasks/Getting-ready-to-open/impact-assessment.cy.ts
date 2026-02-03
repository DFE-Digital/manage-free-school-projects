import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import impactAssessmentEditPage from "../../../pages/tasks/Getting-ready-to-open/edit-impact-assessment-cy";

describe("Testing the impact assessment task - presumption route", () => {

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

    it("Should be able to set impact assessment", () => {
        Logger.log("Select impact assessment");
        taskListPage.isTaskStatusIsNotStarted("ImpactAssessment")
            .selectImpactAssessmentFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectImpactAssessmentFromTaskList();
        summaryPage.clickChange();

        Logger.log("Impact assessment can save null values");
        
        impactAssessmentEditPage
        .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .clickChange();

        cy.executeAccessibilityTests();

        Logger.log("Impact assessment can be edited");
        
        impactAssessmentEditPage
            .checkImpactAssessmentDone()
            .checkSavedToWorkplaces()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Conducted the impact assessment").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the signed-off impact assessment in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("ImpactAssessment");

        taskListPage.selectImpactAssessmentFromTaskList();
        summaryPage.clickChange();

        impactAssessmentEditPage
            .uncheckImpactAssessmentDone()
            .uncheckSavedToWorkplaces()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()
        
        taskListPage.isTaskStatusIsCompleted("ImpactAssessment");
    });
});

describe("Testing the impact assessment task - central route", () => {

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

    it("Should include section 9 letter wording for saved in workplaces label", () => {
        Logger.log("Select impact assessment");
        taskListPage.isTaskStatusIsNotStarted("ImpactAssessment")
            .selectImpactAssessmentFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Sent the section 9 letter to local authority").HasValue("No").HasChangeLink()
            .summaryShows("Date sent").IsEmpty().HasChangeLink()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the section 9 letter and signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log("Go back to task list");
        summaryPage.clickBack();

        taskListPage.selectImpactAssessmentFromTaskList();
        summaryPage.clickChange();

        Logger.log("Impact assessment can save null values");
        
        impactAssessmentEditPage
        .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Sent the section 9 letter to local authority").HasValue("No").HasChangeLink()
            .summaryShows("Date sent").IsEmpty().HasChangeLink()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the section 9 letter and signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .clickChange();

        cy.executeAccessibilityTests();

        Logger.log("Impact assessment can be edited");
        
        impactAssessmentEditPage
            .checkImpactAssessmentDone()
            .checkSavedToWorkplaces()
            .clickContinue()

        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Sent the section 9 letter to local authority").HasValue("No").HasChangeLink()
            .summaryShows("Date sent").IsEmpty().HasChangeLink()
            .summaryShows("Conducted the impact assessment").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the section 9 letter and signed-off impact assessment in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("ImpactAssessment");

        taskListPage.selectImpactAssessmentFromTaskList();

        summaryPage.clickChange();

        impactAssessmentEditPage
            .checkSentSection9()
            .withDateSent("2","ds","2050")
            .clickContinue()
            .errorDateSent().showsError("Enter a date in the correct format")
            .withDateSent("2","2","2090")
            .clickContinue()
            .errorDateSent().showsError("Year must be between 2000 and 2050")
            .withDateSent("2","2","1999")
            .clickContinue()
            .errorDateSent().showsError("Year must be between 2000 and 2050")
            .withDateSent("2","2","2049")
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Sent the section 9 letter to local authority").HasValue("Yes").HasChangeLink()
            .summaryShows("Date sent").HasValue("2 February 2049").HasChangeLink()
            .summaryShows("Conducted the impact assessment").HasValue("Yes").HasChangeLink()
            .summaryShows("Saved the section 9 letter and signed-off impact assessment in Workplaces folder").HasValue("Yes").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickChange();

        impactAssessmentEditPage
            .uncheckSentSection9()
            .withDateSent("", "", "")
            .uncheckImpactAssessmentDone()
            .uncheckSavedToWorkplaces()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Impact assessment")
            .inOrder()
            .summaryShows("Sent the section 9 letter to local authority").HasValue("No").HasChangeLink()
            .summaryShows("Date sent").IsEmpty().HasChangeLink()
            .summaryShows("Conducted the impact assessment").IsEmpty().HasChangeLink()
            .summaryShows("Saved the section 9 letter and signed-off impact assessment in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()
        
        taskListPage.isTaskStatusIsCompleted("ImpactAssessment");
    });
});