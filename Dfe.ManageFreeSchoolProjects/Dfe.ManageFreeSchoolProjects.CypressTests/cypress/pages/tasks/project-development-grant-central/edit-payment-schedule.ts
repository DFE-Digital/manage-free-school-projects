import { BasePage } from '../../basePage';

class EditPaymentSchedule extends BasePage {
    public checkNoPayments(): this {
        cy.getByTestId('payments-list').should('not.contain.class', 'govuk-summary-card');
        return this;
    }

    public checkPaymentAdded(index: string): this {
        cy.getByTestId('payment-added-banner').should('contain.text', `Payment ${index} added.`);
        return this;
    }

    public checkPaymentUpdated(index: string): this {
        cy.getByTestId('payment-updated-banner').should('contain.text', `Payment ${index} updated.`);
        return this;
    }

    public checkPaymentDeleted(): this {
        cy.getByTestId('payment-deleted-banner').should('contain.text', `Payment deleted.`);
        return this;
    }

    selectAddPayment() {
        cy.getByTestId('add-payment').click();
    }

    checkAddPaymentDoesExist() {
        cy.getByTestId('add-payment').should('exist');
    }

    checkAddPaymentDoesNotExist(): this {
        cy.getByTestId('add-payment').should('not.exist');
        return this;
    }

    selectEditPayment(index: string) {
        cy.getByTestId(`change-payment-${index}`).click();
    }

    editPaymentLinkNotShown(index: string): this {
        cy.getByTestId(`change-payment-${index}`).should('not.exist');
        return this;
    }
}

const editPaymentSchedule = new EditPaymentSchedule();
export default editPaymentSchedule;
