import { Logger } from "cypress/common/logger";
import { ProjectRecordCreator } from "cypress/constants/cypressConstants";
import dataGenerator from "cypress/fixtures/dataGenerator";
import createProjectPage from "cypress/pages/createProject/createProjectPage";
import homePage from "cypress/pages/homePage";
import summaryPage from "cypress/pages/task-summary-base";

describe("Testing the presumption route project creation journey", () => {
    beforeEach(() => {
        cy.login({ role: "POTATO" });
        cy.visit(Cypress.env('url'));
    });

    it("Should NOT allow a NON-projectrecordcreator user to access certain URLs", () => {

        Logger.log("Testing that NON-projectrecordcreator role is UNABLE to access Create individual project URLs")
        //Define the URLs that should trigger a failure for the "POTATO"/NON-projectrecordcreator user
        const unauthorizedUrls = [
            "/project/create/method",
            "/project/create/school",
            "/project/create/region",
            "/project/create/localauthority",
            "project/create/school-type",
            "/project/create/checkyouranswers",
            "/project/create/confirmation"];

        //Verify that the "NON-Projectrecordcreator" user cannot access unauthorized URLs
        cy.location().should((loc) => {
            const currentUrl = loc.href;
            if (unauthorizedUrls.some(url => currentUrl.includes(url))) {
                //Fail the test if the user is on an unauthorized URL
                throw new Error("Test failed because the 'NON-Projectrecordcreator' user is on an unauthorized URL");
            }
        });
    });

    it("Should NOT allow a NON-projectrecordcreator user to create a new project using the form", () => {
        Logger.log("Testing that a NON-projectrecordcreator role DOES NOT have the green Create new projects CTA");
        cy.contains('Create new projects').should('not.exist');
        cy.executeAccessibilityTests();
    });

    describe("Creating an individual project - Create new project button should display for projectrecordcreator role", () => {
        beforeEach(() => {
            cy.login({ role: ProjectRecordCreator });
            cy.visit('/');
        });

        it("Should display Create a project card for projectRecordCreator role", () => {
            Logger.log("Testing that a projectrecordcreator role DOES have the green Create new projects CTA");
            cy.contains('Create a project').should('be.visible');
            cy.executeAccessibilityTests();
        });
    });

    describe("Creating an individual project - Create a Presumption route new project", () => {
        beforeEach(() => {
            cy.login({ role: ProjectRecordCreator });
            cy.visit('/');
        });

        it("Should create a project", () => {
            const temporaryProjectId = dataGenerator.generateTemporaryId(25);
            const schoolName = dataGenerator.generateSchoolName();
            const TestTrn = "TR90111";

            cy.executeAccessibilityTests();

            homePage.createNewProjects();

            Logger.log("Check method required");
            cy.executeAccessibilityTests();

            createProjectPage
                .continue()
                .errorMessage("Select what project you want to create");

            cy.executeAccessibilityTests();

            createProjectPage
                .selectOption("Presumption")
                .continue();

            Logger.log("Check project id validation");
            createProjectPage
                .titleIs("What is the temporary project ID?")
                .continue()
                .errorMessage("Enter the temporary project ID")
                .enterProjectId("T-00008")
                .continue()
                .errorMessage("Temporary project ID must only include numbers and letters")
                .enterProjectId("T 00009")
                .continue()
                .errorMessage("Temporary project ID must not include spaces")
                .enterProjectId(dataGenerator.generateAlphaNumeric(26))
                .continue()
                .errorMessage("Temporary project ID must be 25 characters or less");

            cy.executeAccessibilityTests();

            Logger.log("Enter Valid project ID");
            createProjectPage
                .enterProjectId(temporaryProjectId)
                .continue();

            Logger.log("Check school name validation");
            createProjectPage
                .titleIs("What is the current free school name?")
                .continue()
                .errorMessage("Enter the current free school name")
                .enterSchoolName("Invalid#")
                .continue()
                .errorMessage("School name must not include special characters other than , ( ) '")
                .continue()
                .enterSchoolName(dataGenerator.generateAlphaNumeric(101))
                .continue()
                .errorMessage("School name must be 100 characters or less")

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

            Logger.log("Check trust validation");
            createProjectPage
                .titleIs("Search for a trust by TRN")
                .continue()
                .errorMessage("Enter the TRN")
                .enterTRN("POTATO")
                .continue()
                .errorMessage("The TRN must start with the letters TR, followed by at least 5 numbers")
                .enterTRN("TR00000000")
                .continue()
                .errorMessage("TRN (trust reference number) must be 7 characters or less")
                .enterTRN("TR99999")
                .continue()
                .errorMessage("Trust ID not found. Enter a different ID")

            cy.executeAccessibilityTests();

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

            Logger.log("Check school type validation");
            createProjectPage
                .titleIs("What is the school type?")
                .continue()
                .errorMessage("Select the school type");

            cy.executeAccessibilityTests();

            Logger.log("Selecting Mainstream school type");
            createProjectPage
                .selectOption("Mainstream")
                .continue();
            
            
            Logger.log("Check class types validation");
            createProjectPage
                .titleIs("Will it have any of these provisions?")
                .continue()
                .errorMessage("Select yes if it will have a nursery")
                .errorMessage("Select yes if it will have a sixth form")
                .errorMessage("Select yes if it will have alternative provision")
                .errorMessage("Select yes if it will have special educational needs provision")
                .errorMessage("Select yes if it will have residential or boarding provision")

            cy.executeAccessibilityTests();

            Logger.log("Set class types");
            createProjectPage
                .setNurseryTo("Yes")
                .setSixthFormTo("Yes")
                .setResidentialOrBoarding("Yes")
                .setAlternativeProvisionTo("Yes")
                .setSpecialEducationNeedsTo("No")
                .setResidentialOrBoarding("Yes")
                .continue();

            Logger.log("Check school phase validation");
            createProjectPage
                .titleIs("What is the school phase?")
                .continue()
                .errorMessage("Select the school phase");

            cy.executeAccessibilityTests();

            Logger.log("Selecting Secondary school phase");
            createProjectPage
                .selectOption("Secondary")
                .continue();

            Logger.log("Check age range validation-limited");
            createProjectPage
                .titleIs("What is the age range?")
                .enterAgeRangeFrom("2")
                .enterAgeRangeTo("2")
                .continue()
                .errorMessage("'To' age range must be 5 or above")

            cy.executeAccessibilityTests();

            //re-enter incorrect age-range values to check validation
            createProjectPage
              .enterAgeRangeFrom("12")
              .enterAgeRangeTo("6")
              .continue()
              .errorMessage("'From' age range must be less than 'to' age range")

            //enter valid age-range values
            createProjectPage
                .enterAgeRangeFrom("2")
                .enterAgeRangeTo("7")
                .continue();

            Logger.log("Check capacity validation");
            createProjectPage
                .titleIs("What is the capacity?")
                .continue()
                .errorMessage("Enter the nursery capacity")
                .errorMessage("Enter the reception to year 6 capacity")
                .errorMessage("Enter the year 7 to year 11 capacity")
                .errorMessage("Enter the year 12 to year 14 capacity")
                .enterNurseryCapacity("A")
                .enterReceptionToYear6("A")
                .enterYear7ToYear11("A")
                .enterYear12ToYear14("A")
                .continue()
                .errorMessage("Nursery capacity must be a number, like 30")
                .errorMessage("Reception to year 6 capacity must be a number, like 30")
                .errorMessage("Year 7 to year 11 capacity must be a number, like 30")
                .errorMessage("Year 12 to year 14 capacity must be a number, like 30")
                .enterNurseryCapacity("-1")
                .enterReceptionToYear6("-1")
                .enterYear7ToYear11("-1")
                .enterYear12ToYear14("-1")
                .continue()
                .errorMessage("Nursery capacity must be between 0 and 9999")
                .errorMessage("Reception to year 6 capacity must be between 0 and 9999")
                .errorMessage("Year 7 to year 11 capacity must be between 0 and 9999")
                .errorMessage("Year 12 to year 14 capacity must be between 0 and 9999")
                .continue();

            cy.executeAccessibilityTests();

            Logger.log("Enter valid capacity");
            createProjectPage
                .enterNurseryCapacity("200")
                .enterReceptionToYear6("0")
                .enterYear7ToYear11("400")
                .enterYear12ToYear14("150")
                .continue();

            Logger.log("Check faith status validation");
            createProjectPage
                .titleIs("What is the faith status?")
                .continue()
                .errorMessage("Select the faith status");

            cy.executeAccessibilityTests();

            Logger.log("Select Designation");
            createProjectPage
                .selectOption("Designation")
                .continue();

            Logger.log("Check faith type validation");
            createProjectPage
                .titleIs("What is the faith type?")
                .continue()
                .errorMessage("Select the faith type")
                .selectOption("Other")
                .continue()
                .errorMessage("Enter the other faith type")
                .continue()
                .errorMessage("Enter the other faith type")
                .enterOtherFaith(dataGenerator.generateAlpha(101))
                .continue()
                .errorMessage("Other faith type must be 100 characters or less")
                .enterOtherFaith("2")
                .continue()
                .errorMessage("Other faith type must only contain letters and spaces")

            cy.executeAccessibilityTests();

            Logger.log("Select Greek Orthodox");
            createProjectPage
                .selectOption("Greek Orthodox")
                .continue()

            Logger.log("Check provisional opening date validation");
            createProjectPage
                .titleIs("What is the provisional opening date? (optional)")
                .setProvisionalOpeningDate("1", "10", "2020")
                .continue()
                .errorMessage("Provisional opening date date must be in the future");

            cy.executeAccessibilityTests();

            Logger.log("Enter valid provisional opening date");
            createProjectPage
                .setProvisionalOpeningDate("1", "10", "2035")
                .continue();

            Logger.log("Check project assigned to validation");
            createProjectPage
                .titleIs("Who do you want to assign this project to?")
                .continue()
                .errorMessage("Enter the name")
                .errorMessage("Enter the email")
                .enterProjectAssignedToName("j")
                .enterProjectAssignedToEmail("test.person@education.gov.uk")
                .continue()
                .errorMessage("Enter the full name, for example John Smith")
                .enterProjectAssignedToName("joe bloggs")
                .enterProjectAssignedToEmail("test.person@edunation.gov.uk")
                .continue()
                .errorMessage("Email address must be in the format firstname.surname@education.gov.uk")
                .enterProjectAssignedToEmail("@education.gov.uk")
                .continue()
                .errorMessage("Email address must be in the format firstname.surname@education.gov.uk")

            cy.executeAccessibilityTests();

            Logger.log("Set email");
            createProjectPage
                .enterProjectAssignedToName("joe bloggs")
                .enterProjectAssignedToEmail("test.person@education.gov.uk")
                .continue();

            Logger.log("Check answers");
            summaryPage.inOrder()
                .summaryShows("Project type").HasValue("Presumption").HasChangeLink()
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

            cy.executeAccessibilityTests();

            createProjectPage.clickCreateProject();

            Logger.log("Confirm created");
            createProjectPage.hasConfirmedProjectId(temporaryProjectId)
                .hasConfirmedEmailMessage("We have sent a notification email to test.person@education.gov.uk.")

            cy.executeAccessibilityTests();
        });

        it("Should navigate back to change project fields", () => {
            const temporaryProjectId = dataGenerator.generateTemporaryId(25);
            const schoolName = dataGenerator.generateSchoolName();
            const updatedTemporaryProjectId = dataGenerator.generateTemporaryId(25);
            const updatedSchoolName = dataGenerator.generateSchoolName();

            const TestTrn = "TR90111";

            homePage.createNewProjects();

            Logger.log("Use individual method");
            createProjectPage
                .selectOption("Presumption")
                .continue();

            Logger.log("Enter Valid project ID");
            createProjectPage
                .enterProjectId(temporaryProjectId)
                .continue()

            Logger.log("Enter school name");
            createProjectPage
                .enterSchoolName(schoolName)
                .continue()

            Logger.log("Select East of England");
            createProjectPage
                .selectOption("East of England")
                .continue();

            Logger.log("Select Local authority");
            createProjectPage
                .selectOption("Luton")
                .continue();

            Logger.log("Enter valid trust");
            createProjectPage
                .enterTRN(TestTrn)
                .continue();

            Logger.log("Selecting yes moves to next page");
            createProjectPage
                .titleIs("Confirm the trust")
                .selectOption("Yes")
                .continue();

            Logger.log("Selecting Mainstream school type");
            createProjectPage
                .selectOption("Mainstream")
                .continue();

            Logger.log("Select valid class types and continue");
            createProjectPage
                .setSixthFormTo("Yes")
                .setNurseryTo("No")
                .setAlternativeProvisionTo("No")
                .setSpecialEducationNeedsTo("Yes")
                .setResidentialOrBoarding("No")
                .continue();

            Logger.log("Selecting Secondary school phase");
            createProjectPage
                .selectOption("Secondary")
                .continue();

            Logger.log("Enter valid age range");
            createProjectPage
                .enterAgeRangeFrom("2")
                .enterAgeRangeTo("7")
                .continue()

            Logger.log("Enter valid capacity");
            createProjectPage
                .enterReceptionToYear6("0")
                .enterYear7ToYear11("400")
                .enterYear12ToYear14("150")
                .continue()

            Logger.log("Select Designation");
            createProjectPage
                .selectOption("None")
                .continue();

            Logger.log("Enter valid provisional opening date");
            createProjectPage
                .setProvisionalOpeningDate("1", "10", "2035")
                .continue();

            Logger.log("Set project assigned to");
            createProjectPage
                .enterProjectAssignedToName("test person")
                .enterProjectAssignedToEmail("test.person@education.gov.uk")
                .continue();

            Logger.log("Check answers");

            summaryPage.inOrder()
                .summaryShows("Project type").HasValue("Presumption").HasChangeLink()
                .summaryShows("Temporary project ID").HasValue(temporaryProjectId).HasChangeLink()
                .summaryShows("Current free school name").HasValue(schoolName).HasChangeLink()
                .summaryShows("Region").HasValue("East of England").HasChangeLink()
                .summaryShows("Local authority").HasValue("Luton").HasChangeLink()
                .summaryShows("Trust").HasValue("The James Web School").HasChangeLink()
                .summaryShows("School type").HasValue("Mainstream").HasChangeLink()
                .summaryShows("Nursery").HasValue("No").HasChangeLink()
                .summaryShows("Sixth form").HasValue("Yes").HasChangeLink()
                .summaryShows("Residential or boarding").HasValue("No").HasChangeLink()
                .summaryShows("Alternative provision").HasValue("No").HasChangeLink()
                .summaryShows("Special educational needs").HasValue("Yes").HasChangeLink()
                .summaryShows("School phase").HasValue("Secondary").HasChangeLink()
                .summaryShows("Age range").HasValue("2 to 7").HasChangeLink()
                .summaryShows("Reception to year 6 capacity").HasValue("0").HasChangeLink()
                .summaryShows("Year 7 to year 11 capacity").HasValue("400").HasChangeLink()
                .summaryShows("Year 12 to year 14 capacity").HasValue("150").HasChangeLink()
                .summaryShows("Faith status").HasValue("None").HasChangeLink()
                .summaryShows("Faith type").IsEmpty().HasChangeLink()
                .summaryShows("Provisional opening date").HasValue("1 October 2035").HasChangeLink()
                .summaryShows("Project assigned to").HasValue("test person").HasChangeLink()
                .summaryShows("Email").HasValue("test.person@education.gov.uk").HasChangeLink();

            Logger.log("Update Temporary Project ID")
            summaryPage.clickChangeFor("Temporary project ID");
            createProjectPage
                .hasProjectId(temporaryProjectId)
                .enterProjectId(updatedTemporaryProjectId)
                .continue();

            summaryPage.SummaryHasValue("Temporary project ID", updatedTemporaryProjectId);

            Logger.log("Update Current free school name")
            summaryPage.clickChangeFor("Current free school name");
            createProjectPage
                .hasSchoolName(schoolName)
                .enterSchoolName(updatedSchoolName)
                .continue();

            summaryPage.SummaryHasValue("Current free school name", updatedSchoolName);

            Logger.log("Update region and local authority")
            summaryPage.clickChangeFor("Region");
            createProjectPage
                .isOptionChecked("East of England")
                .selectOption("London")
                .continue()
                .selectOption("Camden")
                .continue();

            summaryPage.SummaryHasValue("Region", "London");
            summaryPage.SummaryHasValue("Local authority", "Camden");

            Logger.log("Update local authority")
            summaryPage.clickChangeFor("Local authority");
            createProjectPage
                .isOptionChecked("Camden")
                .selectOption("Hackney")
                .continue();
            summaryPage.SummaryHasValue("Local authority", "Hackney");

            Logger.log("Update Trust")
            summaryPage.clickChangeFor("Trust")
            createProjectPage.enterTRN("TR00012")
                .continue()
                .selectOption("No")
                .continue()
                .enterTRN("TR00012")
                .continue()
                .selectOption("Yes")
                .continue();

            summaryPage.SummaryHasValue("Trust", "The Academy of Central Bedfordshire");

            Logger.log("Change Sixth Form")
            summaryPage.clickChangeFor("Sixth form");
            createProjectPage
                .hasNursery("No")
                .hasSixthForm("Yes")
                .setSixthFormTo("No")
                .setNurseryTo("Yes")
                .continue();
            summaryPage
                .SummaryHasValue("Sixth form", "No")
                .SummaryHasValue("Nursery", "Yes");

            Logger.log("Change residential or boarding")
            summaryPage.clickChangeFor("Residential or boarding")
            createProjectPage
                .setResidentialOrBoarding("No")
                .continue();
            summaryPage.SummaryHasValue("Residential or boarding", "No")


            Logger.log("Change Nursery")
            summaryPage.clickChangeFor("Nursery");
            createProjectPage.setSixthFormTo("Yes")
                .setNurseryTo("No")
                .continue();
            summaryPage
                .SummaryHasValue("Sixth form", "Yes")
                .SummaryHasValue("Nursery", "No");

            Logger.log("Change Alternative provision")
            summaryPage.clickChangeFor("Alternative provision");
            createProjectPage.setAlternativeProvisionTo("Yes")
                .continue();
            summaryPage
                .SummaryHasValue("Alternative provision", "Yes")

            Logger.log("Change Special educational needs")
            summaryPage.clickChangeFor("Special educational needs");
            createProjectPage.setSpecialEducationNeedsTo("No")
                .continue();
            summaryPage
                .SummaryHasValue("Special educational needs", "No")

            Logger.log("Change School phase")
            summaryPage.clickChangeFor("School phase");
            createProjectPage
                .isOptionChecked("Secondary")
                .selectOption("All-through")
                .continue();
            summaryPage
                .SummaryHasValue("School phase", "All-through");

            Logger.log("Change Age range")
            summaryPage.clickChangeFor("Age range");
            createProjectPage
                .hasAgeRangeFrom("2")
                .hasAgeRangeTo("7")
                .enterAgeRangeFrom("4")
                .enterAgeRangeTo("11")
                .continue();
            summaryPage
                .SummaryHasValue("Age range", "4 to 11");

            Logger.log("Change School type")
            summaryPage.clickChangeFor("School type");
            createProjectPage
                .isOptionChecked("Mainstream")
                .selectOption("Special")
                .continue()
                .continue();
            summaryPage
                .SummaryHasValue("School type", "Special")
                .summaryDoesNotShow("Alternative provision (specialist resource provision)")
                .summaryDoesNotShow("Special educational needs (specialist resource provision")

            Logger.log("Nursery is not shown if No is selected");
            Logger.log("Change capacity")
            summaryPage.clickChangeFor("Year 12 to year 14 capacity");
            createProjectPage
                .hasNoNurseryCapacity()
                .hasReceptionToYear6("0")
                .hasYear7ToYear11("400")
                .hasYear12ToYear14("150")
                .enterReceptionToYear6("120")
                .enterYear7ToYear11("600")
                .enterYear12ToYear14("100")
                .continue();

            summaryPage
                .summaryDoesNotShow("Nursery capacity")
                .SummaryHasValue("Reception to year 6 capacity", "0")
                .SummaryHasValue("Year 7 to year 11 capacity", "600")
                .SummaryHasValue("Year 12 to year 14 capacity", "100");

            Logger.log("Change faith status")
            summaryPage.clickChangeFor("Faith status");
            createProjectPage
                .isOptionChecked("None")
                .selectOption("Ethos")
                .continue()
                .selectOption("Roman Catholic")
                .continue();

            summaryPage.SummaryHasValue("Faith status", "Ethos")
                .SummaryHasValue("Faith type", "Roman Catholic");

            Logger.log("Change other faith type")
            summaryPage.clickChangeFor("Faith status");
            createProjectPage
                .selectOption("Designation")
                .continue()
                .selectOption("Other")
                .enterOtherFaith("Test Faith")
                .continue();
            summaryPage.SummaryHasValue("Faith status", "Designation")
                .SummaryHasValue("Faith type", "Other - Test Faith")

            Logger.log("Change Provisional opening date")
            summaryPage.clickChangeFor("Provisional opening date");
            createProjectPage
                .hasProvisionalOpeningDate("1", "10", "2035")
                .setProvisionalOpeningDate("12", "11", "2034")
                .continue()
            summaryPage
                .SummaryHasValue("Provisional opening date", "12 November 2034");

            Logger.log("Change Project assigned to")
            summaryPage.clickChangeFor("Project assigned to")
            createProjectPage.enterProjectAssignedToName("Anne Jones")
            createProjectPage.enterProjectAssignedToEmail("anne.jones@education.gov.uk")
                .continue();
            summaryPage
                .SummaryHasValue("Project assigned to", "Anne Jones")
                .SummaryHasValue("Email", "anne.jones@education.gov.uk");

            createProjectPage.clickCreateProject();

            Logger.log("Confirm created");
            createProjectPage.hasConfirmedProjectId(updatedTemporaryProjectId)
                .hasConfirmedEmailMessage("We have sent a notification email to anne.jones@education.gov.uk.")
        });
    });
});