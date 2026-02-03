import { ProjectDetailsRequest } from "cypress/api/domain";
import projectApi from "cypress/api/projectApi";
import { RequestBuilder } from "cypress/api/requestBuilder";
import { Logger } from "cypress/common/logger";
import summaryPage from "cypress/pages/task-summary-base";
import taskListPage from "cypress/pages/taskListPage";
import editPrincipalDesignatePage from "../../../pages/tasks/pre-opening/edit-principal-designate.cy";

describe("Testing Principal designate task", () => {
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

    it("Should be able to set a Principal designate", () => {
        
        Logger.log("Select principal designate");
        taskListPage.isTaskStatusIsNotStarted("PrincipalDesignate")
            .selectPrincipleDesignateFromTaskList();

        cy.log("Confirm empty Principal designate");
        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").IsEmpty().HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").IsEmpty().HasChangeLink()
            .summaryShows("Principal designate appointed").IsEmpty().HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete();

        cy.log("Go back to task list");
        summaryPage.clickBack();

        cy.log("Confirm not started and open principal designate");
        taskListPage.isTaskStatusIsNotStarted("PrincipalDesignate")
            .selectPrincipleDesignateFromTaskList();

        cy.log("Check confirm puts project in In Progress");
        summaryPage.clickConfirmAndContinue();

        taskListPage.isTaskStatusInProgress("PrincipalDesignate")
            .selectPrincipleDesignateFromTaskList();
        
        summaryPage.clickChange();

        cy.log("Check empty values accepted");

        editPrincipalDesignatePage
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").IsEmpty().HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").IsEmpty().HasChangeLink()
            .summaryShows("Principal designate appointed").IsEmpty().HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .clickChange()
        
        cy.log("Check validation for actual principal designate date");
        
        editPrincipalDesignatePage
            .withPrincipleDesignateAppointedDate("24","","")
            .clickContinue()
            .errorForPrincipleDesignateAppointedDate().showsError("Actual date that principal designate was appointed must include a month and year")
            .withPrincipleDesignateAppointedDate("24","4","")
            .clickContinue()
            .errorForPrincipleDesignateAppointedDate().showsError("Actual date that principal designate was appointed must include a year")
            .withPrincipleDesignateAppointedDate("24","4","2049")
            .checkYesForPrincipleDesignate()
            .checkYesForExternalExpert()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").HasValue("Yes").HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").IsEmpty().HasChangeLink()
            .summaryShows("Principal designate appointed").HasValue("Yes").HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").HasValue("24 April 2049").HasChangeLink()
            .isNotMarkedAsComplete();

        summaryPage.clickChange();
//
        cy.log("Check validation for expected principal designate date");
        
        editPrincipalDesignatePage
            .withExpectedPrincipleDesignateAppointedDate("25","","")
            .clickContinue()
            .errorForExpectedPrincipleDesignateAppointedDate().showsError("Expected date that principal designate will be appointed must include a month and year")
            .withExpectedPrincipleDesignateAppointedDate("25","4","")
            .clickContinue()
            .errorForExpectedPrincipleDesignateAppointedDate().showsError("Expected date that principal designate will be appointed must include a year")
            .withExpectedPrincipleDesignateAppointedDate("25","4","2049")
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").HasValue("Yes").HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").HasValue("25 April 2049").HasChangeLink()
            .summaryShows("Principal designate appointed").HasValue("Yes").HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").HasValue("24 April 2049").HasChangeLink()
            .isNotMarkedAsComplete();

        summaryPage.clickChange();
//
        editPrincipalDesignatePage
            .checkNoForPrincipleDesignate()
            .checkNotApplicableForExternalExpert()
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").HasValue("Not applicable").HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").HasValue("25 April 2049").HasChangeLink()
            .summaryShows("Principal designate appointed").IsEmpty().HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").HasValue("24 April 2049").HasChangeLink()
            .isNotMarkedAsComplete()

        cy.log("can edit principal designate");

        summaryPage.clickChange();


        editPrincipalDesignatePage
            .checkNoForExternalExpert()
            .withPrincipleDesignateAppointedDate("", "", "")
            .clickContinue()

        summaryPage
            .schoolNameIs(project.schoolName)
            .titleIs("Principal designate")
            .inOrder()
            .summaryShows("Commissioned an external expert").HasValue("No").HasChangeLink()
            .summaryShows("Expected date that principal designate will be appointed").HasValue("25 April 2049").HasChangeLink()
            .summaryShows("Principal designate appointed").IsEmpty().HasChangeLink()
            .summaryShows("Actual date that principal designate was appointed").IsEmpty().HasChangeLink()
            .isNotMarkedAsComplete()
            .MarkAsComplete()
            .clickConfirmAndContinue()

        cy.log("principal designate is marked as complete");

        taskListPage.isTaskStatusIsCompleted("PrincipalDesignate")
    });
});