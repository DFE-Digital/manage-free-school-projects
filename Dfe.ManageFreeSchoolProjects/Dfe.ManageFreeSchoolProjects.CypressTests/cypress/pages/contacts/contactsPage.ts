class ContactsPage {

    public selectAboutTheProjectTab(): this {
        cy.contains("About the project").click()
        return this;
    }
    public onContactsTab(): this {
        cy.getById(`header-contacts`).should("be.visible")
        return this;
    }
    public isEmpty(value: string): this {
        cy.getByTestId(value).should('have.value', '');
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

    public hasProjectAssignedToName(value: string): this {
        cy.getByTestId(`project-assigned-to-name`).should(`contain.text`, value);
        return this;
    }

    public hasProjectAssignedToEmail(value: string): this {
        cy.getByTestId(`project-assigned-to-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditProjectAssignedTo(): this {
        cy.getByTestId(`edit-project-assigned-to`).click();

        return this;
    }

    public hasTeamLeadName(value: string): this {
        cy.getByTestId(`team-lead-name`).should(`contain.text`, value);
        return this;
    }

    public hasTeamLeadEmail(value: string): this {
        cy.getByTestId(`team-lead-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditTeamLead(): this {
        cy.getByTestId(`edit-team-lead`).click();

        return this;
    }

    public hasGrade6Name(value: string): this {
        cy.getByTestId(`grade-6-name`).should(`contain.text`, value);
        return this;
    }

    public hasGrade6Email(value: string): this {
        cy.getByTestId(`grade-6-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditGrade6(): this {
        cy.getByTestId(`edit-grade-6`).click();

        return this;
    }

    public hasProjectManagerName(value: string): this {
        cy.getByTestId(`project-manager-name`).should(`contain.text`, value);
        return this;
    }

    public hasProjectManagerEmail(value: string): this {
        cy.getByTestId(`project-manager-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditProjectManager(): this {
        cy.getByTestId(`edit-project-manager`).click();

        return this;
    }

    public hasProjectDirectorName(value: string): this {
        cy.getByTestId(`project-director-name`).should(`contain.text`, value);
        return this;
    }

    public hasProjectDirectorEmail(value: string): this {
        cy.getByTestId(`project-director-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditProjectDirector(): this {
        cy.getByTestId(`edit-project-director`).click();

        return this;
    }

    public hasOfstedContactName(value: string): this {
        cy.getByTestId(`ofsted-contact-name`).should(`contain.text`, value);
        return this;
    }

    public hasOfstedContactEmail(value: string): this {
        cy.getByTestId(`ofsted-contact-email`).should(`contain.text`, value);
        return this;
    }

    public hasOfstedContactPhoneNumber(value: string): this {
        cy.getByTestId(`ofsted-contact-phone-number`).should(`contain.text`, value);
        return this;
    }

    public hasOfstedContactRole(value: string): this {
        cy.getByTestId(`ofsted-contact-role`).should(`contain.text`, value);
        return this;
    }

    public goToEditOfstedContact(): this {
        cy.getByTestId(`edit-ofsted-contact`).click();

        return this;
    }

    public hasTrustContactName(value: string): this {
        cy.getByTestId(`trust-contact-name`).should(`contain.text`, value);
        return this;
    }

    public hasTrustContactEmail(value: string): this {
        cy.getByTestId(`trust-contact-email`).should(`contain.text`, value);
        return this;
    }

    public hasTrustContactPhoneNumber(value: string): this {
        cy.getByTestId(`trust-contact-phone-number`).should(`contain.text`, value);
        return this;
    }

    public hasTrustContactRole(value: string): this {
        cy.getByTestId(`trust-contact-role`).should(`contain.text`, value);
        return this;
    }

    public goToEditTrustContact(): this {
        cy.getByTestId(`edit-trust-contact`).click();

        return this;
    }

    public hasPrincipalDesignateName(value: string): this {
        cy.getByTestId(`principal-designate-name`).should(`contain.text`, value);
        return this;
    }

    public hasPrincipalDesignateEmail(value: string): this {
        cy.getByTestId(`principal-designate-email`).should(`contain.text`, value);
        return this;
    }

    public goToEditPrincipalDesignate(): this {
        cy.getByTestId(`edit-principal-designate`).click();

        return this;
    }

    public goToProjectsOverviewPage(): this {
        cy.getByTestId(`back-button`).click();

        return this;
    }

    public hasProjectTitleHeader(value: string): this {
        cy.getByTestId("project-title-header").should("contain.text", value);

        return this;
    }
    
}

const contactsPage = new ContactsPage();

export default contactsPage;