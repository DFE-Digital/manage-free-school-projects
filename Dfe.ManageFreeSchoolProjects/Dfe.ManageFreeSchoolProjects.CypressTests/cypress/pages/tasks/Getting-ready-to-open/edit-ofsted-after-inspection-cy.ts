import { BasePage } from '../../basePage';

class AfterInspectionEditPage extends BasePage {
    checkSharedOutcomeWithTrust(): this {
        cy.getById('shared-outcome-with-trust').check();
        return this;
    }

    checkProposedToOpenOnGias(): this {
        cy.getById('proposed-to-open-on-gias').check();
        return this;
    }
    checkSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').check();
        return this;
    }

    uncheckSharedOutcomeWithTrust(): this {
        cy.getById('shared-outcome-with-trust').uncheck();
        return this;
    }

    uncheckProposedToOpenOnGias(): this {
        cy.getById('proposed-to-open-on-gias').uncheck();
        return this;
    }
    uncheckSavedToWorkplaces(): this {
        cy.getById('saved-to-workplaces').uncheck();
        return this;
    }

    clearDateThatInspectionAndAnyActionsCompleted(): this {
        cy.getById('date-inspection-and-any-actions-completed').clear();
        return this;
    }

    enterDateThatInspectionAndAnyActionsCompleted(day: string, month: string, year: string): this {
        cy.enterDate('date-inspection-and-any-actions-completed', day, month, year);
        return this;
    }

    selectAnyActionsToMeetConditionCompletedOption(option: 'Yes' | 'No' | 'Not applicable'): this {
        cy.getByRadioOption(option).check();
        return this;
    }
}

const afterInspectionEditPage = new AfterInspectionEditPage();
export default afterInspectionEditPage;
