import { BaseEditPage } from '../../baseEditPage';

class EducationBriefEditPage extends BaseEditPage {
    checkTrustConfirmedPlansAndPoliciesInPlace(): this {
        cy.getById('trust-confirmed-education-plans-and-policies-in-place').check();
        return this;
    }

    checkCommissionedEEToReviewSafeguardingPolicy(): this {
        cy.getById('commissioned-ee-to-review-safeguarding-policy').check();
        return this;
    }

    checkCommissionedEEToReviewPupilAssessmentRecordingAndReportingPolicy(): this {
        cy.getById('commissioned-ee-to-pupil-assessment-recording-and-reporting-policy').check();
        return this;
    }

    checkSavedCopiesOfPlansAndPoliciesInWorkplaces(): this {
        cy.getById('saved-copies-of-plans-and-policies-in-workplaces').check();
        return this;
    }

    checkSavedEESpecificationAndAdviceInWorkplaces(): this {
        cy.getById('saved-ee-specification-and-advice-in-workplaces').check();
        return this;
    }

    uncheckTrustConfirmedPlansAndPoliciesInPlace(): this {
        cy.getById('trust-confirmed-education-plans-and-policies-in-place').uncheck();
        return this;
    }

    uncheckCommissionedEEToReviewSafeguardingPolicy(): this {
        cy.getById('commissioned-ee-to-review-safeguarding-policy').uncheck();
        return this;
    }

    uncheckCommissionedEEToReviewPupilAssessmentRecordingAndReportingPolicy(): this {
        cy.getById('commissioned-ee-to-pupil-assessment-recording-and-reporting-policy').uncheck();
        return this;
    }

    uncheckSavedCopiesOfPlansAndPoliciesInWorkplaces(): this {
        cy.getById('saved-copies-of-plans-and-policies-in-workplaces').uncheck();
        return this;
    }

    uncheckSavedEESpecificationAndAdviceInWorkplaces(): this {
        cy.getById('saved-ee-specification-and-advice-in-workplaces').uncheck();
        return this;
    }

    public withDateTrustProvidedEducationBrief(day: string, month: string, year: string): this {
        const key = 'date-trust-provided-education-brief';
        this.setDate(key, day, month, year);
        return this;
    }

    public withDateEEReviewedEducationBrief(day: string, month: string, year: string): this {
        const key = 'date-ee-reviewed-education-brief';
        this.setDate(key, day, month, year);
        return this;
    }

    errorForDateTrustProvidedEducationBrief(): this {
        this.errorTracking = 'date-trust-provided-education-brief';
        return this;
    }

    errorForDateEEReviewedEducationBrief(): this {
        this.errorTracking = 'date-ee-reviewed-education-brief';
        return this;
    }

    public MarkAsComplete() {
        cy.getById('mark-as-completed').click();
        return this;
    }

    public clickConfirmAndContinue() {
        cy.getByTestId('confirm').click();
    }
}

const educationBriefEditPage = new EducationBriefEditPage();
export default educationBriefEditPage;
