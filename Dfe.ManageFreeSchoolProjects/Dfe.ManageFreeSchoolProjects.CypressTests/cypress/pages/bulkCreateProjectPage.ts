class BulkCreateProjectPage {
    public upload(file: Buffer, filename: string): this {
        cy.getByTestId('upload').selectFile({
            contents: file,
            fileName: filename,
            lastModified: Date.now(),
        });
        return this;
    }

    public continue(): this {
        cy.getByTestId('continue').click();

        return this;
    }

    public hasReloaded(): this {
        cy.getByTestId('upload').should('have.value', '');

        return this;
    }
}

const bulkCreateProjectPage = new BulkCreateProjectPage();

export default bulkCreateProjectPage;
