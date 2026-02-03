
class EditGrade6Page {

    public hasGrade6Title(value: string): this {
        cy.getByTestId(`edit-grade-6-title`).should("contain.text", value);

        return this;
    }

    withGrade6Name(value: string): this {
        cy.getByTestId("edit-grade-6-name").clear().type(value)
        return this;
    }

    withGrade6Email(value: string): this {
        cy.getByTestId("edit-grade-6-email").clear().type(value)
        return this;
    }

    errorForGrade6Name(error: string): this {
        cy.getById('grade-6-name-error').contains(error)
        return this
    }
    errorForGrade6Email(error: string): this {
        cy.getById('grade-6-email-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editGrade6Page = new EditGrade6Page();

export default editGrade6Page;