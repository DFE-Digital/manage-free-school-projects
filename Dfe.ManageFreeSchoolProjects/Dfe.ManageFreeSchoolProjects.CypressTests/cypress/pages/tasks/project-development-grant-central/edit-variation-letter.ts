class editVariationLetter {
    private errorTracking = "";

    public variationTitleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }

    public variationLetterDueDate(day: string, month: string, year: string): this {
        const key = "due-date-variation-letter";
        this.setDate(key, day, month, year);
        return this
    }

    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }

    public checkSavedInWorkplacesForInitialGrant(): this {
        cy.getById("initial-grant-letter-saved-to-workplaces-folder").click()
          .should('be.checked');
        return this;
    }

    errorForDueDate(): this {
        this.errorTracking = "due-date-variation-letter";
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

const changeVariationLetter = new editVariationLetter();
export default changeVariationLetter;
