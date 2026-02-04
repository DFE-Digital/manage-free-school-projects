import { BasePage } from '../../basePage';

class EqualitiesAssessmentEditPage extends BasePage {
    public checkCompletedEqualitiesProcessRecord(): this {
        cy.getByTestId('completed-the-equalities-process-record').click();
        return this;
    }

    public checkSavedEPRInWorkplacesFolder(): this {
        cy.getByTestId('saved-epr-in-workplaces-folder').click();
        return this;
    }
}

const equalitiesAssessmentEditPage = new EqualitiesAssessmentEditPage();
export default equalitiesAssessmentEditPage;
