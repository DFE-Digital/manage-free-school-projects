import { BaseEditPage } from '../../baseEditPage';

class EditPaymentSchedule extends BaseEditPage {
    public withPaymentDueDate(day: string, month: string, year: string): this {
        const key = 'payment-due-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withPaymentActualDate(day: string, month: string, year: string): this {
        const key = 'actual-payment-date';
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

    errorForPaymentActualDate(): this {
        this.errorTracking = 'actual-payment-date';
        return this;
    }

    errorForPaymentDueAmount(): this {
        this.errorTracking = 'payment-due-amount';
        return this;
    }

    errorForPaymentActualAmount(): this {
        this.errorTracking = 'payment-actual-amount';
        return this;
    }
}

const editPaymentSchedule = new EditPaymentSchedule();
export default editPaymentSchedule;
