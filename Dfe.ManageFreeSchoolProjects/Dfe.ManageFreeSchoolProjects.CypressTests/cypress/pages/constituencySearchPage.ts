import validationComponent from './validationComponent';
import { BasePage } from './basePage';

class ConstituencySearchPage extends BasePage {
    public isSearchPage(): this {
        cy.get('#search-constituency').should('be.visible');
        return this;
    }

    public clickContinue(): this {
        cy.get('#search-constituency').closest('form').find('[data-testid="continue"]').click();
        return this;
    }

    public searchLabelIs(hint: string): this {
        cy.get(`[for="search-constituency"]`).should('contains.text', hint);
        return this;
    }

    public errorMessage(error: string): this {
        validationComponent.hasValidationError(error);
        return this;
    }

    public errorHint(error: string): this {
        cy.get('#search-constituency-error').should('contains.text', error);
        return this;
    }

    public enterSearch(searchTerm: string): this {
        cy.get('#search-constituency').should('be.visible').typeFast(searchTerm);
        cy.get('#search-constituency').should('have.value', searchTerm);
        return this;
    }
}

const constituencySearchPage = new ConstituencySearchPage();
export default constituencySearchPage;
