﻿using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.PupilNumbers;
using Dfe.ManageFreeSchoolProjects.Data.Entities.Existing;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project.PupilNumbers
{
    public class UpdateRecruitmentAndViabilityService : IUpdatePupilNumbersSectionService
    {
        public void Execute(Po po, UpdatePupilNumbersRequest request)
        {
            if (request.RecruitmentAndViability == null)
            {
                return;
            }

            po.PupilNumbersAndCapacityMinimumFirstYearRecruitmentForViabilityYrY6 = request.RecruitmentAndViability.ReceptionToYear6.MinimumViableNumber.ToString();
            po.PupilNumbersAndCapacityNoApplicationsReceivedYrY6 = request.RecruitmentAndViability.ReceptionToYear6.ApplicationsReceived.ToString();

            po.PupilNumbersAndCapacityMinimumFirstYearRecruitmentForViabilityY7Y11 = request.RecruitmentAndViability.Year7ToYear11.MinimumViableNumber.ToString();
            po.PupilNumbersAndCapacityNoApplicationsReceivedY7Y11 = request.RecruitmentAndViability.Year7ToYear11.ApplicationsReceived.ToString();

            po.PupilNumbersAndCapacityMinimumFirstYearRecruitmentForViabilityY12Y14 = request.RecruitmentAndViability.Year12ToYear14.MinimumViableNumber.ToString();
            po.PupilNumbersAndCapacityNoApplicationsReceivedY12Y14 = request.RecruitmentAndViability.Year12ToYear14.ApplicationsReceived.ToString();

            var totalMinimumViableNumber = request.RecruitmentAndViability.ReceptionToYear6.MinimumViableNumber +
                                           request.RecruitmentAndViability.Year7ToYear11.MinimumViableNumber +
                                           request.RecruitmentAndViability.Year12ToYear14.MinimumViableNumber;

            var totalApplicationsReceived = request.RecruitmentAndViability.ReceptionToYear6.ApplicationsReceived +
                                            request.RecruitmentAndViability.Year7ToYear11.ApplicationsReceived +
                                            request.RecruitmentAndViability.Year12ToYear14.ApplicationsReceived;

            po.PupilNumbersAndCapacityMinimumFirstYearRecruitmentForViabilityTotal = totalMinimumViableNumber.ToString();
            po.PupilNumbersAndCapacityNoApplicationsReceivedTotal = totalApplicationsReceived.ToString();
        }
    }
}
