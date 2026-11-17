using Dfe.ManageFreeSchoolProjects.API.Contracts.Common;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Grants;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Extensions;
using static Dfe.ManageFreeSchoolProjects.API.Contracts.Project.ClassType;

namespace Dfe.ManageFreeSchoolProjects.API.UseCases.Project;

public static class EnumParsers
{
    public static FaithStatus ParseFaithStatus(string input)
    {
        if (Enum.TryParse<FaithStatus>(input, out var faithStatus))
        {
            return faithStatus;
        }

        try
        {
            // Older records were saved using the description instead of the name
            return input.ToEnumFromDescription<FaithStatus>();
        }
        catch
        {
            return FaithStatus.NotSet;
        }
    }

    public static FaithType ParseFaithType(string input)
    {
        return Enum.TryParse<FaithType>(input, out var faithType) ? faithType : FaithType.NotSet;
    }

    public static Gender ParseGender(string input)
    {
        try
        {
            return input.ToEnumFromDescription<Gender>();
        }
        catch
        {
            return Gender.NotSet;
        }
    }

    public static SixthForm ParseSixthForm(string input)
    {
        return Enum.TryParse<SixthForm>(input, out var parsedSixthForm)
            ? parsedSixthForm
            : SixthForm.NotSet;
    }

    public static Nursery ParseNursery(string input)
    {
        return Enum.TryParse<Nursery>(input, out var paredNursery) ? paredNursery : Nursery.NotSet;
    }

    public static AlternativeProvision ParseAlternativeProvision(string input)
    {
        return Enum.TryParse<AlternativeProvision>(input, out var parsedAlternativeProvision)
            ? parsedAlternativeProvision
            : AlternativeProvision.NotSet;
    }

    public static SpecialEducationNeeds ParseSpecialEducationNeeds(string input)
    {
        return Enum.TryParse<SpecialEducationNeeds>(input, out var parsedSpecialEducationNeeds) ? parsedSpecialEducationNeeds : SpecialEducationNeeds.NotSet;
    }

    public static ResidentialOrBoarding ParseResidentialOrBoarding(string input)
    {
        return Enum.TryParse<ResidentialOrBoarding>(input, out var parsedResidentialOrBoarding)
            ? parsedResidentialOrBoarding
            : ResidentialOrBoarding.NotSet;
    }

    public static TrustType ParseTrustType(string input)
    {
        return Enum.TryParse<TrustType>(input, out var trustType) ? trustType : TrustType.NotSet;
    }

    public static TypeOfMeetingHeld ParseTypeOfMeetingHeld(string input)
    {
        return Enum.TryParse<TypeOfMeetingHeld>(input, out var typeOfMeetingHeld) ? typeOfMeetingHeld : TypeOfMeetingHeld.NotSet;
    }
    
    public static YesNoNotApplicable? ParseInspectionConditionsMetToEnum(string condition)
    {
        return condition switch
        {
            "Yes" => YesNoNotApplicable.Yes,
            "No" => YesNoNotApplicable.No,
            "Not applicable" => YesNoNotApplicable.NotApplicable,
            _ => null
        };

    }
        
    public static string ParseInspectionConditionsMetToString(YesNoNotApplicable? condition)
    {
        return condition switch
        {
            YesNoNotApplicable.Yes => "Yes",
            YesNoNotApplicable.No => "No",
            YesNoNotApplicable.NotApplicable => "Not applicable",
            _ => null
        };
    }
}