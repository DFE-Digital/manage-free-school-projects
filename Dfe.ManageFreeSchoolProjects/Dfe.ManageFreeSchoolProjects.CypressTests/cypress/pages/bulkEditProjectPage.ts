class BulkEditProjectPage {
    public upload(file: Buffer, filename: string): this {
        cy.get('input[name="upload"]').selectFile({
            contents: file,
            fileName: filename,
            lastModified: Date.now(),
        });
        return this;
    }

    public errorMessage(error: string): this {
        cy.get('.govuk-error-summary').should('contains.text', error)
        return this
    }
    
    public continue(): this {
        cy.contains('button','Upload').click()
        return this;
    }

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
        cy.get('h1').contains('Check your answers')
        cy.get('.govuk-summary-card__content').should('have.length', 1)
        return this;
    }

    public successMessage(): this {
        cy.url().should('contains', 'count')
        cy.get('.govuk-panel').should('be.visible')
        cy.contains('a','Go back to the projects listing page').should('be.visible')
        return this;
    }

}

const bulkEditProjectPage = new BulkEditProjectPage();

export default bulkEditProjectPage;
