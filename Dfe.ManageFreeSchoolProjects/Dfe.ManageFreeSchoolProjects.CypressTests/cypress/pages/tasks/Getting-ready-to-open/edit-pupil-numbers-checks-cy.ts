class PupilNumbersChecksEditPage {
    private errorTracking = "";
    
    
    checkReceivedEnoughApplications(): this {
        cy.getById("school-received-enough-applications").check()
        return this
    }

    checkDataMatchesFundingAgreement(): this {
        cy.getById("capacity-data-matches-funding-agreement").check()
        return this
    }

    checkDataMatchesGiasRegistration(): this {
        cy.getById("capacity-data-matches-gias-registration").check()
        return this
    }

    uncheckReceivedEnoughApplications(): this {
        cy.getById("school-received-enough-applications").uncheck()
        return this
    }

    uncheckDataMatchesFundingAgreement(): this {
        cy.getById("capacity-data-matches-funding-agreement").uncheck()
        return this
    }

    uncheckDataMatchesGiasRegistration(): this {
        cy.getById("capacity-data-matches-gias-registration").uncheck()
        return this
    }
    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }

    errorForComments(): this {
        this.errorTracking = "comments";
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

}

const pupilNumbersChecksEditPage = new PupilNumbersChecksEditPage();
export default pupilNumbersChecksEditPage;
