import { ProjectDetailsRequest } from 'cypress/api/domain';
import projectApi from 'cypress/api/projectApi';
import { RequestBuilder } from 'cypress/api/requestBuilder';
import { Logger } from 'cypress/common/logger';
import createProposalPage from 'cypress/pages/proposals/createProposalPage';
import proposalsTable from 'cypress/pages/proposalsTable';

describe('Testing proposals page', () => {
    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/proposals`);
            });
    });

    it('should allow a user to navigate to Add a proposal screen', () => {
        createProposalPage.addProposal().titleHeadingIs('Who is the proposer?');

        cy.executeAccessibilityTests();
    });

    it('should create 2 proposals and verify the records in the poposals table', () => {
        Logger.log('Create a Diocese proposal');
        createProposalPage
            .addProposal()
            .selectProposer('Diocese')
            .continue()
            .titleHeadingIs('What is the name of the Diocese?')
            .enterDioceseName('Diocese of London')
            .continue()
            .titleHeadingIs('What is the faith of the Diocese?')
            .selectOption('Roman Catholic')
            .continue()
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .selectOption('None')
            .continue()
            .clickCreateProposal();

        Logger.log('Create a local authority proposal');
        createProposalPage
            .addProposal()
            .selectProposer('LocalAuthorityThatPushedSpecification')
            .continue()
            .titleHeadingIs('Is the proposal for a school with a religious designation or ethos?')
            .selectOption('Designation')
            .continue()
            .titleHeadingIs('What is the proposed faith type?')
            .selectOption('Church of England')
            .continue()
            .clickCreateProposal();

        Logger.log('Both proposals are listed');
        proposalsTable.hasRowCount(2);

        proposalsTable.getRowByProposer('Diocese').then((row) => {
            row.hasName('Diocese of London')
                .hasReligiousCharacterOrEthos('None')
                .hasStatus('Active')
                .hasViewLink('View');
        });

        proposalsTable.getRowByProposer('Local authority that published the specification').then((row) => {
            row.hasReligiousCharacterOrEthos('Designation')
                .hasProposedFaithOfNewSchool('Church of England')
                .hasStatus('Active')
                .hasViewLink('View');
        });

        cy.executeAccessibilityTests();
    });
});
