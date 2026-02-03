describe("Testing cookie preferences", () => {
    beforeEach(() => {
        cy.login();
        cy.visit("/");
    });

    it("Should have analytics cookies if accepted", () => {
        cy
        .getCookie("_ga").should("not.exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("not.exist")
        .getByTestId("cookie-banner-accept").click()
        .getCookie("_ga").should("exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("exist")
    });

    it("Should remove analytics cookies if rejected", () => {
        cy.getCookie("_ga").should("not.exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("not.exist")
        .getByTestId("cookie-banner-reject").click()
        .getCookie("_ga").should("not.exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("exist");
    });

    it("Should remove analytics cookies if rejected if domain is different", () => {
        cy.getCookie("_ga").should("not.exist")
        .setCookie("_ga", "test", { domain: ".education.gov.uk" })
        .getCookie("_ga").should("exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("not.exist")
        .wait(2000)
        .getByTestId("cookie-banner-reject").click()
        .wait(2000)
        .getCookie("_ga").should("not.exist")
        .getCookie(".ManageFreeSchoolProjects.Consent").should("exist");
    });

    it.only("Should be able to View cookies page ", () => {
        cy.getByTestId('cookie-banner-link-2').click()
            .url().should('contain', 'cookies')
            .get('h1').should('contain', 'Cookie preferences')
            .get('h2').should('contain', 'Analytics cookies (optional)')
            .get('#cookie-consent-deny').should('be.checked')
            .get('[data-qa="submit"]').should('be.visible')
    });
});
