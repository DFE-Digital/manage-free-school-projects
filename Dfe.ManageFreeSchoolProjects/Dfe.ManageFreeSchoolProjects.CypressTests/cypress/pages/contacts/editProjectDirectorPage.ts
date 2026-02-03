
class EditProjectDirectorPage {

    public hasProjectDirectorTitle(value: string): this {
        cy.getByTestId(`edit-project-director-title`).should("contain.text", value);

        return this;
    }

    withProjectDirectorName(value: string): this {
        cy.getByTestId("edit-project-director-name").clear().type(value)
        return this;
    }

    withProjectDirectorEmail(value: string): this {
        cy.getByTestId("edit-project-director-email").clear().type(value)
        return this;
    }

    errorForProjectDirectorName(error: string): this {
        cy.getById('project-director-name-error').contains(error)
        return this
    }
    errorForProjectDirectorEmail(error: string): this {
        cy.getById('project-director-email-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editProjectDirectorPage = new EditProjectDirectorPage();

export default editProjectDirectorPage;