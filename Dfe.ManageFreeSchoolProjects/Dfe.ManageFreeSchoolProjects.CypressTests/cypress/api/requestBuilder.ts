import { v4 } from "uuid";
import { ProjectDetailsRequest } from "./domain";
import { EnvUsername } from "cypress/constants/cypressConstants";
import dataGenerator from "cypress/fixtures/dataGenerator";

export class RequestBuilder {
    public static createProjectDetails(): ProjectDetailsRequest {
        const result: ProjectDetailsRequest = {
            projectId: dataGenerator.generateTemporaryId(),
            applicationNumber: v4().substring(0, 9),
            applicationWave: v4(),
            createdBy: Cypress.env(EnvUsername),
            schoolName: dataGenerator.generateSchoolName()
        };

        return result;
    }
}
