import { BasePage } from './basePage';

export class BaseEditPage extends BasePage {
    protected errorTracking = '';

    public showsError(error: string): this {
        cy.get(`#${this.errorTracking}-error-link`).should('contain.text', error);

        cy.get(`#${this.errorTracking}-error-link`)
            .invoke('attr', 'href')
            .then((href) => {
                cy.get(href as string).should('exist');
            });

        cy.get(`#${this.errorTracking}-error`).should('contain.text', error);
        return this;
    }
}
