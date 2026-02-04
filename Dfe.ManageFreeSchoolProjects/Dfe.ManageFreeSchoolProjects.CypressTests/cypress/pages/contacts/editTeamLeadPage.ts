import { BasePage } from '../basePage';

class EditTeamLeadPage extends BasePage {
    public hasTeamLeadTitle(value: string): this {
        cy.getByTestId(`edit-team-lead-title`).should('contain.text', value);

        return this;
    }

    withTeamLeadName(value: string): this {
        cy.getByTestId('edit-team-lead-name').clear().type(value);
        return this;
    }

    withTeamLeadEmail(value: string): this {
        cy.getByTestId('edit-team-lead-email').clear().type(value);
        return this;
    }

    errorForTeamLeadName(error: string): this {
        cy.getById('team-lead-name-error').contains(error);
        return this;
    }
    errorForTeamLeadEmail(error: string): this {
        cy.getById('team-lead-email-error').contains(error);
        return this;
    }
}

const editTeamLeadPage = new EditTeamLeadPage();

export default editTeamLeadPage;
