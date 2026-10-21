import dateComponent from '../dateComponent';
import validationComponent from '../validationComponent';

class CreateProjectPage {
    public errorMessage(error: string): this {
        validationComponent.hasValidationError(error);
        validationComponent.hasLinkedValidationError(error);
        return this;
    }

    public selectOption(method: string): this {
        cy.getByRadioOption(method).check();
        return this;
    }

    public isOptionChecked(method: string): this {
        cy.getByRadioOption(method).should('be.checked');
        return this;
    }

    public titleIs(title: string) {
        cy.getByTestId('title-heading').should('contain.text', title);
        return this;
    }

    public enterProjectId(value: string) {
        this.typeInto('projectid', value);
        return this;
    }

    public enterApplicationNumber(value: string) {
        this.typeInto('application-number', value);
        return this;
    }

    public enterApplicationWave(value: string) {
        this.typeInto('application-wave', value);
        return this;
    }

    public hasProjectId(value: string) {
        cy.getByTestId('projectid').should('have.value', value);
        return this;
    }

    public projectIDErrorMessage(error: string): this {
        cy.getById('projectid-error').should('contain.text', error);
        return this;
    }

    public enterSchoolName(value: string): this {
        this.typeInto('school', value);
        return this;
    }

    public hasSchoolName(value: string): this {
        cy.getByTestId('school').should('have.value', value);
        return this;
    }

    public enterTRN(value: string): this {
        this.typeInto('trn', value);
        return this;
    }

    public withLocalAuthority(localAuthority: string): this {
        cy.getByTestId(localAuthority).check();

        return this;
    }

    public hasSchoolType(value: string): this {
        cy.getByTestId(value).should('be.checked');

        return this;
    }

    public withSchoolType(value: string): this {
        cy.getByTestId(value).check();

        return this;
    }

    public setNurseryTo(option: 'Yes' | 'No'): this {
        cy.getByTestId(`nursery-${option}`).check();
        return this;
    }

    public hasNursery(value: string): this {
        cy.getByTestId(`nursery-${value}`).should('be.checked');
        return this;
    }

    public setSixthFormTo(option: 'Yes' | 'No'): this {
        cy.getByTestId(`sixth-form-${option}`).check();
        return this;
    }

    public hasSixthForm(value: string): this {
        cy.getByTestId(`sixth-form-${value}`).should('be.checked');
        return this;
    }

    public setAlternativeProvisionTo(option: 'Yes' | 'No'): this {
        cy.getByTestId(`alternative-provision-${option}`).check();
        return this;
    }

    public hasAlternativeProvision(value: string): this {
        cy.getByTestId(`alternative-provision-${value}`).should('be.checked');
        return this;
    }

    public setSpecialEducationNeedsTo(option: 'Yes' | 'No'): this {
        cy.getByTestId(`special-education-needs-${option}`).check();
        return this;
    }

    public setResidentialOrBoarding(option: 'Yes' | 'No'): this {
        cy.getByTestId(`residential-or-boarding-${option}`).check();
        return this;
    }

    public hasSpecialEducationNeeds(value: string): this {
        cy.getByTestId(`special-education-needs-${value}`).should('be.checked');
        return this;
    }

    public enterAgeRangeFrom(value: string): this {
        this.typeInto('age-range-from', value);
        return this;
    }

    public enterAgeRangeTo(value: string): this {
        this.typeInto('age-range-to', value);
        return this;
    }

    public hasAgeRangeFrom(value: string): this {
        cy.getByTestId('age-range-from').should('have.value', value);
        return this;
    }

    public hasAgeRangeTo(value: string): this {
        cy.getByTestId('age-range-to').should('have.value', value);
        return this;
    }

    public enterNurseryCapacity(value: string): this {
        this.typeInto('nursery-capacity', value);

        return this;
    }

    public hasNurseryCapacity(value: string): this {
        this.getNurseryCapacity().should('have.value', value);
        return this;
    }

    public hasNoNurseryCapacity(): this {
        this.getNurseryCapacity().should('not.exist');
        return this;
    }

    public enterReceptionToYear6(value: string): this {
        this.typeInto('yr-y6-capacity', value);
        return this;
    }

