import { BaseEditPage } from '../../baseEditPage';

class PrincipalDesignateEditPage extends BaseEditPage {
    checkYesForPrincipleDesignate(): this {
        cy.getById('principal-designate-appointed').check();
        return this;
    }

    checkNoForPrincipleDesignate(): this {
        cy.getById('principal-designate-appointed').uncheck();
        return this;
    }

    checkYesForExternalExpert(): this {
        cy.getByTestId('commissioned-external-expert-visit-Yes').check();
        return this;
    }

    checkNoForExternalExpert(): this {
        cy.getByTestId('commissioned-external-expert-visit-No').check();
        return this;
    }

    checkNotApplicableForExternalExpert(): this {
        cy.getByTestId('commissioned-external-expert-visit-NotApplicable').check();
        return this;
    }

    errorForPrincipleDesignateAppointedDate(): this {
        this.errorTracking = 'actual-date-that-principal-designate-was-appointed';
        return this;
    }

    withPrincipleDesignateAppointedDate(day: string, month: string, year: string): this {
        const key = 'actual-date-that-principal-designate-was-appointed';
        this.setDate(key, day, month, year);
        return this;
    }

    errorForExpectedPrincipleDesignateAppointedDate(): this {
        this.errorTracking = 'expected-date-that-principal-designate-will-be-appointed';
        return this;
    }

    withExpectedPrincipleDesignateAppointedDate(day: string, month: string, year: string): this {
        const key = 'expected-date-that-principal-designate-will-be-appointed';
        this.setDate(key, day, month, year);
        return this;
    }
}

const principalDesignateEditPage = new PrincipalDesignateEditPage();
export default principalDesignateEditPage;
