
class EditProjectManagerPage {

    public hasProjectManagerTitle(value: string): this {
        cy.getByTestId(`edit-project-manager-title`).should("contain.text", value);

        return this;
    }

    withProjectManagerName(value: string): this {
        cy.getByTestId("edit-project-manager-name").clear().type(value)
        return this;
    }

    withProjectManagerEmail(value: string): this {
        cy.getByTestId("edit-project-manager-email").clear().type(value)
        return this;
    }

    errorForProjectManagerName(error: string): this {
        cy.getById('project-manager-name-error').contains(error)
        return this
    }
    errorForProjectManagerEmail(error: string): this {
        cy.getById('project-manager-email-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editProjectManagerPage = new EditProjectManagerPage();

export default editProjectManagerPage;