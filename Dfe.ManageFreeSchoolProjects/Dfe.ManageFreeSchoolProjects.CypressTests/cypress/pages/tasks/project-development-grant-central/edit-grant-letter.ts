class editGrantLetter {
    private errorTracking = "";

    public titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public clickBack() : this {
        cy.getByClass("govuk-back-link").click();
        return this;
    }

    public grantTitleIs(title: string): this {
        cy.getByTestId("title").should("contain.text", title)
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }
    public addGrantLetter(): this {
        cy.get('.govuk-form-group > .govuk-button').should('include.text', 'Add grant letter')
         .click();
        return this;
    }

    public addGrantLetterNotShown(): this {
        cy.get('.govuk-form-group > .govuk-button')
         .should("not.exist");
        return this;
    }

    public changeGrantLetterNotShown(): this {
        cy.getByTestId(`change-grant-letter`).should("not.exist");
        return this;
    }

    public withTrustLetterDate(day: string, month: string, year: string): this {
        const key = "trust-letter-date";
        this.setDate(key, day, month, year);
        return this
    }

    public checkSavedInWorkplaces(): this {
        cy.getById("saved-letter-in-workplaces-folder").click();
        return this;
    }

    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }

    public grantLetterAdded(): this {
        cy.get('.govuk-notification-banner__content').should("contain.text", 'Grant letter added')
        return this;
    }

    public grantLetterSummaryHasValue( name: string, value: string): this {
        cy.get(".govuk-summary-list__key").contains(name).parent().should("contains.text", value);
        return this;
    }

    public showVariationLabel(): this {
        cy.get('.govuk-grid-column-two-thirds > p').should("contain.text", 'Add variations of the grant letter when needed.')
        return this;
    }

    public addVariation(index: string): this {
        cy.get('.govuk-form-group > .govuk-button').should('include.text', `Add variation ${index}`)
         .click();
        return this;
    }

    public variationLetterAdded(index: string): this {
        cy.get('.govuk-notification-banner__content').should("include.text", `Variation letter ${index} added`)
        return this;
    }

    changeVariationLetter(index: string) {
        cy.getByTestId(`change-variation-letter-${index}`).click();
    }

    changeVariationLetterNotShown(index: string): this {
        cy.getByTestId(`change-variation-letter-${index}`).should("not.exist");
        return this;
    }    

    errorForPaymentDueDate(): this {
        this.errorTracking = "trust-letter-date";
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

    public clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }

}


const editgrantLetter = new editGrantLetter();
export default editgrantLetter;
