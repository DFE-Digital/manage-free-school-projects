import { BaseEditPage } from '../../baseEditPage';

class EditRefunds extends BaseEditPage {
    public withLatestRefundDate(day: string, month: string, year: string): this {
        const key = 'latest-refund-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withTotalAmount(value: string): this {
        cy.getById('total-amount').typeFast(value);
        return this;
    }

    errorForLatestRefundDate(): this {
        this.errorTracking = 'latest-refund-date';
        return this;
    }

    errorForTotalAmount(): this {
        this.errorTracking = 'total-amount';
        return this;
    }
}

const editRefunds = new EditRefunds();
export default editRefunds;
