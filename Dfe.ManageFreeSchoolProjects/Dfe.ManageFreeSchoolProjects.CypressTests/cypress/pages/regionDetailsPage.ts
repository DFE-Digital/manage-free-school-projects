class RegionDetailsPage {
    public selectContinue(): this {
        cy.contains('button.govuk-button', 'Save and continue').click();

        return this;
    }

    public isRegionPage(): this {
        cy.get('#region').should('exist');

        return this;
    }

    public isLocalAuthorityPage(): this {
        cy.get('#local-authority').should('exist');

        return this;
    }

    public hasSchoolName(school: string): this {
        cy.getByTestId('school-name').should('contains.text', school);

        return this;
    }

    public withRegion(region: string): this {
        cy.getByTestId(region).click();

        return this;
    }

    public withLocalAuthority(localAuthority: string): this {
        cy.getByTestId(localAuthority).click();

        return this;
    }
}

const regionDetailsPage = new RegionDetailsPage();

export default regionDetailsPage;
