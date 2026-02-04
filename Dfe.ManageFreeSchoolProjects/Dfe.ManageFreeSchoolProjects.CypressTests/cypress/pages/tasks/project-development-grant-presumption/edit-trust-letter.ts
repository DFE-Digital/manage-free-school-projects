import { BaseEditPage } from '../../baseEditPage';

class EditTrustLetter extends BaseEditPage {
    public withTrustLetterDate(day: string, month: string, year: string): this {
        const key = 'trust-letter-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public checkSavedInWorkplaces(): this {
        cy.getById('saved-letter-in-workplaces-folder').click();
        return this;
    }

    errorForPaymentDueDate(): this {
        this.errorTracking = 'trust-letter-date';
        return this;
    }
}

const editTrustLetter = new EditTrustLetter();
export default editTrustLetter;
