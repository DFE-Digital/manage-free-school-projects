import { BaseEditPage } from '../../baseEditPage';

class PupilNumbersChecksEditPage extends BaseEditPage {
    checkReceivedEnoughApplications(): this {
        cy.getById('school-received-enough-applications').check();
        return this;
    }

    checkDataMatchesFundingAgreement(): this {
        cy.getById('capacity-data-matches-funding-agreement').check();
        return this;
    }

    checkDataMatchesGiasRegistration(): this {
        cy.getById('capacity-data-matches-gias-registration').check();
        return this;
    }

    uncheckReceivedEnoughApplications(): this {
        cy.getById('school-received-enough-applications').uncheck();
        return this;
    }

    uncheckDataMatchesFundingAgreement(): this {
        cy.getById('capacity-data-matches-funding-agreement').uncheck();
        return this;
    }

    uncheckDataMatchesGiasRegistration(): this {
        cy.getById('capacity-data-matches-gias-registration').uncheck();
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'comments';
        return this;
    }
}

const pupilNumbersChecksEditPage = new PupilNumbersChecksEditPage();
export default pupilNumbersChecksEditPage;
