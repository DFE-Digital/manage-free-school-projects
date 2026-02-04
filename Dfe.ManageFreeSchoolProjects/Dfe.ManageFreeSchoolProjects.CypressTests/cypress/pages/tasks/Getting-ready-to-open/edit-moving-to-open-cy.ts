import { BaseEditPage } from '../../baseEditPage';

class MovingToOpenEditPage extends BaseEditPage {
    checkSentToSfso(): this {
        cy.getById('project-brief-to-sfso').check();
        return this;
    }

    checkSentToEducationEstates(): this {
        cy.getById('project-brief-to-education-estates').check();
        return this;
    }

    checkSentToDeliverOfficer(): this {
        cy.getById('project-brief-to-new-delivery-officer').check();
        return this;
    }

    checkEmailSentToRelevantContact(): this {
        cy.getById('sent-emails-to-relevant-contacts').check();
        return this;
    }

    checkEmailSentSchoolPrincipal(): this {
        cy.getById('sent-emails-to-schools-principle').check();
        return this;
    }

    checkSavedToWorkspacesProjectBrief(): this {
        cy.getById('saved-to-workplaces-folder-project-brief').check();
        return this;
    }

    checkSavedToWorkspacesAnnexB(): this {
        cy.getById('saved-to-workplaces-folder-annexb').check();
        return this;
    }

    checkSavedToWorkspacesAnnexE(): this {
        cy.getById('saved-to-workplaces-folder-annexe').check();
        return this;
    }

    uncheckSentToSfso(): this {
        cy.getById('project-brief-to-sfso').uncheck();
        return this;
    }

    uncheckSentToEducationEstates(): this {
        cy.getById('project-brief-to-education-estates').uncheck();
        return this;
    }

    uncheckSentToDeliverOfficer(): this {
        cy.getById('project-brief-to-new-delivery-officer').uncheck();
        return this;
    }

    uncheckEmailSentToRelevantContact(): this {
        cy.getById('sent-emails-to-relevant-contacts').uncheck();
        return this;
    }

    uncheckEmailSentSchoolPrincipal(): this {
        cy.getById('sent-emails-to-schools-principle').uncheck();
        return this;
    }

    uncheckSavedToWorkspacesProjectBrief(): this {
        cy.getById('saved-to-workplaces-folder-project-brief').uncheck();
        return this;
    }

    uncheckSavedToWorkspacesAnnexB(): this {
        cy.getById('saved-to-workplaces-folder-annexb').uncheck();
        return this;
    }

    uncheckSavedToWorkspacesAnnexE(): this {
        cy.getById('saved-to-workplaces-folder-annexe').uncheck();
        return this;
    }

    withActualOpeningDate(day: string, month: string, year: string): this {
        const key = 'actual-opening-date';
        this.setDate(key, day, month, year);
        return this;
    }

    errorForActualStartDate(error: string): this {
        cy.getById('actual-opening-date-error').contains(error);
        return this;
    }
}

const movingToOpenEditPage = new MovingToOpenEditPage();
export default movingToOpenEditPage;
