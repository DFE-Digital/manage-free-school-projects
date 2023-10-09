class ProjectOverviewPage {

    public selectTaskListTab(): this {
        cy.contains("Task list").click()
        
        return this;
    }

    public selectDatesFromTaskList(): this {
        cy.contains("Dates").click()
        return this;
    }

    public hasProjectTitleHeader(value: string): this {
        cy.getByTestId("project-title-header").should("contain.text", value);

        return this;
    }

    public hasCurrentFreeSchoolName(value: string): this {
        cy.getByTestId(`free-school-name`).should(`contain.text`, value);

        return this;
    }

    public hasProjectStatus(value: string): this {
        cy.getByTestId(`project-status`).should(`contain.text`, value);

        return this;
    }

    public hasApplicationNumber(value: string): this {
        cy.getByTestId(`application-number`).should(`contain.text`, value);

        return this;
    }

    public hasProjectId(value: string): this {
        cy.getByTestId(`project-id`).should(`contain.text`, value);

        return this;
    }

    public hasUrn(value: string): this {
        cy.getByTestId(`urn`).should(`contain.text`, value);

        return this;
    }

    public hasApplicationWave(value: string): this {
        cy.getByTestId(`application-wave`).should(`contain.text`, value);

        return this;
    }

    public hasRealisticYearOfOpening(value: string): this {
        cy.getByTestId(`realistic-year-of-opening`).should(
            `contain.text`,
            value,
        );

        return this;
    }

    public hasDateOfEntryIntoPreopening(value: string): this {
        cy.getByTestId(`date-of-entry-into-preopening`).should(
            `contain.text`,
            value,
        );

        return this;
    }

    public hasProvisionalOpeningDateAgreedWithTrust(value: string): this {
        cy.getByTestId(`provisional-opening-date-agreed-with-trust`).should(
            `contain.text`,
            value,
        );

        return this;
    }

    public hasActualOpeningDate(value: string): this {
        cy.getByTestId(`actual-opening-date`).should(`contain.text`, value);

        return this;
    }

    public hasOpeningAcademicYear(value: string): this {
        cy.getByTestId(`opening-academic-year`).should(`contain.text`, value);

        return this;
    }

    public hasLocalAuthority(value: string): this {
        cy.getByTestId(`local-authority`).should(`contain.text`, value);

        return this;
    }

    public hasRegion(value: string): this {
        cy.getByTestId(`region`).should(`contain.text`, value);

        return this;
    }

    public hasConstituency(value: string): this {
        cy.getByTestId(`constituency`).should(`contain.text`, value);

        return this;
    }

    public hasConstituencyMp(value: string): this {
        cy.getByTestId(`constituency-mp`).should(`contain.text`, value);

        return this;
    }

    public hasNumberOfFormsOfEntry(value: string): this {
        cy.getByTestId(`number-of-forms-of-entry`).should(
            `contain.text`,
            value,
        );

        return this;
    }

    public hasSchoolType(value: string): this {
        cy.getByTestId(`school-type`).should(`contain.text`, value);

        return this;
    }

    public hasSchoolPhase(value: string): this {
        cy.getByTestId(`school-phase`).should(`contain.text`, value);

        return this;
    }

    public hasAgeRange(value: string): this {
        cy.getByTestId(`age-range`).should(`contain.text`, value);

        return this;
    }

    public hasGender(value: string): this {
        cy.getByTestId(`gender`).should(`contain.text`, value);

        return this;
    }

    public hasNursery(value: string): this {
        cy.getByTestId(`nursery`).should(`contain.text`, value);

        return this;
    }

    public hasSixthForm(value: string): this {
        cy.getByTestId(`sixth-form`).should(`contain.text`, value);

        return this;
    }

    public hasIndependentConverter(value: string): this {
        cy.getByTestId(`independent-converter`).should(`contain.text`, value);

        return this;
    }

    public hasSpecialistResourceProvision(value: string): this {
        cy.getByTestId(`specalist-resource-provision`).should(
            `contain.text`,
            value,
        );

        return this;
    }

    public hasFaithStatus(value: string): this {
        cy.getByTestId(`faith-status`).should(`contain.text`, value);

        return this;
    }

    public hasFaithType(value: string): this {
        cy.getByTestId(`faith-type`).should(`contain.text`, value);

        return this;
    }

    public hasTrustId(value: string): this {
        cy.getByTestId(`trust-id`).should(`contain.text`, value);

        return this;
    }

    public hasTrustName(value: string): this {
        cy.getByTestId(`trust-name`).should(`contain.text`, value);

        return this;
    }

    public hasTrustType(value: string): this {
        cy.getByTestId(`trust-type`).should(`contain.text`, value);

        return this;
    }
}

const projectOverviewPage = new ProjectOverviewPage();

export default projectOverviewPage;
