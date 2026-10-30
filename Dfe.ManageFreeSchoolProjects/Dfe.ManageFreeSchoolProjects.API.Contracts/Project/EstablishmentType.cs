using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project
{
    public enum EstablishmentType
    {
        [Description("NotSet")]
        NotSet = 0,
        [Description("Academy alternative provision sponsor led")]
        AcademyAlternativeProvisionSponsorLed = 1,
        [Description("Academy special sponsor led")]    
        AcademySpecialSponsorLed = 2,
        [Description("Academy sponsor led")]
        AcademySponsorLed = 3,
        [Description("Community school")]
        CommunitySchool = 4,
        [Description("Community special school")]
        CommunitySpecialSchool = 5,
        [Description("Foundation school")]
        FoundationSchool = 6,
        [Description("Foundation special school")]
        FoundationSpecialSchool = 7,
        [Description("Pupil referral unit")]
        PupilReferralUnit = 8,
        [Description("Voluntary aided school")]
        VoluntaryAidedSchool = 9,
        [Description("Voluntary controlled school")]
        VoluntaryControlledSchool = 10,
    }
}