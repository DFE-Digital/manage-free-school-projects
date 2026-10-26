import { Logger } from 'cypress/common/logger';
import { ProjectRecordCreator } from 'cypress/constants/cypressConstants';
import dataGenerator from 'cypress/fixtures/dataGenerator';
import createProjectPage from 'cypress/pages/createProject/createProjectPage';
import homePage from 'cypress/pages/homePage';
import summaryPage from 'cypress/pages/task-summary-base';

describe('Testing the new school route project creation journey', () => {
    beforeEach(() => {
            cy.login({ role: ProjectRecordCreator });
            cy.visit('/');
        });

    it('should create a new school project successfully - school type = Mainstream AND Faith status = none', () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        
        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log('Check method required');
        cy.executeAccessibilityTests();

        createProjectPage.continue().errorMessage('Select what project you want to create');

        cy.executeAccessibilityTests();

        Logger.log('Selecting New School option and move forward');
        createProjectPage.selectOption('New School').continue();

        Logger.log('Check project id validation');
        createProjectPage
            .titleIs('What is the temporary project ID?')
            .continue()
            .errorMessage('Enter the temporary project ID')
            .enterProjectId('T-00008')
            .continue()
            .errorMessage('Temporary project ID must only include numbers and letters')
            .enterProjectId('T 00009')
            .continue()
            .errorMessage('Temporary project ID must not include spaces')
            .enterProjectId(dataGenerator.generateAlphaNumeric(26))
            .continue()
            .errorMessage('Temporary project ID must be 25 characters or less');

        cy.executeAccessibilityTests();

        Logger.log('Enter Valid project ID');
        createProjectPage.enterProjectId(temporaryProjectId).continue();

        Logger.log('Check school name validation');
        createProjectPage
            .titleIs('What is the current working name of the new school?')
            .continue()
            .errorMessage('Enter the new school name')
            .enterSchoolName('Invalid#')
            .continue()
            .errorMessage("School name must not include special characters other than , ( ) '")
            .enterSchoolName(dataGenerator.generateAlphaNumeric(101))
            .continue()
            .errorMessage('School name must be 100 characters or less');

        cy.executeAccessibilityTests();
        Logger.log('Enter school name');

        createProjectPage.enterSchoolName(schoolName).continue();

        createProjectPage.titleIs('Which region is the school in?').continue().errorMessage('Select the region');

        cy.executeAccessibilityTests();

        Logger.log('Select East of England');
        createProjectPage.selectOption('East of England').continue();

        Logger.log('Check local authority required');
        createProjectPage
            .titleIs('What is the local authority?')
            .continue()
            .errorMessage('Select the local authority');

        cy.executeAccessibilityTests();

        Logger.log('Select Local authority');
        createProjectPage.selectOption('Luton').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check school type validation');
        createProjectPage.titleIs('What is the school type?').continue().errorMessage('Select the school type');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Mainstream school type');
        createProjectPage.selectOption('Mainstream').continue(); // There is a UI conditional rendering based on (selection = Mainstream => two additional fields are rendered) on screen with title: What is the capacity?

        Logger.log('Check class types validation');
        createProjectPage
            .titleIs('Will it have any of these?')
            .continue()
            .errorMessage('Select yes if it will have a nursery')
            .errorMessage('Select yes if it will have a sixth form')
            .errorMessage('Select yes if it will have alternative provision')
            .errorMessage('Select yes if it will have special educational needs provision')
            .errorMessage('Select yes if it will have residential or boarding provision');

        cy.executeAccessibilityTests();

        Logger.log('Set class types');
        createProjectPage
            .setNurseryTo('Yes')
            .setSixthFormTo('Yes')
            .setResidentialOrBoarding('Yes')
            .setAlternativeProvisionTo('Yes')
            .setSpecialEducationNeedsTo('No')
            .setResidentialOrBoarding('Yes')
            .continue();

        Logger.log('Check school phase validation');
        createProjectPage.titleIs('What is the school phase?').continue().errorMessage('Select the school phase');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Secondary school phase');
        createProjectPage.selectOption('Secondary').continue();

        Logger.log('Check age range validation-limited');
        createProjectPage
            .titleIs('What is the age range?')
            .enterAgeRangeFrom('2')
            .enterAgeRangeTo('2')
            .continue()
            .errorMessage("'To' age range must be 5 or above");

        cy.executeAccessibilityTests();

        //re-enter incorrect age-range values to check validation
        createProjectPage
            .enterAgeRangeFrom('12')
            .enterAgeRangeTo('6')
            .continue()
            .errorMessage("'From' age range must be less than 'to' age range");

        //enter valid age-range values
        createProjectPage.enterAgeRangeFrom('2').enterAgeRangeTo('7').continue();

        Logger.log('Check capacity validation');
        createProjectPage
            .titleIs('What is the capacity?')
            .continue()
            .errorMessage('Enter the nursery capacity')
            .errorMessage('Enter the reception to year 6 capacity')
            .errorMessage('Enter the year 7 to year 11 capacity')
            .errorMessage('Enter the year 12 to year 14 capacity')
            // Extra options because school type = Mainstream
            .errorMessage('Enter the AP resources provision')
            .errorMessage('Enter the SEN resourced provision / SEN unit')
            .enterNurseryCapacity('A')
            .enterReceptionToYear6('A')
            .enterYear7ToYear11('A')
            .enterYear12ToYear14('A')
            .enterApResourceProvision('A')
            .enterSenResourcedProvisionSenUnit('A')
            .continue()
            .errorMessage('Nursery capacity must be a number, like 30')
            .errorMessage('Reception to year 6 capacity must be a number, like 30')
            .errorMessage('Year 7 to year 11 capacity must be a number, like 30')
            .errorMessage('Year 12 to year 14 capacity must be a number, like 30')
            .errorMessage('AP resources provision must be a number, like 30')
            .errorMessage('SEN resourced provision / SEN unit must be a number, like 30')
            .enterNurseryCapacity('-1')
            .enterReceptionToYear6('-1')
            .enterYear7ToYear11('-1')
            .enterYear12ToYear14('-1')
            .enterApResourceProvision('-1')
            .enterSenResourcedProvisionSenUnit('-1')
            .continue()
            .errorMessage('Nursery capacity must be between 0 and 9999')
            .errorMessage('Reception to year 6 capacity must be between 0 and 9999')
            .errorMessage('Year 7 to year 11 capacity must be between 0 and 9999')
            .errorMessage('Year 12 to year 14 capacity must be between 0 and 9999')
            .errorMessage('AP resources provision must be between 0 and 9999')
            .errorMessage('SEN resourced provision / SEN unit must be between 0 and 9999');

        cy.executeAccessibilityTests();

        Logger.log('Enter valid capacity');
        createProjectPage
            .enterNurseryCapacity('200')
            .enterReceptionToYear6('0')
            .enterYear7ToYear11('400')
            .enterYear12ToYear14('150')
            .enterApResourceProvision('500')
            .enterSenResourcedProvisionSenUnit('600')
            .continue();

        Logger.log('Check faith status validation');
        createProjectPage.titleIs('What is the faith status?').continue().errorMessage('Select the faith status');

        cy.executeAccessibilityTests();

        Logger.log('Select Faith status');
        createProjectPage.selectOption('None').continue(); // There is a UI conditional rendering based on this selection. It skips Faith type screen when 'None' is selected.

        cy.executeAccessibilityTests();

        Logger.log('Check project assigned to validation');
        createProjectPage
            .titleIs('Who do you want to assign this project to?')
            .continue()
            .errorMessage('Enter the email address')
            .enterProjectAssignedToEmail('test.person@edunation.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk')
            .enterProjectAssignedToEmail('@education.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk');

        cy.executeAccessibilityTests();

        Logger.log('Set email');
        createProjectPage.enterProjectAssignedToEmail('test.person@education.gov.uk').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Project type')
            .HasValue('New School')
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
            .summaryShows('AP resources provision')
            .HasValue('500')
            .HasChangeLink()
            .summaryShows('SEN resourced provision / SEN unit')
            .HasValue('600')
            .HasChangeLink()
            .summaryShows('Faith status')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Faith type')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Email')
            .HasValue('test.person@education.gov.uk')
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProjectPage.clickCreateProject();

        Logger.log('Confirm created');
        createProjectPage
            .hasConfirmedProjectId(temporaryProjectId)
            .hasConfirmedEmailMessage('We have sent a notification email to test.person@education.gov.uk.');

        cy.executeAccessibilityTests();
    });

    it('should create a new school project successfully - school type = Mainstream AND Faith status <> none', () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        
        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log('Check method required');
        cy.executeAccessibilityTests();

        createProjectPage.continue().errorMessage('Select what project you want to create');

        cy.executeAccessibilityTests();

        Logger.log('Selecting New School option and move forward');
        createProjectPage.selectOption('New School').continue();

        Logger.log('Check project id validation');
        createProjectPage
            .titleIs('What is the temporary project ID?')
            .continue()
            .errorMessage('Enter the temporary project ID')
            .enterProjectId('T-00008')
            .continue()
            .errorMessage('Temporary project ID must only include numbers and letters')
            .enterProjectId('T 00009')
            .continue()
            .errorMessage('Temporary project ID must not include spaces')
            .enterProjectId(dataGenerator.generateAlphaNumeric(26))
            .continue()
            .errorMessage('Temporary project ID must be 25 characters or less');

        cy.executeAccessibilityTests();

        Logger.log('Enter Valid project ID');
        createProjectPage.enterProjectId(temporaryProjectId).continue();

        Logger.log('Check school name validation');
        createProjectPage
            .titleIs('What is the current working name of the new school?')
            .continue()
            .errorMessage('Enter the new school name')
            .enterSchoolName('Invalid#')
            .continue()
            .errorMessage("School name must not include special characters other than , ( ) '")
            .enterSchoolName(dataGenerator.generateAlphaNumeric(101))
            .continue()
            .errorMessage('School name must be 100 characters or less');

        cy.executeAccessibilityTests();
        Logger.log('Enter school name');

        createProjectPage.enterSchoolName(schoolName).continue();

        createProjectPage.titleIs('Which region is the school in?').continue().errorMessage('Select the region');

        cy.executeAccessibilityTests();

        Logger.log('Select East of England');
        createProjectPage.selectOption('East of England').continue();

        Logger.log('Check local authority required');
        createProjectPage
            .titleIs('What is the local authority?')
            .continue()
            .errorMessage('Select the local authority');

        cy.executeAccessibilityTests();

        Logger.log('Select Local authority');
        createProjectPage.selectOption('Luton').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check school type validation');
        createProjectPage.titleIs('What is the school type?').continue().errorMessage('Select the school type');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Mainstream school type');
        createProjectPage.selectOption('Mainstream').continue(); // // There is a UI conditional rendering based on (selection = Mainstream => two additional fields are rendered) on screen with title: What is the capacity?

        Logger.log('Check class types validation');
        createProjectPage
            .titleIs('Will it have any of these?')
            .continue()
            .errorMessage('Select yes if it will have a nursery')
            .errorMessage('Select yes if it will have a sixth form')
            .errorMessage('Select yes if it will have alternative provision')
            .errorMessage('Select yes if it will have special educational needs provision')
            .errorMessage('Select yes if it will have residential or boarding provision');

        cy.executeAccessibilityTests();

        Logger.log('Set class types');
        createProjectPage
            .setNurseryTo('Yes')
            .setSixthFormTo('Yes')
            .setResidentialOrBoarding('Yes')
            .setAlternativeProvisionTo('Yes')
            .setSpecialEducationNeedsTo('No')
            .setResidentialOrBoarding('Yes')
            .continue();

        Logger.log('Check school phase validation');
        createProjectPage.titleIs('What is the school phase?').continue().errorMessage('Select the school phase');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Secondary school phase');
        createProjectPage.selectOption('Secondary').continue();

        Logger.log('Check age range validation-limited');
        createProjectPage
            .titleIs('What is the age range?')
            .enterAgeRangeFrom('2')
            .enterAgeRangeTo('2')
            .continue()
            .errorMessage("'To' age range must be 5 or above");

        cy.executeAccessibilityTests();

        //re-enter incorrect age-range values to check validation
        createProjectPage
            .enterAgeRangeFrom('12')
            .enterAgeRangeTo('6')
            .continue()
            .errorMessage("'From' age range must be less than 'to' age range");

        //enter valid age-range values
        createProjectPage.enterAgeRangeFrom('2').enterAgeRangeTo('7').continue();

        Logger.log('Check capacity validation');
        createProjectPage
            .titleIs('What is the capacity?')
            .continue()
            .errorMessage('Enter the nursery capacity')
            .errorMessage('Enter the reception to year 6 capacity')
            .errorMessage('Enter the year 7 to year 11 capacity')
            .errorMessage('Enter the year 12 to year 14 capacity')
            // Extra options because school type = Mainstream
            .errorMessage('Enter the AP resources provision')
            .errorMessage('Enter the SEN resourced provision / SEN unit')
            .enterNurseryCapacity('A')
            .enterReceptionToYear6('A')
            .enterYear7ToYear11('A')
            .enterYear12ToYear14('A')
            .enterApResourceProvision('A')
            .enterSenResourcedProvisionSenUnit('A')
            .continue()
            .errorMessage('Nursery capacity must be a number, like 30')
            .errorMessage('Reception to year 6 capacity must be a number, like 30')
            .errorMessage('Year 7 to year 11 capacity must be a number, like 30')
            .errorMessage('Year 12 to year 14 capacity must be a number, like 30')
            .errorMessage('AP resources provision must be a number, like 30')
            .errorMessage('SEN resourced provision / SEN unit must be a number, like 30')
            .enterNurseryCapacity('-1')
            .enterReceptionToYear6('-1')
            .enterYear7ToYear11('-1')
            .enterYear12ToYear14('-1')
            .enterApResourceProvision('-1')
            .enterSenResourcedProvisionSenUnit('-1')
            .continue()
            .errorMessage('Nursery capacity must be between 0 and 9999')
            .errorMessage('Reception to year 6 capacity must be between 0 and 9999')
            .errorMessage('Year 7 to year 11 capacity must be between 0 and 9999')
            .errorMessage('Year 12 to year 14 capacity must be between 0 and 9999')
            .errorMessage('AP resources provision must be between 0 and 9999')
            .errorMessage('SEN resourced provision / SEN unit must be between 0 and 9999');

        cy.executeAccessibilityTests();

        Logger.log('Enter valid capacity');
        createProjectPage
            .enterNurseryCapacity('200')
            .enterReceptionToYear6('0')
            .enterYear7ToYear11('400')
            .enterYear12ToYear14('150')
            .enterApResourceProvision('500')
            .enterSenResourcedProvisionSenUnit('600')
            .continue();

        Logger.log('Check faith status validation');
        createProjectPage.titleIs('What is the faith status?').continue().errorMessage('Select the faith status');

        cy.executeAccessibilityTests();

        Logger.log('Select Faith status');
        createProjectPage.selectOption('Designation').continue(); // There is a UI conditional rendering based on this selection. It skips Faith type screen when 'None' is selected.

        cy.executeAccessibilityTests();

        Logger.log('Select Faith type');
        createProjectPage.titleIs('What is the faith type?').selectOption('Greek Orthodox').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check project assigned to validation');
        createProjectPage
            .titleIs('Who do you want to assign this project to?')
            .continue()
            .errorMessage('Enter the email address')
            .enterProjectAssignedToEmail('test.person@edunation.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk')
            .enterProjectAssignedToEmail('@education.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk');

        cy.executeAccessibilityTests();

        Logger.log('Set email');
        createProjectPage.enterProjectAssignedToEmail('test.person@education.gov.uk').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Project type')
            .HasValue('New School')
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
            .summaryShows('AP resources provision')
            .HasValue('500')
            .HasChangeLink()
            .summaryShows('SEN resourced provision / SEN unit')
            .HasValue('600')
            .HasChangeLink()
            .summaryShows('Faith status')
            .HasValue('Designation')
            .HasChangeLink()
            .summaryShows('Faith type')
            .HasValue('Greek Orthodox')
            .HasChangeLink()
            .summaryShows('Email')
            .HasValue('test.person@education.gov.uk')
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProjectPage.clickCreateProject();

        Logger.log('Confirm created');
        createProjectPage
            .hasConfirmedProjectId(temporaryProjectId)
            .hasConfirmedEmailMessage('We have sent a notification email to test.person@education.gov.uk.');

        cy.executeAccessibilityTests();
    });

    it('should create a new school project successfully - school type <> Mainstream AND Faith status = none', () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        
        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log('Check method required');
        cy.executeAccessibilityTests();

        createProjectPage.continue().errorMessage('Select what project you want to create');

        cy.executeAccessibilityTests();

        Logger.log('Selecting New School option and move forward');
        createProjectPage.selectOption('New School').continue();

        Logger.log('Check project id validation');
        createProjectPage
            .titleIs('What is the temporary project ID?')
            .continue()
            .errorMessage('Enter the temporary project ID')
            .enterProjectId('T-00008')
            .continue()
            .errorMessage('Temporary project ID must only include numbers and letters')
            .enterProjectId('T 00009')
            .continue()
            .errorMessage('Temporary project ID must not include spaces')
            .enterProjectId(dataGenerator.generateAlphaNumeric(26))
            .continue()
            .errorMessage('Temporary project ID must be 25 characters or less');

        cy.executeAccessibilityTests();

        Logger.log('Enter Valid project ID');
        createProjectPage.enterProjectId(temporaryProjectId).continue();

        Logger.log('Check school name validation');
        createProjectPage
            .titleIs('What is the current working name of the new school?')
            .continue()
            .errorMessage('Enter the new school name')
            .enterSchoolName('Invalid#')
            .continue()
            .errorMessage("School name must not include special characters other than , ( ) '")
            .enterSchoolName(dataGenerator.generateAlphaNumeric(101))
            .continue()
            .errorMessage('School name must be 100 characters or less');

        cy.executeAccessibilityTests();
        Logger.log('Enter school name');

        createProjectPage.enterSchoolName(schoolName).continue();

        createProjectPage.titleIs('Which region is the school in?').continue().errorMessage('Select the region');

        cy.executeAccessibilityTests();

        Logger.log('Select East of England');
        createProjectPage.selectOption('East of England').continue();

        Logger.log('Check local authority required');
        createProjectPage
            .titleIs('What is the local authority?')
            .continue()
            .errorMessage('Select the local authority');

        cy.executeAccessibilityTests();

        Logger.log('Select Local authority');
        createProjectPage.selectOption('Luton').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check school type validation');
        createProjectPage.titleIs('What is the school type?').continue().errorMessage('Select the school type');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Mainstream school type');
        createProjectPage.selectOption('Alternative provision / PRU').continue(); // There is a UI conditional rendering based on (selection = Mainstream => two additional fields are rendered) on screen with title: What is the capacity?

        Logger.log('Check class types validation');
        createProjectPage
            .titleIs('Will it have any of these?')
            .continue()
            .errorMessage('Select yes if it will have a nursery')
            .errorMessage('Select yes if it will have a sixth form')
            .errorMessage('Select yes if it will have residential or boarding provision');

        cy.executeAccessibilityTests();

        Logger.log('Set class types');
        createProjectPage
            .setNurseryTo('Yes')
            .setSixthFormTo('Yes')
            .setResidentialOrBoarding('Yes')
            .continue();

        Logger.log('Check school phase validation');
        createProjectPage.titleIs('What is the school phase?').continue().errorMessage('Select the school phase');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Secondary school phase');
        createProjectPage.selectOption('Secondary').continue();

        Logger.log('Check age range validation-limited');
        createProjectPage
            .titleIs('What is the age range?')
            .enterAgeRangeFrom('2')
            .enterAgeRangeTo('2')
            .continue()
            .errorMessage("'To' age range must be 5 or above");

        cy.executeAccessibilityTests();

        //re-enter incorrect age-range values to check validation
        createProjectPage
            .enterAgeRangeFrom('12')
            .enterAgeRangeTo('6')
            .continue()
            .errorMessage("'From' age range must be less than 'to' age range");

        //enter valid age-range values
        createProjectPage.enterAgeRangeFrom('2').enterAgeRangeTo('7').continue();

        Logger.log('Check capacity validation');
        createProjectPage
            .titleIs('What is the capacity?')
            .continue()
            .errorMessage('Enter the nursery capacity')
            .errorMessage('Enter the reception to year 6 capacity')
            .errorMessage('Enter the year 7 to year 11 capacity')
            .errorMessage('Enter the year 12 to year 14 capacity')
            .enterNurseryCapacity('A')
            .enterReceptionToYear6('A')
            .enterYear7ToYear11('A')
            .enterYear12ToYear14('A')
            .continue()
            .errorMessage('Nursery capacity must be a number, like 30')
            .errorMessage('Reception to year 6 capacity must be a number, like 30')
            .errorMessage('Year 7 to year 11 capacity must be a number, like 30')
            .errorMessage('Year 12 to year 14 capacity must be a number, like 30')
            .enterNurseryCapacity('-1')
            .enterReceptionToYear6('-1')
            .enterYear7ToYear11('-1')
            .enterYear12ToYear14('-1')
            .continue()
            .errorMessage('Nursery capacity must be between 0 and 9999')
            .errorMessage('Reception to year 6 capacity must be between 0 and 9999')
            .errorMessage('Year 7 to year 11 capacity must be between 0 and 9999')
            .errorMessage('Year 12 to year 14 capacity must be between 0 and 9999');

        cy.executeAccessibilityTests();

        Logger.log('Enter valid capacity');
        createProjectPage
            .enterNurseryCapacity('200')
            .enterReceptionToYear6('0')
            .enterYear7ToYear11('400')
            .enterYear12ToYear14('150')
            .continue();

        Logger.log('Check faith status validation');
        createProjectPage.titleIs('What is the faith status?').continue().errorMessage('Select the faith status');

        cy.executeAccessibilityTests();

        Logger.log('Select Faith status');
        createProjectPage.selectOption('None').continue(); // There is a UI conditional rendering based on this selection. It skips Faith type screen when 'None' is selected.

        cy.executeAccessibilityTests();

        Logger.log('Check project assigned to validation');
        createProjectPage
            .titleIs('Who do you want to assign this project to?')
            .continue()
            .errorMessage('Enter the email address')
            .enterProjectAssignedToEmail('test.person@edunation.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk')
            .enterProjectAssignedToEmail('@education.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk');

        cy.executeAccessibilityTests();

        Logger.log('Set email');
        createProjectPage.enterProjectAssignedToEmail('test.person@education.gov.uk').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Project type')
            .HasValue('New School')
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
            .summaryShows('School type')
            .HasValue('Alternative provision / PRU')
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
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Faith type')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Email')
            .HasValue('test.person@education.gov.uk')
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProjectPage.clickCreateProject();

        Logger.log('Confirm created');
        createProjectPage
            .hasConfirmedProjectId(temporaryProjectId)
            .hasConfirmedEmailMessage('We have sent a notification email to test.person@education.gov.uk.');

        cy.executeAccessibilityTests();
    });

    it('should create a new school project successfully - school type <> Mainstream AND Faith status <> none', () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        
        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log('Check method required');
        cy.executeAccessibilityTests();

        createProjectPage.continue().errorMessage('Select what project you want to create');

        cy.executeAccessibilityTests();

        Logger.log('Selecting New School option and move forward');
        createProjectPage.selectOption('New School').continue();

        Logger.log('Check project id validation');
        createProjectPage
            .titleIs('What is the temporary project ID?')
            .continue()
            .errorMessage('Enter the temporary project ID')
            .enterProjectId('T-00008')
            .continue()
            .errorMessage('Temporary project ID must only include numbers and letters')
            .enterProjectId('T 00009')
            .continue()
            .errorMessage('Temporary project ID must not include spaces')
            .enterProjectId(dataGenerator.generateAlphaNumeric(26))
            .continue()
            .errorMessage('Temporary project ID must be 25 characters or less');

        cy.executeAccessibilityTests();

        Logger.log('Enter Valid project ID');
        createProjectPage.enterProjectId(temporaryProjectId).continue();

        Logger.log('Check school name validation');
        createProjectPage
            .titleIs('What is the current working name of the new school?')
            .continue()
            .errorMessage('Enter the new school name')
            .enterSchoolName('Invalid#')
            .continue()
            .errorMessage("School name must not include special characters other than , ( ) '")
            .enterSchoolName(dataGenerator.generateAlphaNumeric(101))
            .continue()
            .errorMessage('School name must be 100 characters or less');

        cy.executeAccessibilityTests();
        Logger.log('Enter school name');

        createProjectPage.enterSchoolName(schoolName).continue();

        createProjectPage.titleIs('Which region is the school in?').continue().errorMessage('Select the region');

        cy.executeAccessibilityTests();

        Logger.log('Select East of England');
        createProjectPage.selectOption('East of England').continue();

        Logger.log('Check local authority required');
        createProjectPage
            .titleIs('What is the local authority?')
            .continue()
            .errorMessage('Select the local authority');

        cy.executeAccessibilityTests();

        Logger.log('Select Local authority');
        createProjectPage.selectOption('Luton').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check school type validation');
        createProjectPage.titleIs('What is the school type?').continue().errorMessage('Select the school type');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Mainstream school type');
        createProjectPage.selectOption('Alternative provision / PRU').continue(); // There is a UI conditional rendering based on (selection = Mainstream => two additional fields are rendered) on screen with title: What is the capacity?

        Logger.log('Check class types validation');
        createProjectPage
            .titleIs('Will it have any of these?')
            .continue()
            .errorMessage('Select yes if it will have a nursery')
            .errorMessage('Select yes if it will have a sixth form')
            .errorMessage('Select yes if it will have residential or boarding provision');

        cy.executeAccessibilityTests();

        Logger.log('Set class types');
        createProjectPage
            .setNurseryTo('Yes')
            .setSixthFormTo('Yes')
            .setResidentialOrBoarding('Yes')
            .continue();

        Logger.log('Check school phase validation');
        createProjectPage.titleIs('What is the school phase?').continue().errorMessage('Select the school phase');

        cy.executeAccessibilityTests();

        Logger.log('Selecting Secondary school phase');
        createProjectPage.selectOption('Secondary').continue();

        Logger.log('Check age range validation-limited');
        createProjectPage
            .titleIs('What is the age range?')
            .enterAgeRangeFrom('2')
            .enterAgeRangeTo('2')
            .continue()
            .errorMessage("'To' age range must be 5 or above");

        cy.executeAccessibilityTests();

        //re-enter incorrect age-range values to check validation
        createProjectPage
            .enterAgeRangeFrom('12')
            .enterAgeRangeTo('6')
            .continue()
            .errorMessage("'From' age range must be less than 'to' age range");

        //enter valid age-range values
        createProjectPage.enterAgeRangeFrom('2').enterAgeRangeTo('7').continue();

        Logger.log('Check capacity validation');
        createProjectPage
            .titleIs('What is the capacity?')
            .continue()
            .errorMessage('Enter the nursery capacity')
            .errorMessage('Enter the reception to year 6 capacity')
            .errorMessage('Enter the year 7 to year 11 capacity')
            .errorMessage('Enter the year 12 to year 14 capacity')
            .enterNurseryCapacity('A')
            .enterReceptionToYear6('A')
            .enterYear7ToYear11('A')
            .enterYear12ToYear14('A')
            .continue()
            .errorMessage('Nursery capacity must be a number, like 30')
            .errorMessage('Reception to year 6 capacity must be a number, like 30')
            .errorMessage('Year 7 to year 11 capacity must be a number, like 30')
            .errorMessage('Year 12 to year 14 capacity must be a number, like 30')
            .enterNurseryCapacity('-1')
            .enterReceptionToYear6('-1')
            .enterYear7ToYear11('-1')
            .enterYear12ToYear14('-1')
            .continue()
            .errorMessage('Nursery capacity must be between 0 and 9999')
            .errorMessage('Reception to year 6 capacity must be between 0 and 9999')
            .errorMessage('Year 7 to year 11 capacity must be between 0 and 9999')
            .errorMessage('Year 12 to year 14 capacity must be between 0 and 9999');

        cy.executeAccessibilityTests();

        Logger.log('Enter valid capacity');
        createProjectPage
            .enterNurseryCapacity('200')
            .enterReceptionToYear6('0')
            .enterYear7ToYear11('400')
            .enterYear12ToYear14('150')
            .continue();

        Logger.log('Check faith status validation');
        createProjectPage.titleIs('What is the faith status?').continue().errorMessage('Select the faith status');

        cy.executeAccessibilityTests();

        Logger.log('Select Faith status');
        createProjectPage.selectOption('Designation').continue(); // There is a UI conditional rendering based on this selection. It skips Faith type screen when 'None' is selected.

        cy.executeAccessibilityTests();

        Logger.log('Select Faith type');
        createProjectPage.titleIs('What is the faith type?').selectOption('Greek Orthodox').continue();

        cy.executeAccessibilityTests();

        Logger.log('Check project assigned to validation');
        createProjectPage
            .titleIs('Who do you want to assign this project to?')
            .continue()
            .errorMessage('Enter the email address')
            .enterProjectAssignedToEmail('test.person@edunation.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk')
            .enterProjectAssignedToEmail('@education.gov.uk')
            .continue()
            .errorMessage('Email address must be in the format firstname.surname@education.gov.uk');

        cy.executeAccessibilityTests();

        Logger.log('Set email');
        createProjectPage.enterProjectAssignedToEmail('test.person@education.gov.uk').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Project type')
            .HasValue('New School')
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
            .summaryShows('School type')
            .HasValue('Alternative provision / PRU')
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
            .summaryShows('Email')
            .HasValue('test.person@education.gov.uk')
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProjectPage.clickCreateProject();

        Logger.log('Confirm created');
        createProjectPage
            .hasConfirmedProjectId(temporaryProjectId)
            .hasConfirmedEmailMessage('We have sent a notification email to test.person@education.gov.uk.');

        cy.executeAccessibilityTests();
    });
});