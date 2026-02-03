class ReadinessToOpenMeetingEditPage {
    private errorTracking = "";

    selectAROMIsExpectedToHappenYes(): this {
        cy.getById("a-rom-is-expected-to-happen-yes").click()
        return this
    }

    selectAROMIsExpectedToHappenNo(): this {
        cy.getById("a-rom-is-expected-to-happen-no").click()
        return this
    }

    selectFormalMeeting(): this {
        cy.getById("formal-meeting-option").click()
        return this
    }

    selectInformalMeeting(): this {
        cy.getById("informal-meeting-option").click()
        return this
    }

    selectNoRomHeld(): this {
        cy.getById("no-rom-held-option").click()
        return this
    }

    checkPrincipalDesignate(): this {
        cy.getById("principal-designate-has-provided-checklist").check()
        return this
    }

    checkCommissionedAnExternalExpert(): this {
        cy.getById("commissioned-external-expert-to-attend-any-meetings").check()
        return this
    }

    checkSavedTheInternalRomReport(): this {
        cy.getById("saved-the-internal-rom-report-workplaces-folder").check()
        return this
    }

    checkSavedTheExternalRomReport(): this {
        cy.getById("saved-the-external-rom-report-workplaces-folder").check()
        return this
    }

    whyMeetingWasNotHeld(): this {
        cy.getById("why-meeting-not-held").type('This is the reason why meeting was not held')
        return this
    }

    uncheckPrincipalDesignate(): this {
        cy.getById("principal-designate-has-provided-checklist").uncheck()
        return this
    }

    uncheckCommissionedAnExternalExpert(): this {
        cy.getById("commissioned-external-expert-to-attend-any-meetings").uncheck()
        return this
    }

    uncheckSavedTheInternalRomReport(): this {
        cy.getById("saved-the-internal-rom-report-workplaces-folder").uncheck()
        return this
    }

    uncheckSavedTheExternalRomReport(): this {
        cy.getById("saved-the-external-rom-report-workplaces-folder").uncheck()
        return this
    }

    
    expectedDateOfTheMeeting(day: string, month: string, year: string): this {
        const key = "expected-date-of-the-meeting";
        this.setDate(key, day, month, year);
        return this
    }


    dateOfTheFormalMeeting(day: string, month: string, year: string): this {
        const key = "date-of-the-formal-meeting";
        this.setDate(key, day, month, year);
        return this
    }

    dateOfTheInformalMeeting(day: string, month: string, year: string): this {
        const key = "date-of-the-informal-meeting";
        this.setDate(key, day, month, year);
        return this
    }

    private setDate(key: string, day: string, month: string, year: string) {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }
    clickContinue(): this {
        cy.getByTestId("continue").click();
        return this;
    }

    errorForExpectedMeetingDate(error: string): this {
        cy.getById('expected-date-of-the-meeting-error').contains(error)
        return this
    }

    errorForFormalMeetingDate(error: string): this {
        cy.getById('date-of-the-formal-meeting-error').contains(error)
        return this
    }

    errorForInformalMeetingDate(error: string): this {
        cy.getById('date-of-the-informal-meeting-error').contains(error)
        return this
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

const readinessToOpenMeetingEditPage = new ReadinessToOpenMeetingEditPage();
export default readinessToOpenMeetingEditPage;
