using Dfe.ManageFreeSchoolProjects.API.Contracts.BulkEdit;
using Dfe.ManageFreeSchoolProjects.API.Extensions;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit
{
    public interface IBulkEditCommit
    {
        Task Execute(BulkEditRequest request);
    }

    public class BulkEditCommit<TDto>(
            IHeaderRegister<TDto> headerRegister,
            IBulkEditDataRetrieval<TDto> dataRetrieval,
            IBulkDataCommit<TDto> dataCommit
            ) : IBulkEditCommit where TDto : IBulkEditDto
    {
        public async Task Execute(BulkEditRequest request)
        {
            var headers = headerRegister.GetHeaders();

            var IdColumnIndex = request.Headers.Find(x => string.Compare(x.Name, headerRegister.IdentifingHeader, true) == 0).Index;

            var projectIds = request.Rows.Select(x => x.Columns.Where(y => y.ColumnIndex == IdColumnIndex).Select(y => y.Value).FirstOrDefault()).ToList();
            var projects = await dataRetrieval.Retrieve(projectIds);

            foreach (var columns in request.Rows.Select(x => x.Columns))
            {
                var currentRow = projects[columns.Find(x => x.ColumnIndex == IdColumnIndex).Value];

                foreach (var column in columns)
                {
                    if (column.ColumnIndex == IdColumnIndex)
                    {
                        continue;
                    }

                    if(column.Value.IsNullOrEmpty())
                    {
                        continue;
                    }
                    var headerName = request.Headers.Find(x => x.Index == column.ColumnIndex)?.Name;
                    var header = headers.Find(x => string.Compare(x.Name, headerName, true) == 0);
                    var value = column.Value;
                    header.DataInteraction.ApplyToDto(value, currentRow);
                }
            }

            await dataCommit.Save(projects.Values.ToList());
        }
    }
}
