import { Logger } from 'cypress/common/logger';
describe('Navigate through footer links', () => {
    beforeEach(() => {
        cy.login();
        cy.visit('/');
    });

    it('user should be able to navigate to the footer links', () => {
        cy.get('.govuk-footer').should('be.visible');

        Logger.log('Get help footer links');
        cy.get('h2').should('contain', 'Get help');
        cy.contains('a', 'Get help with a Regions Group system').should('be.visible');

        Logger.log('Get feedback footer links');
        cy.get('h2').should('contain', 'Give feedback');
        cy.contains('a', 'Give feedback about a Regions Group system (opens in a new tab)').should('be.visible');
        cy.contains('a', 'Suggest a change to a Regions Group system (opens in a new tab)').should('be.visible');

        Logger.log('Get accessibility and cookies link');
        cy.contains('a', 'Accessibility statement')
            .should('be.visible')
            .should('have.attr', 'href', 'https://accessibility-statements.education.gov.uk/s/32')
            .should('have.attr', 'target', '_blank');

        cy.contains('a', 'Cookies').should('be.visible').click();
        cy.url().then((href) => {
            expect(href).includes('cookies');
        });

        Logger.log('Get licence and copyright link');
        cy.contains('a', 'Open Government Licence v3.0').should('be.visible');
        cy.contains('a', '© Crown copyright').should('be.visible');
    });
});
