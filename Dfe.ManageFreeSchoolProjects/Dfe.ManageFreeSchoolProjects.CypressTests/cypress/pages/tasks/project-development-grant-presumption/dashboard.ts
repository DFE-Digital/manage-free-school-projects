class PDGDashboard {

    selectChangePaymentSchedule() {
        cy.getByTestId("change-payment-schedule").click();
    }
    
    selectChangeTrustLetter() {
        cy.getByTestId("change-trust-letter").click();
    }

    selectChangeStopPayments() {
        cy.getByTestId("change-stop-payments").click();
    }

    selectChangeRefunds() {
        cy.getByTestId("change-refunds").click();
    }

    selectChangeWriteOff() {
        cy.getByTestId("change-write-off").click();
    }

    changePaymentScheduleNotShown(): this {
        cy.getByTestId("change-payment-schedule").should('not.exist');
        return this;
    }
    
    changeTrustLetterNotShown(): this {
        cy.getByTestId("change-trust-letter").should('not.exist');
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

    changeWriteOffNotShown(): this {
        cy.getByTestId("change-write-off").should('not.exist');
        return this;
    }

}


const pdgDashboard = new PDGDashboard();
export default pdgDashboard;
