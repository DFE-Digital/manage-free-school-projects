import { ProjectDetailsRequest } from 'cypress/api/domain';
import projectApi from 'cypress/api/projectApi';
import { RequestBuilder } from 'cypress/api/requestBuilder';
import { Logger } from 'cypress/common/logger';
import summaryPage from 'cypress/pages/task-summary-base';
import taskListPage from 'cypress/pages/taskListPage';
import externalExpertVisitEditPage from '../../../pages/tasks/After-opening/edit-external-expert-visit';

describe('Testing the External expert visit task', () => {
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

    it('Should be able to set external expert visit', () => {
        Logger.log('Select External expert visit');
        taskListPage.isTaskStatusIsNotStarted('CommissionedExternalExpert').selectExternalExpertVisitFromTaskList();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs('External expert visit')
            .inOrder()
            .summaryShows('Commissioned an external expert (EE) visit')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Date the EE visit took place')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Saved EE specification and report in Workplaces folder')
            .IsEmpty()
            .HasChangeLink()
            .isNotMarkedAsComplete();

        Logger.log('Go back to task list');
        summaryPage.clickBack();

        taskListPage.selectExternalExpertVisitFromTaskList();

        Logger.log('Change the commissioned an external expert (EE) visit row');
        summaryPage.clickChangeFor('Commissioned an external expert (EE) visit');

        externalExpertVisitEditPage.schoolNameIs(project.schoolName).titleIs('Edit External expert visit');

        cy.executeAccessibilityTests();

        externalExpertVisitEditPage
            .checkCommissionedExternalExpertVisit()
            .withVisitDate('21', '1', '2049')
            .checkSavedToWorkplaces()
            .clickContinue();

        Logger.log('Should update the task status');

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs('External expert visit')
            .inOrder()
            .summaryShows('Commissioned an external expert (EE) visit')
            .HasValue('Yes')
            .HasChangeLink()
            .summaryShows('Date the EE visit took place')
            .HasValue('21 January 2049')
            .HasChangeLink()
            .summaryShows('Saved EE specification and report in Workplaces folder')
            .HasValue('Yes')
            .HasChangeLink()
            .isNotMarkedAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress('CommissionedExternalExpert');

        taskListPage.selectExternalExpertVisitFromTaskList();

        Logger.log('Change the commissioned an external expert (EE) visit row');
        summaryPage.clickChangeFor('Commissioned an external expert (EE) visit');

        externalExpertVisitEditPage.schoolNameIs(project.schoolName).titleIs('Edit External expert visit');

        externalExpertVisitEditPage
            .withVisitDate('aq', '12', '2010')
            .clickContinue()
            .errorForVisitDate()
            .showsError('Day must be a number, like 12')
            .withVisitDate('21', '12', '1999')
            .clickContinue()
            .errorForVisitDate()
            .showsError('Year must be between 2000 and 2050')
            .withVisitDate('21', '04', '2040')
            .uncheckCommissionedExternalExpertVisit()
            .uncheckSavedToWorkplaces()
            .clickContinue();

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs('External expert visit')
            .inOrder()
            .summaryShows('Commissioned an external expert (EE) visit')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Date the EE visit took place')
            .HasValue('21 April 2040')
            .HasChangeLink()
            .summaryShows('Saved EE specification and report in Workplaces folder')
            .IsEmpty()
            .HasChangeLink()
            .MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted('CommissionedExternalExpert');
    });
});
