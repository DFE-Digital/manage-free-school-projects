import { Logger } from "cypress/common/logger";
import { ProjectRecordCreator } from "cypress/constants/cypressConstants";
import homePage from "cypress/pages/homePage";
import updateMultipleFields from "cypress/pages/updateMultipleFields";
import checkYourAnswers from "cypress/pages/checkYourAnswers";

describe("Update multiple fields - Bulk edit", () => {
    beforeEach(() => {
        cy.login({ role: "POTATO" });
        cy.visit(Cypress.env('url'));
    });

    describe("Update multiple fields of projects", () => {
        beforeEach(() => {
            cy.login({ role: ProjectRecordCreator });
            cy.visit('/');
        });

        it("should able to upload a csv or  xlsx file", () => {
            Logger.log(" Navigate to update multiple fields card");
            cy.contains('Update multiple fields').should('be.visible');
            cy.executeAccessibilityTests();
            homePage.updateMultipleFields();
            cy.executeAccessibilityTests();
            //upload without selecting any file
            updateMultipleFields
                .clickUpload()
                .errorForUpload('Select a file')
            cy.executeAccessibilityTests();

            //upload a valid xlsx file
            cy.fixture('bulk-edit-records.xlsx').then(fileContent => {
                cy.get('[data-testid="upload"]').attachFile({
                    fileContent: fileContent,
                    fileName: 'bulk-edit-records.xlsx',
                    mimeType: 'text/zip'

                })
            })

            updateMultipleFields
                .clickUpload()
            checkYourAnswers
                .recordsVisible()
                .editProjects()

            cy.get('.govuk-panel').should('contain.text', 'free school projects edited')


        });
    });

});    