class AddGrantLetter {
    private errorTracking = "";

    public titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }

    public withInitialGrantLetterDate(day: string, month: string, year: string): this {
        const key = "date-signed-initial-grant-letter";
        this.setDate(key, day, month, year);
        return this
    }

    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }

    public withFullGrantLetterDate(day: string, month: string, year: string): this {
        const key = "date-signed-full-grant-letter";
        this.setDate(key, day, month, year);
        return this
    }

    public checkSavedInWorkplacesForInitialGrant(): this {
        cy.get('.govuk-checkboxes__item').first().click()
        return this;
    }

    public checkSavedInWorkplacesForFullGrant(): this {
        cy.get('.govuk-checkboxes__item').eq(1).click()
        return this;
    }

    errorForInitialGrantLetterDate(): this {
        this.errorTracking = "date-signed-initial-grant-letter";
        return this;
    }

    errorForFullGrantLetterDate(): this {
        this.errorTracking = "date-signed-full-grant-letter";
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

    public clickDiscard() : this {
        cy.getByTestId("discard").click();
        return this;
    }

    public clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const addGrantLetters = new AddGrantLetter();
export default addGrantLetters;