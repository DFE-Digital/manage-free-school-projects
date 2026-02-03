
class EditProjectAssignedToPage {

    public hasProjectAssignedToTitle(value: string): this {
        cy.getByTestId(`edit-project-assigned-to-title`).should("contain.text", value);

        return this;
    }

    withProjectAssignedToName(value: string): this {
        cy.getByTestId("edit-project-assigned-to-name").clear().type(value)
        return this;
    }

    withNullProjectAssignedToName(): this {
        cy.getByTestId("edit-project-assigned-to-name").clear()
        return this;
    }

    withProjectAssignedToEmail(value: string): this {
        cy.getByTestId("edit-project-assigned-to-email").clear().type(value)
        return this;
    }

    withNullProjectAssignedToEmail(): this {
        cy.getByTestId("edit-project-assigned-to-email").clear()
        return this;
    }

    errorForProjectAssignedToName(error: string): this {
        cy.getById('project-assigned-to-name-error').contains(error)
        return this
    }
    errorForProjectAssignedToEmail(error: string): this {
        cy.getById('project-assigned-to-email-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editProjectAssignedToPage = new EditProjectAssignedToPage();

export default editProjectAssignedToPage;