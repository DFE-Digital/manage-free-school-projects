import { BaseEditPage } from '../../baseEditPage';

class EditPayment extends BaseEditPage {
    public withPaymentDueDate(day: string, month: string, year: string): this {
        const key = 'payment-due-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withPaymentActualDate(day: string, month: string, year: string): this {
        const key = 'payment-actual-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withPaymentDueAmount(comment: string): this {
        cy.getById('payment-due-amount').typeFast(comment);
        return this;
    }

    public withPaymentActualAmount(comment: string): this {
        cy.getById('payment-actual-amount').typeFast(comment);
        return this;
    }

    errorForPaymentDueDate(): this {
        this.errorTracking = 'payment-due-date';
        return this;
    }

    errorForPaymentDueAmount(): this {
        this.errorTracking = 'payment-due-amount';
        return this;
    }

    errorForPaymentActualDate(): this {
        this.errorTracking = 'payment-actual-date';
        return this;
    }

    errorForPaymentActualAmount(): this {
        this.errorTracking = 'payment-actual-amount';
        return this;
    }

    public clickDelete(): this {
        cy.getByTestId('delete').click();
        return this;
    }
}

const editPayment = new EditPayment();
export default editPayment;
