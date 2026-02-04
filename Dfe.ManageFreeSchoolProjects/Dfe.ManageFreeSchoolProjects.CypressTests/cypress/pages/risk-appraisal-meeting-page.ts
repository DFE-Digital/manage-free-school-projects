import { BaseEditPage } from './baseEditPage';

class RiskAppraisalMeetingEditPage extends BaseEditPage {
    withInitialRiskAppraisalMeetingCompleted(setting: 'Yes' | 'No'): this {
        const control = 'Initial risk appraisal meeting completed';
        cy.contains(control)
            .parent()
            .contains(setting)
            .invoke('attr', 'for')
            .then((id) => {
                cy.get('#' + id).click();
            });
        return this;
    }

    withForecastDate(day: string, month: string, year: string): this {
        const key = 'forecast-date';
        this.setDate(key, day, month, year);
        return this;
    }

    withActualDate(day: string, month: string, year: string): this {
        const key = 'actual-date';
        this.setDate(key, day, month, year);
        return this;
    }

    withComments(comment: string): this {
        cy.getByTestId('comments-on-decision').typeFast(comment);
        return this;
    }

    withReason(reason: string): this {
        cy.getByTestId('reason-not-applicable').typeFast(reason);
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'comments-on-decision';
        return this;
    }

    errorForReason(): this {
        this.errorTracking = 'reason-not-applicable';
        return this;
    }

    errorForForecastDate(): this {
        this.errorTracking = 'forecast-date';
        return this;
    }

    errorForActualDate(): this {
        this.errorTracking = 'actual-date';
        return this;
    }
}

const riskAppraisalMeetingEditPage = new RiskAppraisalMeetingEditPage();
export default riskAppraisalMeetingEditPage;
