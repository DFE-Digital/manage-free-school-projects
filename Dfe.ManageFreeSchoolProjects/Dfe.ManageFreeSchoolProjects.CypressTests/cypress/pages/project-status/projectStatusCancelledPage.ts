class ProjectStatusCancelledPage {
    private errorTracking = "";
    

    public addCancelledYear(day: string, month: string, year: string): this {
        cy.enterDate("year-cancelled", day, month, year);
        return this;
    }

    public cancelledYearHasValue(day: string, month: string, year: string): this {
        cy.checkDate("year-cancelled", day, month, year);
        return this;
    }

    public clickSaveAndContinue(): this {
        cy.getByTestId("save-and-continue").click();
        return this;
    }

    public clickBackLink(): this {
        cy.getById("back-link").click();
        return this;
    }

    errorForCancelledDate(error: string): this {
        cy.getById("year-cancelled-error-link").contains(error)
        return this
    }

    errorForReasonForCancellation(error: string): this {
        cy.getById("project-cancelled-reason-type-error-link").contains(error)
        return this
    }

    errorForCancelledAsAResultOFNationalPipelineReview(error: string): this {
        cy.getById("project-cancelled-as-a-result-of-national-review-of-pipeline-error-link").contains(error)
        return this
    }


    errorForNotesAboutTheCancellation(error: string): this {
        cy.getById("add-notes-about-the-cancellation-error-link").contains(error)
        return this
    }


    public selectGovernance(): this {
        cy.getByTestId("project-cancelled-reason-type-governance").click();
        return this;
    }

    public selectProjectCancelledAsAResultOfNationalPipelineReviewYes(): this {
        cy.getById("project-cancelled-as-a-result-of-national-review-of-pipeline-1").click();
        return this;
    }

    public withAddNotesAboutTheCancellation(value: string): this {
        cy.getById("add-notes-about-the-cancellation").clear().type(value);
        return this;
    }

}

const projectStatusCancelledPage = new ProjectStatusCancelledPage();

export default projectStatusCancelledPage;