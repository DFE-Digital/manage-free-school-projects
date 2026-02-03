class ProjectStatusWithdrawnPage {
    private errorTracking = "";
    

    public addWithdrawnYear(day: string, month: string, year: string): this {
        cy.enterDate("year-withdrawn", day, month, year);
        return this;
    }

    public withdrawnYearHasValue(day: string, month: string, year: string): this {
        cy.checkDate("year-withdrawn", day, month, year);
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

    errorForWithdrawnDate(error: string): this {
        cy.getById("year-withdrawn-error-link").contains(error)
        return this
        
    }

    errorForReasonForWithdrawal(error: string): this {
        cy.getById("project-withdrawn-reason-type-error-link").contains(error)
        return this
    }

    errorForWithdrawnAsAResultOFNationalPipelineReview(error: string): this {
        cy.getById("project-withdrawn-as-a-result-of-national-review-of-pipeline-error-link").contains(error)
        return this
    }


    errorForNotesAboutTheWithdrawal(error: string): this {
        cy.getById("add-notes-about-the-withdrawal-error-link").contains(error)
        return this
    }

    public selectGovernance(): this {
        cy.getByTestId("project-withdrawn-reason-type-governance").click();
        return this;
    }

    public selectProjectWithdrawnAsAResultOfNationalPipelineReviewYes(): this {
        cy.getById("project-withdrawn-as-a-result-of-national-review-of-pipeline-1").click();
        return this;
    }

    public withAddNotesAboutTheWithdrawal(value: string): this {
        cy.getById("add-notes-about-the-withdrawal").clear().type(value);
        return this;
    }

}

const projectStatusWithdrawnPage = new ProjectStatusWithdrawnPage();

export default projectStatusWithdrawnPage;