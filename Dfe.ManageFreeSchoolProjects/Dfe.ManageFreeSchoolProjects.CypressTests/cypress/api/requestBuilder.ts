import { CreateProjectRiskRequest, ProjectDetailsRequest, ProjectType } from './domain';
import { EnvUsername } from 'cypress/constants/cypressConstants';
import dataGenerator from 'cypress/fixtures/dataGenerator';

export class RequestBuilder {
    public static createNewProjectDetails(): ProjectDetailsRequest {
        return {
            projectId: dataGenerator.generateTemporaryId(25),
            projectType: ProjectType.PresumptionRoute,
            createdBy: Cypress.expose(EnvUsername),
            schoolName: dataGenerator.generateSchoolName(),
            TRN: 'TR90111',
            applicationWave: 'FS - Presumption',
            projectAssignedToName: 'Test Person',
            projectAssignedToEmail: 'test.person@education.gov.uk',
        };
    }

    public static createProjectDetails(): ProjectDetailsRequest {
        return { ...this.createNewProjectDetails() };
    }

    public static createProjectDetailsNonPresumption(): ProjectDetailsRequest {
        return { ...this.createNewProjectDetails(), applicationWave: 'Other Wave' };
    }

    public static createProjectDetailsNewSchool(): ProjectDetailsRequest {
        return {
            ...this.createNewProjectDetails(),
            applicationWave: 'New School',
            projectType: ProjectType.NewSchool,
            // A New School project is run by a local authority, so it has no trust.
            TRN: '',
        };
    }

    public static createProjectDetailsCentralRoute(): ProjectDetailsRequest {
        return {
            ...this.createNewProjectDetails(),
            applicationWave: 'Other Wave',
            applicationNumber: dataGenerator.generateTemporaryId(5),
            projectType: ProjectType.CentralRoute,
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
