using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.ManageFreeSchoolProjects.API.Contracts.Project.Grants;
using Dfe.ManageFreeSchoolProjects.Constants;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageFreeSchoolProjects.Pages.Project.Tasks.PDG.Central;

public class EditPDGVariationLetter(IGrantLettersService grantLettersService) : PageModel
{
    [BindProperty(SupportsGet = true)] public string ProjectId { get; set; }

    [BindProperty(SupportsGet = true)] public string Variation { get; set; }

    public string CurrentFreeSchoolName { get; set; }

    [BindProperty(BinderType = typeof(DateInputModelBinder))]
    public DateTime? DueDateOfVariationLetter { get; set; }

    [BindProperty] public bool VariationLetterSavedToWorkplaces { get; set; }

    public GrantVariationLetter VariationLetter { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var grantLetters = await grantLettersService.Get(ProjectId);

        var parsedVariation = ParseVariation(Variation);

        VariationLetter = grantLetters.VariationLetters.SingleOrDefault(x => x.Variation == parsedVariation);
        DueDateOfVariationLetter = VariationLetter?.LetterDate;
        VariationLetterSavedToWorkplaces = VariationLetter?.SavedToWorkplacesFolder ?? false;

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var updatedVariationLetter = new GrantVariationLetter
        {
            Variation = ParseVariation(Variation),
            LetterDate = DueDateOfVariationLetter,
            SavedToWorkplacesFolder = VariationLetterSavedToWorkplaces
        };

        await grantLettersService.UpdateVariationLetter(ProjectId, updatedVariationLetter);

        return Redirect(string.Format(RouteConstants.EditPDGGrantLetters, ProjectId));
    }

    private static GrantVariationLetter.LetterVariation ParseVariation(string variationNumber)
    {
        // if (!int.TryParse(variationNumber, out var variationAsInt)) 
        //     return GrantVariationLetter.LetterVariation.NotSet;
        //
        // if (Enum.IsDefined(typeof(GrantVariationLetter.LetterVariation), variationAsInt))
        //     return (GrantVariationLetter.LetterVariation)variationAsInt;
        //
        // return GrantVariationLetter.LetterVariation.NotSet;

        return Enum.TryParse<GrantVariationLetter.LetterVariation>(variationNumber, out var variationEnum)
            ? variationEnum
            : GrantVariationLetter.LetterVariation.NotSet;
    }
}