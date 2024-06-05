using Dfe.ManageFreeSchoolProjects.Configuration;
using Dfe.ManageFreeSchoolProjects.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Pages.Admin.Features
{
    public class EditFeatureFlagsModel : PageModel
    {
        private IFeatureFlagCache _featureFlagCache;
        private IFeatureManager _featureManager;

        [BindProperty(Name = "feature-flag")]
        public List<string> SelectedFeatureFlags { get; set; } = new();

        public List<string> AvailableFeatureFlags { get; set; } = new();

        public EditFeatureFlagsModel(
            IFeatureManager featureManager,
            IFeatureFlagCache featureFlagCache)
        {
            _featureFlagCache = featureFlagCache;
            _featureManager = featureManager;
        }

        public async Task OnGet()
        {
            var type = typeof(FeatureFlags);

            AvailableFeatureFlags = type.GetFields().Select(x => x.Name).ToList();

            foreach (var featureFlag in AvailableFeatureFlags)
            {
                var isEnabled = await _featureManager.IsEnabledAsync(featureFlag);

                if (isEnabled)
                {
                    SelectedFeatureFlags.Add(featureFlag);
                }
            }
        }

        public IActionResult OnPost()
        {
            var cacheItem = new FeatureFlagCacheItem
            {
                FeatureFlags = new Dictionary<string, bool>()
            };

            foreach(var selectedFeatureFlag in SelectedFeatureFlags)
            {
                cacheItem.FeatureFlags.Add(selectedFeatureFlag, true);
            }

            _featureFlagCache.Update(cacheItem);

            return Redirect("/");
        }
    }
}
