class CheckYourAnswers {

    public chooseFile(): this {
        cy.get('input[name="upload"]').click();
        return this;
    }

    public editProjects(): this {
        cy.contains('button','Edit projects').click()
        return this;
    }

    public checkYourAnswersHeading(string): this {
        cy.get('.govuk-heading-xl').should('contain.text', string)
        return this;
    }

    public recordsVisible(): this {
        cy.get('.govuk-heading-xl').contains('Check your answers')
        cy.get('.govuk-summary-card__content').should('have.length', 1)
        return this;
    }
}

const checkYourAnswers = new CheckYourAnswers();

export default checkYourAnswers;
