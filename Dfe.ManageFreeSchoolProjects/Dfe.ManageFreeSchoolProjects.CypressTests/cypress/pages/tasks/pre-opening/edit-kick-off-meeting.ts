import { BaseEditPage } from '../../baseEditPage';

class KickOffMeetingEditPage extends BaseEditPage {
    withComments(comment: string): this {
        cy.getById('funding-arrangement-details-agreed').typeFast(comment);
        return this;
    }

    withRealisticYearOfOpeningStartDate(year: string): this {
        cy.getById('realistic-year-of-opening-startyear').typeFast(year);
        return this;
    }

    withRealisticYearOfOpeningEndDate(year: string): this {
        cy.getById('realistic-year-of-opening-endyear').typeFast(year);
        return this;
    }

    checkSavedDocumentsInWorkplacesFolder(): this {
        cy.getById('saved-documents-in-workplaces-folder').check();
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'funding-arrangement-details-agreed';
        return this;
    }

    checkFundingArrangementAgreed(): this {
        cy.getById('funding-arrangement-agreed').check();
        return this;
    }

    errorForRealisticStartDate(error: string): this {
        cy.getById('realistic-year-of-opening-error').contains(error);
        return this;
    }

    errorForProvisionalOpeningDate(): this {
        this.errorTracking = 'provisional-opening-date';
        return this;
    }
}

const kickOffMeetingEditPage = new KickOffMeetingEditPage();
export default kickOffMeetingEditPage;
