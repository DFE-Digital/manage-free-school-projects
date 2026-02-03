class FundingAgreementEditPage {
    private errorTracking = "";
    private summaryCounter = -1;
    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }

    checkTailoredTheFundingAgreement(): this {
        cy.getById("tailored-the-funding-agreement").check()
        return this
    }

    uncheckTailoredTheFundingAgreement(): this {
        cy.getById("tailored-the-funding-agreement").uncheck()
        return this
    }

    checkSharedFAWithTrust(): this {
        cy.getById("shared-fa-with-the-trust").check()
        return this
    }

    uncheckSharedFAWithTrust(): this {
        cy.getById("shared-fa-with-the-trust").uncheck()
        return this
    }

    checkTrustHasSignedTheFa(): this {
        cy.getById("trust-has-signed-the-fa-Yes").check()
        return this
    }

    uncheckTrustHasSignedTheFa(): this {
        cy.getById("trust-has-signed-the-fa-No").check()
        return this
    }

    withDateTrustSignedFA(day: string, month: string, year: string): this {
        const key = "date-the-trust-signed-fa";
        this.setDate(key, day, month, year);
        return this
    }

    withExpectedDate(day: string, month: string, year: string): this {
        const key = "expected-date-fa-is-signed-on-secretary-of-states-behalf";
        this.setDate(key, day, month, year);
        return this
    }

    checkFundingAgreementSigned(): this {
        cy.getById("funding-agreement-signed").check()
        return this
    }

    uncheckFundingAgreementSigned(): this {
        cy.getById("funding-agreement-signed").uncheck()
        return this
    }

    withDateFAWasSigned(day: string, month: string, year: string): this {
        const key = "date-fa-was-signed";
        this.setDate(key, day, month, year);
        return this
    }

    checkSavedFADocumentsInWorkplacesFolder(): this {
        cy.getById("saved-fa-documents-in-workplaces-folder").check()
        return this
    }

    uncheckSavedFADocumentsInWorkplacesFolder(): this {
        cy.getById("saved-fa-documents-in-workplaces-folder").uncheck()
        return this
    }

    public HasValue(value): this {
        cy.get(".govuk-summary-list__value").eq(this.summaryCounter).should("contains.text", value);
        return this;
    }
    
    errorForTrustSignedFADate(): this {
        this.errorTracking = "date-the-trust-signed-fa";
        return this;
    }

    errorForExpectedDate(): this {
        this.errorTracking = "expected-date-fa-is-signed-on-secretary-of-states-behalf";
        return this;
    }

    errorForDateFAWasSigned(): this {
        this.errorTracking = "date-fa-was-signed";
        return this;
    }
    
    showsError(error: string)
    {
        cy.get(`#${this.errorTracking}-error-link`)
            .should("contain.text", error);

        cy.get(`#${this.errorTracking}-error-link`)
            .invoke('attr', 'href')
            .then((href) => {
                cy.get(href as string).should("exist");
            });

        cy.get(`#${this.errorTracking}-error`)
            .should("contain.text", error);
        return this;
    }
    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }

}


const fundingAgreementEditPage = new FundingAgreementEditPage();
export default fundingAgreementEditPage;
