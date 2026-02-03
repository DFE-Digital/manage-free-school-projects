class ImpactAssessmentEditPage {
    private errorTracking = "";
    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }
    
    checkImpactAssessmentDone(): this {
        cy.getById("impact-assessment-done").check()
        return this
    }

    checkSavedToWorkplaces(): this {
        cy.getById("saved-to-workplaces").check()
        return this
    }

    uncheckImpactAssessmentDone(): this {
        cy.getById("impact-assessment-done").uncheck()
        return this
    }

    uncheckSavedToWorkplaces(): this {
        cy.getById("saved-to-workplaces").uncheck()
        return this
    }

    checkSentSection9(): this {
        cy.getById("sent-section9-letter-to-local-authority").check()
        return this
    }

    uncheckSentSection9(): this {
        cy.getById("sent-section9-letter-to-local-authority").uncheck()
        return this
    }

    withDateSent(day: string, month: string, year: string): this {
        const key = "date-sent";
        this.setDate(key, day, month, year);
        return this
    }

    errorDateSent(): this {
        this.errorTracking = "date-sent";
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

const impactAssessmentEditPage = new ImpactAssessmentEditPage();
export default impactAssessmentEditPage;
