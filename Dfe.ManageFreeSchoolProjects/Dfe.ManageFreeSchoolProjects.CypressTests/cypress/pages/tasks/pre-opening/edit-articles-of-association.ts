import { BaseEditPage } from '../../baseEditPage';

class ArticlesOfAssociationEditPage extends BaseEditPage {
    checkSubmittedArticlesMatch(): this {
        cy.getById('checked-submitted-articles-match').check();
        return this;
    }

    checkChairHaveSubmittedConfirmation(): this {
        cy.getById('chair-have-submitted-confirmation').check();
        return this;
    }

    checkArrangementsMatchGovernancePlans(): this {
        cy.getById('arrangements-match-governance-plans').check();
        return this;
    }

    withActualDate(day: string, month: string, year: string): this {
        const key = 'actual-date';
        this.setDate(key, day, month, year);
        return this;
    }

    withComments(comment: string): this {
        cy.getByTestId('comments-on-decision').typeFast(comment);
        return this;
    }

    withSharepointLink(value: string): this {
        cy.getByTestId('sharepoint-link').typeFast(value);
        return this;
    }

    withSharepointLinkExceedingMaxLength(): this {
        cy.getByTestId('sharepoint-link')
            .clear()
            .invoke('val', `https://${'a'.repeat(501)}`);
        return this;
    }

    errorForComments(): this {
        this.errorTracking = 'comments-on-decision';
        return this;
    }

    errorForSharepointLink(): this {
        this.errorTracking = 'sharepoint-link';
        return this;
    }

    errorForActualDate(): this {
        this.errorTracking = 'actual-date';
        return this;
    }
}

const articlesOfAssociationEditPage = new ArticlesOfAssociationEditPage();
export default articlesOfAssociationEditPage;
