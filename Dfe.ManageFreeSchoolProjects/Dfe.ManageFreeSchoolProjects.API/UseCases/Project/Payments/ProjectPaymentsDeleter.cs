using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Payments;
using Dfe.ManageFreeSchoolProjects.API.Exceptions;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Payments
{
    public static class ProjectPaymentsDeleter
    {
        static bool paymentDeleted = false;
       
        public static Po Delete(Po po, int paymentIndex)
        {

            if (paymentIndex <= 1)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue, po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf1stPayment, po.ProjectDevelopmentGrantFundingDateOf1stActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue = po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf1stPayment = po.ProjectDevelopmentGrantFundingAmountOf2ndPayment;
                po.ProjectDevelopmentGrantFundingDateOf1stActualPayment = po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment;
            }

            if (paymentIndex <= 2)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue, po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf2ndPayment, po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment);
                
                po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue = po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf2ndPayment = po.ProjectDevelopmentGrantFundingAmountOf3rdPayment;
                po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment = po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment;
            }

            if (paymentIndex <= 3)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue, po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf3rdPayment, po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue = po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf3rdPayment = po.ProjectDevelopmentGrantFundingAmountOf4thPayment;
                po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment = po.ProjectDevelopmentGrantFundingDateOf4thActualPayment;
            }

            if (paymentIndex <= 4)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf4thPayment, po.ProjectDevelopmentGrantFundingDateOf4thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf4thPayment = po.ProjectDevelopmentGrantFundingAmountOf5thPayment;
                po.ProjectDevelopmentGrantFundingDateOf4thActualPayment = po.ProjectDevelopmentGrantFundingDateOf5thActualPayment;
            }

            if (paymentIndex <= 5)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf5thPayment, po.ProjectDevelopmentGrantFundingDateOf5thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf5thPayment = po.ProjectDevelopmentGrantFundingAmountOf6thPayment;
                po.ProjectDevelopmentGrantFundingDateOf5thActualPayment = po.ProjectDevelopmentGrantFundingDateOf6thActualPayment;
            }

            if (paymentIndex <= 6)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf6thPayment, po.ProjectDevelopmentGrantFundingDateOf6thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf6thPayment = po.ProjectDevelopmentGrantFundingAmountOf7thPayment;
                po.ProjectDevelopmentGrantFundingDateOf6thActualPayment = po.ProjectDevelopmentGrantFundingDateOf7thActualPayment;
            }

            if (paymentIndex <= 7)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf7thPayment, po.ProjectDevelopmentGrantFundingDateOf7thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf7thPayment = po.ProjectDevelopmentGrantFundingAmountOf8thPayment;
                po.ProjectDevelopmentGrantFundingDateOf7thActualPayment = po.ProjectDevelopmentGrantFundingDateOf8thActualPayment;
            }

            if (paymentIndex <= 8)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf8thPayment, po.ProjectDevelopmentGrantFundingDateOf8thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf8thPayment = po.ProjectDevelopmentGrantFundingAmountOf9thPayment;
                po.ProjectDevelopmentGrantFundingDateOf8thActualPayment = po.ProjectDevelopmentGrantFundingDateOf9thActualPayment;
            }

            if (paymentIndex <= 9)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf9thPayment, po.ProjectDevelopmentGrantFundingDateOf9thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf9thPayment = po.ProjectDevelopmentGrantFundingAmountOf10thPayment;
                po.ProjectDevelopmentGrantFundingDateOf9thActualPayment = po.ProjectDevelopmentGrantFundingDateOf10thActualPayment;
            }

            if (paymentIndex <= 10)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf10thPayment, po.ProjectDevelopmentGrantFundingDateOf10thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf10thPayment = po.ProjectDevelopmentGrantFundingAmountOf11thPayment;
                po.ProjectDevelopmentGrantFundingDateOf10thActualPayment = po.ProjectDevelopmentGrantFundingDateOf11thActualPayment;
            }

            if (paymentIndex <= 11)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf11thPayment, po.ProjectDevelopmentGrantFundingDateOf11thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue = po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue;
                po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue = po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue;
                po.ProjectDevelopmentGrantFundingAmountOf11thPayment = po.ProjectDevelopmentGrantFundingAmountOf12thPayment;
                po.ProjectDevelopmentGrantFundingDateOf11thActualPayment = po.ProjectDevelopmentGrantFundingDateOf12thActualPayment;
            }

            if (paymentIndex == 12)
            {
                CheckPaymentExists(po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue, po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue, po.ProjectDevelopmentGrantFundingAmountOf12thPayment, po.ProjectDevelopmentGrantFundingDateOf12thActualPayment);

                po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue = null;
                po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue = null;
                po.ProjectDevelopmentGrantFundingAmountOf12thPayment = null;
                po.ProjectDevelopmentGrantFundingDateOf12thActualPayment = null;
            }

            if (paymentIndex > 12)
            {
                throw new NotFoundException("Index out of range");
            }

            paymentDeleted = false;

            return po;
        }

        public static void CheckPaymentExists(string paymentScheduleAmount, DateTime? paymentScheduleDate, string paymentActualAmount, DateTime? paymentActualDate)
        {
            if (paymentScheduleAmount is null && paymentScheduleDate is null && paymentActualAmount is null && paymentActualDate is null && paymentDeleted == false)
            {
                throw new NotFoundException("Payment not found");
            }

            else if (paymentDeleted == false)
            {
                paymentDeleted = true;
            }
        }

    }
}

