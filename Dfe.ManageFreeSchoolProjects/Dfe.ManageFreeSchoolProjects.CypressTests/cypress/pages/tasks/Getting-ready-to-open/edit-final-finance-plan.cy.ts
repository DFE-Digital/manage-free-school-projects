class FinalFinancePlanEditPage {
    private errorTracking = "";
    private summaryCounter = -1;
    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }

   

    withExpectedDateGrade6WillSignOffFinalPlan(day: string, month: string, year: string): this {
        const key = "expected-date-grade6-will-signoff-final-plan";
        this.setDate(key, day, month, year);
        return this
    }

    errorExpectedDateGrade6WillSignOffFinalPlan(): this {
        this.errorTracking = "expected-date-grade6-will-signoff-final-plan";
        return this;
    }


    withGrade6SignedOffFinalPlanDate(day: string, month: string, year: string): this {
        const key = "grade-6-signed-off-final-plan-date";
        this.setDate(key, day, month, year);
        return this
    }

    checkConfirmedTrustHasProvidedFinalPlan(): this {
        cy.getById("confirmed-trust-has-provided-final-plan").check()
        return this
    }
    
    uncheckConfirmedTrustHasProvidedFinalPlan(): this {
        cy.getById("confirmed-trust-has-provided-final-plan").uncheck()
        return this
    }

    checkSentFinalPlanToRevenueFundingMailbox(): this {
        cy.getById("sent-final-plan-to-revenue-funding-mailbox").check()
        return this
    }

    uncheckSentFinalPlanToRevenueFundingMailbox(): this {
        cy.getById("sent-final-plan-to-revenue-funding-mailbox").uncheck()
        return this
    }

    checkAddedAnyUnderwrittenPlacesToFinalPlan(): this {
        cy.getById("added-any-underwritten-places-to-the-finance-plan").check()
        return this
    }

    uncheckAddedAnyUnderwrittenPlacesToFinalPlan(): this {
        cy.getById("added-any-underwritten-places-to-the-finance-plan").uncheck()
        return this
    }

    checkSavedFinalPlanInWorkplacesFolder(): this {
        cy.getById("saved-final-plan-in-workplaces-folder").check()
        return this
    }

    uncheckSavedFinalPlanInWorkplacesFolder(): this {
        cy.getById("saved-final-plan-in-workplaces-folder").uncheck()
        return this
    }

    public HasValue(value): this {
        cy.get(".govuk-summary-list__value").eq(this.summaryCounter).should("contains.text", value);
        return this;
    }
    
    errorGrade6SignedOffFinalPlanDate(): this {
        this.errorTracking = "grade-6-signed-off-final-plan-date";
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


const finalFinancePlanEditPage = new FinalFinancePlanEditPage();
export default finalFinancePlanEditPage;
