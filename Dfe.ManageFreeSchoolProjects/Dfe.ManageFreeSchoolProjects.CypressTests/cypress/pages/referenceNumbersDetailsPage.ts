class ReferenceNumbersDetailsPage {
    private errorTracking = '';

    public titleIs(title: string): this {
        cy.getByTestId('title').should('contains.text', title);
        return this;
    }

    public schoolNameIs(school: string) {
        cy.getByTestId('school-name').should('contains.text', school);
        return this;
    }

    withProjectId(projectId: string): this {
        cy.getById('project-id').typeFast(projectId);
        return this;
    }

    clearProjectId(): this {
        cy.getById('project-id').clear();
        return this;
    }

    errorForProjectId(): this {
        this.errorTracking = 'project-id';
        return this;
    }

    withUrn(urn: string): this {
        cy.getById('urn').typeFast(urn);
        return this;
    }

    errorForUrn(): this {
        this.errorTracking = 'urn';
        return this;
    }

    showsError(error: string) {
        cy.get(`#${this.errorTracking}-error-link`).should('contain.text', error);

        cy.get(`#${this.errorTracking}-error-link`)
            .invoke('attr', 'href')
            .then((href) => {
                cy.get(href as string).should('exist');
            });

        cy.get(`#${this.errorTracking}-error`).should('contain.text', error);
        return this;
    }

    public clickContinue(): this {
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
}

const referenceNumbersDetailsPage = new ReferenceNumbersDetailsPage();

export default referenceNumbersDetailsPage;
