import { ApiClient } from "./apiClient";
import { CreatePDGPaymentScheduleRequest, ResponseWrapper } from "./domain";

class PaymentScheduleApi {
    public constructor(private readonly apiClient: ApiClient) { }

    public post(
        projectId: string,
        request: CreatePDGPaymentScheduleRequest,
    ): Cypress.Chainable {
        return this.apiClient
            .post<CreatePDGPaymentScheduleRequest, ResponseWrapper<never>>(
                `/api/v1/client/projects/${projectId}/payments`,
                request,
            )
            .then((response) => {
                return response.data;
            });
    }
}

const paymentScheduleApi = new PaymentScheduleApi(new ApiClient());

export default paymentScheduleApi;