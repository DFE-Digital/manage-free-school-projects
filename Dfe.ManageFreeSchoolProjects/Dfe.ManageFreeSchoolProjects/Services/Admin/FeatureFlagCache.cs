using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.Services.Admin
{
    public interface IFeatureFlagCache : ICookieCacheService<FeatureFlagCacheItem>
    {
    }

    public class FeatureFlagCache : CookieCacheService<FeatureFlagCacheItem>, IFeatureFlagCache
    {
        public FeatureFlagCache(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider DataProtectionProvider)
            : base(httpContextAccessor, DataProtectionProvider, "Features")
        {
        }
    }

    public class  FeatureFlagCacheItem
    {
        public Dictionary<string, bool> FeatureFlags { get; set; } = new Dictionary<string, bool>();
    }
}
