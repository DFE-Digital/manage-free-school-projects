import { CreateProjectRiskRequest, ProjectDetailsRequest } from './domain';
import { EnvUsername } from 'cypress/constants/cypressConstants';
import dataGenerator from 'cypress/fixtures/dataGenerator';

export class RequestBuilder {
    public static createNewProjectDetails(): ProjectDetailsRequest {
        return {
            projectId: dataGenerator.generateTemporaryId(25),
            projectType: 'Presumption',
            createdBy: Cypress.env(EnvUsername),
            schoolName: dataGenerator.generateSchoolName(),
            TRN: 'TR90111',
            applicationWave: 'FS - Presumption',
            projectAssignedToName: 'Test Person',
            projectAssignedToEmail: 'test.person.education.gov.uk',
        };
    }

    public static createProjectDetails(): ProjectDetailsRequest {
        return { ...this.createNewProjectDetails() };
    }

    public static createProjectDetailsNonPresumption(): ProjectDetailsRequest {
        return { ...this.createNewProjectDetails(), applicationWave: 'Other Wave' };
    }

    public static createProjectDetailsCentralRoute(): ProjectDetailsRequest {
        return {
            ...this.createNewProjectDetails(),
            applicationWave: 'Other Wave',
            applicationNumber: dataGenerator.generateTemporaryId(5),
            projectType: 'Central Route',
        };
    }

    public static CreateProjectRiskRequest(): CreateProjectRiskRequest {
        return {
            overall: {
                riskRating: 1,
                summary: 'This is my risk summary',
            },
        };
    }
}
