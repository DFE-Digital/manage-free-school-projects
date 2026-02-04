import { BaseEditPage } from '../../baseEditPage';

class ApplicationsEvidenceEditPage extends BaseEditPage {
    checkConfirmedPupilNumbers(): this {
        cy.getById('confirmed-pupil-numbers').check();
        return this;
    }

    checkBuildUpFormSavedToWorkplaces(): this {
        cy.getById('build-up-form-in-workplaces').check();
        return this;
    }

    checkUnderWritingAgreementSavedToWorkplaces(): this {
        cy.getById('underwriting-agreement-in-workplaces').check();
        return this;
    }

    uncheckConfirmedPupilNumbers(): this {
        cy.getById('confirmed-pupil-numbers').uncheck();
        return this;
    }

    uncheckBuildUpFormSavedToWorkplaces(): this {
        cy.getById('build-up-form-in-workplaces').uncheck();
        return this;
    }

    uncheckUnderWritingAgreementSavedToWorkplaces(): this {
        cy.getById('underwriting-agreement-in-workplaces').uncheck();
        return this;
    }
    withComments(comment: string): this {
        cy.getById('comments').typeFast(comment);
        return this;
    }

    clearComments(): this {
        cy.getById('comments').clear();
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'comments';
        return this;
    }
}

const applicationsEvidenceEditPage = new ApplicationsEvidenceEditPage();
export default applicationsEvidenceEditPage;
