class EditUnderwrittenPlacesPage {
    public titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public schoolNameIs(school: string) {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }

    public clickContinue(): this {
        cy.getByTestId("continue").click();
        return this;
    }

    public checkConfirmationFromLASavedInWorkplacesFolder(): this {
        cy.getById("confirmation-from-local-authority-saved-in-workplaces-folder").click();
        return this;
    }

    public withPrimaryYear1Places(value: string): this {
        cy.getById("primary-year-1-places").typeFast(value);
        return this;
    }

    public withPrimaryYear3Places(value: string): this {
        cy.getById("primary-year-3-places").typeFast(value);
        return this;
    }

    public withPrimaryYear7Places(value: string): this {
        cy.getById("primary-year-7-places").typeFast(value);
        return this;
    }

    public withComments(value: string): this {
        cy.getById("comments-about-underwritten-places").typeFast(value);
        return this;
    }

    public withCommentsExceedingMaxLength(): this {
        cy.getById("comments-about-underwritten-places").clear().invoke("val", "a".repeat(1000));
        return this;
    }
}

const editUnderwrittenPlacesPage = new EditUnderwrittenPlacesPage();

export default editUnderwrittenPlacesPage;