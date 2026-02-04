import { BaseEditPage } from '../../baseEditPage';

class EditVariationLetter extends BaseEditPage {
    public variationTitleIs(title: string): this {
        cy.getByTestId('title').should('contains.text', title);
        return this;
    }

    public variationLetterDueDate(day: string, month: string, year: string): this {
        const key = 'due-date-variation-letter';
        this.setDate(key, day, month, year);
        return this;
    }

    public checkSavedInWorkplacesForInitialGrant(): this {
        cy.getById('initial-grant-letter-saved-to-workplaces-folder').click().should('be.checked');
        return this;
    }

    errorForDueDate(): this {
        this.errorTracking = 'due-date-variation-letter';
        return this;
    }

    public clickDiscard(): this {
        cy.getByTestId('discard').click();
        return this;
    }
}

const editVariationLetter = new EditVariationLetter();
export default editVariationLetter;
