import { ProjectRow } from "./projectRow";

class ProjectTable {
    public getRowByProjectId(projectId: string): Cypress.Chainable<ProjectRow> {
        return cy.getByTestId(`row-${projectId}`).then((el) => {
            return new ProjectRow(el);
        });
    }

    public getRowByProjectType(projectType: string): Cypress.Chainable<ProjectRow> {
        return cy.getByTestId(`project-type`).then((el) => {
            return new ProjectRow(el);
        });
    }
    public getProjectIds() {
        const result: Array<string> = [];

        return cy
            .containsByTestId("row-")
            .each(($el) => {
                result.push($el.text());

                return result;
            })
            .then(() => {
                return result;
            });
    }

    public allRowsHaveRegion(region: string) {
        cy.containsByTestId(`row-`).each((el) => {
            const projectRow = new ProjectRow(el.get(0));
            projectRow.hasRegionName(region);
        });
    }

    public allRowsHaveProjectManagedBy(projectManagedBy: string) {
        cy.containsByTestId(`row-`).each((el) => {
            const projectRow = new ProjectRow(el.get(0));
            projectRow.hasProjectManagedBy(projectManagedBy);
        });
    }

    public allRowsHaveProjectStatus(projectStatus: string) {
        cy.containsByTestId(`row-`).each((el) => {
            const projectRow = new ProjectRow(el.get(0));
            projectRow.hasStatus(projectStatus);
        });
    }

    public allRowsHaveLocalAuthority(localAuthority: string) {
        cy.containsByTestId(`row-`).each((el) => {
            const projectRow = new ProjectRow(el.get(0));
            projectRow.hasLocalAuthority(localAuthority);
        });
    }

    public allRowsHaveViewLink() {
        cy.containsByTestId(`row-`).each((el) => {
            const projectRow = new ProjectRow(el.get(0));
            projectRow.hasViewLink('View');
        });
    }
}

const projectTable = new ProjectTable();

export default projectTable;
