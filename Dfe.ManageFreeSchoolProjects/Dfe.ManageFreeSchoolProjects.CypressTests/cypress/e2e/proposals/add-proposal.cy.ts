import { ProjectDetailsRequest } from 'cypress/api/domain';
import projectApi from 'cypress/api/projectApi';
import { RequestBuilder } from 'cypress/api/requestBuilder';
import { Logger } from 'cypress/common/logger';
import createProposalPage from 'cypress/pages/proposals/createProposalPage';
import summaryPage from 'cypress/pages/task-summary-base';

describe('Testing add new proposal journeys', () => {
    let project: ProjectDetailsRequest;
    const TestTrn = 'TR90111';

    before(() => {
        project = RequestBuilder.createProjectDetails();

        projectApi.post({
            projects: [project],
        });
    });

    beforeEach(() => {
        cy.login();

        cy.visit(`/projects/${project.projectId}/proposals`);
    });

    it('Create Academy trust (including Diocese academy trust) proposal', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('AcademyTrust').continue();

        Logger.log('Check trust validation');
        createProposalPage
            .titleHeadingIs('Search for a trust by TRN')
            .continue()
            .errorMessage('Enter the TRN')
            .enterTRN('POTATO')
            .continue()
            .errorMessage('The TRN must start with the letters TR, followed by at least 5 numbers')
            .enterTRN('TR00000000')
            .continue()
            .errorMessage('TRN (trust reference number) must be 7 characters or less')
            .enterTRN('TR99999')
            .continue()
            .errorMessage('Trust ID not found. Enter a different ID');

        cy.executeAccessibilityTests();

        Logger.log('Enter valid trust');
        createProposalPage.enterTRN(TestTrn).continue();

        Logger.log('Confirm trust validation');
        createProposalPage
            .titleHeadingIs('Confirm the trust')
            .continue()
            .errorMessage('Please select an option to confirm the trust');

        cy.executeAccessibilityTests();

        Logger.log('Selecting No returns to previous page');
        createProposalPage
            .confirmTrust('No')
            .continue()
            .titleHeadingIs('Search for a trust by TRN')
            .enterTRN(TestTrn)
            .continue();

        createProposalPage
            .titleHeadingIs('Confirm the trust')
            .back()
            .titleHeadingIs('Search for a trust by TRN')
            .enterTRN(TestTrn)
            .continue();

        createProposalPage
            .titleHeadingIs('Confirm the trust')
            .hasCorrectTrustName('The James Web School')
            .hasCorrectTrustType('SAT (single academy trust)')
            .confirmTrust('Yes')
            .continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Academy trust (including Diocese academy trust)')
            .HasChangeLink()
            .summaryShows('Trust reference number')
            .HasValue(TestTrn)
            .HasChangeLink()
            .summaryShows('Trust name')
            .HasValue('The James Web School')
            .HasChangeLink()
            .summaryShows('Trust type')
            .HasValue('SAT (single academy trust)')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create Diocese proposal', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('Diocese').continue();

        Logger.log('Name of the Diocese');
        createProposalPage
            .titleHeadingIs('What is the name of the Diocese?')
            .continue()
            .errorMessage('Enter the name of the Diocese')
            .enterDioceseName('Diocese of London')
            .continue();

        Logger.log('Faith of the Diocese');
        createProposalPage
            .titleHeadingIs('What is the faith of the Diocese?')
            .continue()
            .errorMessage('Select the faith of the diocese');

        createProposalPage.selectOption('Roman Catholic').continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Diocese')
            .HasChangeLink()
            .summaryShows('Name of Diocese')
            .HasValue('Diocese of London')
            .HasChangeLink()
            .summaryShows('Faith of Diocese')
            .HasValue('Roman Catholic')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create Another religious organisation proposal', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('AnotherReligiousOrganisation').continue();

        Logger.log('Name of the other religious organisation');
        createProposalPage
            .titleHeadingIs('What is the name of the other religious organisation?')
            .continue()
            .errorMessage('Enter the name of the other religious organisation')
            .enterOtherReligiousOrganisationName('Methodist mouvement of renaissance')
            .continue();

        Logger.log('Faith type of the other religious organisation');
        createProposalPage
            .titleHeadingIs('What is the faith type of the other religious organisation?')
            .continue()
            .errorMessage('Select the faith of the other religious organisation')
            .selectOption('Church of England')
            .continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Another religious organisation')
            .HasChangeLink()
            .summaryShows('Name of other religious organistation')
            .HasValue('Methodist mouvement of renaissance')
            .HasChangeLink()
            .summaryShows('Faith of other religious organisation')
            .HasValue('Church of England')
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create authority that published the specification proposal - Faith status = None', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('LocalAuthorityThatPushedSpecification').continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Local authority that published the specification')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create authority that published the specification proposal - Faith status <> None', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('LocalAuthorityThatPushedSpecification').continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('Designation').continue();

        cy.executeAccessibilityTests();

        Logger.log('Select Proposed faith type');
        createProposalPage
            .titleHeadingIs('What is the proposed faith type?')
            .selectOption('Church of England')
            .continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Local authority that published the specification')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('Designation')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .HasValue('Church of England')
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create Another local authority proposal', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('AnotherLocalAuthority').continue();

        cy.executeAccessibilityTests();

        Logger.log('Region is the other local authority');
        createProposalPage
            .titleHeadingIs('Which region is the other local authority in?')
            .continue()
            .errorMessage('Select the region')
            .selectOption('East of England')
            .continue();

        Logger.log('Other local authority');
        createProposalPage
            .titleHeadingIs('What is the other local authority?')
            .continue()
            .errorMessage('Select the local authority')
            .selectOption('Central Bedfordshire')
            .continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue('Another local authority')
            .HasChangeLink()
            .summaryShows('Name of another Local authority')
            .HasValue('Central Bedfordshire')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });

    it('Create Joint proposal between the local authority that published the specification and another local authority proposal', () => {
        createProposalPage.addProposal();

        Logger.log('Who is the proposer?');
        cy.executeAccessibilityTests();

        createProposalPage.continue().errorMessage('Select the proposer');

        cy.executeAccessibilityTests();

        createProposalPage.selectProposer('JointProposal').continue();

        cy.executeAccessibilityTests();

        Logger.log('Region is the other local authority');
        createProposalPage
            .titleHeadingIs('Which region is the other local authority in?')
            .continue()
            .errorMessage('Select the region')
            .selectOption('East of England')
            .continue();

        Logger.log('Other local authority');
        createProposalPage
            .titleHeadingIs('What is the other local authority?')
            .continue()
            .errorMessage('Select the local authority')
            .selectOption('Central Bedfordshire')
            .continue();

        Logger.log('Check proposed faith status');
        createProposalPage
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .continue()
            .errorMessage('Select the faith status');

        createProposalPage.selectOption('None').continue();

        Logger.log('Check answers');
        summaryPage
            .inOrder()
            .summaryShows('Type of proposer')
            .HasValue(
                'Joint proposal between the local authority that published the specification and another local authority'
            )
            .HasChangeLink()
            .summaryShows(
                'Name of another LA that has submitted a joint proposal with the LA that has published the specification'
            )
            .HasValue('Central Bedfordshire')
            .HasChangeLink()
            .summaryShows('Is the proposal for a school with a religious character or religious ethos')
            .HasValue('None')
            .HasChangeLink()
            .summaryShows('Proposed faith of new school')
            .IsEmpty()
            .HasChangeLink()
            .summaryShows('If other, which faith is it')
            .IsEmpty()
            .HasChangeLink();

        cy.executeAccessibilityTests();

        createProposalPage.clickCreateProposal();
    });
});
