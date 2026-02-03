class FundingAgreementSubmissionPage {

    checkDraftedFASubmission(): this {
        cy.getById("drafted-fa-submission").check()
        return this
    }

    uncheckDraftedFASubmission(): this {
        cy.getById("drafted-fa-submission").uncheck()
        return this
    }

    checkRegionalDirectorSignedOffFASubmission(): this {
        cy.getById("regional-director-signed-off-fa-submission").check()
        return this
    }

    uncheckRegionalDirectorSignedOffFAHealtCheck(): this {
        cy.getById("regional-director-signed-off-fa-submission").uncheck()
        return this
    }

    checkMinisterSignedOffFASubmission(): this {
        cy.getById("minister-signed-off-fa-submission").check()
        return this
    }

    uncheckMinisterSignedOffFASubmission(): this {
        cy.getById("minister-signed-off-fa-submission").uncheck()
        return this
    }

    checkIncludedSignedOffImpactAssessment(): this {
        cy.getById("included-signed-off-impact-assessment").check()
        return this
    }

    uncheckIncludedSignedOffImpactAssessment(): this {
        cy.getById("included-signed-off-impact-assessment").uncheck()
        return this
    }

    checkSavedFASubmissionInWorkplacesFolder(): this {
        cy.getById("saved-fa-submission-in-workplaces-folder").check()
        return this
    }

    uncheckSavedFASubmissionInWorkplacesFolder(): this {
        cy.getById("saved-fa-submission-in-workplaces-folder").uncheck()
        return this
    }
    
    clickContinue() : this {
        cy.getByTestId("continue").click();
        return this;
    }

}


const fundingAgreementSubmissionEditPage = new FundingAgreementSubmissionPage();
export default fundingAgreementSubmissionEditPage;
