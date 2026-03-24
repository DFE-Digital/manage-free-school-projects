import { EnvAuthKey, EnvUrl, EnvUsername, ProjectRecordCreator } from '../constants/cypressConstants';

export class AuthenticationInterceptor {
    register(params?: AuthenticationInterceptorParams) {
        cy.env([EnvAuthKey]).then(({ authKey }) => {
            cy.intercept(
                {
                    url: Cypress.expose(EnvUrl) + '/**',
                    middleware: true,
                },
                (req) => {
                    // Set an auth header on every request made by the browser
                    req.headers = {
                        ...req.headers,
                        Authorization: `Bearer ${authKey}`,
                        'x-user-context-role-0': params?.role ? params.role : ProjectRecordCreator,
                        'x-user-context-name': params?.username ? params.username : Cypress.expose(EnvUsername),
                    };
                }
            ).as('AuthInterceptor');
        });
    }
}

export type AuthenticationInterceptorParams = {
    role?: string;
    username?: string;
};
