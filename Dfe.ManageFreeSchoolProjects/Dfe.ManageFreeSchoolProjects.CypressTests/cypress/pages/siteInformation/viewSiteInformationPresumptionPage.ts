export class ViewSiteInformationPresumptionPage {
    public hasTemporarySiteAddress(line1: string, line2: string, city: string): this {

        cy.hasAddress("temporary-site-address", line1, line2, city);

        return this;
    }

    public hasTemporarySitePostcode(postcode: string): this {
        cy.getByTestId("temporary-site-postcode").should("contain.text", postcode);

        return this;
    }

    public hasTemporarySiteDatePlanningPermissionObtained(value: string): this {
        cy.getByTestId("temporary-site-date-planning-permission-obtained").should("contain.text", value);

        return this;
    }

    public hasTemporarySiteStartDateOfOccupation(value: string): this {
        cy.getByTestId("temporary-site-start-date-of-site-occupation").should("contain.text", value);

        return this;
    }

    public hasPermanentSiteAddress(line1: string, line2: string, city: string): this {
        cy.hasAddress("permanent-site-address", line1, line2, city);

        return this;
    }

    public hasPermanentSitePostcode(postcode: string): this {
        cy.getByTestId("permanent-site-postcode").should("contain.text", postcode);

        return this;
    }

    public hasPermanentSiteDatePlanningPermissionObtained(value: string): this {
        cy.getByTestId("permanent-site-date-planning-permission-obtained").should("contain.text", value);

        return this;
    }

    public hasPermanentSiteStartDateOfOccupation(value: string): this {
        cy.getByTestId("permanent-site-start-date-of-site-occupation").should("contain.text", value);

        return this;
    }

    public hasSchoolName(value: string): this {
        cy.getByTestId("school-name").should("contain.text", value);

        return this;
    }

    public changeTemporarySite(): this {
        cy.getByTestId("change-temporary-site").click();

        return this;
    }

    public changePermanentSite(): this {
        cy.getByTestId("change-permanent-site").click();

        return this;
    }

    public checkInsetTextExists() {
        cy.getByTestId("central-route-hint").should("exist");

        return this;
    }

    public changeTemporarySiteShouldNotExist(): this {
        cy.getById("change-temporary-site").should("not.exist");

        return this;
    }

    public changePermanentSiteShouldNotExist(): this {
        cy.getById("change-permanent-site").should("not.exist");

        return this;
    }

    public backToProject(): this {
        cy.get(".govuk-back-link").click();

        return this;
    }

    public hasProjectStatus(value: string): this {
        cy.getById(`status-tag`).should(`contain.text`, value);

        return this;
    }

    public hasProjectTitleHeader(value: string): this {
        cy.getByTestId("project-title-header").should("contain.text", value);

        return this;
    }
}

const viewSiteInformationPresumptionPage = new ViewSiteInformationPresumptionPage();

export default viewSiteInformationPresumptionPage;