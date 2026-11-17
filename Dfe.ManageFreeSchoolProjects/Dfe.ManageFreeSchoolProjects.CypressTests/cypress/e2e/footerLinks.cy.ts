import { Logger } from 'cypress/common/logger';
describe('Navigate through footer links', () => {
    beforeEach(() => {
        cy.login();
        cy.visit('/');
    });

    it('user should be able to see the footer links', () => {
        cy.get('.govuk-footer').should('be.visible');

        Logger.log('Problems and feedback section');
        cy.get('.govuk-footer').within(() => {
            cy.get('h2').should('contain', 'Problems and feedback');
            cy.contains('p', 'Tell us if something is broken or could be improved.').should('be.visible');
            cy.contains('a', 'Report a problem or give feedback')
                .should('be.visible')
                .and('have.attr', 'target', '_blank');

            // The Unified Intake base URL is environment specific, so only assert the service code
            cy.contains('a', 'Report a problem or give feedback')
                .invoke('attr', 'href')
                .should('contain', 'serviceCode=MFSP');
        });

        Logger.log('Get accessibility and cookies link');
        cy.get('.govuk-footer').within(() => {
            cy.contains('a', 'Accessibility statement')
                .should('be.visible')
                .and('have.attr', 'href', 'https://accessibility-statements.education.gov.uk/s/32');
            cy.contains('a', 'Accessibility statement').should('have.attr', 'target', '_blank');
            cy.contains('a', 'Cookies').should('be.visible').and('have.attr', 'href', '/public/cookies?returnUrl=%2f');
        });

        Logger.log('Get licence and copyright link');
        cy.get('.govuk-footer').within(() => {
            cy.contains('a', 'Open Government Licence v3.0')
                .should('be.visible')
                .and('have.attr', 'href', 'https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/');
            cy.contains('a', '© Crown copyright')
                .should('be.visible')
                .and(
                    'have.attr',
                    'href',
                    'https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/'
                );
        });
    });

    it('user should be able to navigate to the cookies page from the footer', () => {
        cy.get('.govuk-footer').contains('a', 'Cookies').click();

        cy.url().should('contain', 'cookies');
        cy.get('h1').should('contain', 'Cookie preferences');
    });
});
