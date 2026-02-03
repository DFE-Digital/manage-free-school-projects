import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import fundingAgreementEditPage from "../../../pages/tasks/pre-opening/edit-funding-agreement.cy";

describe("Testing Funding agreement Task", () => {

    let project: ProjectDetailsRequest;

    beforeEach(() => {
        cy.login();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
            .then(() => {
                cy.visit(`/projects/${project.projectId}/tasks`);
            });
    });

    it("Should successfully set funding agreement task", () => {

        cy.log("Select funding agreement");
        taskListPage.isTaskStatusIsNotStarted("FundingAgreement")
            .selectFundingAgreementFromTaskList();

        cy.log("Confirm empty funding agreement");

        cy.log("Confirm empty funding agreement");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement")
            .inOrder()
            .summaryShows("Tailored the funding agreement (FA)").IsEmpty().HasChangeLink()
            .summaryShows("Shared FA with the trust").IsEmpty().HasChangeLink()
            .summaryShows("Trust has signed the FA").IsEmpty().HasChangeLink()
            .summaryShows("Expected date FA is signed on Secretary of State's behalf").IsEmpty().HasChangeLink()
            .summaryShows("Funding agreement signed").IsEmpty().HasChangeLink()
            .summaryShows("Saved FA documents in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();


        cy.executeAccessibilityTests();
        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open funding agreement");
        taskListPage.isTaskStatusIsNotStarted("FundingAgreement")
            .selectFundingAgreementFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("FundingAgreement")
            .selectFundingAgreementFromTaskList();

        cy.log("Check search page");

        summaryPage.clickChange();

        cy.executeAccessibilityTests({ "aria-allowed-attr": { enabled: false } });
        
        cy.log("Trust signed FA date validation")

        fundingAgreementEditPage
            .checkTrustHasSignedTheFa()
            .withDateTrustSignedFA("2","ds","2050")
            .clickContinue()
            .errorForTrustSignedFADate().showsError("Enter a date in the correct format")
            .withDateTrustSignedFA("2","2","2090")
            .clickContinue()
            .errorForTrustSignedFADate().showsError("Year must be between 2000 and 2050")
            .withDateTrustSignedFA("2","2","1999")
            .clickContinue()
            .errorForTrustSignedFADate().showsError("Year must be between 2000 and 2050")
            .withDateTrustSignedFA("","","")
            
        cy.log("Trust signed FA date validation")

        cy.log("Expected date FA is signed on Secretary of State's behalf validation")

        fundingAgreementEditPage
            .withExpectedDate("2","ds","2050")
            .clickContinue()
            .errorForExpectedDate().showsError("Enter a date in the correct format")
            .withExpectedDate("2","2","2090")
            .clickContinue()
            .errorForExpectedDate().showsError("Year must be between 2000 and 2050")
            .withExpectedDate("2","2","1999")
            .clickContinue()
            .errorForExpectedDate().showsError("Year must be between 2000 and 2050")
            .withExpectedDate("","","")

        cy.log("Expected date FA is signed on Secretary of State's behalf validation")

        cy.log("Actual date FA was signed validation")

        fundingAgreementEditPage
            .checkFundingAgreementSigned()
            .clickContinue()
            .errorForDateFAWasSigned().showsError("Enter the actual date FA was signed")
            .withDateFAWasSigned("2","ds","2050")
            .clickContinue()
            .errorForDateFAWasSigned().showsError("Enter a date in the correct format")
            .withDateFAWasSigned("2","2","2090")
            .clickContinue()
            .errorForDateFAWasSigned().showsError("Year must be between 2000 and 2050")
            .withDateFAWasSigned("2","2","1999")
            .clickContinue()
            .errorForDateFAWasSigned().showsError("Year must be between 2000 and 2050")

        cy.log("Actual date FA was signed validation")

        cy.log("Fill in valid funding agreement")

        fundingAgreementEditPage
            .checkTailoredTheFundingAgreement()
            .checkSharedFAWithTrust()
            .checkTrustHasSignedTheFa()
            .withDateTrustSignedFA("2","2","2050")
            .withExpectedDate("2","2","2050")
            .checkFundingAgreementSigned()
            .withDateFAWasSigned("2","2","2050")
            .checkSavedFADocumentsInWorkplacesFolder()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement")
            .inOrder()
            .summaryShows("Tailored the funding agreement (FA)").HasValue("Yes").HasChangeLink()
            .summaryShows("Shared FA with the trust").HasValue("Yes").HasChangeLink()
            .summaryShows("Trust has signed the FA").HasValue("Yes").HasChangeLink()
            .summaryShows("Date the trust signed FA").HasValue("2 February 2050").HasChangeLink()
            .summaryShows("Expected date FA is signed on Secretary of State's behalf").HasValue("2 February 2050").HasChangeLink()
            .summaryShows("Funding agreement signed").HasValue("Yes").HasChangeLink()
            .summaryShows("Date FA was signed").HasValue("2 February 2050").HasChangeLink()
            .summaryShows("Saved FA documents in Workplaces folder").HasValue("Yes").HasChangeLink()

        cy.log("Unselect select date signed")
       
        summaryPage.clickChange();
        fundingAgreementEditPage
            .uncheckTailoredTheFundingAgreement()    
            .uncheckSharedFAWithTrust()
            .uncheckTrustHasSignedTheFa()
            .uncheckFundingAgreementSigned()
            .uncheckSavedFADocumentsInWorkplacesFolder()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Funding agreement")
            .inOrder()
            .summaryShows("Tailored the funding agreement (FA)").IsEmpty().HasChangeLink()
            .summaryShows("Shared FA with the trust").IsEmpty().HasChangeLink()
            .summaryShows("Trust has signed the FA").HasValue("No").HasChangeLink()
            .summaryDoesNotShow("Date the trust signed FA")
            .summaryShows("Expected date FA is signed on Secretary of State's behalf").HasValue("2 February 2050").HasChangeLink()
            .summaryShows("Funding agreement signed").IsEmpty().HasChangeLink()
            .summaryDoesNotShow("Date FA was signed")
            .summaryShows("Saved FA documents in Workplaces folder").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        taskListPage
            .selectFundingAgreementFromTaskList()
        
        summaryPage.isMarkedAsComplete()
    })
    
    
    
})