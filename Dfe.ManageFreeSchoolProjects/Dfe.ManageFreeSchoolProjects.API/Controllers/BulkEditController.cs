using Dfe.ManageFreeSchoolProjects.API.Contracts.BulkEdit;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Risk;
using Dfe.ManageFreeSchoolProjects.API.Contracts.ResponseModels;
using Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit;
using Dfe.ManageFreeSchoolProjects.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Dfe.ManageFreeSchoolProjects.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/bulkedit")]
    [ApiController]
    public class BulkEditController(ILogger<BulkEditController> logger, IBulkEditValidation bulkEditValidation, IBulkEditCommit bulkEditCommit) : ControllerBase
    {

        [HttpPost]
        [Route("validate")]
        public async Task<ActionResult<ApiSingleResponseV2<BulkEditValidateResponse>>> validate(BulkEditRequest request)
        {
            logger.LogMethodEntered();

            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var response = await bulkEditValidation.Execute(request);

            return new ObjectResult(new ApiSingleResponseV2<BulkEditValidateResponse>(response)) { StatusCode = StatusCodes.Status200OK };
        }

        [HttpPost]
        [Route("commit")]
        public async Task<ActionResult> commit(BulkEditRequest request)
        {
            logger.LogMethodEntered();

            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            var validation = await bulkEditValidation.Execute(request);

            if(validation.ValidationResultRows.Exists(r => r.Columns.Exists(x => x.Error != null)))
            {
                return BadRequest();
            }

            await bulkEditCommit.Execute(request);

            return NoContent();
        }
    }
}
