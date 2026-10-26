import { BasePage } from '../basePage';
import validationComponent from '../validationComponent';

class CreateProposalPage extends BasePage {
    public back(): this {
        cy.getByTestId('back').click();
        return this;
    }

    public addProposal(): this {
        cy.contains('Add a proposal').click();
        return this;
    }

    public continue(): this {
        cy.get('[data-testid="continue"]').click();
        return this;
    }

    public clickCreateProposal() {
        cy.getByTestId('create-proposal').click();

        // Wait for the redirect back to the proposal list. Every test ends on this call, and
        // without the wait the test finishes while the POST is still in flight: Cypress's
        // test isolation then resets the page, which can abort the write and leaves the next
        // test's `cy.visit` racing it.
        cy.getById('add-proposal').should('be.visible');

        return this;
    }
    
    public errorMessage(error: string): this {
        validationComponent.hasValidationError(error);
        validationComponent.hasLinkedValidationError(error);
        return this;
    }

    public selectOption(option: string): this {
        cy.getByRadioOption(option).check();
        return this;
    }

    public selectProposer(
        proposer:
            | 'AcademyTrust'
            | 'Diocese'
            | 'AnotherReligiousOrganisation'
            | 'LocalAuthorityThatPushedSpecification'
            | 'AnotherLocalAuthority'
            | 'JointProposal'
    ): this {
        cy.getByTestId(proposer).check();
        return this;
    }

    public confirmTrust(option: 'Yes' | 'No'): this {
        cy.getByTestId(`confirm-trust-${option.toLowerCase()}`).check();
        return this;
    }

    public enterTRN(value: string): this {
        this.typeInto('trn', value);
        return this;
    }

    public enterDioceseName(value: string): this {
        this.typeInto('name-of-diocese', value);
        return this;
    }

    public enterOtherReligiousOrganisationName(value: string): this {
        this.typeInto('name-of-other-religious-organisation', value);
        return this;
    }

    public hasCorrectTrustName(trustName: string) {
        cy.getByTestId('trust-name').should('contain.text', trustName);
        return this;
    }
    public hasCorrectTrustType(trustType: string) {
        cy.getByTestId('trust-type').should('contain.text', trustType);
        return this;
    }


    private typeInto(testId: string, value: string) {
        const selector = `[data-testid="${testId}"]`;

        cy.get(selector).should('be.visible').and('be.enabled').clear();

        if (value !== '') {
            cy.get(selector).should('be.visible').type(value);
        }
    }
}

const createProposalPage = new CreateProposalPage();

export default createProposalPage;