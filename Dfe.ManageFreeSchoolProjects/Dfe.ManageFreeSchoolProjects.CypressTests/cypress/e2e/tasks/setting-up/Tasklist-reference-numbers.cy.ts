import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import referenceNumbersDetailsPage from "cypress/pages/referenceNumbersDetailsPage";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import dataGenerator from "cypress/fixtures/dataGenerator";

describe("Testing project overview", () => {
    let project: ProjectDetailsRequest;
    let existingProject: ProjectDetailsRequest;

    let newProjectId: string = dataGenerator.generateTemporaryId(20)

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();
        existingProject = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project, existingProject],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it("Should be able to edit reference numbers", () => {
        cy.executeAccessibilityTests();

        cy.log("Selecting reference numbers from the tasklist");

        taskListPage.isTaskStatusIsNotStarted("ReferenceNumbers")
            .selectReferenceNumbersFromTaskList();

        Logger.log("Confirm reference numbers");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Reference numbers")
            .inOrder()
            .summaryShows("Project ID").HasValue(project.projectId).HasChangeLink()
            .summaryShows("URN").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.executeAccessibilityTests();
        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open reference numbers");
        taskListPage.isTaskStatusIsNotStarted("ReferenceNumbers")
            .selectReferenceNumbersFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("ReferenceNumbers")
            .selectReferenceNumbersFromTaskList()
        
        summaryPage.clickChange();

        cy.executeAccessibilityTests();

        Logger.log("Check project id is required");
        referenceNumbersDetailsPage
            .clearProjectId()
            .clickContinue()
            .errorForProjectId().showsError("Enter the project ID")
            .withProjectId(newProjectId + "-")
            .clickContinue()
            .errorForProjectId().showsError("Project ID must only include numbers and letters")
            .withProjectId(newProjectId + " a")
            .clickContinue()
            .errorForProjectId().showsError("Project ID must not include spaces")
            .withProjectId(dataGenerator.generateAlphaNumeric(26))
            .clickContinue()
            .errorForProjectId().showsError("Project ID must be 25 characters or less")
            .withProjectId(existingProject.projectId)
            .clickContinue()
            .errorForProjectId().showsError("Project ID already exists");
            

        cy.log("Fill in valid project id without urn")

        referenceNumbersDetailsPage
            .withProjectId(newProjectId)
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Reference numbers")
            .inOrder()
            .summaryShows("Project ID").HasValue(newProjectId).HasChangeLink()
            .summaryShows("URN").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()

        summaryPage.clickChange();

        cy.executeAccessibilityTests();

        Logger.log("Check urn validation");
        referenceNumbersDetailsPage
            .withUrn("zyxwvu654321")
            .clickContinue()
            .errorForUrn().showsError("URN must be 6 numbers, like 123456")
            .withUrn("65753")
            .clickContinue()
            .errorForUrn().showsError("URN must be 6 numbers, like 123456")
            .withUrn("456789")
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Reference numbers")
            .inOrder()
            .summaryShows("Project ID").HasValue(newProjectId).HasChangeLink()
            .summaryShows("URN").HasValue("456789").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage
            .selectReferenceNumbersFromTaskList()
        
        summaryPage.isMarkedAsComplete()
    
    });
});
