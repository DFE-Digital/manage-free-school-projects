import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import projectOverviewPage from "../pages/projectOverviewPage";
import projectStatusPage from "../pages/project-status/projectStatusPage";
import taskListPage from "../pages/taskListPage";
import contactsPage from "../pages/contacts/contactsPage";
import projectStatusWithdrawnPage from "cypress/pages/project-status/projectStatusWithdrawnPage";
import projectStatusCancelledPage from "cypress/pages/project-status/projectStatusCancelledPage";
import dataGenerator from "cypress/fixtures/dataGenerator";

describe("Testing that we can change the project status", () => {
    let project: ProjectDetailsRequest;

    describe("Change project status", () => {

        it("Change status for central route project", () => {

          cy.login();
    
            project = RequestBuilder.createProjectDetailsCentralRoute();
    
            projectApi
                .post({
                    projects: [project],
                })
                .then(() => {
                    cy.visit(`/projects/${project.projectId}/overview`);
                });

            projectOverviewPage
                 .clickChangeProjectStatus();

            projectStatusPage
                 .selectApplicationCompetitionStage()
                 .clickSaveAndContinue()

            projectOverviewPage
                .hasProjectStatus("Application competition stage")
                .clickChangeProjectStatus();

            projectStatusPage
                .selectApplicationStage()
                .clickSaveAndContinue();

            projectOverviewPage
                .hasProjectStatus("Application stage")
                .clickChangeProjectStatus();

            projectStatusPage
                .selectOpenNotIncludedFigures()
                .clickSaveAndContinue();

            projectOverviewPage
                .hasProjectStatus("Open - not included in figures")
                .clickChangeProjectStatus();

            projectStatusPage
                .selectPreOpeningNotIncludedFigures()
                .clickSaveAndContinue();

            projectOverviewPage
                .hasProjectStatus("Pre-opening - not included in figures")
                .clickChangeProjectStatus();

            projectStatusPage
                .selectRejected()
                .clickSaveAndContinue();

            projectOverviewPage
                .hasProjectStatus("Rejected")
                .clickChangeProjectStatus();

            projectStatusPage
                .selectWithdrawnInApplication()
                .clickSaveAndContinue()
            projectStatusWithdrawnPage                
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Enter a date for the date the project was withdrawn")
                .addWithdrawnYear("1", "1", "error")
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Enter a date in the correct format")
                .addWithdrawnYear("1", "1", "1999")
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Year must be between 2000 and 2050")
                .clickSaveAndContinue()
                .addWithdrawnYear("1", "1", "2051")
                .errorForWithdrawnDate("Year must be between 2000 and 2050")
                .addWithdrawnYear("1", "1", "2045")
                .errorForReasonForWithdrawal("Enter the main reason for withdrawal")
                .errorForWithdrawnAsAResultOFNationalPipelineReview("Enter whether the project was withdrawn as a result of the 2024/25 national review of pipeline projects")
                .errorForNotesAboutTheWithdrawal("Enter the notes about the withdrawal")
                .selectGovernance()
                .selectProjectWithdrawnAsAResultOfNationalPipelineReviewYes()
                .withAddNotesAboutTheWithdrawal(dataGenerator.generateAlphaNumeric(501))
                .clickSaveAndContinue()
                .errorForNotesAboutTheWithdrawal("Notes about the withdrawal must be 500 characters or less")
                .withAddNotesAboutTheWithdrawal("Some notes")
                .clickSaveAndContinue();

            Logger.log("user is sent back to projects overview page");

            projectOverviewPage
                .hasReasonForWithdrawal("Governance")
                .hasProjectWithdrawnAsPartOfThePipelineReview("Yes")
                .hasCommentaryForWithdrawal("Some notes")
                .hasWithdrawnDate("1 January 2045")
                .clickChangeProjectStatus();

            projectStatusPage
            .selectOpen()
            .clickSaveAndContinue()

            projectOverviewPage
                .hasProjectStatus("Open")
                .selectTaskListTab()
                .clickChangeProjectStatus();
  
            projectStatusPage
                .openIsChecked()
                .selectCancelled()
                .clickSaveAndContinue()
            projectStatusCancelledPage                
                .clickSaveAndContinue()
                .errorForCancelledDate("Enter a date for the date the project was cancelled")
                .addCancelledYear("1", "1", "error")
                .clickSaveAndContinue()
                .errorForCancelledDate("Enter a date in the correct format")
                .addCancelledYear("1", "1", "1999")
                .clickSaveAndContinue()
                .errorForCancelledDate("Year must be between 2000 and 2050")
                .clickSaveAndContinue()
                .addCancelledYear("1", "1", "2051")
                .errorForCancelledDate("Year must be between 2000 and 2050")
                .addCancelledYear("1", "1", "2045")
                .errorForReasonForCancellation("Enter the main reason for cancellation")
                .errorForCancelledAsAResultOFNationalPipelineReview("Enter whether the project was cancelled as a result of the 2024/25 national review of pipeline projects")
                .errorForNotesAboutTheCancellation("Enter the notes about the cancellation")
                .selectGovernance()
                .selectProjectCancelledAsAResultOfNationalPipelineReviewYes()
                .withAddNotesAboutTheCancellation(dataGenerator.generateAlphaNumeric(501))
                .clickSaveAndContinue()
                .errorForNotesAboutTheCancellation("Notes about the cancellation must be 500 characters or less")
                .withAddNotesAboutTheCancellation("Some notes")
                .clickSaveAndContinue();
            
            Logger.log("user is sent back to tasklist page");

            taskListPage
                .onTasklistTab()
                .hasProjectStatus("Cancelled")
                .selectAboutTheProjectTab();

            projectOverviewPage
                .hasReasonForCancellation("Governance")
                .hasProjectCancelledAsPartOfThePipelineReview("Yes")
                .hasCommentaryForCancellation("Some notes")
                .hasCancelledDate("1 January 2045")
                .selectContactsTab();

            Logger.log("change status to closed");
        
            contactsPage
                .hasProjectStatus("Cancelled")
                .clickChangeProjectStatus();

            projectStatusPage
                .cancelledIsChecked()
                .selectClosed()
                .clickSaveAndContinue()
                .errorForClosedDate("Enter a date in the correct format")
                .addClosedYear("1", "1", "error")
                .clickSaveAndContinue()
                .errorForClosedDate("Enter a date in the correct format")
                .addClosedYear("1", "1", "1999")
                .clickSaveAndContinue()
                .errorForClosedDate("Year must be between 2000 and 2050")
                .clickSaveAndContinue()
                .addClosedYear("1", "1", "2051")
                .errorForClosedDate("Year must be between 2000 and 2050")
                .addClosedYear("1", "1", "2048")
                .clickSaveAndContinue()

            Logger.log("user is sent back to contacts page");
            
            contactsPage
                .onContactsTab()
                .hasProjectStatus("Closed")
                .selectAboutTheProjectTab();

            Logger.log("closed date is shown on project overview");

            projectOverviewPage
                .hasClosedDate("1 January 2048")

            Logger.log("change status to withdrawn");

            projectOverviewPage
                .clickChangeProjectStatus()

            projectStatusPage
                .closedIsChecked()
                .closedYearHasValue("1", "1", "2048")
                .selectWithdrawn()
                .clickSaveAndContinue()

            projectStatusWithdrawnPage                
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Enter a date for the date the project was withdrawn")
                .addWithdrawnYear("1", "1", "error")
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Enter a date in the correct format")
                .addWithdrawnYear("1", "1", "1999")
                .clickSaveAndContinue()
                .errorForWithdrawnDate("Year must be between 2000 and 2050")
                .clickSaveAndContinue()
                .addWithdrawnYear("1", "1", "2051")
                .errorForWithdrawnDate("Year must be between 2000 and 2050")
                .addWithdrawnYear("1", "1", "2045")
                .errorForReasonForWithdrawal("Enter the main reason for withdrawal")
                .errorForWithdrawnAsAResultOFNationalPipelineReview("Enter whether the project was withdrawn as a result of the 2024/25 national review of pipeline projects")
                .errorForNotesAboutTheWithdrawal("Enter the notes about the withdrawal")
                .selectGovernance()
                .selectProjectWithdrawnAsAResultOfNationalPipelineReviewYes()
                .withAddNotesAboutTheWithdrawal(dataGenerator.generateAlphaNumeric(501))
                .clickSaveAndContinue()
                .errorForNotesAboutTheWithdrawal("Notes about the withdrawal must be 500 characters or less")
                .withAddNotesAboutTheWithdrawal("Some notes")
                .clickSaveAndContinue();

            Logger.log("user is sent back to projects overview page");

            projectOverviewPage
                .hasReasonForWithdrawal("Governance")
                .hasProjectWithdrawnAsPartOfThePipelineReview("Yes")
                .hasCommentaryForWithdrawal("Some notes")
                .hasWithdrawnDate("1 January 2045")
        
        });

        it("Change status for Presumption project", () => {

            cy.login();
    
            project = RequestBuilder.createNewProjectDetails();
    
            projectApi
                .post({
                    projects: [project],
                })
                .then(() => {
                    cy.visit(`/projects/${project.projectId}/overview`);
                });
                projectOverviewPage
                .hasProjectStatus("Pre-opening")
                .clickChangeProjectStatus()
              
                projectStatusPage
                .preOpeningIsChecked()
                .clickBackLink()

                Logger.log("verify user is back on projectList Page");
                
                projectOverviewPage
                    .hasTrustId(project.TRN)
                    .selectTaskListTab()
                
                taskListPage
                    .hasProjectStatus("Pre-opening")
                    .clickChangeProjectStatus();

                projectStatusPage
                    .preOpeningIsChecked()
                    .clickBackLink()

                Logger.log("verify user is back on tasklist page");
                
                taskListPage
                    .onTasklistTab()
                    .selectContactTab()
                
                contactsPage
                    .hasProjectStatus("Pre-opening")
                    .clickChangeProjectStatus();

                projectStatusPage
                    .preOpeningIsChecked()
                    .clickBackLink()
                
                Logger.log("verify user is back on contacts page");
                
                contactsPage
                    .onContactsTab()
                    .selectAboutTheProjectTab()

                Logger.log("change status to open");

                projectOverviewPage
                    .hasProjectStatus("Pre-opening")
                    .clickChangeProjectStatus()
                
                projectStatusPage
                    .preOpeningIsChecked()
                    .selectOpen()
                    .clickSaveAndContinue()

                projectOverviewPage
                    .hasProjectStatus("Open")
                    .hasTrustId(project.TRN)
                    .selectTaskListTab()

                Logger.log("change status to cancelled");

                taskListPage
                    .hasProjectStatus("Open")
                    .clickChangeProjectStatus();

                projectStatusPage
                    .openIsChecked()
                    .selectCancelled()
                    .clickSaveAndContinue()

                projectStatusCancelledPage                
                    .clickSaveAndContinue()
                    .errorForCancelledDate("Enter a date for the date the project was cancelled")
                    .addCancelledYear("1", "1", "error")
                    .clickSaveAndContinue()
                    .errorForCancelledDate("Enter a date in the correct format")
                    .addCancelledYear("1", "1", "1999")
                    .clickSaveAndContinue()
                    .errorForCancelledDate("Year must be between 2000 and 2050")
                    .clickSaveAndContinue()
                    .addCancelledYear("1", "1", "2051")
                    .errorForCancelledDate("Year must be between 2000 and 2050")
                    .addCancelledYear("1", "1", "2045")
                    .errorForReasonForCancellation("Enter the main reason for cancellation")
                    .errorForCancelledAsAResultOFNationalPipelineReview("Enter whether the project was cancelled as a result of the 2024/25 national review of pipeline projects")
                    .errorForNotesAboutTheCancellation("Enter the notes about the cancellation")
                    .selectGovernance()
                    .selectProjectCancelledAsAResultOfNationalPipelineReviewYes()
                    .withAddNotesAboutTheCancellation(dataGenerator.generateAlphaNumeric(501))
                    .clickSaveAndContinue()
                    .errorForNotesAboutTheCancellation("Notes about the cancellation must be 500 characters or less")
                    .withAddNotesAboutTheCancellation("Some notes")
                    .clickSaveAndContinue();
  
                Logger.log("user is sent back to tasklist");
                
                taskListPage
                    .onTasklistTab()
                    .hasProjectStatus("Cancelled")
                    .selectAboutTheProjectTab()
  
                Logger.log("cancelled date is shown on project overview");
              
                projectOverviewPage
                    .hasReasonForCancellation("Governance")
                    .hasProjectCancelledAsPartOfThePipelineReview("Yes")
                    .hasCommentaryForCancellation("Some notes")
                    .hasCancelledDate("1 January 2045")
                    .selectContactsTab()
  
                Logger.log("change status to closed");
                
                contactsPage
                    .hasProjectStatus("Cancelled")
                    .clickChangeProjectStatus();
    
                projectStatusPage
                    .cancelledIsChecked()
                    .selectClosed()
                    .clickSaveAndContinue()
                    .errorForClosedDate("Enter a date in the correct format")
                    .addClosedYear("1", "1", "error")
                    .clickSaveAndContinue()
                    .errorForClosedDate("Enter a date in the correct format")
                    .addClosedYear("1", "1", "1999")
                    .clickSaveAndContinue()
                    .errorForClosedDate("Year must be between 2000 and 2050")
                    .clickSaveAndContinue()
                    .addClosedYear("1", "1", "2051")
                    .errorForClosedDate("Year must be between 2000 and 2050")
                    .addClosedYear("1", "1", "2048")
                    .clickSaveAndContinue()
    
                Logger.log("user is sent back to contacts page");
                
                contactsPage
                    .onContactsTab()
                    .hasProjectStatus("Closed")
                    .selectAboutTheProjectTab();
    
                Logger.log("closed date is shown on project overview");
    
                projectOverviewPage
                    .hasClosedDate("1 January 2048")
    
                Logger.log("change status to withdrawn");
    
                projectOverviewPage
                    .clickChangeProjectStatus()
    
                projectStatusPage
                    .closedIsChecked()
                    .closedYearHasValue("1", "1", "2048")
                    .selectWithdrawn()
                    .clickSaveAndContinue()
                
                projectStatusWithdrawnPage                
                    .clickSaveAndContinue()
                    .errorForWithdrawnDate("Enter a date for the date the project was withdrawn")
                    .addWithdrawnYear("1", "1", "error")
                    .clickSaveAndContinue()
                    .errorForWithdrawnDate("Enter a date in the correct format")
                    .addWithdrawnYear("1", "1", "1999")
                    .clickSaveAndContinue()
                    .errorForWithdrawnDate("Year must be between 2000 and 2050")
                    .clickSaveAndContinue()
                    .addWithdrawnYear("1", "1", "2051")
                    .errorForWithdrawnDate("Year must be between 2000 and 2050")
                    .addWithdrawnYear("1", "1", "2045")
                    .errorForReasonForWithdrawal("Enter the main reason for withdrawal")
                    .errorForWithdrawnAsAResultOFNationalPipelineReview("Enter whether the project was withdrawn as a result of the 2024/25 national review of pipeline projects")
                    .errorForNotesAboutTheWithdrawal("Enter the notes about the withdrawal")
                    .selectGovernance()
                    .selectProjectWithdrawnAsAResultOfNationalPipelineReviewYes()
                    .withAddNotesAboutTheWithdrawal(dataGenerator.generateAlphaNumeric(501))
                    .clickSaveAndContinue()
                    .errorForNotesAboutTheWithdrawal("Notes about the withdrawal must be 500 characters or less")
                    .withAddNotesAboutTheWithdrawal("Some notes")
                    .clickSaveAndContinue();

            Logger.log("user is sent back to projects overview page");

            projectOverviewPage
                .hasReasonForWithdrawal("Governance")
                .hasProjectWithdrawnAsPartOfThePipelineReview("Yes")
                .hasCommentaryForWithdrawal("Some notes")
                .hasWithdrawnDate("1 January 2045")
        });
    });
});