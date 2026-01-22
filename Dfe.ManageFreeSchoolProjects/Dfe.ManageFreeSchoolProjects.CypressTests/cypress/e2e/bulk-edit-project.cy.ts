import { BulkProjectTable, BulkEditRow, ProjectDetailsRequest } from "cypress/api/domain";
import bulkEditProjectPage from "cypress/pages/bulkEditProjectPage";
import { ProjectRecordCreator } from "cypress/constants/cypressConstants";
import { v4 } from "uuid";
import { stringify } from "csv-stringify/sync";
import { utils, write } from "xlsx/xlsx";
import { RequestBuilder } from "cypress/api/requestBuilder";
import projectApi from "cypress/api/projectApi";
import { Logger } from "cypress/common/logger";
import homePage from "cypress/pages/homePage";

describe("Bulk editing project", () => {

    let project: ProjectDetailsRequest;
    let now: Date;

    beforeEach(() => {
        cy.login({ role: ProjectRecordCreator });
        cy.visit('/');

        now = new Date();

        project = RequestBuilder.createProjectDetails();

        projectApi
            .post({
                projects: [project],
            })
        homePage
          .bulkEdit()       
    });

    it("Should edit the valid CSV file and show success message", () => {
        const emptyRow: BulkEditRow = {
            projectId: v4(),
        };

        const csv = createCsv([emptyRow]);

        bulkEditProjectPage
            .upload(csv, "file.csv").continue()
            .recordsVisible()
            .editProjects()
            .successMessage()
    });

    it("Should validate when no csv or xlsx file is selected", () => {
        bulkEditProjectPage
            .continue()
            .errorMessage('Select a file');
    });

    it("Should validate if csv file has no data", () => {
        const emptyRow: BulkEditRow = {
            projectId: v4(),
        };
        const csv = createEmptyCsv([]);

        bulkEditProjectPage
            .upload(csv, "file.csv").continue()
            .errorMessage('The selected file must have project data in it');
    });

    it("Should validate if the csv file has incorrect data", () => {
        const emptyRow: BulkEditRow = {
            projectId: v4(),
        };
        const csv = createCsvWithIncorrectData([]);

        bulkEditProjectPage
            .upload(csv, "file.csv").continue()
            .errorMessage('The enter data tab has 3 validation errors')
    });

    it.skip("Should validate an uploaded Excel file", () => {
        Logger.log(" Navigate to update multiple fields card");
        cy.contains('Update multiple fields').should('be.visible').click()
        const completeRow: BulkEditRow = {
            projectId: "1mg9101rvi5tluigc0x3lv7s7",
            localAuthority: "Luton",
            actualOpeningDate: "27/09/2040",
            status: "Pre-openingkjgkjgh"
        };

        const emptyRow: BulkEditRow = {
            projectId: v4(),
        };

        const buffer = createExcel([completeRow, emptyRow]);
        bulkEditProjectPage
            .upload(buffer, "file.xlsx").continue()
            .recordsVisible()
            .editProjects()

    });

    function createTable(rows: Array<BulkEditRow>) {
        const result: BulkProjectTable<BulkEditRow> = {
            headers: [
                "Project ID",
                "Project status",
                "Actual opening date",
                "Local authority"
            ],
            rows: rows,
        };

        return result;
    }

    function createCsv(rows: Array<BulkEditRow>): Buffer {
        const table = createTable(rows);

        const result = "Project ID,Project status,Actual opening date,Local authority\n" +
            `${project.projectId},Pre-opening,27/09/2040`

        return Buffer.from(result);
    }

    function createEmptyCsv(rows: Array<BulkEditRow>): Buffer {
        const table = createTable(rows);

        const result = "Project ID,Project status,Actual opening date,Local authority\n"
        
        return Buffer.from(result);
    }

    function createCsvWithIncorrectData(rows: Array<BulkEditRow>): Buffer {
        const table = createTable(rows);

        const result = "Project ID,Project status,Actual opening date,Local authority\n" +
            `${project.projectId},Preopening,27/13/2040,Test`

        return Buffer.from(result);
    }

    function createExcel(rows: Array<BulkEditRow>): Buffer {
        const table = createTable(rows);

        const worksheet = utils.json_to_sheet(table.rows, {
            header: table.headers,
        });

        const workbook = utils.book_new();
        utils.book_append_sheet(workbook, worksheet);

        const buffer = write(workbook, {
            type: "buffer",
            bookType: "xlsx",
            compression: true,
        });

        Logger.log(JSON.stringify(table.rows))
        return buffer;
    }
});
