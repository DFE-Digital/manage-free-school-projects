export class BasePage {
    public titleIs(title: string): this {
        cy.getByTestId('title').should('contains.text', title);
        return this;
    }

    public schoolNameIs(school: string): this {
        cy.getByTestId('school-name').should('contains.text', school);
        return this;
    }

    public clickContinue(): this {
        cy.getByTestId('continue').click();
        return this;
    }

    public clickBack(): this {
        cy.get('.govuk-back-link').click();
        return this;
    }

    protected setDate(key: string, day: string, month: string, year: string): void {
        cy.get('#' + `${key}-day`).typeFast(day);
        cy.get('#' + `${key}-month`).typeFast(month);
        cy.get('#' + `${key}-year`).typeFast(year);
    }
}
