const invalidTRNString = 'POTATO';
const invalidTRNStringWithSpaces = 'P O T A T O';
const invalidTRNNumbersString = '1234567';
const nonExistentTrustId = 'TR09999';

const SQLInjectionAttempt = "' OR 1=1";
const crossSiteScriptingAttempt = "<script>window.alert('Hello World!')</script>";

class TrustDetailsPage {
    public selectSaveAndContinue(): this {
        cy.get('body').then(($body) => {
            if ($body.find('[data-testid="continue"]').length) {
                cy.getByTestId('continue').click();
                return;
            }

            if ($body.find('button.govuk-button:contains("Save and continue")').length) {
                cy.contains('button.govuk-button', 'Save and continue').click();
                return;
            }

            cy.contains('button.govuk-button', 'Continue').click();
        });
        return this;
    }

    public verifyValidationMessagesWhenNoDataSet(): this {
        cy.getById('trn-error').should('be.visible').contains('Enter the TRN');

        return this;
    }

    public enterInvalidTRNStringInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(invalidTRNString);

        return this;
    }

    public enterInvalidTRNStringWithSpacesInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(invalidTRNStringWithSpaces);

        return this;
    }

    public enterInvalidTRNNumbersStringInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(invalidTRNNumbersString);

        return this;
    }

    public enterNonExistentTrustIdInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(nonExistentTrustId);

        return this;
    }

    public enterSQLInjectionAttemptInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(SQLInjectionAttempt);

        return this;
    }

    public enterCrossSiteScriptingAttemptInTRNPage(): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(crossSiteScriptingAttempt);

        return this;
    }

    public enterValidTrustId(validTrustId: string): this {
        cy.getById('trn').click();
        cy.getById('trn').typeFast(validTrustId);

        return this;
    }

    public verifyValidationMessagesWhenInvalidTRNFormatEntered(): this {
        cy.getById('trn-error')
            .should('be.visible')
            .contains('The TRN must start with the letters TR, followed by at least 5 numbers');

        return this;
    }

    public verifyValidationMessagesWhenTRNTooLongEntered(): this {
        cy.getById('trn-error')
            .should('be.visible')
            .contains('TRN (trust reference number) must be 7 characters or less');

        return this;
    }

    public verifyValidationMessagesWhenNonExistentTRNEntered(): this {
        cy.getById('trn-error').should('be.visible').contains('Trust ID not found. Enter a different ID');

        return this;
    }
}

const trustDetailsPage = new TrustDetailsPage();

export default trustDetailsPage;
