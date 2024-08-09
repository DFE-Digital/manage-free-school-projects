using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Payments
{
    public static class ProjectPaymentsUpdater
    {
        public static Po Update(Po po, Payment payment)
        {

            switch (payment.PaymentIndex)
            {
                case 1:
                    po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf1stPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf1stActualPayment = payment.PaymentActualDate;
                    break;

                case 2:
                    po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf2ndPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment = payment.PaymentActualDate;
                    break;

                case 3:
                    po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf3rdPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment = payment.PaymentActualDate;
                    break;

                case 4:
                    po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf4thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf4thActualPayment = payment.PaymentActualDate;
                    break;

                case 5:
                    po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf5thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf5thActualPayment = payment.PaymentActualDate;
                    break;

                case 6:
                    po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf6thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf6thActualPayment = payment.PaymentActualDate;
                    break;

                case 7:
                    po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf7thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf7thActualPayment = payment.PaymentActualDate;
                    break;

                case 8:
                    po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf8thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf8thActualPayment = payment.PaymentActualDate;
                    break;

                case 9:
                    po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf9thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf9thActualPayment = payment.PaymentActualDate;
                    break;

                case 10:
                    po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf10thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf10thActualPayment = payment.PaymentActualDate;
                    break;

                case 11:
                    po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf11thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf11thActualPayment = payment.PaymentActualDate;
                    break;

                case 12:
                    po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue = payment.PaymentScheduleAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue = payment.PaymentScheduleDate;
                    po.ProjectDevelopmentGrantFundingAmountOf12thPayment = payment.PaymentActualAmount.ToString();
                    po.ProjectDevelopmentGrantFundingDateOf12thActualPayment = payment.PaymentActualDate;
                    break;

                default:
                    throw new NotFoundException($"Index {payment.PaymentIndex} cannot be found");
            }

            return po;
        }
    }
}

