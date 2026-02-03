class AdmissionsArrangementsEditPage {
    private errorTracking = "";
    private summaryCounter = -1;
    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }
    
    withExpectedDate(day: string, month: string, year: string): this {
        const key = "expected-date-that-trust-will-confirm-arrangements";
        this.setDate(key, day, month, year);
        return this
    }

    withConfirmedDate(day: string, month: string, year: string): this {
        const key = "actual-date-that-trust-confirmed-arrangements";
        this.setDate(key, day, month, year);
        return this
    }

    checkTrustConfirmedAdmissionsArrangementsTemplate(): this {
        cy.getById("trust-confirmed-admissions-arrangements-template").check()
        return this
    }
    
    checkTrustConfirmedAdmissionsArrangementsPolicies(): this {
        cy.getById("trust-confirmed-admissions-arrangements-policies").check()
        return this
    }

    checkSavedToWorkplaces(): this {
        cy.getById("saved-to-workplaces").check()
        return this
    }

    uncheckTrustConfirmedAdmissionsArrangementsTemplate(): this {
        cy.getById("trust-confirmed-admissions-arrangements-template").uncheck()
        return this
    }

    uncheckTrustConfirmedAdmissionsArrangementsPolicies(): this {
        cy.getById("trust-confirmed-admissions-arrangements-policies").uncheck()
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
    
    errorForExpectedDate(): this {
        this.errorTracking = "expected-date-that-trust-will-confirm-arrangements";
        return this;
    }

    errorForConfirmedDate(): this {
        this.errorTracking = "actual-date-that-trust-confirmed-arrangements";
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

const admissionsArrangementsEditPage = new AdmissionsArrangementsEditPage();
export default admissionsArrangementsEditPage;
