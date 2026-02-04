import { BaseEditPage } from '../../baseEditPage';

class ExternalExpertVisitEditPage extends BaseEditPage {
    private summaryCounter = -1;

    withVisitDate(day: string, month: string, year: string): this {
        const key = 'external-expert-visit-date';
        this.setDate(key, day, month, year);
        return this;
    }

    checkCommissionedExternalExpertVisit(): this {
        cy.getById('commissioned-external-expert-visit').check();
        return this;
    }

    checkSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').check();
        return this;
    }

    uncheckCommissionedExternalExpertVisit(): this {
        cy.getById('commissioned-external-expert-visit').uncheck();
        return this;
    }

    uncheckSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').uncheck();
        return this;
    }

    public HasValue(value): this {
        cy.get('.govuk-summary-list__value').eq(this.summaryCounter).should('contains.text', value);
        return this;
    }

    errorForVisitDate(): this {
        this.errorTracking = 'external-expert-visit-date';
        return this;
    }
}

const externalExpertVisitEditPage = new ExternalExpertVisitEditPage();
export default externalExpertVisitEditPage;
