import { Logger } from "cypress/common/logger";
import dataGenerator from "cypress/fixtures/dataGenerator";
import createProjectPage from "cypress/pages/createProject/createProjectPage";
import homePage from "cypress/pages/homePage";
import projectOverviewPage from "cypress/pages/projectOverviewPage";

describe("Testing project overview", () => {

    beforeEach(() => {
        cy.login();
    });

    it("Should create a project and display the project configuration in the overview", () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();

        createProject(temporaryProjectId, schoolName);

        cy.visit(`/projects/${temporaryProjectId}/overview`);

        projectOverviewPage
          .hasProjectTitleHeader(schoolName)
          .hasProjectStatus("Pre-opening")
          .hasProvisionalOpeningDateAgreedWithTrust("1 October 2035")
          .hasTrustId("TR90111")
          .hasTrustName("The James Web School")
          .hasTrustType("SAT (single academy trust)")
          .hasProjectId(temporaryProjectId)
          .hasCurrentFreeSchoolName(schoolName)
          .hasSchoolType("Mainstream")
          .hasSchoolPhase("Secondary")
          .hasFaithStatus("Designation")
          .hasFaithType("Greek Orthodox")
          .hasLocalAuthority("Luton")
          .hasRegion("East of England")
          .hasProjectType()
    });

    function createProject(temporaryProjectId: string, schoolName: string) {

        const testTrn = "TR90111";

        Logger.log("Creating a new project using the individual route");
        homePage.createNewProjects();

        createProjectPage
            .selectOption("Presumption")
            .continue();

        createProjectPage
            .enterProjectId(temporaryProjectId)
            .continue();

        createProjectPage
            .enterSchoolName(schoolName)
            .continue();

        createProjectPage
            .selectOption("East of England")
            .continue();

        createProjectPage
            .selectOption("Luton")
            .continue();

        createProjectPage
            .enterTRN(testTrn)
            .continue();

        createProjectPage
            .selectOption("Yes")
            .continue();

        createProjectPage
            .selectOption("Mainstream")
            .continue();

        createProjectPage
            .setNurseryTo("Yes")
            .setSixthFormTo("Yes")
            .setAlternativeProvisionTo("No")
            .setSpecialEducationNeedsTo("Yes")
            .setResidentialOrBoarding("Yes")
            .continue();
        
        createProjectPage
            .selectOption("Secondary")
            .continue();

        createProjectPage
            .enterAgeRangeFrom("2")
            .enterAgeRangeTo("7")
            .continue();

        createProjectPage
            .enterNurseryCapacity("200")
            .enterReceptionToYear6("0")
            .enterYear7ToYear11("400")
            .enterYear12ToYear14("150")
            .continue();

        createProjectPage
            .selectOption("Designation")
            .continue();

        createProjectPage
            .selectOption("Greek Orthodox")
            .continue();

        createProjectPage
            .setProvisionalOpeningDate("1", "10", "2035")
            .continue();

        createProjectPage
            .enterProjectAssignedToName("Test Person")
            .enterProjectAssignedToEmail("test.person@education.gov.uk")
            .continue();

        createProjectPage.clickCreateProject();
    }
});


