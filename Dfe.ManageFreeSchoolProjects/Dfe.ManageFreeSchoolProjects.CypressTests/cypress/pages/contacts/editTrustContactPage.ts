
class EditTrustContactPage {

    public hasTrustContactTitle(value: string): this {
        cy.getByTestId(`edit-trust-contact-title`).should("contain.text", value);

        return this;
    }

    withTrustContactName(value: string): this {
        cy.getByTestId("edit-trust-contact-name").clear().type(value)
        return this;
    }

    withTrustContactEmail(value: string): this {
        cy.getByTestId("edit-trust-contact-email").clear().type(value)
        return this;
    }

    withTrustContactPhoneNumber(value: string): this {
        cy.getByTestId("edit-trust-contact-phone-number").clear().type(value)
        return this;
    }

    withTrustContactRole(value: string): this {
        cy.getByTestId("edit-trust-contact-role").clear().type(value)
        return this;
    }

    errorForTrustContactName(error: string): this {
        cy.getById('trust-contact-name-error').contains(error)
        return this
    }
    errorForTrustContactEmail(error: string): this {
        cy.getById('trust-contact-email-error').contains(error)
        return this
    }

    errorForTrustContactPhoneNumber(error: string): this {
        cy.getById('trust-contact-phone-number-error').contains(error)
        return this
    }
    errorForTrustContactRole(error: string): this {
        cy.getById('trust-contact-role-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editTrustContactPage = new EditTrustContactPage();

export default editTrustContactPage;