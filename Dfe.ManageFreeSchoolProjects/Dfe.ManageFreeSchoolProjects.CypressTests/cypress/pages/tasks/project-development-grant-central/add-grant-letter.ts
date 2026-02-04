import { BaseEditPage } from '../../baseEditPage';

class AddGrantLetter extends BaseEditPage {
    public withInitialGrantLetterDate(day: string, month: string, year: string): this {
        const key = 'date-signed-initial-grant-letter';
        this.setDate(key, day, month, year);
        return this;
    }

    public withFullGrantLetterDate(day: string, month: string, year: string): this {
        const key = 'date-signed-full-grant-letter';
        this.setDate(key, day, month, year);
        return this;
    }

    public checkSavedInWorkplacesForInitialGrant(): this {
        cy.get('.govuk-checkboxes__item').first().click();
        return this;
    }

    public checkSavedInWorkplacesForFullGrant(): this {
        cy.get('.govuk-checkboxes__item').eq(1).click();
        return this;
    }

    errorForInitialGrantLetterDate(): this {
        this.errorTracking = 'date-signed-initial-grant-letter';
        return this;
    }

    errorForFullGrantLetterDate(): this {
        this.errorTracking = 'date-signed-full-grant-letter';
        return this;
    }

    public clickDiscard(): this {
        cy.getByTestId('discard').click();
        return this;
    }
}

const addGrantLetter = new AddGrantLetter();
export default addGrantLetter;
