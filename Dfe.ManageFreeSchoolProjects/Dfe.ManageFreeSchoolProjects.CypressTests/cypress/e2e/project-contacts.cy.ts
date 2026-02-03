import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import dataGenerator from "cypress/fixtures/dataGenerator";
import contactsPage from "cypress/pages/contacts/contactsPage";
import editProjectAssignedToPage from "../pages/contacts/editProjectAssignedToPage";
import editTeamLeadPage from "../pages/contacts/editTeamLeadPage";
import editGrade6Page from "../pages/contacts/editGrade6Page";
import editProjectManagerPage from "cypress/pages/contacts/editProjectManagerPage";
import editProjectDirectorPage from "cypress/pages/contacts/editProjectDirectorPage";
import editOfstedContactPage from "cypress/pages/contacts/editOfstedContactPage";
import editTrustContactPage from "cypress/pages/contacts/editTrustContactPage";
import editPrincipalDesignatePage from "cypress/pages/contacts/editPrincipalDesignatePage";

describe("Testing that we can add contacts", () => {
    let project: ProjectDetailsRequest;
    let now: Date;

    beforeEach(() => {
        cy.login();

        now = new Date();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/contacts`);
            });
    });

    describe("Editing Contacts", () => {
        it("Should be able to edit Project assigned to", () => {

            cy.executeAccessibilityTests();

            contactsPage
                .hasProjectTitleHeader(project.schoolName)
                .hasProjectStatus("Pre-opening")
                .hasProjectAssignedToName(project.projectAssignedToName)
                .hasProjectAssignedToEmail(project.projectAssignedToEmail)            
            Logger.log("Edit Project assigned to")
            contactsPage.goToEditProjectAssignedTo();

            cy.executeAccessibilityTests();

            Logger.log("Check edit contact validation");
            editProjectAssignedToPage
                .hasProjectAssignedToTitle("Edit Project assigned to")
                .clickContinue()
                .withProjectAssignedToName("$da")
                .clickContinue()
                .errorForProjectAssignedToName("Project assigned to name must not include special characters other than , ( )")
                .withProjectAssignedToName("da")
                .clickContinue()
                .errorForProjectAssignedToName("Enter the full name, for example John Smith")
                .withProjectAssignedToName("da 1")
                .clickContinue()
                .errorForProjectAssignedToName("Project assigned to name must not include numbers")
                .withProjectAssignedToName(dataGenerator.generateAlphaNumeric(101))
                .clickContinue()
                .errorForProjectAssignedToName("Project assigned to name must be 100 characters or less")
                .withNullProjectAssignedToName()
                .clickContinue()
                .errorForProjectAssignedToName("Enter the name")
                .withProjectAssignedToName("Test Person")
                .withProjectAssignedToEmail("firstname.surname@educaion.gov.uk")
                .clickContinue()
                .errorForProjectAssignedToEmail("Email address must be in the format firstname.surname@education.gov.uk")
                .withProjectAssignedToEmail(dataGenerator.generateAlphaNumeric(101))
                .clickContinue()
                .errorForProjectAssignedToEmail("Project assigned to email must be 100 characters or less")
                .withNullProjectAssignedToEmail()
                .clickContinue()
                .errorForProjectAssignedToEmail("Enter the email")
                .withProjectAssignedToEmail("test.person@education.gov.uk")
                .clickContinue();

            contactsPage
                .hasProjectAssignedToName("Test Person")
                .hasProjectAssignedToEmail("test.person@education.gov.uk")

            });

        it("Should be able to edit Team lead", () => {

                cy.executeAccessibilityTests();
    
                contactsPage
                    .hasProjectTitleHeader(project.schoolName)
                    .hasProjectStatus("Pre-opening")
                    .isEmpty("team-lead-name")
                    .isEmpty("team-lead-email")
                Logger.log("Edit Team lead")
                contactsPage.goToEditTeamLead();
    
                cy.executeAccessibilityTests();
    
                Logger.log("Check edit contact validation");
                editTeamLeadPage
                    .hasTeamLeadTitle("Edit Team lead")
                    .clickContinue()
                    .withTeamLeadName("$da")
                    .clickContinue()
                    .errorForTeamLeadName("Team lead name must not include special characters other than , ( )")
                    .withTeamLeadName("da")
                    .clickContinue()
                    .errorForTeamLeadName("Enter the full name, for example John Smith")
                    .withTeamLeadName("da 1")
                    .clickContinue()
                    .errorForTeamLeadName("Team lead name must not include numbers")
                    .withTeamLeadName(dataGenerator.generateAlphaNumeric(101))
                    .clickContinue()
                    .errorForTeamLeadName("Team lead name must be 100 characters or less")
                    .withTeamLeadName("Test Person")
                    .withTeamLeadEmail("firstname.surname@educaion.gov.uk")
                    .clickContinue()
                    .errorForTeamLeadEmail("Email address must be in the format firstname.surname@education.gov.uk")
                    .withTeamLeadEmail(dataGenerator.generateAlphaNumeric(101))
                    .clickContinue()
                    .errorForTeamLeadEmail("Team lead email must be 100 characters or less")
                    .withTeamLeadEmail("test.person@education.gov.uk")
                    .clickContinue();
    
                contactsPage
                    .hasTeamLeadName("Test Person")
                    .hasTeamLeadEmail("test.person@education.gov.uk")
    
                });

                it("Should be able to edit Grade 6", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("grade-6-name")
                        .isEmpty("grade-6-email")
                    Logger.log("Edit Grade 6")
                    contactsPage.goToEditGrade6();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editGrade6Page
                        .hasGrade6Title("Edit Grade 6")
                        .clickContinue()
                        .withGrade6Name("$da")
                        .clickContinue()
                        .errorForGrade6Name("Grade 6 name must not include special characters other than , ( )")
                        .withGrade6Name("da")
                        .clickContinue()
                        .errorForGrade6Name("Enter the full name, for example John Smith")
                        .withGrade6Name("da 1")
                        .clickContinue()
                        .errorForGrade6Name("Grade 6 name must not include numbers")
                        .withGrade6Name(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForGrade6Name("Grade 6 name must be 100 characters or less")
                        .withGrade6Name("Test Person")
                        .withGrade6Email("firstname.surname@educaion.gov.uk")
                        .clickContinue()
                        .errorForGrade6Email("Email address must be in the format firstname.surname@education.gov.uk")
                        .withGrade6Email(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForGrade6Email("Grade 6 email must be 100 characters or less")
                        .withGrade6Email("test.person@education.gov.uk")
                        .clickContinue();
        
                    contactsPage
                        .hasGrade6Name("Test Person")
                        .hasGrade6Email("test.person@education.gov.uk")
        
                });

                it("Should be able to edit Project manager", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("project-manager-name")
                        .isEmpty("project-manager-email")
                    Logger.log("Edit Project manager")
                    contactsPage.goToEditProjectManager();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editProjectManagerPage
                        .hasProjectManagerTitle("Edit Project manager")
                        .clickContinue()
                        .withProjectManagerName("$da")
                        .clickContinue()
                        .errorForProjectManagerName("Project manager name must not include special characters other than , ( )")
                        .withProjectManagerName("da")
                        .clickContinue()
                        .errorForProjectManagerName("Enter the full name, for example John Smith")
                        .withProjectManagerName("da 1")
                        .clickContinue()
                        .errorForProjectManagerName("Project manager name must not include numbers")
                        .withProjectManagerName(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForProjectManagerName("Project manager name must be 100 characters or less")
                        .withProjectManagerName("Test Person")
                        .withProjectManagerEmail("firstname.surname")
                        .clickContinue()
                        .errorForProjectManagerEmail("Enter an email address in the correct format, like firstname.surname@outlook.com")
                        .withProjectManagerEmail(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForProjectManagerEmail("Project manager email must be 100 characters or less")
                        .withProjectManagerEmail("test.person@gmail.com")
                        .clickContinue();
        
                    contactsPage
                        .hasProjectManagerName("Test Person")
                        .hasProjectManagerEmail("test.person@gmail.com")
        
                });

                it("Should be able to edit Project director", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("project-director-name")
                        .isEmpty("project-director-email")
                    Logger.log("Edit Project director")
                    contactsPage.goToEditProjectDirector();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editProjectDirectorPage
                        .hasProjectDirectorTitle("Edit Project director")
                        .clickContinue()
                        .withProjectDirectorName("$da")
                        .clickContinue()
                        .errorForProjectDirectorName("Project director name must not include special characters other than , ( )")
                        .withProjectDirectorName("da")
                        .clickContinue()
                        .errorForProjectDirectorName("Enter the full name, for example John Smith")
                        .withProjectDirectorName("da 1")
                        .clickContinue()
                        .errorForProjectDirectorName("Project director name must not include numbers")
                        .withProjectDirectorName(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForProjectDirectorName("Project director name must be 100 characters or less")
                        .withProjectDirectorName("Test Person")
                        .withProjectDirectorEmail("firstname.surname")
                        .clickContinue()
                        .errorForProjectDirectorEmail("Enter an email address in the correct format, like firstname.surname@outlook.com")
                        .withProjectDirectorEmail(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForProjectDirectorEmail("Project director email must be 100 characters or less")
                        .withProjectDirectorEmail("test.person@gmail.com")
                        .clickContinue();
        
                    contactsPage
                        .hasProjectDirectorName("Test Person")
                        .hasProjectDirectorEmail("test.person@gmail.com")
        
                });

                it("Should be able to edit Ofsted contact", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("ofsted-contact-name")
                        .isEmpty("ofsted-contact-email")
                    Logger.log("Edit Ofsted contact")
                    contactsPage.goToEditOfstedContact();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editOfstedContactPage
                        .hasOfstedContactTitle("Edit Ofsted contact")
                        .clickContinue()
                        .withOfstedContactName("$da")
                        .clickContinue()
                        .errorForOfstedContactName("Ofsted contact name must not include special characters other than , ( )")
                        .withOfstedContactName("da")
                        .clickContinue()
                        .errorForOfstedContactName("Enter the full name, for example John Smith")
                        .withOfstedContactName("da 1")
                        .clickContinue()
                        .errorForOfstedContactName("Ofsted contact name must not include numbers")
                        .withOfstedContactName(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForOfstedContactName("Ofsted contact name must be 100 characters or less")
                        .withOfstedContactName("Test Person")
                        .withOfstedContactEmail("firstname.surname")
                        .clickContinue()
                        .errorForOfstedContactEmail("Enter an email address in the correct format, like firstname.surname@outlook.com")
                        .withOfstedContactEmail(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForOfstedContactEmail("Ofsted contact email must be 100 characters or less")
                        .withOfstedContactEmail("test.person@gmail.com")
                        .withOfstedContactPhoneNumber("0")
                        .clickContinue()
                        .errorForOfstedContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withOfstedContactPhoneNumber("(0++  --0)")
                        .clickContinue()
                        .errorForOfstedContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withOfstedContactPhoneNumber("55555555555555555555")
                        .clickContinue()
                        .errorForOfstedContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withOfstedContactPhoneNumber("07123456e")
                        .clickContinue()
                        .errorForOfstedContactPhoneNumber("Phone number must only include numbers and ( ) - +")
                        .withOfstedContactPhoneNumber("(0) 7123456-89")
                        .withOfstedContactRole("^^^")
                        .clickContinue()
                        .errorForOfstedContactRole("Ofsted contact role must not include special characters other than , ( ) '")
                        .withOfstedContactRole(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForOfstedContactRole("Ofsted contact role must be 100 characters or less")
                        .withOfstedContactRole("Tester")
                        .clickContinue();

                    contactsPage
                        .hasOfstedContactName("Test Person")
                        .hasOfstedContactEmail("test.person@gmail.com")
                        .hasOfstedContactPhoneNumber("(0) 7123456-89")
                        .hasOfstedContactRole("Tester");
        
                });

                it("Should be able to edit Trust contact", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("trust-contact-name")
                        .isEmpty("trust-contact-email")
                    Logger.log("Edit Trust contact")
                    contactsPage.goToEditTrustContact();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editTrustContactPage
                        .hasTrustContactTitle("Edit Trust contact")
                        .clickContinue()
                        .withTrustContactName("$da")
                        .clickContinue()
                        .errorForTrustContactName("Trust contact name must not include special characters other than , ( )")
                        .withTrustContactName("da")
                        .clickContinue()
                        .errorForTrustContactName("Enter the full name, for example John Smith")
                        .withTrustContactName("da 1")
                        .clickContinue()
                        .errorForTrustContactName("Trust contact name must not include numbers")
                        .withTrustContactName(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForTrustContactName("Trust contact name must be 100 characters or less")
                        .withTrustContactName("Test Person")
                        .withTrustContactEmail("firstname.surname")
                        .clickContinue()
                        .errorForTrustContactEmail("Enter an email address in the correct format, like firstname.surname@outlook.com")
                        .withTrustContactEmail(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForTrustContactEmail("Trust contact email must be 100 characters or less")
                        .withTrustContactEmail("test.person@gmail.com")
                        .withTrustContactPhoneNumber("0")
                        .clickContinue()
                        .errorForTrustContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withTrustContactPhoneNumber("(0++  --0)")
                        .clickContinue()
                        .errorForTrustContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withTrustContactPhoneNumber("55555555555555555555")
                        .clickContinue()
                        .errorForTrustContactPhoneNumber("Phone number must be between 5 numbers and 15 numbers")
                        .withTrustContactPhoneNumber("07123456e")
                        .clickContinue()
                        .errorForTrustContactPhoneNumber("Phone number must only include numbers and ( ) - +")
                        .withTrustContactPhoneNumber("(0) 7123456-89")
                        .withTrustContactRole("^^^")
                        .clickContinue()
                        .errorForTrustContactRole("Trust contact role must not include special characters other than , ( ) '")
                        .withTrustContactRole(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForTrustContactRole("Trust contact role must be 100 characters or less")
                        .withTrustContactRole("Tester")
                        .clickContinue();

                    contactsPage
                        .hasTrustContactName("Test Person")
                        .hasTrustContactEmail("test.person@gmail.com")
                        .hasTrustContactPhoneNumber("(0) 7123456-89")
                        .hasTrustContactRole("Tester");
        
                });

                it("Should be able to edit Principal designate", () => {

                    cy.executeAccessibilityTests();
        
                    contactsPage
                        .hasProjectTitleHeader(project.schoolName)
                        .hasProjectStatus("Pre-opening")
                        .isEmpty("principal-designate-name")
                        .isEmpty("principal-designate-email")
                    Logger.log("Edit Principal designate")
                    contactsPage.goToEditPrincipalDesignate();
        
                    cy.executeAccessibilityTests();
        
                    Logger.log("Check edit contact validation");
                    editPrincipalDesignatePage
                        .hasPrincipalDesignateTitle("Edit Principal designate")
                        .clickContinue()
                        .withPrincipalDesignateName("$da")
                        .clickContinue()
                        .errorForPrincipalDesignateName("Principal designate name must not include special characters other than , ( )")
                        .withPrincipalDesignateName("da")
                        .clickContinue()
                        .errorForPrincipalDesignateName("Enter the full name, for example John Smith")
                        .withPrincipalDesignateName("da 1")
                        .clickContinue()
                        .errorForPrincipalDesignateName("Principal designate name must not include numbers")
                        .withPrincipalDesignateName(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForPrincipalDesignateName("Principal designate name must be 100 characters or less")
                        .withPrincipalDesignateName("Test Person")
                        .withPrincipalDesignateEmail("firstname.surname")
                        .clickContinue()
                        .errorForPrincipalDesignateEmail("Enter an email address in the correct format, like firstname.surname@outlook.com")
                        .withPrincipalDesignateEmail(dataGenerator.generateAlphaNumeric(101))
                        .clickContinue()
                        .errorForPrincipalDesignateEmail("Principal designate email must be 100 characters or less")
                        .withPrincipalDesignateEmail("test.person@gmail.com")
                        .clickContinue();

                    contactsPage
                        .hasPrincipalDesignateName("Test Person")
                        .hasPrincipalDesignateEmail("test.person@gmail.com")
        
                });

    })
})