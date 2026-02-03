import { ApiClient } from "./apiClient";
import {CreateProjectRiskRequest, ProjectTaskSummaryResponse, ResponseWrapper} from "./domain";

class ProjectTaskSummaryApi {
    public constructor(private apiClient: ApiClient) { }

    public get(
        projectId: { projectId: string },
    ): Cypress.Chainable {
        return this.apiClient
            .get<ResponseWrapper<ProjectTaskSummaryResponse>>(
                `/api/v1/client/projects/${projectId.projectId}/tasks/summary`,
            )
            .then((response) => {
                return response;
            });
    }
}

const projectTaskSummaryApi = new ProjectTaskSummaryApi(new ApiClient());

export default projectTaskSummaryApi;