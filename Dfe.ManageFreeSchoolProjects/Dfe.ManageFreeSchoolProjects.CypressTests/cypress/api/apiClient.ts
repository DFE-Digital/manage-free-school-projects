import { EnvApi, EnvApiKey } from 'cypress/constants/cypressConstants';

export class ApiClient {
    public put<TRequest extends object, TResponse extends object>(
        endpoint: string,
        request: TRequest
    ): Cypress.Chainable<TResponse> {
        return cy.env([EnvApiKey]).then(({ apiKey }) => {
            return cy
                .request<TResponse>({
                    method: 'PUT',
                    url: Cypress.expose(EnvApi) + endpoint,
                    headers: this.getHeaders(apiKey),
                    body: request,
                })
                .then((response) => {
                    return response.body;
                });
        });
    }

    public post<TRequest extends object, TResponse extends object>(
        endpoint: string,
        request: TRequest
    ): Cypress.Chainable<TResponse> {
        return cy.env([EnvApiKey]).then(({ apiKey }) => {
            return cy
                .request<TResponse>({
                    method: 'POST',
                    url: Cypress.expose(EnvApi) + endpoint,
                    headers: this.getHeaders(apiKey),
                    body: request,
                })
                .then((response) => {
                    return response.body;
                });
        });
    }

    public get<TResponse extends object>(endpoint: string): Cypress.Chainable<TResponse> {
        return cy.env([EnvApiKey]).then(({ apiKey }) => {
            return cy
                .request<TResponse>({
                    method: 'GET',
                    url: Cypress.expose(EnvApi) + endpoint,
                    headers: this.getHeaders(apiKey),
                })
                .then((response) => {
                    return response.body;
                });
        });
    }

    protected getHeaders(apiKey: string): object {
        return {
            'Content-type': 'application/json',
            ApiKey: apiKey,
        };
    }
}
