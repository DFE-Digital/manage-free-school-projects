import { BaseEditPage } from '../../baseEditPage';

class EditWriteOff extends BaseEditPage {
    public withWriteOffReason(comment: string): this {
        cy.getById('write-off-reason').typeFast(comment);
        return this;
    }

    public withWriteOffAmount(comment: string): this {
        cy.getById('write-off-amount').typeFast(comment);
        return this;
    }

    public withWriteOffDate(day: string, month: string, year: string): this {
        const key = 'write-off-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withFinanceBusinessPartnerApproval(comment: string): this {
        cy.getById('finance-partner').typeFast(comment);
        return this;
    }

    public withApprovalDate(day: string, month: string, year: string): this {
        const key = 'approval-date';
        this.setDate(key, day, month, year);
        return this;
    }

    public withIsWriteOff(value: string): this {
        cy.getById(`is-write-off-${value}`).click();
        return this;
    }

    public errorForWriteOffReason(): this {
        this.errorTracking = 'write-off-reason';
        return this;
    }

    public errorForWriteOffAmount(): this {
        this.errorTracking = 'write-off-amount';
        return this;
    }

    public errorForWriteOffDate(): this {
        this.errorTracking = 'write-off-date';
        return this;
    }

    public errorForFinanceBusinessPartnerApproval(): this {
        this.errorTracking = 'finance-partner';
        return this;
    }

    public errorForApprovalDate(): this {
        this.errorTracking = 'approval-date';
        return this;
    }
}

const editWriteOff = new EditWriteOff();
export default editWriteOff;
