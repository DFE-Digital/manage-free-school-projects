import { BaseEditPage } from '../../baseEditPage';

class ImpactAssessmentEditPage extends BaseEditPage {
    checkImpactAssessmentDone(): this {
        cy.getById('impact-assessment-done').check();
        return this;
    }

    checkSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').check();
        return this;
    }

    uncheckImpactAssessmentDone(): this {
        cy.getById('impact-assessment-done').uncheck();
        return this;
    }

    uncheckSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').uncheck();
        return this;
    }

    checkSentSection9(): this {
        cy.getById('sent-section9-letter-to-local-authority').check();
        return this;
    }

    uncheckSentSection9(): this {
        cy.getById('sent-section9-letter-to-local-authority').uncheck();
        return this;
    }

    withDateSent(day: string, month: string, year: string): this {
        const key = 'date-sent';
        this.setDate(key, day, month, year);
        return this;
    }

    errorDateSent(): this {
        this.errorTracking = 'date-sent';
        return this;
    }
}

const impactAssessmentEditPage = new ImpactAssessmentEditPage();
export default impactAssessmentEditPage;
