export class ProposalRow {
    constructor(private element: Element) {}

    public hasProposer(value: string): this {
        this.containsText('proposal-proposer', value);

        return this;
    }

    public hasName(value: string): this {
        this.containsText('proposal-name', value);

        return this;
    }

    public hasReligiousCharacterOrEthos(value: string): this {
        this.containsText('proposal-religious-character-or-ethos', value);

        return this;
    }

    public hasProposedFaithOfNewSchool(value: string): this {
        this.containsText('proposal-proposed-faith-of-new-school', value);

        return this;
    }

    public hasStatus(value: string): this {
        this.containsText('status', value);

        return this;
    }

    public hasViewLink(value: string): this {
        this.containsText('action', value);

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element).within(() => {
            cy.getByTestId(id).should('contain.text', value);
        });
    }
}