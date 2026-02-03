class PdgDashboard {
    selectChangeTotalGrant() {
        cy.getByTestId("change-total-grant").click();
    }

    selectChangePaymentSchedule() {
        cy.getByTestId("change-payment-schedule")
        .should("contain.text", "Change")
        .click();
    }
    
    selectChangeTrustLetter() {
        cy.getByTestId("change-trust-letter").click();
    }

    selectChangeGrantLetter() {
        cy.getByTestId("change-grant-letters").click();
    }

    selectChangeWriteOff() {
        cy.getByTestId("change-write-off").click();
    }

    selectChangeStopPayments() {
        cy.getByTestId("change-stop-payments").click();
    }

    selectChangeRefunds() {
        cy.getByTestId("change-refunds").click();
    }

    selectViewDetailsPaymentSchedule() {
        cy.getByTestId("change-payment-schedule")
            .should("contain.text", "View Details")
            .click();
    }
    
    selectViewDetailsGrantLetter() {
        cy.getByTestId("change-grant-letters")
            .should("contain.text", "View Details")
            .click();
    }
    

    changePaymentScheduleNotShown(): this {
        cy.getByTestId("change-payment-schedule").should('not.exist');
        return this;
    }
    changeTotalGrantScheduleNotShown(): this {
        cy.getByTestId("change-total-grant").should('not.exist');
        return this;
    }
    
    changeTrustLetterNotShown(): this {
        cy.getByTestId("change-total-grant").should('not.exist');
        return this;
    }

    changeWriteOffNotShown(): this {
        cy.getByTestId("change-write-off").should('not.exist');
        return this;
    }

    changeStopPaymentsNotShown(): this {
        cy.getByTestId("change-stop-payments").should('not.exist');
        return this;
    }

    changeRefundsNotShown(): this {
        cy.getByTestId("change-refunds").should('not.exist');
        return this;
    }
}

const pdgDashboard = new PdgDashboard();
export default pdgDashboard;
