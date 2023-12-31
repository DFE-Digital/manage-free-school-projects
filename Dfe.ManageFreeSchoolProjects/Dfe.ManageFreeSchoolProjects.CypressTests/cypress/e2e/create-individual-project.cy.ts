import { Logger } from "cypress/common/logger";
import { ProjectRecordCreator } from "cypress/constants/cypressConstants";
import createProjectPage from "cypress/pages/createProject/createProjectPage";
import homePage from "cypress/pages/homePage";
import dataGenerator from "cypress/fixtures/dataGenerator";
import summaryPage from "cypress/pages/task-summary-base";

describe("Creating an individual project - NEGATIVE ROLE TESTS - USER DOES NOT GET GREEN CREATE NEW PROJECT BUTTON", () => {
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

});

describe("Creating an individual project - Create new project button should display for projectrecordcreator role", () => {
    beforeEach(() => {
        cy.login({ role: ProjectRecordCreator });
        cy.visit('/');
    });

    it("Should display Create new projects button for projectRecordCreator role", () => {
        Logger.log("Testing that a projectrecordcreator role DOES have the green Create new projects CTA");
        cy.contains('Create new projects').should('be.visible');
        cy.executeAccessibilityTests();
    });
});

describe("Creating an individual project - Create a new project", () => {
    beforeEach(() => {
        cy.login({ role: ProjectRecordCreator });
        cy.visit('/');
    });

    it("Should create a project", () => {
        const temporaryProjectId = dataGenerator.generateTemporaryId();
        const schoolName = dataGenerator.generateSchoolName();
        const TestTrn = "TR00111";

        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log("Check method required");
        cy.executeAccessibilityTests();

        createProjectPage
            .continue()
            .errorMessage("The method field is required.")
        cy.executeAccessibilityTests();

        Logger.log("Use individual method");
        createProjectPage
            .selectOption("Create one project")
            .continue();
        cy.executeAccessibilityTests();
            
        Logger.log("Check project id validation");
        createProjectPage
            .titleIs("What is the temporary project ID?")
            .continue()
            .errorMessage("The temporary project ID field is required")
            .enterProjectId("T-00008")
            .continue()
            .errorMessage("Temporary project ID must only include numbers and letters")
            .enterProjectId("T 00009")
            .continue()
            .errorMessage("Temporary project ID must not include spaces")
            .enterProjectId(dataGenerator.generateAlphaNumeric(26))
            .continue()
            .errorMessage("The temporary project ID must be 25 characters or less")

        Logger.log("Enter Valid project ID");
        cy.executeAccessibilityTests();
        createProjectPage
            .enterProjectId(temporaryProjectId)
            .continue()
       
        Logger.log("Check back navigation");
        createProjectPage
            .back()
            .titleIs("What is the temporary project ID?")
            .hasProjectId(temporaryProjectId)
            .continue()

        Logger.log("Check school name validation");
        cy.executeAccessibilityTests();
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
            .errorMessage("The school name must be 100 characters or less")
            
        Logger.log("Enter school name");
        cy.executeAccessibilityTests();
        createProjectPage
            .enterSchoolName(schoolName)
            .continue()

        Logger.log("Check back navigation");
        cy.executeAccessibilityTests();	
        createProjectPage
            .back()
            .titleIs("What is the current free school name?")
            .hasSchoolName(schoolName)
            .continue()

        Logger.log("Check region required");
        cy.executeAccessibilityTests();	
        createProjectPage
            .titleIs("What is the region of the school?")
            .continue()
            .errorMessage("Select the region of the free school")

        Logger.log("Select East of England");
        cy.executeAccessibilityTests();	
        createProjectPage
            .selectOption("East of England")
            .continue();

        Logger.log("Check back navigation");
        cy.executeAccessibilityTests();	
        createProjectPage
            .back()
            .titleIs("What is the region of the school?")
            .isOptionChecked("East of England")
            .continue();

        Logger.log("Check local authority required");
        cy.executeAccessibilityTests();	
        createProjectPage
            .titleIs("What is the local authority?")
            .continue()
            .errorMessage("Select the local authority of the free school");

        Logger.log("Select Local authority");
        cy.executeAccessibilityTests();	
        createProjectPage
            .selectOption("Luton")
            .continue();

        Logger.log("Check trust validation");
        cy.executeAccessibilityTests();	
        createProjectPage
            .titleIs("Search for a trust by TRN")
            .continue()
            .errorMessage("Enter the TRN")
            .enterTRN("POTATO")
            .continue()
            .errorMessage("The TRN must be in the format TRXXXXX")
            .enterTRN("TR00000000")
            .continue()
            .errorMessage("The TRN (trust reference number) must be 7 characters or less")
            .enterTRN("TR99999")
            .continue()
            .errorMessage("Trust ID not found")
            
        Logger.log("Enter valid trust");
        cy.executeAccessibilityTests();	   
        createProjectPage
            .enterTRN(TestTrn)
            .continue();

        Logger.log("Confirm trust validation");
        cy.executeAccessibilityTests();	       
        createProjectPage            
            .titleIs("Confirm the trust")
            .continue()
            .errorMessage("Confirm that the trust displayed is correct");

        Logger.log("Selecting No returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("No")
            .continue()
            .titleIs("Search for a trust by TRN")
            .enterTRN(TestTrn)
            .continue();

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("Confirm the trust")
            .back()
            .titleIs("Search for a trust by TRN")
            .enterTRN(TestTrn)
            .continue();

        Logger.log("Selecting yes moves to next page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("Confirm the trust")
            .selectOption("Yes")
            .continue();

        Logger.log("Check school type validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the school type?")
            .continue()
            .errorMessage("Select school type");

        Logger.log("Selecting Mainstream school type");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Mainstream")
            .continue();
            
        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the school type?")
            .isOptionChecked("Mainstream")
            .continue();
        
        Logger.log("Check school phase validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the school phase?")
            .continue()
            .errorMessage("Select the school phase");

        Logger.log("Selecting Secondary school phase");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Secondary")
            .continue();

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the school phase?")
            .isOptionChecked("Secondary")
            .continue();

        Logger.log("Check school phase validation and back navigation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("Will it have any of these class types?")
            .continue()
            .errorMessage("Select yes if it will have a nursery")
            .errorMessage("Select yes if it will have a sixth form")
            .setNurseryTo("Yes")
            .continue()
            .errorMessage("Select yes if it will have a sixth form")
            .back()
            .titleIs("What is the school phase?")
            .continue();

        Logger.log("Select invalid class types");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .setSixthFormTo("Yes")
            .continue()
            .errorMessage("Select yes if it will have a nursery");

        Logger.log("Select valid class types and continue");
        createProjectPage
            .setSixthFormTo("Yes")
            .setNurseryTo("No")
            .continue();

        Logger.log("Check age range validation-limited as tested elsewhere");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the age range?")
            .enterAgeRangeFrom("2")
            .enterAgeRangeTo("2")
            .continue()
            .errorMessage("'To' age range must be 5 or above")
            .enterAgeRangeFrom("2")
            .enterAgeRangeTo("7")
            .continue()

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the age range?")
            .hasAgeRangeFrom("2")
            .hasAgeRangeTo("7")
            .continue();
 
        Logger.log("Check capacity validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the capacity?")
            .enterReceptionToYear6("-1")
            .enterYear7ToYear11("A")
            .enterYear12ToYear14("")
            .continue()
            .errorMessage("Reception to year 6 capacity must be between 0 and 9999")
            .errorMessage("Year 7 to year 11 capacity must be a number")
            .errorMessage("Enter the Year 12 - Year 14 Capacity")
            .enterReceptionToYear6("")
            .enterYear7ToYear11("-1")
            .enterYear12ToYear14("A")
            .continue()
            .errorMessage("Enter the Reception - Year 6 Capacity")
            .errorMessage("Year 7 to year 11 capacity must be between 0 and 9999")
            .errorMessage("Year 12 to year 14 capacity must be a number")
            .enterReceptionToYear6("A")
            .enterYear7ToYear11("")
            .enterYear12ToYear14("-1")
            .continue()
            .errorMessage("Reception to year 6 capacity must be a number")
            .errorMessage("Enter the Year 7 - Year 11 Capacity")
            .errorMessage("Year 12 to year 14 capacity must be between 0 and 9999")
            .continue()

        Logger.log("Enter valid capacity");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .enterReceptionToYear6("0")
            .enterYear7ToYear11("400")
            .enterYear12ToYear14("150")
            .continue()

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the capacity?")
            .hasReceptionToYear6("0")
            .hasYear7ToYear11("400")
            .hasYear12ToYear14("150")
            .continue();

        Logger.log("Check forms of entry optional");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("How many forms of entry are there?")
            .continue()
            .titleIs("What is the faith status?")
            .back();

        Logger.log("Check forms of entry validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("How many forms of entry are there?")
            .enterFormsOfEntry("-")
            .continue()
            .errorMessage("Forms of entry must not include special characters other than , ( ) '")
            .enterFormsOfEntry(dataGenerator.generateAlphaNumeric(101))
            .continue()
            .errorMessage("The forms of entry must be 100 characters or less");

        Logger.log("Enter valid form of entry");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .enterFormsOfEntry("3")
            .continue()

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("How many forms of entry are there?")
            .hasFormsOfEntry("3")
            .continue();

        Logger.log("Check faith status validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the faith status?")
            .continue()
            .errorMessage("Select the faith status of the free school");


        Logger.log("Select Designation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Designation")
            .continue();

        Logger.log("Check faith type validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("What is the faith type?")
            .continue()
            .errorMessage("Select the faith type of the free school")
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

        Logger.log("Select Greek Orthodox");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Greek Orthodox")
            .continue()
    
        Logger.log("Check provisional opening date validation");
        cy.executeAccessibilityTests();	       
        createProjectPage       
            .titleIs("What is the provisional opening date?")
            .setProvisionalOpeningDate("1", "10", "2020")
            .continue()
            .errorMessage("Provisional opening date date must be in the future");

        Logger.log("Enter valid provisional opening date");
        cy.executeAccessibilityTests();	       
        createProjectPage       
            .setProvisionalOpeningDate("1", "10", "2025")
            .continue();

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the provisional opening date?")
            .hasProvisionalOpeningDate("1", "10", "2025")
            .continue();

        Logger.log("Check notify email validation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("Who do you want to notify about this project?")
            .continue()
            .errorMessage("Please enter an email")
            .enterNotifyEmail("a")
            .continue()
            .errorMessage("Enter an email address in the correct format. For example, firstname.surname@education.gov.uk")
            .enterNotifyEmail("test.person@edunation.gov.uk")
            .continue()
            .errorMessage("Enter an email address in the correct format. For example, firstname.surname@education.gov.uk");
        
        Logger.log("Set email");
        cy.executeAccessibilityTests();	 
        createProjectPage   
            .enterNotifyEmail("test.person@education.gov.uk")
            .continue();

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("Who do you want to notify about this project?")
            .hasNotifyEmail("test.person@education.gov.uk")
            .continue();

        Logger.log("Check answers");
        cy.executeAccessibilityTests();

        summaryPage.inOrder()
            .summaryShows("Temporary Project ID").HasValue(temporaryProjectId).HasChangeLink()
            .summaryShows("Current free school name").HasValue(schoolName).HasChangeLink()
            .summaryShows("Region").HasValue("East of England").HasChangeLink()
            .summaryShows("Local authority").HasValue("Luton").HasChangeLink()
            .summaryShows("Trust").HasValue("Aurora Academies Trust").HasChangeLink()
            .summaryShows("Sixth form").HasValue("Yes").HasChangeLink()
            .summaryShows("Nursery").HasValue("No").HasChangeLink()
            .summaryShows("School phase").HasValue("Secondary").HasChangeLink()
            .summaryShows("Age range").HasValue("2-7").HasChangeLink()
            .summaryShows("School type").HasValue("Mainstream").HasChangeLink()
            .summaryShows("Reception to year 6 capacity").HasValue("0").HasChangeLink()
            .summaryShows("Year 7 to year 11 capacity").HasValue("400").HasChangeLink()
            .summaryShows("Year 12 to year 14 capacity").HasValue("150").HasChangeLink()
            .summaryShows("Faith status").HasValue("Designation").HasChangeLink()
            .summaryShows("Faith type").HasValue("Greek Orthodox").HasChangeLink()
            .summaryShows("Forms of entry").HasValue("3").HasChangeLink()
            .summaryShows("Provisional opening date agreed with trust").HasValue("1 October 2025").HasChangeLink();
    });
});