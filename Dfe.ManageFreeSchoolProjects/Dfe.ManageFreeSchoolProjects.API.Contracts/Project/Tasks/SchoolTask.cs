using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks
{
    public record SchoolTask
    {
        public string CurrentFreeSchoolName { get; set; }
        public SchoolType SchoolType { get; set; }
        public SchoolPhase SchoolPhase { get; set; }
        public string AgeRange { get; set; }
        public ClassType.Nursery Nursery { get; set; }
        public ClassType.SixthForm SixthForm { get; set; }
        public ClassType.AlternativeProvision AlternativeProvision { get; set; }
        public ClassType.SpecialEducationNeeds SpecialEducationNeeds { get; set; }
        
        public ClassType.ResidentialOrBoarding ResidentialOrBoarding { get; set; }
        public Gender Gender { get; set; }
        public string FormsOfEntry { get; set; }
        public FaithStatus FaithStatus { get; set; }
        public FaithType FaithType { get; set; }
        public string OtherFaithType { get; set; }
        public bool MarkedAsComplete { get; set; }
    }
    
    public enum SchoolPhase
    {
        [Description("NotSet")]
        NotSet,
        [Description("Primary")]
        Primary,
        [Description("Secondary")]
        Secondary,
        [Description("16 to 19")]
        SixteenToNineteen, 
        [Description("All-through")]
        AllThrough, 
    }

    public enum FaithType
    {
        [Description("NotSet")]
        NotSet,
        [Description("Church of England")]
        ChurchOfEngland,
        [Description("Christian")]
        Christian,
        [Description("Greek Orthodox")]
        GreekOrthodox,
        [Description("Hindu")]
        Hindu,
        [Description("Jewish")]
        Jewish,
        [Description("Methodist")]
        Methodist,
        [Description("Muslim")]
        Muslim,
        [Description("Roman Catholic")]
        RomanCatholic,
        [Description("Sikh")]
        Sikh,
        [Description("Other")]
        Other,
        [Description("None")]
        None,
    }

    public enum FaithStatus
    {
        NotSet,
        [Description("This is also known as character.")]
        Designation,
        Ethos,
        None
    }

    public enum Gender
    {
        NotSet,
        [Description("Boys only")]
        BoysOnly,
        [Description("Girls only")]
        GirlsOnly,
        [Description("Mixed")]
        Mixed
    }

    public enum FaithOfDiocese
    {
        [Description("Church of England")]
        ChurchOfEngland = 1,
       
        [Description("Roman Catholic")]
        RomanCatholic = 2
    }

    public enum ProposalStatus
    {
        [Description("Active")]
        Active
    }

    public enum ProposalProposer
    {
        [Description("Academy trust (including Diocese academy trust)")]
        AcademyTrust = 1,
        [Description("Diocese")]
        Diocese = 2,
        [Description("Another religious organisation")]
        AnotherReligiousOrganisation = 3,
        [Description("Local authority that published the specification")]
        LocalAuthorityThatPushedSpecification = 4,
        [Description("Another local authority")]
        AnotherLocalAuthority = 5,
        [Description("Joint proposal between the local authority that published the specification and another local authority")]
        JointProposal = 6
    }
}
