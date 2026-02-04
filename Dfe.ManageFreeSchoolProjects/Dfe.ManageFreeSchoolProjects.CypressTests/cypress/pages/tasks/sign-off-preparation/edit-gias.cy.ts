import { BasePage } from '../../basePage';

class GiasEditPage extends BasePage {
    checkCheckTrustInformation(): this {
        cy.getById('checked-trust-information').check();
        return this;
    }

    checkApplicationFormSent(): this {
        cy.getById('application-form-sent').check();
        return this;
    }

    checkCopySavedToWorkspaces(): this {
        cy.getById('saved-to-workplaces').check();
        return this;
    }

    checkSentTrustURN(): this {
        cy.getById('urn-Sent').check();
        return this;
    }

    unCheckCheckTrustInformation(): this {
        cy.getById('checked-trust-information').uncheck();
        return this;
    }

    unCheckApplicationFormSent(): this {
        cy.getById('application-form-sent').uncheck();
        return this;
    }

    unCheckCopySavedToWorkspaces(): this {
        cy.getById('saved-to-workplaces').uncheck();
        return this;
    }

    unCheckSentTrustURN(): this {
        cy.getById('urn-Sent').uncheck();
        return this;
    }

    public MarkAsComplete() {
        cy.getById('mark-as-completed').click();
        return this;
    }

    public clickConfirmAndContinue() {
        cy.getByTestId('confirm').click();
    }
}

const giasEditPage = new GiasEditPage();
export default giasEditPage;
