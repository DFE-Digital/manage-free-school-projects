using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace Dfe.ManageFreeSchoolProjects.Services.Admin
{
    public class FeatureFlagSessionManager : ISessionManager
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IDataProtectionProvider dataProtectionProvider;

        public FeatureFlagSessionManager(
            IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            this.dataProtectionProvider = dataProtectionProvider;
        }

        public Task<bool?> GetAsync(string featureName)
        {
            // Need this because the ISessionManager is a singleton and the feature flag cache is not
            // We do not want to make the feature flag cache a singleton so we cannot use DI
            var featureFlagCache = new FeatureFlagCache(_httpContextAccessor, dataProtectionProvider);

            var cacheItemExists = featureFlagCache.HasEntry();

            if (cacheItemExists)
            {
                var cacheItem = featureFlagCache.Get();

                if (cacheItem.FeatureFlags.ContainsKey(featureName))
                {
                    return Task.FromResult(cacheItem.FeatureFlags[featureName] as bool?);
                }
            }

            return Task.FromResult<bool?>(null);
        }

        public Task SetAsync(string featureName, bool enabled)
        {
            return Task.CompletedTask;
        }
    }
}
