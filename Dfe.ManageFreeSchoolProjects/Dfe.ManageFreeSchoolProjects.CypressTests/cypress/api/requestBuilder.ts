import { CreateProjectRiskRequest, ProjectDetailsRequest } from "./domain";
import { EnvUsername } from "cypress/constants/cypressConstants";
import dataGenerator from "cypress/fixtures/dataGenerator";

export class RequestBuilder {
    
    public static createNewProjectDetails(): ProjectDetailsRequest {
        const result: ProjectDetailsRequest =  {
            projectId: dataGenerator.generateTemporaryId(25),
            createdBy: Cypress.env(EnvUsername),
            schoolName: dataGenerator.generateSchoolName(),
            TRN: 'TR90111',
            applicationWave: "FS - Presumption",
            projectAssignedToName: "Test Person",
            projectAssignedToEmail: "test.person.education.gov.uk"
        };
        return result;
    }

    public static createProjectDetails(): ProjectDetailsRequest {
        return {...this.createNewProjectDetails()};
    }

    public static createProjectDetailsNonPresumption(): ProjectDetailsRequest {
        return {...this.createNewProjectDetails(), applicationWave: "Other Wave"};
    }

    public static createProjectDetailsCentralRoute(): ProjectDetailsRequest {
        return {...this.createNewProjectDetails(), applicationWave: "Other Wave", applicationNumber: dataGenerator.generateTemporaryId(5), projectType: "Central Route"};
    }

    public static CreateProjectRiskRequest(): CreateProjectRiskRequest {
        const result: CreateProjectRiskRequest = {
            overall: {
                riskRating: 1,
                summary: "This is my risk summary"
            },
        };

        return result;
    }
}

