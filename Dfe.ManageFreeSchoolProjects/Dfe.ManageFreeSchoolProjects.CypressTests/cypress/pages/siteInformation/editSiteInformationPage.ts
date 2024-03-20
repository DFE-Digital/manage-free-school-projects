export class EditSiteInformationPage {
    public withAddressLine1(value: string): this {
        cy.getById("address-line1").clear().type(value);
        return this;
    }

    public withAddressLine2(value: string): this {
        cy.getById("address-line2").clear().type(value);
        return this;
    }

    public withPostcode(value: string): this {
        cy.getById("postcode").clear().type(value);
        return this;
    }

    public hasSchoolName(value: string): this {
        cy.getByTestId("school-name").should("contain.text", value);
        return this;
    }

    public withFieldsExceedingMaxLength(): this {
        cy.getById("address-line1").clear().invoke("val", "a".repeat(101));
        cy.getById("address-line2").clear().invoke("val", "a".repeat(301));
        cy.getById("postcode").clear().invoke("val", "a".repeat(11));

        return this;
    }

    public saveAndContinue(): this {
        cy.getByTestId("continue").click();
        return this;
    }
}

const editSiteInformationPage = new EditSiteInformationPage();

export default editSiteInformationPage;