
class EditOfstedContactPage {

    public hasOfstedContactTitle(value: string): this {
        cy.getByTestId(`edit-ofsted-contact-title`).should("contain.text", value);

        return this;
    }

    withOfstedContactName(value: string): this {
        cy.getByTestId("edit-ofsted-contact-name").clear().type(value)
        return this;
    }

    withOfstedContactEmail(value: string): this {
        cy.getByTestId("edit-ofsted-contact-email").clear().type(value)
        return this;
    }

    withOfstedContactPhoneNumber(value: string): this {
        cy.getByTestId("edit-ofsted-contact-phone-number").clear().type(value)
        return this;
    }

    withOfstedContactRole(value: string): this {
        cy.getByTestId("edit-ofsted-contact-role").clear().type(value)
        return this;
    }

    errorForOfstedContactName(error: string): this {
        cy.getById('ofsted-contact-name-error').contains(error)
        return this
    }
    errorForOfstedContactEmail(error: string): this {
        cy.getById('ofsted-contact-email-error').contains(error)
        return this
    }

    errorForOfstedContactPhoneNumber(error: string): this {
        cy.getById('ofsted-contact-phone-number-error').contains(error)
        return this
    }
    errorForOfstedContactRole(error: string): this {
        cy.getById('ofsted-contact-role-error').contains(error)
        return this
    }

    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editOfstedContactPage = new EditOfstedContactPage();

export default editOfstedContactPage;