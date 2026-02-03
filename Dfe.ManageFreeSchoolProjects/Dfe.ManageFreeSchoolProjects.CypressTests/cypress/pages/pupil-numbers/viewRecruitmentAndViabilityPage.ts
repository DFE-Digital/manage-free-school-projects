class ViewRecruitmentAndViabilityPage {
    public hasReceptionToYear6(
        minimumViableNumber: string,
        applicationsReceived: string,
        percentageComparedToMvn: string,
        percentageComparedToPan: string,
        acceptedOffers: string,
        acceptedPercentageComparedToMvn: string,
        acceptedPercentageComparedToPan: string) {

        this.hasRow("reception-to-year6", 
                    minimumViableNumber, 
                    applicationsReceived, 
                    percentageComparedToMvn, 
                    percentageComparedToPan, 
                    acceptedOffers, 
                    acceptedPercentageComparedToMvn, 
                    acceptedPercentageComparedToPan);

        return this;
    }

    public hasYear7ToYear11(
        minimumViableNumber: string,
        applicationsReceived: string,
        percentageComparedToMvn: string,
        percentageComparedToPan: string,
        acceptedOffers: string,
        acceptedPercentageComparedToMvn: string,
        acceptedPercentageComparedToPan: string) {

        this.hasRow("year7-to-year11", 
            minimumViableNumber, 
            applicationsReceived, 
            percentageComparedToMvn, 
            percentageComparedToPan, 
            acceptedOffers, 
            acceptedPercentageComparedToMvn, 
            acceptedPercentageComparedToPan);

        return this;
    }

    public hasYear12ToYear14(
        minimumViableNumber: string,
        applicationsReceived: string,
        percentageComparedToMvn: string,
        percentageComparedToPan: string,
        acceptedOffers: string,
        acceptedPercentageComparedToMvn: string,
        acceptedPercentageComparedToPan: string) {

        this.hasRow("year12-to-year14", 
            minimumViableNumber, 
            applicationsReceived, 
            percentageComparedToMvn, 
            percentageComparedToPan, 
            acceptedOffers, 
            acceptedPercentageComparedToMvn, 
            acceptedPercentageComparedToPan);

        return this;
    }

    public hasTotal(
        minimumViableNumber: string,
        applicationsReceived: string,
        acceptedOffers: string
    ) {
        cy.getByTestId(`total-mvn`).should("contain.text", minimumViableNumber);
        cy.getByTestId(`total-applications-received`).should("contain.text", applicationsReceived);
        cy.getByTestId(`total-accepted-offers`).should("contain.text", acceptedOffers);
    }

    public hasRow(
        idPrefix: string,
        minimumViableNumber: string,
        applicationsReceived: string,
        receivedPercentageComparedToMvn: string,
        receivedPercentageComparedToPan: string,
        acceptedOffers: string,
        acceptedPercentageComparedToMvn: string,
        acceptedPercentageComparedToPan: string) {
        cy.getByTestId(`${idPrefix}-mvn`).should("contain.text", minimumViableNumber);
        cy.getByTestId(`${idPrefix}-applications-received`).should("contain.text", applicationsReceived);
        cy.getByTestId(`${idPrefix}-mvn-ratio`).should("contain.text", receivedPercentageComparedToMvn);
        cy.getByTestId(`${idPrefix}-pan-ratio`).should("contain.text", receivedPercentageComparedToPan);
        cy.getByTestId(`${idPrefix}-mvn-ratio`).should("contain.text", receivedPercentageComparedToMvn);
        cy.getByTestId(`${idPrefix}-pan-ratio`).should("contain.text", receivedPercentageComparedToPan);
        cy.getByTestId(`${idPrefix}-accepted-offers`).should("contain.text", acceptedOffers);
        cy.getByTestId(`${idPrefix}-accepted-mvn-ratio`).should("contain.text", acceptedPercentageComparedToMvn);
        cy.getByTestId(`${idPrefix}-accepted-pan-ratio`).should("contain.text", acceptedPercentageComparedToPan);

        return this;


    }
}

const viewRecruitmentAndViabilityPage = new ViewRecruitmentAndViabilityPage();

export default viewRecruitmentAndViabilityPage;