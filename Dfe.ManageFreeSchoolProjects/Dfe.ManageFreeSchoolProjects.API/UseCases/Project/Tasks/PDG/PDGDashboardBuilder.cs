﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks.PDG;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.Refunds;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.StopPayments;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.TrustLetterPDGLetterSent;
using Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG.WriteOff;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.Tasks.PDG
{
    public static class PDGDashboardBuilder
    {
        public static PDGDashboard Build(Po po)
        {
            if (po == null)
            {
                return new PDGDashboard();
            }


            var stopPaymentTask = StopPaymentBuilder.Build(po);
            var refundsTask = RefundsBuilder.Build(po);
            var writeOffTask = WriteOffBuilder.Build(po);

            return new PDGDashboard
            {
                InitialGrant = ParseDecimalAllowNull(po.ProjectDevelopmentGrantFundingInitialGrantAllocation), 
                RevisedGrant = ParseDecimalAllowNull(po.ProjectDevelopmentGrantFundingRevisedGrantAllocation),
                PaymentScheduleAmount = GetPaymentScheduleAmount(po),
                PaymentScheduleDate = GetPaymentScheduleDate(po),
                PaymentActualAmount = PaymentCalculation.GetTotalAmount(po),
                PaymentActualDate = GetPaymentActualDate(po),
                TrustSignedPDGLetterDate = GetTrustSignedPDGLetterDate(po),
                PDGLetterSavedInWorkplaces = GetPDGLetterSavedInWorkplaces(po),
                PaymentStopped = stopPaymentTask.PaymentStopped,
                PaymentStoppedDate = stopPaymentTask.PaymentStoppedDate,
                LatestRefundDate = refundsTask.LatestRefundDate,
                RefundsTotalAmount = refundsTask.TotalAmount,
                IsWriteOffSetup = writeOffTask.IsWriteOffSetup,
                WriteOffAmount = writeOffTask.WriteOffAmount,
                WriteOffReason = writeOffTask.WriteOffReason,
                WriteOffDate = writeOffTask.WriteOffDate,
                FinanceBusinessPartnerApprovalReceivedFrom = writeOffTask.FinanceBusinessPartnerApprovalReceivedFrom,
                ApprovalDate = writeOffTask.ApprovalDate,
            };
        }
        
        private static bool? GetPDGLetterSavedInWorkplaces(Po po)
        {
            var linkSaved = TrustPDGLetterSentBuilder.GetPDGLetterSavedInWorkplaces(po);
            return linkSaved == false ? null : linkSaved;
        }

        private static DateTime? GetTrustSignedPDGLetterDate(Po po)
        {
            if (po.ProjectDevelopmentGrantFunding4thPdgGrantVariationDate != null)
            {
                return po.ProjectDevelopmentGrantFunding4thPdgGrantVariationDate;
            }

            if (po.ProjectDevelopmentGrantFunding3rdPdgGrantVariationDate != null)
            {
                return po.ProjectDevelopmentGrantFunding3rdPdgGrantVariationDate;
            }

            if (po.ProjectDevelopmentGrantFunding2ndPdgGrantVariationDate != null)
            {
                return po.ProjectDevelopmentGrantFunding2ndPdgGrantVariationDate;
            }

            if (po.ProjectDevelopmentGrantFunding1stPdgGrantVariationDate != null)
            {
                return po.ProjectDevelopmentGrantFunding1stPdgGrantVariationDate;
            }

            if (po.ProjectDevelopmentGrantFundingPdgGrantLetterDate != null)
            {
                return po.ProjectDevelopmentGrantFundingPdgGrantLetterDate;
            }

            return null;
        }



        private static decimal? GetPaymentScheduleAmount(Po po)
        {
            if (po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue == null &&
                po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue == null)
                return null;

            return ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf1stPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf2ndPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf3rdPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf4thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf5thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf6thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf7thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf8thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf9thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf10thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf11thPaymentDue) +
                   ParseDecimal(po.ProjectDevelopmentGrantFundingAmountOf12thPaymentDue);
        }

        private static DateTime? GetPaymentScheduleDate(Po po)
        {
            if (po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf12thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf11thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf10thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf9thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf8thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf7thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf6thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf5thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf4thPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf3rdPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf2ndPaymentDue;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf1stPaymentDue;
            }

            return null;
        }

        private static DateTime? GetPaymentActualDate(Po po)
        {
            if (po.ProjectDevelopmentGrantFundingDateOf12thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf12thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf11thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf11thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf10thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf10thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf9thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf9thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf8thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf8thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf7thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf7thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf6thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf6thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf5thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf5thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf4thActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf4thActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf3rdActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf2ndActualPayment;
            }

            if (po.ProjectDevelopmentGrantFundingDateOf1stActualPayment != null)
            {
                return po.ProjectDevelopmentGrantFundingDateOf1stActualPayment;
            }

            return null;
        }

        private static decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return decimal.Parse(value);
        }

        private static decimal? ParseDecimalAllowNull(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return decimal.Parse(value);
        }
    }
}