    public hasReceptionToYear6(value: string): this {
        cy.getByTestId('yr-y6-capacity').should('have.value', value);
        return this;
    }

    public enterYear7ToYear11(value: string): this {
        this.typeInto('y7-y11-capacity', value);
        return this;
    }

    public hasYear7ToYear11(value: string): this {
        cy.getByTestId('y7-y11-capacity').should('have.value', value);
        return this;
    }

    public enterYear12ToYear14(value: string): this {
        this.typeInto('y12-y14-capacity', value);
        return this;
    }

    public hasYear12ToYear14(value: string): this {
        cy.getByTestId('y12-y14-capacity').should('have.value', value);
        return this;
    }

    public enterFormsOfEntry(value: string): this {
        cy.get('#forms-of-entry').clear().type(value);
        return this;
    }

    public hasFormsOfEntry(value: string): this {
        cy.get('#forms-of-entry').should('have.value', value);
        return this;
    }

    public enterOtherFaith(value: string): this {
        this.typeInto('other-faith-type', value);
        return this;
    }

    public hasOtherFaith(value: string): this {
        cy.getByTestId('other-faith-type').should('have.value', value);
        return this;
    }

    public setProvisionalOpeningDate(day: string, month: string, year: string): this {
        const key = 'provisional-opening-date';
        dateComponent.setDate(key, day, month, year);
        return this;
    }

    public hasProvisionalOpeningDate(day: string, month: string, year: string): this {
        const key = 'provisional-opening-date';
        dateComponent.checkDate(key, day, month, year);
        return this;
    }

    public hasFaithStatus(value: string): this {
        cy.getByTestId(value).should('be.checked');
        return this;
    }

    // The create journey's "assign this project to" page only collects an email address;
    // the name field it used to have was removed from ProjectAssignedTo.cshtml.
    public enterProjectAssignedToEmail(value: string) {
        this.typeInto('email', value);
        return this;
    }

    public hasProjectAssignedEmail(value: string) {
        cy.getByTestId('email').should('have.value', value);
        return this;
    }

    // Uses `cy.get` rather than the `getByTestId` custom command so the button is a
    // requeryable subject. Two consecutive `continue()` calls span a navigation, and a
    // non-requeryable subject means the second click lands on the outgoing page's button.
    public continue(): this {
        cy.get('[data-testid="continue"]').click();
        return this;
    }

    public back(): this {
        cy.getByTestId('back').click();
        return this;
    }

    public clickCreateProject() {
        cy.getByTestId('create-project').click();
        return this;
    }

    public hasConfirmedProjectId(projectId: string) {
        cy.getByTestId('created-project-id').should('contain.text', projectId);
        return this;
    }

    public hasCorrectTrustName(trustName: string) {
        cy.getByTestId('trust-name').should('contain.text', trustName);
        return this;
    }
    public hasCorrectTrustType(trustType: string) {
        cy.getByTestId('trust-type').should('contain.text', trustType);
        return this;
    }

    public hasConfirmedEmailMessage(value: string) {
        cy.getByTestId('confirmation-email').should('contain.text', value);
        return this;
    }

    private getNurseryCapacity() {
        return cy.getByTestId('nursery-capacity');
    }

    // Each command gets its own `cy.get` so the input is re-queried every time.
    // `getByTestId` is a custom command rather than a query, so Cypress cannot requery its
    // subject; a `getByTestId(...).clear().type(...)` chain run while the page is still
    // settling after a `continue()` therefore acts on a stale node. Cypress reports that
    // inconsistently — sometimes "detached from the DOM", sometimes the misleading
    // "cy.type() failed because it targeted a disabled element".
    // An empty value clears the field without typing, which is what the capacity fields want.
    private typeInto(testId: string, value: string) {
        const selector = `[data-testid="${testId}"]`;

        // The `should` matters as much as the split: it keeps re-running the query until the
        // input is actually live in the current document, so a type() issued straight after a
        // continue() cannot act on the outgoing page's node while it is being torn down.
        cy.get(selector).should('be.visible').and('be.enabled').clear();

        if (value !== '') {
            cy.get(selector).should('be.visible').type(value);
        }
    }
}

const createProjectPage = new CreateProjectPage();

export default createProjectPage;
