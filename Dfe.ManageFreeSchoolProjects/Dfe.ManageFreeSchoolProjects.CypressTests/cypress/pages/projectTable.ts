import { ProjectRow } from './projectRow';

class ProjectTable {
    public getRowByProjectId(projectId: string): Cypress.Chainable<ProjectRow> {
        return cy.getByTestId(`row-${projectId}`).then((el) => {
            return new ProjectRow(el);
        });
    }

    public getRows(): Cypress.Chainable<ProjectRow> {
        return cy.containsByTestId(`row-`).then((el) => {
            return new ProjectRow(el);
        });
    }
    public getProjectIds() {
        const result: Array<string> = [];

        return cy
            .containsByTestId('row-')
            .each(($el) => {
                result.push($el.text());

                return result;
            })
            .then(() => {
                return result;
            });
    }

    public allRowsHaveRegion(region: string) {
        this.allRowsHaveCellText('region-name', region);
    }

    public allRowsHaveProjectManagedBy(projectManagedBy: string) {
        this.allRowsHaveCellText('project-managed-by', projectManagedBy);
    }

    public allRowsHaveProjectStatus(projectStatus: string) {
        this.allRowsHaveCellText('status', projectStatus);
    }

    public allRowsHaveLocalAuthority(localAuthority: string) {
        this.allRowsHaveCellText('local-authority', localAuthority);
    }

    public allRowsHaveViewLink() {
        this.allRowsHaveCellText('action', 'View');
    }

    // Applying a filter POSTs the form and reloads the page, so the unfiltered table is
    // still in the DOM for a moment. `.each()` snapshots the rows from its first attempt
    // and never re-queries, so it could latch onto those pre-reload rows and then assert
    // against detached elements. A single `should` callback re-runs the `cy.get` on every
    // retry instead, so it waits for the filtered table to render.
    private allRowsHaveCellText(cellTestId: string, value: string) {
        cy.get('[data-testid*="row-"]').should(($rows) => {
            expect($rows.length, 'number of project rows').to.be.at.least(1);

            $rows.each((_, row) => {
                const cell = Cypress.$(row).find(`[data-testid="${cellTestId}"]`);

                expect(cell, `${cellTestId} cell`).to.have.length(1);
                expect(cell.text().trim(), `${cellTestId} text`).to.contain(value);
            });
        });
    }
}

const projectTable = new ProjectTable();

export default projectTable;
