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


        const currentEnvironment = Cypress.env('environment'); 

        const expectedTrustNames = {
            'dev': 'Aurora Academies Trust',
            'test': 'The James Web School'};

        const expectedTrustName = expectedTrustNames[currentEnvironment];

        const expectedTrustTypes = {
            'dev': 'MAT (multi-academy trust)',
            'test': 'SAT (single academy trust)'};

        const expectedTrustType = expectedTrustTypes[currentEnvironment];


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
            .hasCorrectTrustName(expectedTrustName)
            .hasCorrectTrustType(expectedTrustType)
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
            .setProvisionalOpeningDate("1", "10", "2035")
            .continue();

        Logger.log("Back returns to previous page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .back()
            .titleIs("What is the provisional opening date?")
            .hasProvisionalOpeningDate("1", "10", "2035")
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
            .summaryShows("Provisional opening date agreed with trust").HasValue("1 October 2035").HasChangeLink();

        createProjectPage.clickCreateProject();

        Logger.log("Confirm created");
        cy.executeAccessibilityTests();

        createProjectPage.hasConfirmedProjectId(temporaryProjectId)
        .hasConfirmedEmailMessage("We have sent a notification email to test.person@education.gov.uk.")

    });

    it("Should navigate back to change project fields", () => {
        Cypress.config('redirectionLimit', 100)
        const temporaryProjectId = dataGenerator.generateTemporaryId();
        const schoolName = dataGenerator.generateSchoolName();
        const updatedTemporaryProjectId = dataGenerator.generateTemporaryId();
        const updatedSchoolName = dataGenerator.generateSchoolName();

        const TestTrn = "TR00111";

        cy.executeAccessibilityTests();

        homePage.createNewProjects();

        Logger.log("Use individual method");
        createProjectPage
            .selectOption("Create one project")
            .continue();
        cy.executeAccessibilityTests();
            
        Logger.log("Enter Valid project ID");
        cy.executeAccessibilityTests();
        createProjectPage
            .enterProjectId(temporaryProjectId)
            .continue()

        Logger.log("Enter school name");
        cy.executeAccessibilityTests();
        createProjectPage
            .enterSchoolName(schoolName)
            .continue()

        Logger.log("Select East of England");
        cy.executeAccessibilityTests();	
        createProjectPage
            .selectOption("East of England")
            .continue();

        Logger.log("Select Local authority");
        cy.executeAccessibilityTests();	
        createProjectPage
            .selectOption("Luton")
            .continue();
            
        Logger.log("Enter valid trust");
        cy.executeAccessibilityTests();	   
        createProjectPage
            .enterTRN(TestTrn)
            .continue();

        Logger.log("Selecting yes moves to next page");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .titleIs("Confirm the trust")
            .selectOption("Yes")
            .continue();

        Logger.log("Selecting Mainstream school type");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Mainstream")
            .continue();

        Logger.log("Selecting Secondary school phase");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("Secondary")
            .continue();

        Logger.log("Select valid class types and continue");
        createProjectPage
            .setSixthFormTo("Yes")
            .setNurseryTo("No")
            .continue();

        Logger.log("Enter valid age range");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .enterAgeRangeFrom("2")
            .enterAgeRangeTo("7")
            .continue()
 
        Logger.log("Enter valid capacity");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .enterReceptionToYear6("0")
            .enterYear7ToYear11("400")
            .enterYear12ToYear14("150")
            .continue()

        Logger.log("Enter valid form of entry");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .enterFormsOfEntry("3")
            .continue()

        Logger.log("Select Designation");
        cy.executeAccessibilityTests();	       
        createProjectPage
            .selectOption("None")
            .continue();

        Logger.log("Enter valid provisional opening date");
        cy.executeAccessibilityTests();	       
        createProjectPage       
            .setProvisionalOpeningDate("1", "10", "2035")
            .continue();

        Logger.log("Set email");
        cy.executeAccessibilityTests();	 
        createProjectPage   
            .enterNotifyEmail("test.person@education.gov.uk")
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
            .summaryShows("Faith status").HasValue("None").HasChangeLink()
            .summaryShows("Faith type").IsEmpty().HasChangeLink()
            .summaryShows("Forms of entry").HasValue("3").HasChangeLink()
            .summaryShows("Provisional opening date agreed with trust").HasValue("1 October 2035").HasChangeLink();

        cy.log("Check back behaviour Temporary Project ID")
        summaryPage.clickChangeFor("Temporary Project ID");
        createProjectPage.back()
        summaryPage.SummaryHasValue("Temporary Project ID", temporaryProjectId);

        cy.log("Update Temporary Project ID")
        summaryPage.clickChangeFor("Temporary Project ID");
        createProjectPage.enterProjectId(updatedTemporaryProjectId)
            .continue()
        summaryPage.SummaryHasValue("Temporary Project ID", updatedTemporaryProjectId);
        
        cy.log("Check back behaviour Current free school name")
        summaryPage.clickChangeFor(" Current free school name");
        createProjectPage.back()
        summaryPage.SummaryHasValue(" Current free school name", schoolName);

        cy.log("Update Current free school name")
        summaryPage.clickChangeFor("Current free school name");
        createProjectPage.enterSchoolName(updatedSchoolName)
            .continue()
        summaryPage.SummaryHasValue("Current free school name", updatedSchoolName);

        cy.log("Check back behaviour for region and local authority")
        summaryPage.clickChangeFor("Region");
        createProjectPage.selectOption("London")
            .back()
        summaryPage.SummaryHasValue("Region", "East of England");
        
        summaryPage.clickChangeFor("Region");
        createProjectPage.selectOption("London")
            .continue()
            .selectOption("Camden")
            .back()
            .selectOption("London")
            .continue()
            .selectOption("Camden")
            .back()
            .back()
        summaryPage
            .SummaryHasValue("Region", "East of England")
            .SummaryHasValue("Local authority", "Luton");   
        
        cy.log("Update region and local authority")
        summaryPage.clickChangeFor("Region");
        createProjectPage.selectOption("London")
            .continue()
            .selectOption("Camden")
            .continue();
        summaryPage.SummaryHasValue("Region", "London");
        summaryPage.SummaryHasValue("Local authority", "Camden");

        cy.log("Update local authority")
        summaryPage.clickChangeFor("Local authority");
        createProjectPage.selectOption("Hackney")
            .continue();
        summaryPage.SummaryHasValue("Local authority", "Hackney");

        cy.log("Update Trust")
        
        cy.log("Check back behaviour Trust")
        summaryPage.clickChangeFor("Trust");
        createProjectPage.back()
        summaryPage.SummaryHasValue("Trust", "Aurora Academies Trust");
        summaryPage.clickChangeFor("Trust")
        createProjectPage.enterTRN("TR00012")
            .continue()
            .back()
            .enterTRN("TR00012")
            .continue()
            .selectOption("No")
            .continue()
            .back()
        summaryPage.SummaryHasValue("Trust", "Aurora Academies Trust");

        cy.log("Update Trust")
        summaryPage.clickChangeFor("Trust");
        createProjectPage.enterTRN("TR00012")
            .continue()
            .selectOption("Yes")
            .continue()
        summaryPage.SummaryHasValue("Trust", "The Academy of Central Bedfordshire");

        cy.log("Check back behaviour for Sixth Form")
        summaryPage.clickChangeFor("Sixth form");
        createProjectPage.setSixthFormTo("No")
            .setNurseryTo("Yes")
            .back()
        summaryPage
            .SummaryHasValue("Sixth form", "Yes")
            .SummaryHasValue("Nursery", "No")
        
        cy.log("Check back behaviour for Nursery")
        summaryPage.clickChangeFor("Nursery");
        createProjectPage.setSixthFormTo("No")
            .setNurseryTo("Yes")
            .back()
        summaryPage
            .SummaryHasValue("Sixth form", "Yes")
            .SummaryHasValue("Nursery", "No")

        cy.log("Change Sixth Form")
        summaryPage.clickChangeFor("Sixth form");
        createProjectPage.setSixthFormTo("No")
            .setNurseryTo("Yes")
            .continue();
        summaryPage
            .SummaryHasValue("Sixth form", "No")
            .SummaryHasValue("Nursery", "Yes");

        cy.log("Change Nursery")
        summaryPage.clickChangeFor("Nursery");
        createProjectPage.setSixthFormTo("Yes")
            .setNurseryTo("No")
            .continue();
        summaryPage
            .SummaryHasValue("Sixth form", "Yes")
            .SummaryHasValue("Nursery", "No");

        cy.log("Check back behaviour for School phase")
        summaryPage.clickChangeFor("School phase");
        createProjectPage.selectOption("Primary")
            .back()
        summaryPage
            .SummaryHasValue("School phase", "Secondary");

        cy.log("Change School phase")
        summaryPage.clickChangeFor("School phase");
        createProjectPage.selectOption("All-through")
            .continue();
        summaryPage
            .SummaryHasValue("School phase", "All-through");
        
        cy.log("Check back behaviour for Age range")
        summaryPage.clickChangeFor("Age range");
        createProjectPage.enterAgeRangeFrom("4")
            .enterAgeRangeTo("11")
            .back();
        summaryPage
            .SummaryHasValue("Age range", "2-7");

        cy.log("Change Age range")
        summaryPage.clickChangeFor("Age range");
        createProjectPage.enterAgeRangeFrom("4")
            .enterAgeRangeTo("11")
            .continue();
        summaryPage
            .SummaryHasValue("Age range", "4-11");
            
        cy.log("Check back behaviour for School type")
        summaryPage.clickChangeFor("School type");
        createProjectPage.selectOption("Special")
            .back();
        summaryPage
            .SummaryHasValue("School type", "Mainstream");

        cy.log("Change School type")
        summaryPage.clickChangeFor("School type");
        createProjectPage.selectOption("Special")
            .continue();
        summaryPage
            .SummaryHasValue("School type", "Special");
        
        cy.log("Check back behaviour for capacity")
        summaryPage.clickChangeFor("Reception to year 6 capacity");
        createProjectPage.enterReceptionToYear6("120")
            .back();
        summaryPage
            .SummaryHasValue("Reception to year 6 capacity", "0");

        summaryPage.clickChangeFor("Year 7 to year 11 capacity");
        createProjectPage.enterYear7ToYear11("120")
            .back();
        summaryPage
            .SummaryHasValue("Year 7 to year 11 capacity", "400");

        summaryPage.clickChangeFor("Year 12 to year 14 capacity");
        createProjectPage.enterYear12ToYear14("120")
            .back();
        summaryPage
            .SummaryHasValue("Year 12 to year 14 capacity", "150");

        cy.log("Change capacity")
        summaryPage.clickChangeFor("Year 12 to year 14 capacity");
        createProjectPage
            .enterReceptionToYear6("120")
            .enterYear7ToYear11("600")
            .enterYear12ToYear14("100")
            .continue();
        summaryPage
            .SummaryHasValue("Reception to year 6 capacity", "0")
            .SummaryHasValue("Year 7 to year 11 capacity", "600")
            .SummaryHasValue("Year 12 to year 14 capacity", "100");

        cy.log("Check back behaviour for faith status")
        summaryPage.clickChangeFor("Faith status");
        createProjectPage.selectOption("Ethos")
            .back();
        summaryPage.SummaryHasValue("Faith status", "None");
        summaryPage.clickChangeFor("Faith type");
        createProjectPage.selectOption("Ethos")
            .back();
        summaryPage.SummaryHasValue("Faith status", "None")
            .clickChangeFor("Faith status");
        createProjectPage.selectOption("Ethos")
            .continue()
            .selectOption("Roman Catholic")
            .back()
            .back()
        summaryPage.SummaryHasValue("Faith status", "None")
            .SummaryHasValue("Faith type", "Empty");
        
        cy.log("Change faith status")
        summaryPage.clickChangeFor("Faith status");
        createProjectPage.selectOption("Ethos")
            .continue()
            .selectOption("Roman Catholic")
            .continue();
        summaryPage.SummaryHasValue("Faith status", "Ethos")
            .SummaryHasValue("Faith type", "Roman Catholic");

        cy.log("Change other faith type")
        summaryPage.clickChangeFor("Faith status");
        createProjectPage.selectOption("Designation")
            .continue()
            .selectOption("Other")
            .enterOtherFaith("Test Faith")
            .continue();
        summaryPage.SummaryHasValue("Faith status", "Designation")
            .SummaryHasValue("Faith type", "Other - Test Faith")

        cy.log("Check back behaviour for Forms of entry")
        summaryPage.clickChangeFor("Forms of entry");
        createProjectPage.enterFormsOfEntry("4")
            .back()
        summaryPage
            .SummaryHasValue("Forms of entry", "3");

        cy.log("Change Forms of entry")
        summaryPage.clickChangeFor("Forms of entry");
        createProjectPage.enterFormsOfEntry("4")
            .continue()
        summaryPage
            .SummaryHasValue("Forms of entry", "4");

        cy.log("Check back behaviour for Provisional opening date")
        summaryPage.clickChangeFor("Provisional opening date");
        createProjectPage.setProvisionalOpeningDate("12", "11", "2034")
            .back()
        summaryPage
            .SummaryHasValue("Provisional opening date agreed with trust", "1 October 2035");

        cy.log("Change Provisional opening date agreed with trust")
        summaryPage.clickChangeFor("Provisional opening date agreed with trust");
        createProjectPage.setProvisionalOpeningDate("12", "11", "2034")
            .continue()
        summaryPage
            .SummaryHasValue("Provisional opening date agreed with trust", "12 November 2034");
        
        createProjectPage.clickCreateProject();

        Logger.log("Confirm created");
        cy.executeAccessibilityTests();
        createProjectPage.hasConfirmedProjectId(updatedTemporaryProjectId)
        .hasConfirmedEmailMessage("We have sent a notification email to test.person@education.gov.uk.")
    });
});