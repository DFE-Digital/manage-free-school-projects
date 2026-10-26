import { ProposalRow } from './proposalRow';

class ProposalsTable {
    public getRowByProposalId(proposalId: string): Cypress.Chainable<ProposalRow> {
        return cy.getByTestId(`row-${proposalId}`).then((el) => {
            return new ProposalRow(el);
        });
    }

    public getRows(): Cypress.Chainable<ProposalRow> {
        return cy.containsByTestId(`row-`).then((el) => {
            return new ProposalRow(el);
        });
    }

    public getRowByProposer(proposer: string): Cypress.Chainable<ProposalRow> {
        const escaped = proposer.replace(/[.*+?^${}()|[\]\\]/g, String.raw`\$&`);

        return cy
            .contains('[data-testid="proposal-proposer"]', new RegExp(String.raw`^\s*${escaped}\s*$`))
            .parents('[data-testid*="row-"]')
            .then(($row) => {
                return new ProposalRow($row[0]);
            });
    }

    public getFirstRow(): Cypress.Chainable<ProposalRow> {
        return cy
            .containsByTestId(`row-`)
            .first()
            .then((el) => {
                return new ProposalRow(el);
            });
    }

    public hasRowCount(expected: number): this {
        cy.get('[data-testid*="row-"]').should('have.length', expected);
        return this;
    }
}

const proposalsTable = new ProposalsTable();

export default proposalsTable;