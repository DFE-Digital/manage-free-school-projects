import { BulkProjectTable, BulkProjectRow } from "cypress/api/domain";
import bulkCreateProjectPage from "cypress/pages/bulkCreateProjectPage";
import { v4 } from "uuid";
import { stringify } from "csv-stringify/sync";

import ExcelJS from "exceljs";

describe("Creating a bulk project", () => {
    beforeEach(() => {
        cy.login();
        cy.visit("/project/create/bulk");
    });

    it("Should validate an uploaded CSV file", () => {
        const completeRow: BulkProjectRow = {
            projectId: v4(),
            projectTitle: v4(),
            trustName: v4(),
            region: v4(),
            localAuthority: v4(),
            realisticOpeningDate: v4(),
            status: v4(),
        };

        const emptyRow: BulkProjectRow = {
            projectId: v4(),
        };

        const csv = createCsv([completeRow, emptyRow]);

        bulkCreateProjectPage.upload(csv, "file.csv").continue();
    });

    it("Should validate an uploaded Excel file", async () => {
        const completeRow: BulkProjectRow = {
            projectId: v4(),
            projectTitle: v4(),
            trustName: v4(),
            region: v4(),
            localAuthority: v4(),
            realisticOpeningDate: v4(),
            status: v4(),
        };

        const emptyRow: BulkProjectRow = {
            projectId: v4(),
        };

        const buffer = await createExcel([completeRow, emptyRow]);

        bulkCreateProjectPage.upload(buffer, "file.xlsx").continue();
    });

    function createTable(rows: Array<BulkProjectRow>) {
        const result: BulkProjectTable<BulkProjectRow> = {
            headers: [
                "projectTitle",
                "projectId",
                "trustName",
                "region",
                "localAuthority",
                "realisticOpeningDate",
                "status",
            ],
            rows: rows,
        };

        return result;
    }

    function createCsv(rows: Array<BulkProjectRow>): Buffer {
        const table = createTable(rows);

        const result = stringify(table.rows, {
            columns: table.headers,
            header: true,
        });

        return Buffer.from(result);
    }

    async function createExcel(rows: Array<BulkProjectRow>): Promise<Buffer> {
        const table = createTable(rows);

        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet("Sheet1");

        // Add header row
        worksheet.addRow(table.headers);

        for (const row of table.rows) {
            const rowData = table.headers.map(
                (header) => row[header as keyof BulkProjectRow] ?? "",
            );
            worksheet.addRow(rowData);
        }

        const buffer = await workbook.xlsx.writeBuffer();
        return Buffer.from(buffer);
    }
});
