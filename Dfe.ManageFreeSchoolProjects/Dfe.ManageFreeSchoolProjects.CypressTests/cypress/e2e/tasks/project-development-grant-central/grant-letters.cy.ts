import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import pdgDashboard from "cypress/pages/tasks/project-development-grant-central/pdgDashboard";
import editGrantLetter from "cypress/pages/tasks/project-development-grant-central/edit-grant-letter";
import addGrantLetter from "cypress/pages/tasks/project-development-grant-central/add-grant-letter";
import addVariationLetter from "cypress/pages/tasks/project-development-grant-central/add-variation-letter";
import editVariationLetter from "cypress/pages/tasks/project-development-grant-central/edit-variation-letter";
import { GrantManagers } from "cypress/constants/cypressConstants";

describe("Grant letters Task", () => {
    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login({ role: GrantManagers });
        project = RequestBuilder.createProjectDetailsNonPresumption();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks/`);
            });
    });

    it("Should successfully add grant letter and variations", () => {

        cy.log("Select Project development grant (PDG)");
        taskListPage.isTaskStatusIsNotStarted("PDG")
            .selectPDGFromTaskList();

        cy.log("Confirm empty dashboard");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Project development grant (PDG)")
            .inOrder()
            .skip(5)
            .summaryShows("No information has been added.")
        cy.executeAccessibilityTests();

        pdgDashboard
          .selectChangeGrantLetter();

        cy.executeAccessibilityTests();

        cy.log("All fields are optional");
        editGrantLetter
            .grantTitleIs("Edit Grant letters")
            .schoolNameIs(project.schoolName)
            .addGrantLetter();
        cy.executeAccessibilityTests();

        addGrantLetter
            .titleIs("Add grant letter")
            .schoolNameIs(project.schoolName)
            .withInitialGrantLetterDate("a", "12", "2025")
            .withFullGrantLetterDate("12", "14", "2025")
            .clickContinue()
            .errorForInitialGrantLetterDate().showsError("Day must be a number, like 12")
            .errorForFullGrantLetterDate().showsError("Month must be between 1 and 12")
        cy.executeAccessibilityTests();

        cy.log('Confirm all set')
        addGrantLetter
            .withInitialGrantLetterDate("12", "09", "2025")
            .checkSavedInWorkplacesForInitialGrant()
            .withFullGrantLetterDate("15", "02", "2028")
            .checkSavedInWorkplacesForFullGrant()
            .clickContinue()
        editGrantLetter
            .grantLetterAdded()

        summaryPage
            .inOrder()
            .summaryShows("Date when DfE signed the initial grant letter").HasValue("12 September 2025")
            .summaryShows("Saved the signed initial grant letter in Workplaces folder").HasValue("Yes")
            .summaryShows("Date when DfE signed the final grant letter").HasValue("15 February 2028")
            .summaryShows("Saved the signed full grant letter in Workplaces folder").HasValue("Yes")
        cy.executeAccessibilityTests();

        editGrantLetter
            .showVariationLabel()
            .addVariation("1");

        addVariationLetter
            .schoolNameIs(project.schoolName)
            .variationTitleIs('Add variation letter 1')
            .variationLetterDueDate(" ", "", " ")
            .clickContinue()
            .errorForDueDate().showsError("Enter a date for the due date of variation letter")
        cy.executeAccessibilityTests();

        addVariationLetter
            .variationLetterDueDate("23", "05", "2028")
            .checkSavedInWorkplacesForInitialGrant()
            .clickContinue();

        editGrantLetter
            .variationLetterAdded("1")
            .changeVariationLetter("1")
        cy.executeAccessibilityTests();

        editVariationLetter
            .variationLetterDueDate("01", "01", "2029")
            .clickContinue();
        cy.executeAccessibilityTests();

        editGrantLetter
            .showVariationLabel()
            .addVariation("2");

        addVariationLetter
            .clickDiscard()    

        summaryPage
            .inOrder()
            .skip(4)
            .summaryShows("Due date of variation letter").HasValue("1 January 2029")
            .summaryShows("Saved the variation letter in Workplaces folder").HasValue("Yes")
            .clickBack()

        summaryPage.MarkAsComplete()
            .clickConfirmAndContinue();

        taskListPage.isTaskStatusIsCompleted("PDG");
    });
});
