import { BaseEditPage } from './baseEditPage';

class ReferenceNumbersDetailsPage extends BaseEditPage {
    // Override the base clickContinue with custom behavior
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
}

const referenceNumbersDetailsPage = new ReferenceNumbersDetailsPage();

export default referenceNumbersDetailsPage;
