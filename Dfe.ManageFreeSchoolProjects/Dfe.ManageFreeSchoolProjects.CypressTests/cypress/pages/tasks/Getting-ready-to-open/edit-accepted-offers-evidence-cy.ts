import { BaseEditPage } from '../../baseEditPage';

class AcceptedOffersEvidenceEditPage extends BaseEditPage {
    checkSeenAcceptedOffersEvidence(): this {
        cy.getById('seen-accepted-offers-evidence').check();
        return this;
    }

    checkSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').check();
        return this;
    }

    uncheckSeenAcceptedOffersEvidence(): this {
        cy.getById('seen-accepted-offers-evidence').uncheck();
        return this;
    }

    uncheckSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').uncheck();
        return this;
    }

    withComments(comment: string): this {
        cy.getById('comments').typeFast(comment);
        return this;
    }

    clearComments(): this {
        cy.getById('comments').clear();
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'comments';
        return this;
    }
}

const acceptedOffersEvidenceEditPage = new AcceptedOffersEvidenceEditPage();
export default acceptedOffersEvidenceEditPage;
