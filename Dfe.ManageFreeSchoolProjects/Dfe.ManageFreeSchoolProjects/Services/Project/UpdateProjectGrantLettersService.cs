using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Grants;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;

namespace Dfe.ManageFreeSchoolProjects.Services.Project;

public interface IGrantLettersService
{
    Task UpdateGrantLetters(string projectId, ProjectGrantLetters updatedGrantVariationLetter);
    Task<ProjectGrantLetters> Get(string projectId);
}

public class GrantLettersService(MfspApiClient apiClient) : IGrantLettersService
{
    public async Task UpdateGrantLetters(string projectId, ProjectGrantLetters updatedGrantVariationLetter)
    {
        var endpoint = $"/api/v1/client/projects/{projectId}/grant-letters";
        await apiClient.Put<ProjectGrantLetters, ApiSingleResponseV2<object>>(endpoint, updatedGrantVariationLetter);
    }

    public async Task<ProjectGrantLetters> Get(string projectId)
    {
        var endpoint = $"/api/v1/client/projects/{projectId}/grant-letters";
        var response = await apiClient.Get<ApiSingleResponseV2<ProjectGrantLetters>>(endpoint);
        return response.Data;
    }
}
