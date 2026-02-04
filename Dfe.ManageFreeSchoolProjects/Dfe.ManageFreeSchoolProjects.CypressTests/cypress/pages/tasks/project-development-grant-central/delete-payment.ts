import { BasePage } from '../../basePage';

class DeletePayment extends BasePage {
    public clickDelete(): this {
        cy.getByTestId('delete').click();
        return this;
    }

    public clickNo(): this {
        cy.getByTestId('payment-schedule-link').click();
        return this;
    }
}

const deletePayment = new DeletePayment();
export default deletePayment;
