import { contains } from "cypress/types/jquery";

class TaskListPage {

    public onTasklistTab(): this {
        cy.getById(`header-contacts`).should( 'contain.text', 'Task list')
        return this;
    }
    public selectAboutTheProjectTab(): this {
        cy.contains("About the project").click()
        return this;
    }
    public selectContactTab(): this {
        cy.contains("Contacts").click()
        return this;
    }

    public hasProjectStatus(value: string): this {
        cy.getById(`status-tag`).should(`contain.text`, value);

        return this;
    }

    public clickChangeProjectStatus(): this {
        cy.getById("change-project-status").click();
        return this;
    }
    
    public selectDatesFromTaskList(): this {
        cy.getByTestId("Dates-task").click()
        return this;
    }

    public selectSchoolFromTaskList(): this {
        cy.getByTestId("School-task").click()
        return this;
    }

    public selectReferenceNumbersFromTaskList(): this {
        cy.getByTestId("ReferenceNumbers-task").click()
        return this;
    }

    public selectRegionAndLAFromTaskList(): this {
        cy.getByTestId("RegionAndLocalAuthority-task").click()
        return this;
    }

    public selectTrustFromTaskList(): this {
        cy.getByTestId("Trust-task").click()
        return this;
    }

    public selectConstituencyFromTaskList(): this {
        cy.getByTestId("Constituency-task").click()
        return this;
    }

    public selectRiskAppraisalMeetingFromTaskList(): this {
        cy.getByTestId("RiskAppraisalMeeting-task").click()
        return this;
    }

    public selectArticlesOfAssociationFromTaskList(): this {
        cy.getByTestId("ArticlesOfAssociation-task").click()
        return this;
    }

    public selectFinancePlanFromTaskList(): this {
        cy.getByTestId("FinancePlan-task").click()
        return this;
    }

    public selectFinalFinancePlanFromTaskList(): this {
        cy.getByTestId("FinalFinancePlan-task").click()
        return this;
    }

    public selectKickOffMeetingFromTaskList(): this {
        cy.getByTestId("KickOffMeeting-task").click()
        return this;
    }

    public selectFundingAgreementFromTaskList(): this {
        cy.getByTestId("FundingAgreement-task").click()
        return this;
    }

    public selectFundingAgreementHealthCheckFromTaskList(): this {
        cy.getByTestId("FundingAgreementHealthCheck-task").click()
        return this;
    }

    public selectFundingAgreementSubmissionFromTaskList(): this {
        cy.getByTestId("FundingAgreementSubmission-task").click()
        return this;
    }

    public selectGovernancePlanFromTaskList(): this {
        this.getDraftGovernancePlanTask().click();
        return this;
    }

    public draftGovernancePlanTaskDoesNotShow(): this {
        this.getDraftGovernancePlanTask().should("not.exist");

        return this;
    }

    public selectStatutoryConsultationFromTaskList(): this {
        cy.getByTestId("StatutoryConsultation-task").click()
        return this;
    }

    public selectGiasFromTaskList(): this {
        cy.getByTestId("Gias-task").click()
        return this;
    }

    public selectEducationBriefFromList(): this {
        cy.getByTestId("EducationBrief-task").click()
        return this;
    }

    public selectAdmissionsArrangementsFromTaskList(): this {
        cy.getByTestId("AdmissionsArrangements-task").click()
        return this;
    }

    public selectImpactAssessmentFromTaskList(): this {
        cy.getByTestId("ImpactAssessment-task").click()
        return this;
    }

    public selectEqualitiesAssessmentFromTaskList(): this {
        cy.getByTestId("EqualitiesAssessment-task").click()
        return this;
    }

    public selectAcceptedOffersEvidenceFromTaskList(): this {
        cy.getByTestId("EvidenceOfAcceptedOffers-task").click()
        return this;
    }

    public selectPupilNumbersChecksTaskList(): this {
        cy.getByTestId("PupilNumbersChecks-task").click()
        return this;
    }

    public selectOfstedPreregistrationFromTaskList(): this {
        cy.getByTestId("OfstedInspection-task").click()
        return this;
    }
    public selectExternalExpertVisitFromTaskList(): this {
        cy.getByTestId("CommissionedExternalExpert-task").click()
        return this;
    }

    public selectPrincipleDesignateFromTaskList(): this {
        cy.getByTestId("PrincipalDesignate-task").click()
        return this;
    }

    public selectReadinessToOpenMeetingFromTaskList(): this {
        cy.getByTestId("ReadinessToOpenMeeting-task").click()
        return this;
    }

    public selectPreFundingAgreementCheckpointMeetingFromTaskList(): this {
        cy.getByTestId("PreFundingAgreementCheckpointMeeting-task").click()
        return this;
    }

    public selectMovingToOpenFromTaskList(): this {
        cy.getByTestId("MovingToOpen-task").click()
        return this;
    }

    public selectApplicationsEvidenceFromTaskList(): this {
        this.getApplicationsEvidenceTask().click()
        return this;
    }

    public selectDueDiligenceFromTaskList(): this {
        this.getDueDiligenceChecksTask().click()    
        return this;
    }
    getDueDiligenceChecksTask() {
        return cy.getByTestId("DueDiligenceChecks-task");
    }

    public selectPDGFromTaskList(): this {
        cy.getByTestId("PDG-task").click()
        return this;
    }

    public assertApplicationsEvidenceIsNotVisibleTaskList(): this {
        this.getApplicationsEvidenceTask().should("not.exist")
        return this;
    }

    public isTaskStatusIsNotStarted(taskName: string): this {
        cy.get(`[data-testid="task-${taskName}-status"]`).should("contains.text", "Not started");
        return this;
    }

    public isTaskStatusInProgress(taskName: string): this {
        cy.get(`[data-testid="task-${taskName}-status"]`).should("contains.text", "In progress");
        return this;
    }

    public isTaskStatusIsCompleted(taskName: string): this {
        cy.get(`[data-testid="task-${taskName}-status"]`).contains("Completed");
        return this;
    }

    public isTaskStatusHidden(taskName: string): this {
        cy.get(`[data-testid="task-${taskName}-status"]`).should("not.exist");
        return this;
    }

    private getDraftGovernancePlanTask() {
        return cy.getByTestId("GovernancePlan-task");
    }

    private getApplicationsEvidenceTask() {
        return cy.getByTestId("ApplicationsEvidence-task");
    }

    public taskCompletedCountMessage(countMessage: string): this {
        cy.getById("completed-count").contains(countMessage);
        return this;
    }
}

const taskListPage = new TaskListPage();

export default taskListPage;