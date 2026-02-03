class editPaymentSchedule {
    private errorTracking = "";

    public titleIs(title: string): this {
        cy.getByTestId("title").should("contains.text", title)
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId("school-name").should("contains.text", school);
        return this;
    }
    
    public checkNoPayments(): this {
        cy.getByTestId("payments-list").should("not.contain.class", "govuk-summary-card")
        return this;
    }
    
    public checkPaymentAdded(index: string): this {
        cy.getByTestId("payment-added-banner").should("contain.text", `Payment ${index} added.`)
        return this;
    }

    public checkPaymentUpdated(index: string): this {
        cy.getByTestId("payment-updated-banner").should("contain.text", `Payment ${index} updated.`)
        return this;
    }

    public checkPaymentDeleted(): this {
        cy.getByTestId("payment-deleted-banner").should("contain.text", `Payment deleted.`)
        return this;
    }

    public paymentSummaryHasValue(paymentIndex: string, name: string, value: string): this {
        var paymentSummaryCardID = `payment-${paymentIndex}`;
        var paymentSummaryCard = cy.getByTestId(paymentSummaryCardID);
        paymentSummaryCard.contains(".govuk-summary-list__key").contains(name).parent().should("contains.text", value);
        return this;
    }

    selectAddPayment() {
        cy.getByTestId("add-payment").click();
    }

    checkAddPaymentDoesExist() {
        cy.getByTestId("add-payment").should("exist");
    }

    checkAddPaymentDoesNotExist(): this {
        cy.getByTestId("add-payment").should("not.exist");
        return this;
    }

    selectEditPayment(index: string) {
        cy.getByTestId(`change-payment-${index}`).click();
    }

    editPaymentLinkNotShown(index: string): this {
        cy.getByTestId(`change-payment-${index}`).should("not.exist");
        return this;
    }

    public clickBack() : this {
        cy.getByClass("govuk-back-link").click();
        return this;
    }
}

const paymentSchedule = new editPaymentSchedule();
export default paymentSchedule;
