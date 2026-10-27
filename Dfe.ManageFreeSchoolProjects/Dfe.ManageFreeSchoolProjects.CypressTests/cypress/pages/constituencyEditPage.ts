import { BasePage } from './basePage';

const noneOption = 'None of the above - I want to search again';

class ConstituencyEditPage extends BasePage {
    public isResultsPage(): this {
        cy.get('#constituency').should('exist');
        return this;
    }

    // Scoped to this page's form so it cannot click the search page's Continue button.
    public clickContinue(): this {
        cy.get('#constituency').closest('form').find('[data-testid="continue"]').click();
        return this;
    }

    hasResult(result: string): this {
        cy.get('#constituency').contains('label', result).should('exist');
        return this;
    }

    hasNoneOption(): this {
        cy.get('#constituency').contains('label', noneOption).should('exist');
        return this;
    }

    selectNoneOption(): this {
        return this.selectOption(noneOption);
    }

    selectOption(option: string): this {
        cy.getByRadioOption(option).check();
        return this;
    }

    clickSearchAgain(): this {
        cy.getByTestId('search-again').click();
        return this;
    }
}

const constituencyEditPage = new ConstituencyEditPage();
export default constituencyEditPage;
