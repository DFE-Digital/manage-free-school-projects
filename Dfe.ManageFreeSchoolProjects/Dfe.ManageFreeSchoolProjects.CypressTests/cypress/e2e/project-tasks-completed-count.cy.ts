import {ProjectDetailsRequest, ProjectTaskSummaryResponse, ResponseWrapper} from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import projectTaskSummary from "cypress/api/projectTaskSummary";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import externalExpertVisitEditPage from "../pages/tasks/After-opening/edit-external-expert-visit.cy";
import schoolDetailsPage from "../pages/schoolDetailsPage";

describe("Testing the tasks completed count", () => {

    let project: ProjectDetailsRequest;
    let projectNonPresumption: ProjectDetailsRequest;
    let taskCount;
    let nonPresumptionTaskCount

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();
        projectNonPresumption = RequestBuilder.createProjectDetailsNonPresumption();
        
        projectApi
            .post({
                projects: [projectNonPresumption],
            })

        projectApi
            .post({
                projects: [project],
            })

        projectTaskSummary
            .get( { projectId : project.projectId })
            .then((response : ResponseWrapper<ProjectTaskSummaryResponse>) => {
                expect(response).to.not.equal(null)
                taskCount = response.data.taskCount
            })
        
        projectTaskSummary
            .get( { projectId : projectNonPresumption.projectId })
            .then((response : ResponseWrapper<ProjectTaskSummaryResponse>) => {
                expect(response).to.not.equal(null)
                nonPresumptionTaskCount = response.data.taskCount
            })
        
        cy.visit(`/projects/${project.projectId}/tasks`);
        
    });

    it("Should be able to set external expert visit", () => {
        Logger.log("Select External expert visit ");
        taskListPage.taskCompletedCountMessage(`You have completed 0 of ${taskCount} sections.`)
        taskListPage.isTaskStatusIsNotStarted("CommissionedExternalExpert")
            .selectExternalExpertVisitFromTaskList();

        Logger.log("Mark External expert visit as completed");
        
        summaryPage
            .titleIs("External expert visit")
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        Logger.log("Go back to task list");

        taskListPage.taskCompletedCountMessage(`You have completed 1 of ${taskCount} sections.`)
            .isTaskStatusIsCompleted("CommissionedExternalExpert")
            .selectApplicationsEvidenceFromTaskList();

        Logger.log("Mark Applications evidence as completed");
        
        summaryPage
            .titleIs(" Applications evidence")
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        Logger.log("Go back to task list");

        taskListPage.taskCompletedCountMessage(`You have completed 2 of ${taskCount} sections.`)
            .isTaskStatusIsCompleted("CommissionedExternalExpert")
            .isTaskStatusIsCompleted("ApplicationsEvidence")
            .selectSchoolFromTaskList()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("School")
            .clickChange()

        Logger.log("set school type as Alternative provision");
        
        schoolDetailsPage
            .titleIs("Edit School")
            .withSchoolType("AlternativeProvision")
            .withSchoolPhase("Secondary")
            .withAgeRange("11", "16")
            .withFormsOfEntry("3")
            .withGender("Mixed")
            .withNursery("Yes")
            .withSixthForm("No")
            .withResidentialOrBoarding("No")
            .withFaithStatus("Designation")
            .withFaithType("faith-type-Jewish")
            .clickContinue();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("School")
            .clickConfirmAndContinue()

        Logger.log("completed count and sections count is adjusted correctly");

        taskListPage.taskCompletedCountMessage(`You have completed 2 of ${taskCount -1} sections.`)
            .isTaskStatusIsCompleted("CommissionedExternalExpert")


        Logger.log("Visit non presumption project");
        
        cy.visit(`/projects/${projectNonPresumption.projectId}/tasks`);

        taskListPage.taskCompletedCountMessage(`You have completed 0 of ${nonPresumptionTaskCount} sections.`)
        taskListPage.isTaskStatusIsNotStarted("CommissionedExternalExpert")
            .selectExternalExpertVisitFromTaskList();

        Logger.log("non presumption projects has the correct count");

        summaryPage
            .titleIs("External expert visit")
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue();
        
        taskListPage.taskCompletedCountMessage(`You have completed 1 of ${nonPresumptionTaskCount} sections.`)
    });
});