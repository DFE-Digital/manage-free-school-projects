import { ApiClient } from "./apiClient";
import { CreatePDGGrantLettersRequest, ResponseWrapper } from "./domain";

class GrantLettersApi {
    public constructor(private readonly apiClient: ApiClient) { }

    public put(
        projectId: string,
        request: CreatePDGGrantLettersRequest,
    ): Cypress.Chainable {
        return this.apiClient
            .put<CreatePDGGrantLettersRequest, ResponseWrapper<never>>(
                `/api/v1/client/projects/${projectId}/grant-letters`,
                request,
            )
            .then((response) => {
                return response.data;
            });
    }
}

const grantLettersApi = new GrantLettersApi(new ApiClient());

export default grantLettersApi;