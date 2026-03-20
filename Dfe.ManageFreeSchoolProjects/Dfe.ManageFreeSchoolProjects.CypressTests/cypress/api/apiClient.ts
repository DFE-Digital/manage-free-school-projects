import { EnvApi, EnvApiKey } from 'cypress/constants/cypressConstants';

export class ApiClient {
    public put<TRequest extends object, TResponse extends object>(
        endpoint: string,
        request: TRequest
    ): Cypress.Chainable<TResponse> {
        return cy
            .request<TResponse>({
                method: 'PUT',
                url: Cypress.expose(EnvApi) + endpoint,
                headers: this.getHeaders(),
                body: request,
            })
            .then((response) => {
                return response.body;
            });
    }

    public post<TRequest extends object, TResponse extends object>(
        endpoint: string,
        request: TRequest
    ): Cypress.Chainable<TResponse> {
        return cy
            .request<TResponse>({
                method: 'POST',
                url: Cypress.expose(EnvApi) + endpoint,
                headers: this.getHeaders(),
                body: request,
            })
            .then((response) => {
                return response.body;
            });
    }

    public get<TResponse extends object>(endpoint: string): Cypress.Chainable<TResponse> {
        return cy
            .request<TResponse>({
                method: 'GET',
                url: Cypress.expose(EnvApi) + endpoint,
                headers: this.getHeaders(),
            })
            .then((response) => {
                return response.body;
            });
    }

    protected getHeaders(): object {
        return cy.env([EnvApiKey]).then(({ apiKey }) => {
            return {
                'Content-type': 'application/json',
                ApiKey: apiKey,
            };
        });
    }
}
