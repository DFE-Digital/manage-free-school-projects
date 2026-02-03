import { Logger } from "cypress/common/logger";
import { ProjectRecordCreator } from "cypress/constants/cypressConstants";
import dataGenerator from "cypress/fixtures/dataGenerator";
import createProjectPage from "cypress/pages/createProject/createProjectPage";
import homePage from "cypress/pages/homePage";
import summaryPage from "cypress/pages/task-summary-base";

describe("Smoke Test Create an individual Central project - (Up Until Check your answers Page)", () => {
    beforeEach(() => {
        cy.login({ role: ProjectRecordCreator });
        cy.visit('/');
    });

    it("Should create a central project up until Check your answers page", { tags: ['smoke'] }, () => {
        const applicationNumber = dataGenerator.generateTemporaryId(10);
        const applicationWave = dataGenerator.generateTemporaryId(10);
        const temporaryProjectId = dataGenerator.generateTemporaryId(25);
        const schoolName = dataGenerator.generateSchoolName();
        const TestTrn = "TR90111";

        homePage.createNewProjects();

        Logger.log("Check method required");
        cy.executeAccessibilityTests();

        createProjectPage
            .selectOption("Central")
            .continue();

        Logger.log("Enter application number");
        createProjectPage
            .titleIs("What is the application number?")
            .enterApplicationNumber(applicationNumber)
            .continue()

        Logger.log("Enter application wave ");
        createProjectPage
            .titleIs("What is the application wave?")
            .enterApplicationWave(applicationWave)
            .continue()

        cy.executeAccessibilityTests();

        Logger.log("Enter Valid project ID");
        createProjectPage
            .enterProjectId(temporaryProjectId)
            .continue();

        cy.executeAccessibilityTests();
        Logger.log("Enter school name");

        createProjectPage
            .enterSchoolName(schoolName)
            .continue();

        createProjectPage
            .titleIs("What is the region of the school?")
            .continue()
            .errorMessage("Select the region")

        cy.executeAccessibilityTests();

        Logger.log("Select East of England");
        createProjectPage
            .selectOption("East of England")
            .continue();

        Logger.log("Check local authority required");
        createProjectPage
            .titleIs("What is the local authority?")
            .continue()
            .errorMessage("Select the local authority");

        cy.executeAccessibilityTests();

        Logger.log("Select Local authority");
        createProjectPage
            .selectOption("Luton")
            .continue();

        Logger.log("Enter valid trust");
        createProjectPage
            .enterTRN(TestTrn)
            .continue();

        Logger.log("Confirm trust validation");
        createProjectPage
            .titleIs("Confirm the trust")
            .continue()
            .errorMessage("Select yes if the trust is correct");

        cy.executeAccessibilityTests();

        Logger.log("Selecting No returns to previous page");
        createProjectPage
            .selectOption("No")
            .continue()
            .titleIs("Search for a trust by TRN")
            .enterTRN(TestTrn)
            .continue();

        createProjectPage
            .titleIs("Confirm the trust")
            .back()
            .titleIs("Search for a trust by TRN")
            .enterTRN(TestTrn)
            .continue();

        createProjectPage
            .titleIs("Confirm the trust")
            .hasCorrectTrustName("The James Web School")
            .hasCorrectTrustType("SAT (single academy trust)")
            .selectOption("Yes")
            .continue();

        Logger.log("Selecting Mainstream school type");
        createProjectPage
            .selectOption("Mainstream")
            .continue();

        Logger.log("Set class types");
        createProjectPage
            .titleIs("Will it have any of these provisions?")
            .setNurseryTo("Yes")
            .setSixthFormTo("Yes")
            .setResidentialOrBoarding("Yes")
            .setAlternativeProvisionTo("Yes")
            .setSpecialEducationNeedsTo("No")
            .setResidentialOrBoarding("Yes")
            .continue();

        Logger.log("Selecting Secondary school phase");
        createProjectPage
            .titleIs("What is the school phase?")
            .selectOption("Secondary")
            .continue();

        //enter valid age-range values
        createProjectPage
            .enterAgeRangeFrom("2")
            .enterAgeRangeTo("7")
            .continue();

        Logger.log("Enter valid capacity");
        createProjectPage
            .enterNurseryCapacity("200")
            .enterReceptionToYear6("0")
            .enterYear7ToYear11("400")
            .enterYear12ToYear14("150")
            .continue();

        Logger.log("Select Designation");
        createProjectPage
            .selectOption("Designation")
            .continue();

        cy.executeAccessibilityTests();

        Logger.log("Select Greek Orthodox");
        createProjectPage
            .selectOption("Greek Orthodox")
            .continue()

        Logger.log("Enter valid provisional opening date");
        createProjectPage
            .setProvisionalOpeningDate("1", "10", "2035")
            .continue();
        cy.executeAccessibilityTests();

        Logger.log("Set email");
        createProjectPage
            .enterProjectAssignedToName("joe bloggs")
            .enterProjectAssignedToEmail("test.person@education.gov.uk")
            .continue();

        Logger.log("Check answers");
        summaryPage.inOrder()
            .summaryShows("Project type").HasValue("Central").HasChangeLink()
            .summaryShows("Application number").HasValue(applicationNumber).HasChangeLink()
            .summaryShows("Application wave").HasValue(applicationWave).HasChangeLink()
            .summaryShows("Temporary project ID").HasValue(temporaryProjectId).HasChangeLink()
            .summaryShows("Current free school name").HasValue(schoolName).HasChangeLink()
            .summaryShows("Region").HasValue("East of England").HasChangeLink()
            .summaryShows("Local authority").HasValue("Luton").HasChangeLink()
            .summaryShows("Trust").HasValue("The James Web School").HasChangeLink()
            .summaryShows("School type").HasValue("Mainstream").HasChangeLink()
            .summaryShows("Nursery").HasValue("Yes").HasChangeLink()
            .summaryShows("Sixth form").HasValue("Yes").HasChangeLink()
            .summaryShows("Residential or boarding").HasValue("Yes").HasChangeLink()
            .summaryShows("Alternative provision").HasValue("Yes").HasChangeLink()
            .summaryShows("Special educational needs").HasValue("No").HasChangeLink()
            .summaryShows("School phase").HasValue("Secondary").HasChangeLink()
            .summaryShows("Age range").HasValue("2 to 7").HasChangeLink()
            .summaryShows("Nursery capacity").HasValue("200").HasChangeLink()
            .summaryShows("Reception to year 6 capacity").HasValue("0").HasChangeLink()
            .summaryShows("Year 7 to year 11 capacity").HasValue("400").HasChangeLink()
            .summaryShows("Year 12 to year 14 capacity").HasValue("150").HasChangeLink()
            .summaryShows("Faith status").HasValue("Designation").HasChangeLink()
            .summaryShows("Faith type").HasValue("Greek Orthodox").HasChangeLink()
            .summaryShows("Provisional opening date").HasValue("1 October 2035").HasChangeLink()
            .summaryShows("Project assigned to").HasValue("joe bloggs").HasChangeLink()
            .summaryShows("Email").HasValue("test.person@education.gov.uk").HasChangeLink();

        Logger.log("Create Project button is visible");
        summaryPage
            .createProjectbutton('Create project')

        cy.executeAccessibilityTests();
    });
});
