class ExternalExpertVisitEditPage {
    private errorTracking = "";
    private summaryCounter = -1;
    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }
    
    withVisitDate(day: string, month: string, year: string): this {
        const key = "external-expert-visit-date";
        this.setDate(key, day, month, year);
        return this
    }

    checkCommissionedExternalExpertVisit(): this {
        cy.getById("commissioned-external-expert-visit").check()
        return this
    }
    
    checkSavedToWorkplaces(): this {
        cy.getById("saved-to-workplaces").check()
        return this
    }

    uncheckCommissionedExternalExpertVisit(): this {
        cy.getById("commissioned-external-expert-visit").uncheck()
        return this
    }

    uncheckSavedToWorkplaces(): this {
        cy.getById("saved-to-workplaces").uncheck()
        return this
    }
    
    

    public HasValue(value): this {
        cy.get(".govuk-summary-list__value").eq(this.summaryCounter).should("contains.text", value);
        return this;
    }
    
    errorForVisitDate(): this {
        this.errorTracking = "external-expert-visit-date";
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

const externalExpertVisitEditPage = new ExternalExpertVisitEditPage();
export default externalExpertVisitEditPage;
