class UpdateMultipleFields {

    public clickUpload(): this {
        cy.contains('button','Upload').click()
        return this;
    }
    
    public errorForUpload(error: string): this {
        cy.get('.govuk-error-summary').should('contains.text', error)
        return this
    }
  
}

const updateMultipleFields = new UpdateMultipleFields();

export default updateMultipleFields;
