import { ApiClient } from "./apiClient";
import { CreatePDGGrantVariationLetterRequest, ResponseWrapper } from "./domain";

class GrantVariationLetterApi {
    public constructor(private readonly apiClient: ApiClient) { }

    public put(
        projectId: string,
        request: CreatePDGGrantVariationLetterRequest,
    ): Cypress.Chainable {
        return this.apiClient
            .put<CreatePDGGrantVariationLetterRequest, ResponseWrapper<never>>(
                `/api/v1/client/projects/${projectId}/grant-letters/variation-letter`,
                request,
            )
            .then((response) => {
                return response.data;
            });
    }
}

const grantVariationLetterApi = new GrantVariationLetterApi(new ApiClient());

export default grantVariationLetterApi;