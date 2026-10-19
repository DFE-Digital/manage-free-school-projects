import { Logger } from 'cypress/common/logger';
import { ProjectRecordCreator } from 'cypress/constants/cypressConstants';
import dataGenerator from 'cypress/fixtures/dataGenerator';
import createProjectPage from 'cypress/pages/createProject/createProjectPage';
import homePage from 'cypress/pages/homePage';
import summaryPage from 'cypress/pages/task-summary-base';

describe('Smoke Test Create an individual Presumption project - (Up Until Check your answers Page)', () => {
    beforeEach(() => {
        cy.login({ role: ProjectRecordCreator });
        cy.visit('/');
    });

    it('Should create a presumption project, up until Check your answers page', { tags: ['smoke'] }, () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        const TestTrn = 'TR90111';

        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        createProjectPage.titleIs('What project do you want to create?');
        cy.executeAccessibilityTests();

        createProjectPage.selectOption('Presumption').continue();

        Logger.log('Enter Valid project ID');
        createProjectPage.titleIs('What is the temporary project ID?');
        cy.executeAccessibilityTests();

        createProjectPage.enterProjectId(temporaryProjectId).continue();

        Logger.log('Enter school name');
        createProjectPage.titleIs('What is the current free school name?');
        cy.executeAccessibilityTests();

        createProjectPage.enterSchoolName(schoolName).continue();

        createProjectPage.titleIs('Which region is the school in?');

        cy.executeAccessibilityTests();

        Logger.log('Select East of England');
        createProjectPage.selectOption('East of England').continue();

        createProjectPage.titleIs('What is the local authority?');

        cy.executeAccessibilityTests();

        Logger.log('Select Local authority');
        createProjectPage.selectOption('Luton').continue();

        Logger.log('Enter valid trust');
        createProjectPage.enterTRN(TestTrn).continue();

        Logger.log('Confirm the trust');
        createProjectPage.titleIs('Confirm the trust');
        cy.executeAccessibilityTests();

        createProjectPage
            .hasCorrectTrustName('The James Web School')
            .hasCorrectTrustType('SAT (single academy trust)')
            .selectOption('Yes')
            .continue();

        Logger.log('Selecting Mainstream school type');
        createProjectPage.selectOption('Mainstream').continue();

        Logger.log('Set class types');
        createProjectPage
            .setNurseryTo('Yes')
            .setSixthFormTo('Yes')
            .setAlternativeProvisionTo('Yes')
            .setSpecialEducationNeedsTo('No')
            .setResidentialOrBoarding('Yes')
            .continue();

        Logger.log('Selecting Secondary school phase');
        createProjectPage.titleIs('What is the school phase?');
        cy.executeAccessibilityTests();

        createProjectPage.selectOption('Secondary').continue();

        createProjectPage.enterAgeRangeFrom('2').enterAgeRangeTo('7').continue();

        Logger.log('Enter valid capacity');
        createProjectPage.titleIs('What is the capacity?');
        cy.executeAccessibilityTests();

        createProjectPage
            .enterNurseryCapacity('200')
            .enterReceptionToYear6('0')
            .enterYear7ToYear11('400')
            .enterYear12ToYear14('150')
            .continue();

        Logger.log('Select Designation');
        createProjectPage.selectOption('Designation').continue();

        cy.executeAccessibilityTests();

        Logger.log('Select Greek Orthodox');
        createProjectPage.selectOption('Greek Orthodox').continue();

        cy.executeAccessibilityTests();

        Logger.log('Enter valid provisional opening date');
        createProjectPage.setProvisionalOpeningDate('1', '10', '2035').continue();

        Logger.log('Set email');
        createProjectPage.enterProjectAssignedToEmail('test.person@education.gov.uk').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Project type')
            .HasValue('Presumption')
            .HasChangeLink()
            .summaryShows('Temporary project ID')
            .HasValue(temporaryProjectId)
            .HasChangeLink()
            .summaryShows('Current school name')
            .HasValue(schoolName)
            .HasChangeLink()
            .summaryShows('Region')
            .HasValue('East of England')
            .HasChangeLink()
            .summaryShows('Local authority')
            .HasValue('Luton')
            .HasChangeLink()
            .summaryShows('Trust')
            .HasValue('The James Web School')
            .HasChangeLink()
            .summaryShows('School type')
            .HasValue('Mainstream')
            .HasChangeLink()
            .summaryShows('Nursery')
            .HasValue('Yes')
            .HasChangeLink()
            .summaryShows('Sixth form')
            .HasValue('Yes')
            .HasChangeLink()
            .summaryShows('Residential or boarding')
            .HasValue('Yes')
            .HasChangeLink()
            .summaryShows('Alternative provision')
            .HasValue('Yes')
            .HasChangeLink()
            .summaryShows('Special educational needs')
            .HasValue('No')
            .HasChangeLink()
            .summaryShows('School phase')
            .HasValue('Secondary')
            .HasChangeLink()
            .summaryShows('Age range')
            .HasValue('2 to 7')
            .HasChangeLink()
            .summaryShows('Nursery capacity')
            .HasValue('200')
            .HasChangeLink()
            .summaryShows('Reception to year 6 capacity')
            .HasValue('0')
            .HasChangeLink()
            .summaryShows('Year 7 to year 11 capacity')
            .HasValue('400')
            .HasChangeLink()
            .summaryShows('Year 12 to year 14 capacity')
            .HasValue('150')
            .HasChangeLink()
            .summaryShows('Faith status')
            .HasValue('Designation')
            .HasChangeLink()
            .summaryShows('Faith type')
            .HasValue('Greek Orthodox')
            .HasChangeLink()
            .summaryShows('Provisional opening date')
            .HasValue('1 October 2035')
            .HasChangeLink()
            .summaryShows('Email')
            .HasValue('test.person@education.gov.uk')
            .HasChangeLink();

        Logger.log('Create Project');
        summaryPage.createProjectbutton('Create project');
        cy.executeAccessibilityTests();
    });
});
