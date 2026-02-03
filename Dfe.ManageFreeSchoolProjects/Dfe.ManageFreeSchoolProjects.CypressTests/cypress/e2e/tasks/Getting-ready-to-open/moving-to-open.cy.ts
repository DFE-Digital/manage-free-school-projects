import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import movingToOpenEditPage from "../../../pages/tasks/Getting-ready-to-open/edit-moving-to-open-cy";

describe("Testing the moving to open task", () => {

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

    it("Should be able to set moving to open", () => {
        Logger.log("Select moving to open");
        taskListPage.isTaskStatusIsNotStarted("MovingToOpen")
            .selectMovingToOpenFromTaskList();
        
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Moving to open")
            .inOrder()
            .summaryShows("Sent the project brief to").IsEmpty().HasChangeLink()
            .summaryShows("Sent emails to").IsEmpty().HasChangeLink()
            .summaryShows("Saved documents in moving to open Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Actual opening date").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log("Go back to task list");
        summaryPage.clickBack();
        
        taskListPage.selectMovingToOpenFromTaskList();
        summaryPage.clickChange();
        
        Logger.log("Moving to open can save null values");
        
         movingToOpenEditPage
             .clickContinue()

        Logger.log("Moving to open can be edited");

        summaryPage.clickChange(); 

        movingToOpenEditPage
            .withActualOpeningDate("60","12","2050")
            .clickContinue()
            .errorForActualStartDate("Day must be between 1 and 31")
            .withActualOpeningDate("30","12","1999")
            .clickContinue()
            .errorForActualStartDate("Year must be between 2000 and 2050")
            .withActualOpeningDate("30","12","2050")
            .checkSentToSfso()
            .checkSentToEducationEstates()
            .checkSentToDeliverOfficer()
            .checkEmailSentToRelevantContact()
            .checkEmailSentSchoolPrincipal()
            .checkSavedToWorkspacesProjectBrief()
            .checkSavedToWorkspacesAnnexB()
            .checkSavedToWorkspacesAnnexE()
            .clickContinue()
       
        Logger.log("Should update the task status");

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Moving to open")
            .inOrder()
            .summaryShows("Sent the project brief to")
            .HasValue("SFSO (Schools Financial Support and Oversight)").HasChangeLink()
            .HasValue("Education Estates").HasChangeLink()
            .HasValue("New delivery officer").HasChangeLink()
            .summaryShows("Sent emails to")
            .HasValue("The school's principal about handing project over to SFSO").HasChangeLink()
            .HasValue("Relevant contacts to confirm school is moving to open").HasChangeLink()
            .summaryShows("Saved documents in moving to open Workplaces folder")
            .HasValue("Project brief").HasChangeLink()
            .HasValue("Annex B checklist documents").HasChangeLink()
            .HasValue("Completed Annex E checklist").HasChangeLink()
            .summaryShows("Actual opening date").HasValue("30 December 2050").HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusInProgress("MovingToOpen");
        
        taskListPage.selectMovingToOpenFromTaskList();
        summaryPage.clickChange();
        
        Logger.log("Should be able to clear some values");
        
        movingToOpenEditPage
            .withActualOpeningDate("29","12","2050")
            .uncheckSentToDeliverOfficer()
            .uncheckEmailSentToRelevantContact()
            .uncheckSavedToWorkspacesProjectBrief()
            .uncheckSavedToWorkspacesAnnexE()
            .uncheckSavedToWorkspacesAnnexB()
            .clickContinue()
        
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Moving to open")
            .inOrder()
            .summaryShows("Sent the project brief to")
            .HasValue("SFSO (Schools Financial Support and Oversight)").HasChangeLink()
            .HasValue("Education Estates").HasChangeLink()
            .DoesNotHaveValue("New delivery officer").HasChangeLink()
            .summaryShows("Sent emails to")
            .HasValue("The school's principal about handing project over to SFSO").HasChangeLink()
            .DoesNotHaveValue("Relevant contacts to confirm school is moving to open").HasChangeLink()
            .summaryShows("Saved documents in moving to open Workplaces folder").IsEmpty().HasChangeLink()
            .summaryShows("Actual opening date").HasValue("29 December 2050").HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage.isTaskStatusIsCompleted("MovingToOpen");
    });
});