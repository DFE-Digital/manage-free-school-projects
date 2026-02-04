import { BaseEditPage } from '../../baseEditPage';

class EditTotalGrant extends BaseEditPage {
    public withTotalGrantAmount(comment: string): this {
        cy.getById('total-grant-amount').typeFast(comment);
        return this;
    }

    errorForPaymentActualAmount(): this {
        this.errorTracking = 'total-grant-amount';
        return this;
    }
}

const editTotalGrant = new EditTotalGrant();
export default editTotalGrant;
