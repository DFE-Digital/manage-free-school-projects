
class HomePage {

    FilterData: { [key: string]: string } = {};

    public createNewProjects(): this { 
        cy.contains("Create a project").click();
        //   cy.login({role: ProjectRecordCreator})
        return this;
    }

    public createProject(): this {
        cy.getByTestId("create-project").click();

        return this;
    }

    public bulkEdit(): this{
        cy.contains('Update multiple fields').should('be.visible').click();
        
        return this;
    }

    public deleteProject(): this {
        cy.getByTestId("delete-project-button").click();

        return this;
    }

    public openFilter(): this {
        cy.getByTestId("filter-button").click();

        return this;
    }

    public withProjectFilter(project: string): this {
        cy.getByTestId("search-by-project").typeFast(project);

        return this;
    }

    public hasProjectFilter(project: string): this {
        cy.getByTestId("search-by-project")
            .should("have.value", project);

        return this;
    }

    public withRegionFilter(region: string): this {
        cy.getByTestId(`${region}-option`).check();

        return this;
    }

    public hasRegionFilter(region: string): this {
        cy.getByTestId(`${region}-option`).should("be.checked");

        return this;
    }

    public withLocalAuthorityFilter(localAuthority: string): this {
        cy.getByTestId(`${localAuthority}-option`).check();

        return this;
    }

    public hasLocalAuthorityFilter(localAuthority: string): this {
        cy.getByTestId(`${localAuthority}-option`).should("be.checked");

        return this;
    }

    public withProjectManagedByFilter(projectManagedBy: string): this {
        cy.getByTestId(`${projectManagedBy}-option`).check();

        return this;
    }

    public hasProjectManagedByFilter(projectManagedBy: string): this {
        cy.getByTestId(`${projectManagedBy}-option`).should("be.checked");

        return this;
    }
/*
    public withProjectStatusFilter(projectStatus: string): this {
        cy.getByTestId(`${projectStatus}-option`).check({ force: true });

        return this;
    }

*/

/*
    public hasProjectStatusFilter(projectStatus: string): this {
        cy.getByTestId(`${projectStatus}-option`).should("be.checked");

        return this;
    }
*/

    public applyFilters(): this {
        cy.getByTestId("apply-filters").click();

        return this;
    }

    public clearFilters(): this {
        cy.getByTestId("clear-filters").click();

        return this;
    } 

    public tryViewProjectWithFilters(): this {

        const dataTestIds = {
            projectTitle: '[data-testid="project-title"]',
            projectId: '[data-testid="project-id"]',
            regionName: '[data-testid="region-name"]',
            localAuthority: '[data-testid="local-authority"]',
            projectManagedBy: '[data-testid="project-managed-by"]',
        //    status: '[data-testid="status"]'
        };


        cy.get('tbody')
          .find('tr')
          .filter((_, row) => {
            const $row = Cypress.$(row);
    
            const hasProjectTitleOrId = $row.find(dataTestIds.projectTitle).text().trim() !== "" ||
                                        $row.find(dataTestIds.projectId).text().trim() !== "";
            const hasRegionName = $row.find(dataTestIds.regionName).text().trim() !== "";
            const hasLocalAuthority = $row.find(dataTestIds.localAuthority).text().trim() !== "";
            const hasProjectManagedBy = $row.find(dataTestIds.projectManagedBy).text().trim() !== "";
          //  const hasStatus = $row.find(dataTestIds.status).text().trim() !== "";
    
            return hasProjectTitleOrId && hasRegionName && hasLocalAuthority && hasProjectManagedBy //&& hasStatus;
          })
          .first()
          .then((firstRow) => {
            this.FilterData.projectId = Cypress.$(firstRow).find(dataTestIds.projectId).text().trim();
            this.FilterData.projectTitle = Cypress.$(firstRow).find(dataTestIds.projectTitle).text().trim() ||
                                       Cypress.$(firstRow).find(dataTestIds.projectId).text().trim();
            this.FilterData.regionName = Cypress.$(firstRow).find(dataTestIds.regionName).text().trim();
            this.FilterData.localAuthority = Cypress.$(firstRow).find(dataTestIds.localAuthority).text().trim();
            this.FilterData.projectManagedBy = Cypress.$(firstRow).find(dataTestIds.projectManagedBy).text().trim();
           // this.FilterData.status = Cypress.$(firstRow).find(dataTestIds.status).text().trim();     
            
          }).then(() => {

            this.withProjectFilter(this.FilterData.projectId);
            this.withRegionFilter(this.FilterData.regionName);
            this.withLocalAuthorityFilter(this.FilterData.localAuthority);
            this.withProjectManagedByFilter(this.FilterData.projectManagedBy);
         //   this.withProjectStatusFilter(this.FilterData.status);

            this.applyFilters();
            
            cy.get('.govuk-table').find('td').contains('a', 'View').first().click();

          });
    
        return this;
    }


    public clickHeader(): this { 
        cy.getByClass('dfe-header__link--service').click();

        return this;
    }

    public regionsFilterInputCleared(): this {
        cy.get('#search-by-region-container input')
        .each(($input) => {
            cy.wrap($input).should('not.be.checked');
        });

        return this;
     }
     
     public selectAllRegions(): this {
        cy.get('#search-by-region-container input')
        .each(($input) => {
            cy.wrap($input).check();
        });

        return this;
     }

     public selectAllLocalAuthorities(): this {
        cy.get('#la-checkbox-container input')
        .each(($input) => {
            cy.wrap($input).check();
        });

        return this;
     }

     public withAllProjectAssignedTo(): this {
        cy.get('#search-by-pmb-container input')
        .each(($input) => {
            cy.wrap($input).check();
        });

        return this;
     }

     public allRegionsSelected(): this {
        cy.get('#search-by-region-container input')
        .each(($input) => {
            cy.wrap($input).should('be.checked');
        });

        return this;
     }

    public downloadProjectDataExport() {
        cy.getByTestId("download-data-export").click();
    }
}

const homePage = new HomePage();

export default homePage;
