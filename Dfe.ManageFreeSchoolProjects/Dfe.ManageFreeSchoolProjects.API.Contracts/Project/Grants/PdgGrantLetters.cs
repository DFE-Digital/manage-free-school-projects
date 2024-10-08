using System.ComponentModel;

namespace Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Grants;

public record ProjectGrantLetters
{
    public DateTime? InitialGrantLetterDate { get; set; }
    
    public string InitialGrantLetterLink { get; set; }

    public DateTime? FinalGrantLetterDate { get; set; }
    
    public string GrantLetterLink { get; set; }
    
    public bool? InitialGrantLetterSavedToWorkplaces { get; set; }
    
    public bool? FinalGrantLetterSavedToWorkplaces { get; set; }
    
    public IEnumerable<GrantVariationLetter> VariationLetters { get; set; }
}

public record GrantVariationLetter
{
    public LetterVariation Variation { get; set; }
    public DateTime? LetterDate { get; set; }
    public string LetterLink { get; set; }
    public bool? SavedToWorkplacesFolder { get; set; }
    
    public enum LetterVariation
    {
        NotSet = 0,
        FirstVariation = 1,
        SecondVariation = 2,
        ThirdVariation = 3,
        FourthVariation = 4
    }
}