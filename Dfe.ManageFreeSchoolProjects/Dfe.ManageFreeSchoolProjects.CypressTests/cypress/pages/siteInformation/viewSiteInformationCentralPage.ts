export class ViewSiteInformationCentralPage {
    public hasTemporaryAddress(): this {
        cy.getById("temporary-address").should("be.visible");

        return this;
    }

    public hasTemporaryPostcode(): this {
        cy.getById("temporary-postcode").should("be.visible");

        return this;
    }

    public hasTemporarySiteRiskRating(): this {
        cy.getById("temporary-site-risk-rating").should("be.visible");

        return this;
    }

    public hasTemporaryPlanningDecision(): this {
        cy.getById("temporary-planning-decision").should("be.visible");

        return this;
    }

    public hasTemporaryKeyDatesHeading(): this {
        cy.getById("temporary-key-dates-heading").should("be.visible");

        return this;
    }

    public hasTemporaryKeyDates(): this {
        cy.getById("temporary-key-dates").should("be.visible");

        return this;
    }

    public hasTemporaryForecastDates(): this {
        cy.getById("temporary-forecast-dates").should("be.visible");

        return this;
    }

    public hasTemporaryActualDates(): this {
        cy.getById("temporary-actual-dates").should("be.visible");

        return this;
    }

    public hasTemporaryHeadsOfTermsAgreed(): this {
        cy.getById("temporary-heads-of-terms-agreed").should("be.visible");

        return this;
    }

    public hasTemporaryContractorAppointed(): this {
        cy.getById("temporary-contractor-appointed").should("be.visible");

        return this;
    }
    public hasTemporaryPlanningDecisionDate(): this {
        cy.getById("temporary-planning-decision-date").should("be.visible");

        return this;
    }
    public hasTemporaryAccomodationFirstReadyForOccupation(): this {
        cy.getById("temporary-accomodation-first-ready-for-occupation").should("be.visible");

        return this;
    }

    public hasPermanentAddress(): this {
        cy.getById("permanent-address").should("be.visible");

        return this;
    }

    public hasPermanentPostcode(): this {
        cy.getById("permanent-postcode").should("be.visible");

        return this;
    }

    public hasPermanentSiteRiskRating(): this {
        cy.getById("permanent-site-risk-rating").should("be.visible");

        return this;
    }

    public hasPermanentPlanningDecision(): this {
        cy.getById("permanent-planning-decision").should("be.visible");

        return this;
    }

    public hasPermanentKeyDatesHeading(): this {
        cy.getById("permanent-key-dates-heading").should("be.visible");

        return this;
    }

    public hasPermanentKeyDates(): this {
        cy.getById("permanent-key-dates").should("be.visible");

        return this;
    }

    public hasPermanentForecastDates(): this {
        cy.getById("permanent-forecast-dates").should("be.visible");

        return this;
    }

    public hasPermanentActualDates(): this {
        cy.getById("permanent-actual-dates").should("be.visible");

        return this;
    }

    public hasPermanentHeadsOfTermsAgreed(): this {
        cy.getById("permanent-heads-of-terms-agreed").should("be.visible");

        return this;
    }

    public hasPermanentContractorAppointed(): this {
        cy.getById("permanent-contractor-appointed").should("be.visible");

        return this;
    }
    public hasPermanentPlanningDecisionDate(): this {
        cy.getById("permanent-planning-decision-date").should("be.visible");

        return this;
    }
    public hasPermanentAccomodationFirstReadyForOccupation(): this {
        cy.getById("permanent-accomodation-first-ready-for-occupation").should("be.visible");

        return this;
    }

    public hasCapitalRating(): this {
        cy.getById("capital-rating").should("be.visible");

        return this;
    }
    public hasRiskSummary(): this {
        cy.getById("risk-summary").should("be.visible");

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

const viewSiteInformationCentralPage = new ViewSiteInformationCentralPage();

export default viewSiteInformationCentralPage;