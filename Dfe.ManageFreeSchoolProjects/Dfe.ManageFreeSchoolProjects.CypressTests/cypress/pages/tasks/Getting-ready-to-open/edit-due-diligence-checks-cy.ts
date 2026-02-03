class DueDiligenceChecksEditPage { 
    private errorTracking = "";

    checkReceivedChairOfTrusteesDbsCountersignedCertificate(): this { 
        cy.getById("received-chair-of-trustees-countersigned-certificate").check()
        return this
    }

    checkNonSpecialistChecksDoneOnAllTrustMembersAndTrusteesInLast2Years() : this { 
        cy.getById("non-specialist-checks-done-on-all-trust-members-and-trustees").check()
        return this
    }


    requestedCounterExtremismChecks(option : "Yes" | "No") : this { 
        cy.getById(`requested-counter-extremism-checks-${option.toLowerCase()}`).click()
        return this
    }

    enterDateWhenAllChecksWereCompleted(day : string, month: string, year: string): this {
        cy.enterDate("date-when-all-checks-completed", day, month, year)
        return this
    }

    checkSavedNonSpecialistChecksSpreadsheetInWorkplaces(): this { 
        cy.getById("saved-the-non-specialist-check-spreadsheet-in-workplaces").check()
        return this
    }

    checkDeletedAnyCopiesOfChairsDBSCertificate(): this { 
        cy.getById("deleted-any-copies-of-chairs-dbs-certificate").check()
        return this
    }

    checkDeletedEmailContainingSuitabilityAndDeclarationForms(): this { 
        cy.getById("deleted-emails-containing-suitability-and-declaration-forms").check()
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }

    errorForDate(): this {
        this.errorTracking = "date-when-all-checks-completed";
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

const dueDiligenceChecksEditPage = new DueDiligenceChecksEditPage();
export default dueDiligenceChecksEditPage;