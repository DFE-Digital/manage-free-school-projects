class KickOffMeetingEditPage {
    private errorTracking = "";

    titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    schoolNameIs(school: string) {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }

    withComments(comment: string): this {
        cy.getById("funding-arrangement-details-agreed").typeFast(comment)
        return this;
    }

    withRealisticYearOfOpeningStartDate(year: string): this {
        cy.getById("realistic-year-of-opening-startyear").typeFast(year)
        return this;
    }

    withRealisticYearOfOpeningEndDate(year: string): this {
        cy.getById("realistic-year-of-opening-endyear").typeFast(year)
        return this;
    }

    checkSavedDocumentsInWorkplacesFolder(): this {
        cy.getById("saved-documents-in-workplaces-folder").check()
        return this
    }

    errorForComments(): this {
        this.errorTracking = "funding-arrangement-details-agreed";
        return this;
    }

    checkFundingArrangementAgreed(): this {
        cy.getById("funding-arrangement-agreed").check()
        return this
    }

    errorForRealisticStartDate(error: string): this {
        cy.getById('realistic-year-of-opening-error').contains(error)
        return this
    }
    
    errorForProvisionalOpeningDate(): this {
        this.errorTracking = "provisional-opening-date";
        return this;
    }

    showsError(error: string) {
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

    clickContinue(): this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const kickOffMeetingEditPage = new KickOffMeetingEditPage();
export default kickOffMeetingEditPage;
