class PreFundingAgreementCheckpointMeetingEditPage {
    private errorTracking = "";

    selectFormalCheckpointMeeting(): this {
        cy.getById("formal-checkpoint-meeting-option").click()
        return this
    }

    selectInternalReviewMeeting(): this {
        cy.getById("internal-review-meeting-option").click()
        return this
    }

    selectNoMeetingHeld(): this {
        cy.getById("no-meeting-held-option").click()
        return this
    }

    checkCommissionedExternalExpert(): this {
        cy.getById("commissioned-external-expert").check()
        return this
    }

    checkSavedMeetingNote(): this {
        cy.getById("saved-meeting-note-in-workplaces-folder").check()
        return this
    }

    checkSentAnEmailToTheTrust(): this {
        cy.getById("sent-an-email=to-the-trust").check()
        return this
    }

    whyMeetingWasNotHeld(): this {
        cy.getById("why-meeting-not-held").type('This is the reason why meeting was not held')
        return this
    }

    uncheckCommissionedExternalExpert(): this {
        cy.getById("commissioned-external-expert").uncheck()
        return this
    }

    uncheckSavedMeetingNote(): this {
        cy.getById("saved-meeting-note-in-workplaces-folder").uncheck()
        return this
    }

    uncheckSentAnEmailToTheTrust(): this {
        cy.getById("sent-an-email=to-the-trust").uncheck()
        return this
    }

    dateOfTheFormalCheckpointMeeting(day: string, month: string, year: string): this {
        const key = "date-of-the-formal-meeting";
        this.setDate(key, day, month, year);
        return this
    }

    dateOfTheInternalReviewMeeting(day: string, month: string, year: string): this {
        const key = "date-of-the-internal-review-meeting";
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

    errorForFormalCheckpointMeetingDate(error: string): this {
        cy.getById('date-of-the-formal-meeting-error').contains(error)
        return this
    }

    errorForInternalReviewMeetingDate(error: string): this {
        cy.getById('date-of-the-internal-review-meeting-error').contains(error)
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

const preFundingAgreementCheckpointMeetingEditPage = new PreFundingAgreementCheckpointMeetingEditPage();
export default preFundingAgreementCheckpointMeetingEditPage;
