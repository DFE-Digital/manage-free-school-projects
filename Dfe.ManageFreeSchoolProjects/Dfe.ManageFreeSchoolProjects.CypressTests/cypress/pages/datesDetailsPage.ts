import { BasePage } from './basePage';

class DatesDetailsPage extends BasePage {
    // Override the base clickContinue with custom behavior
    public clickContinue(): this {
        cy.get('body').then(($body) => {
            if ($body.find('[data-testid="continue"]').length) {
                cy.getByTestId('continue').click();
                return;
            }

            if ($body.find('button.govuk-button:contains("Save and continue")').length) {
                cy.contains('button.govuk-button', 'Save and continue').click();
                return;
            }

            cy.contains('button.govuk-button', 'Continue').click();
        });
        return this;
    }

    public addCancelledDate(day: string, month: string, year: string): this {
        cy.enterDate('project-cancelled-date', day, month, year);
        return this;
    }

    public withEntryIntoPreOpening(day: string, month: string, year: string): this {
        cy.enterDate('entry-into-pre-opening', day, month, year);
        return this;
    }

    public withProvisionalOpeningDateAgreedWithTrust(day: string, month: string, year: string): this {
        cy.enterDate('provisional-opening-date-agreed-with-trust', day, month, year);
        return this;
    }

    public withRealisticYearOfOpeningStartDate(year: string): this {
        cy.getById('realistic-year-of-opening-startyear').typeFast(year);
        return this;
    }

    public withRealisticYearOfOpeningEndDate(year: string): this {
        cy.getById('realistic-year-of-opening-endyear').typeFast(year);
        return this;
    }

    public errorForRealisticStartDate(error: string): this {
        cy.getById('realistic-year-of-opening-error').contains(error);
        return this;
    }
}

const datesDetailsPage = new DatesDetailsPage();

export default datesDetailsPage;
