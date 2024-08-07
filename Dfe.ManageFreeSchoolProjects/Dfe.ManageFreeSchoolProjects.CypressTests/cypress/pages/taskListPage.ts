class TaskListPage {

    public onTasklistTab(): this {
        cy.getById(`header-task-list`).should("be.visible")
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
        cy.getByTestId("dates-task").click()
        return this;
    }

    public selectSchoolFromTaskList(): this {
        cy.getByTestId("school-task").click()
        return this;
    }

    public selectReferenceNumbersFromTaskList(): this {
        cy.getByTestId("reference-numbers-task").click()
        return this;
    }

    public selectRegionAndLAFromTaskList(): this {
        cy.getByTestId("region-la-task").click()
        return this;
    }

    public selectTrustFromTaskList(): this {
        cy.getByTestId("trust-task").click()
        return this;
    }

    public selectConstituencyFromTaskList(): this {
        cy.getByTestId("constituency-task").click()
        return this;
    }

    public selectRiskAppraisalMeetingFromTaskList(): this {
        cy.getByTestId("risk-appraisal-meeting-task").click()
        return this;
    }

    public selectArticlesOfAssociationFromTaskList(): this {
        cy.getByTestId("articles-of-association-task").click()
        return this;
    }

    public selectFinancePlanFromTaskList(): this {
        cy.getByTestId("finance-plan-task").click()
        return this;
    }

    public selectFinalFinancePlanFromTaskList(): this {
        cy.getByTestId("finalFinancePlan-task").click()
        return this;
    }

    public selectKickOffMeetingFromTaskList(): this {
        cy.getByTestId("kick-off-meeting-task").click()
        return this;
    }

    public selectFundingAgreementFromTaskList(): this {
        cy.getByTestId("funding-agreement-task").click()
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

    public selectDraftGovernancePlanFromTaskList(): this {
        this.getDraftGovernancePlanTask().click();
        return this;
    }

    public draftGovernancePlanTaskDoesNotShow(): this {
        this.getDraftGovernancePlanTask().should("not.exist");

        return this;
    }

    public selectStatutoryConsultationFromTaskList(): this {
        cy.getByTestId("statutory-consultation-task").click()
        return this;
    }

    public selectGiasFromTaskList(): this {
        cy.getByTestId("get-information-about-schools").click()
        return this;
    }

    public selectEducationBriefFromList(): this {
        cy.getByTestId("educationBrief-task").click()
        return this;
    }

    public selectAdmissionsArrangementsFromTaskList(): this {
        cy.getByTestId("admissions-arrangements-task").click()
        return this;
    }

    public selectImpactAssessmentFromTaskList(): this {
        cy.getByTestId("impactAssessment-task").click()
        return this;
    }

    public selectEqualitiesAssessmentFromTaskList(): this {
        cy.getByTestId("equalities-assessment-task").click()
        return this;
    }

    public selectAcceptedOffersEvidenceFromTaskList(): this {
        cy.getByTestId("acceptedoffersevidence-task").click()
        return this;
    }

    public selectPupilNumbersChecksTaskList(): this {
        cy.getByTestId("pupilnumberschecks-task").click()
        return this;
    }

    public selectOfstedPreregistrationFromTaskList(): this {
        cy.getByTestId("ofstedpreregistration-task").click()
        return this;
    }
    public selectExternalExpertVisitFromTaskList(): this {
        cy.getByTestId("externalExpertVisit-task").click()
        return this;
    }

    public selectPrincipleDesignateFromTaskList(): this {
        cy.getByTestId("PrincipalDesignate-task").click()
        return this;
    }

    public selectMovingToOpenFromTaskList(): this {
        cy.getByTestId("movingToOpen-task").click()
        return this;
    }

    public selectApplicationsEvidenceFromTaskList(): this {
        this.getApplicationsEvidenceTask().click()
        return this;
    }

    public selectPDGFromTaskList(): this {
        cy.getByTestId("pdg-task").click()
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

    private getDraftGovernancePlanTask() {
        return cy.getByTestId("draft-governance-plan-task");
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