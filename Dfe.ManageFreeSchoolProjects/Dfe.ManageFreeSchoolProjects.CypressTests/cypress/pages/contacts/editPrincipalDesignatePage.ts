
class EditPrincipalDesignatePage {

    public hasPrincipalDesignateTitle(value: string): this {
        cy.getByTestId(`edit-principal-designate-title`).should("contain.text", value);

        return this;
    }

    withPrincipalDesignateName(value: string): this {
        cy.getByTestId("edit-principal-designate-name").clear().type(value)
        return this;
    }

    withPrincipalDesignateEmail(value: string): this {
        cy.getByTestId("edit-principal-designate-email").clear().type(value)
        return this;
    }

    errorForPrincipalDesignateName(error: string): this {
        cy.getById('principal-designate-name-error').contains(error)
        return this
    }
    errorForPrincipalDesignateEmail(error: string): this {
        cy.getById('principal-designate-email-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editPrincipalDesignatePage = new EditPrincipalDesignatePage();

export default editPrincipalDesignatePage;