class Root {

    checkAccessToPage(url: string): this {
        cy.visit(url, {failOnStatusCode: false})
        cy.getById("error-heading").should("contain", "Sorry, there is a problem with the system")
        return this
    }

}

const root = new Root();

export default root;