import { ProjectDetailsRequest } from 'cypress/api/domain';
import projectApi from 'cypress/api/projectApi';
import { RequestBuilder } from 'cypress/api/requestBuilder';
import { Logger } from 'cypress/common/logger';
import taskListPage from 'cypress/pages/taskListPage';
import schoolDetailsPage from 'cypress/pages/schoolDetailsPage';
import summaryPage from 'cypress/pages/task-summary-base';
import validationComponent from 'cypress/pages/validationComponent';

describe('Testing the school task for a new school project', () => {
    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetailsNewSchool();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it('Should show the establishment type and allow it to be left unanswered', () => {
        taskListPage.selectSchoolFromTaskList();

        Logger.log('Establishment type is shown for a new school project');
        summaryPage
            .titleIs('School')
            .inOrder()
            .summaryShows('Current school name')
            .HasValue(project.schoolName)
            .summaryShows('School type')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Establishment type')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('School phase')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        Logger.log('Establishment type is not mandatory');
        summaryPage.clickChangeFor('Establishment type');

        schoolDetailsPage
            .titleIs('Edit School')
            .hasEstablishmentType()
            .withSchoolType('Mainstream')
            .withSchoolPhase('AllThrough')
            .withAgeRange('11', '16')
            .withFormsOfEntry('3')
            .withGender('Mixed')
            .withNursery('Yes')
            .withSixthForm('No')
            .withResidentialOrBoarding('Yes')
            .withAlternativeProvision('No')
            .withSpecialEducationNeeds('No')
            .withFaithStatus('None')
            .clickContinue();

        summaryPage
            .titleIs('School')
            .inOrder()
            .summaryShows('Current school name')
            .summaryShows('School type')
            .HasValue('Mainstream')
            .summaryShows('Establishment type')
            .IsEmpty()
            .summaryShows('School phase')
            .HasValue('All-through');

        Logger.log('Establishment type is shown once answered');
        summaryPage.clickChangeFor('Establishment type');

        schoolDetailsPage.withEstablishmentType('AcademySponsorLed').clickContinue();

        summaryPage
            .inOrder()
            .summaryShows('Current school name')
            .summaryShows('School type')
            .HasValue('Mainstream')
            .summaryShows('Establishment type')
            .HasValue('Academy sponsor led')
            .HasChangeLink();

        Logger.log('Establishment type is kept when validation fails');
        summaryPage.clickChangeFor('Establishment type');

        schoolDetailsPage
            .establishmentTypeIsSelected('AcademySponsorLed')
            .withEstablishmentType('CommunitySchool')
            .withFormsOfEntryExceedingLimit()
            .clickContinue();

        validationComponent.hasValidationError('Forms of entry must be 100 characters or less');

        schoolDetailsPage
            .hasEstablishmentType()
            .establishmentTypeIsSelected('CommunitySchool')
            .withFormsOfEntry('3')
            .clickContinue();

        summaryPage.SummaryHasValue('Establishment type', 'Community school');
    });
});
