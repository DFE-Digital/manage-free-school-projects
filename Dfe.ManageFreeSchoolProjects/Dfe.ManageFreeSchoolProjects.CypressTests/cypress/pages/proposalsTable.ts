import { ProposalRow } from './proposalRow';

const ROW_SELECTOR = '[data-testid*="row-"]';

class ProposalsTable {
    public getRowByProposalId(proposalId: string): Cypress.Chainable<ProposalRow> {
        const selector = `[data-testid="row-${proposalId}"]`;

        return cy.wrap(new ProposalRow(() => cy.get(selector)), { log: false });
    }

    public getRows(): Cypress.Chainable<ProposalRow> {
        return cy.wrap(new ProposalRow(() => cy.get(ROW_SELECTOR).first()), { log: false });
    }

    public getRowByProposer(proposer: string): Cypress.Chainable<ProposalRow> {
        const escaped = proposer.replace(/[.*+?^${}()|[\]\\]/g, String.raw`\$&`);
        const exactProposer = new RegExp(String.raw`^\s*${escaped}\s*$`);

        return cy.wrap(
            new ProposalRow(() =>
                cy.contains('[data-testid="proposal-proposer"]', exactProposer).parents(ROW_SELECTOR).first()
            ),
            { log: false }
        );
    }

    public getFirstRow(): Cypress.Chainable<ProposalRow> {
        return cy.wrap(new ProposalRow(() => cy.get(ROW_SELECTOR).first()), { log: false });
    }

    public hasRowCount(expected: number): this {
        cy.get(ROW_SELECTOR).should('have.length', expected);
        return this;
    }
}

const proposalsTable = new ProposalsTable();

export default proposalsTable;
