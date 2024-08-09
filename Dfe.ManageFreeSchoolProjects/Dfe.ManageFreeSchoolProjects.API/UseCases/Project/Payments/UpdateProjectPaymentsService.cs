using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Payments;
public interface IUpdateProjectPaymentsService
{
    Task Execute(string projectId, Payment payment);
}
public class UpdateProjectPaymentsService : IUpdateProjectPaymentsService
{
    private readonly MfspContext _context;

    public UpdateProjectPaymentsService(MfspContext context)
    {
        _context = context;
    }

    public async Task Execute(string projectId, Payment payment)
    {
        var dbProject = await _context.Kpi.FirstOrDefaultAsync(x => x.ProjectStatusProjectId == projectId);

        if (dbProject == null)
        {
            throw new NotFoundException($"Project with id {projectId} not found");
        }

        var po = await _context.Po.FirstOrDefaultAsync(p => p.Rid == dbProject.Rid);

        int firstNullPaymentIndex;

        if (
            po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf1stPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf1stActualPayment is null
            )
        {
            firstNullPaymentIndex = 1;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf2ndPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment is null
            )
        {
            firstNullPaymentIndex = 2;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf3rdPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment is null
            )
        {
            firstNullPaymentIndex = 3;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf4thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf4thActualPayment is null
            )
        {
            firstNullPaymentIndex = 4;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf5thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf5thActualPayment is null
            )
        {
            firstNullPaymentIndex = 5;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf6thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf6thActualPayment is null
            )
        {
            firstNullPaymentIndex = 6;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf7thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf7thActualPayment is null
            )
        {
            firstNullPaymentIndex = 7;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf8thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf8thActualPayment is null
            )
        {
            firstNullPaymentIndex = 8;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf9thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf9thActualPayment is null
            )
        {
            firstNullPaymentIndex = 9;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf10thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf10thActualPayment is null
            )
        {
            firstNullPaymentIndex = 10;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf11thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf11thActualPayment is null
            )
        {
            firstNullPaymentIndex = 11;
        }

        else if (
            po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue is null &&
            po.ProjectDevelopmentGrantFundingAmountOf12thPayment is null &&
            po.ProjectDevelopmentGrantFundingDateOf12thActualPayment is null
            )
        {
            firstNullPaymentIndex = 12;
        }

        else
        {
            throw new NotFoundException("Cannot add more that 12 payments");
        }

        if (payment.PaymentIndex is null)
        {
            payment.PaymentIndex = firstNullPaymentIndex;
        }

        else if (payment.PaymentIndex >= firstNullPaymentIndex)
        {
            throw new NotFoundException($"Index {payment.PaymentIndex} cannot be found");
        }

        ProjectPaymentsUpdater.Update(po, payment);

        await _context.SaveChangesAsync();

    }
}

