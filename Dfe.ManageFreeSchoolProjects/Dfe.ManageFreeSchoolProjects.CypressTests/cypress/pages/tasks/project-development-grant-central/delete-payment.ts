class DeletePayment {
    private errorTracking = "";

    public titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }

    public clickDelete() : this {
        cy.getByTestId("delete").click();
        return this;
    }

    public clickNo() : this {
        cy.getByTestId("payment-schedule-link").click();
        return this;
    }
}

const deletePayment = new DeletePayment();
export default deletePayment;