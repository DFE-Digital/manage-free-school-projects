import { BaseEditPage } from '../../baseEditPage';

class EditStopPayments extends BaseEditPage {
    public paymentStoppedDateIsVisible(): this {
        cy.getById('payment-stopped-date').should('be.visible');
        return this;
    }

    public paymentStoppedDateIsNotVisible(): this {
        cy.getById('payment-stopped-date').should('be.not.visible');
        return this;
    }

    public withPaymentStopped(value: string): this {
        cy.getById(`payment-stopped-${value}`).click();
        return this;
    }

    public withPaymentStoppedDate(day: string, month: string, year: string): this {
        const key = 'payment-stopped-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public paymentStoppedDateIsBlank(): this {
        const key = 'payment-stopped-date';
        cy.get('#' + `${key}-day`).should('be.empty');
        cy.get('#' + `${key}-month`).should('be.empty');
        cy.get('#' + `${key}-year`).should('be.empty');
        return this;
    }

    errorForPaymentStoppedDate(): this {
        this.errorTracking = 'payment-stopped-date';
        return this;
    }
}

const editStopPayments = new EditStopPayments();
export default editStopPayments;
