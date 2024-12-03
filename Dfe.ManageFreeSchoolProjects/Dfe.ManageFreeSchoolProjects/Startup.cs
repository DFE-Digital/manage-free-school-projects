using Azure.Identity;
using Dfe.ManageFreeSchoolProjects.Authorization;
using Dfe.ManageFreeSchoolProjects.Configuration;
using Dfe.ManageFreeSchoolProjects.Security;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Services.Constituency;
using Dfe.ManageFreeSchoolProjects.Services.Contacts;
using Dfe.ManageFreeSchoolProjects.Services.Dashboard;
using Dfe.ManageFreeSchoolProjects.Services.Project;
using Dfe.ManageFreeSchoolProjects.Services.Tasks;
using Dfe.ManageFreeSchoolProjects.Services.Trust;
using Dfe.ManageFreeSchoolProjects.Services.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.IO;
using System.Security.Claims;
using Dfe.ManageFreeSchoolProjects.Services.Reports;
using Dfe.ManageFreeSchoolProjects.Services.BulkEdit;
using System.Collections.Generic;
using System.Globalization;
using Dfe.BuildFreeSchools.Pages;
using Microsoft.AspNetCore.Localization;

namespace Dfe.ManageFreeSchoolProjects;

public class Startup
{
    private readonly TimeSpan _authenticationExpiration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        _authenticationExpiration = TimeSpan.FromMinutes(int.Parse(Configuration["AuthenticationExpirationInMinutes"] ?? "60"));
    }

    private IConfiguration Configuration { get; }

    private IConfigurationSection GetConfigurationSectionFor<T>()
    {
        string sectionName = typeof(T).Name.Replace("Options", string.Empty);
        return Configuration.GetRequiredSection(sectionName);
    }

    private T GetTypedConfigurationFor<T>()
    {
        return GetConfigurationSectionFor<T>().Get<T>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddFeatureManagement();
        services.AddHealthChecks();
        services
           .AddRazorPages(options =>
           {
               options.Conventions.AuthorizeFolder("/");
               options.Conventions.AllowAnonymousToPage("/Public/AccessibilityStatement");
               options.Conventions.AllowAnonymousToPage("/Public/Cookies");
               options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
           }) 
           .AddViewOptions(options =>
           {
               options.HtmlHelperOptions.ClientValidationEnabled = false;
           });

        services.AddControllersWithViews()
           .AddMicrosoftIdentityUI();
        SetupDataprotection(services);
        services.AddScoped<IGetDashboardService, GetDashboardService>();
        services.AddScoped<MfspApiClient, MfspApiClient>();
        services.AddScoped<ICreateUserService, CreateUserService>();
        services.AddScoped<ICreateProjectCache, CreateProjectCache>();
        services.AddScoped<IGetProjectOverviewService, GetProjectOverviewService>();
        services.AddScoped<IProjectTableReader, ProjectTableReader>();
        services.AddScoped<ICreateBulkProjectValidator, CreateBulkProjectValidator>();
        services.AddScoped<IGetProjectByTaskService, GetProjectByTaskService>();
        services.AddScoped<IUpdateProjectByTaskService, UpdateProjectByTaskService>();
        services.AddScoped<IGetProjectByTaskSummaryService, GetProjectByTaskSummaryService>();
        services.AddScoped<IGetLocalAuthoritiesService, GetLocalAuthoritiesService>();
        services.AddScoped<ICreateProjectService, CreateProjectService>();
        services.AddScoped<IGetTaskStatusService, GetTaskStatusService>();
        services.AddScoped<IUpdateTaskStatusService, UpdateTaskStatusService>();
        services.AddScoped<ICreateTasksService, CreateTasksService>();
        services.AddScoped<IGetProjectRiskService, GetProjectRiskService>();
        services.AddScoped<ICreateProjectRiskCache, CreateProjectRiskCache>();
        services.AddScoped<ICreateProjectRiskService, CreateProjectRiskService>();
        services.AddScoped<IGetTrustByRefService, GetTrustByRefService>();
        services.AddScoped<ISearchTrustByRefService, SearchTrustByRefService>();
        services.AddScoped<IGetContactsService, GetContactsService>();
        services.AddScoped<IAddContactsService, AddContactsService>();
        services.AddScoped<IUpdateProjectStatusService, UpdateProjectStatusService>();
        services.AddScoped<ISearchConstituency, SearchConstituency>();
        services.AddScoped<IAgeRangeCleanerService, AgeRangeCleanerService>();
        services.AddScoped<INotifyUserService, NotifyUserService>();
        services.AddScoped<IGetProjectManagersService, GetProjectManagersService>();
        services.AddScoped<IAnalyticsConsentService, AnalyticsConsentService>();
        services.AddScoped<IAllProjectsReportService, AllProjectsReportService>();
        services.AddScoped<IGetProjectSitesService, GetProjectSitesService>();
        services.AddScoped<IUpdateProjectSitesService, UpdateProjectSitesService>();
        services.AddScoped<IGetPupilNumbersService, GetPupilNumbersService>();
        services.AddScoped<IUpdatePupilNumbersService, UpdatePupilNumbersService>();
        services.AddScoped<IGetProjectPaymentsService, GetProjectPaymentsService>();
        services.AddScoped<IUpdateProjectPaymentsService, UpdateProjectPaymentsService>();
        services.AddScoped<IAddProjectPaymentsService, AddProjectPaymentsService>();
        services.AddScoped<IDeleteProjectPaymentsService, DeleteProjectPaymentsService>();
        services.AddScoped<IGrantLettersService, GrantLettersService>(); 
        services.AddScoped<IPDGPaymentInfoService, PDGPaymentInfoService>();
        services.AddScoped<IBulkEditFileReader, BulkEditFileReader>();
        services.AddScoped<IBulkEditFileValidator, BulkEditFileValidator>();
        services.AddScoped<IBulkEditValidateService, BulkEditValidateService>();
        services.AddScoped<IBulkEditCommitService, BulkEditCommitService>();
        services.AddScoped<IBulkEditCache, BulkEditCache>();

        services.AddScoped<IDashboardFiltersCache, DashboardFiltersCache>();

        services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.Session);
        services.AddSession(options =>
        {
            options.IdleTimeout = _authenticationExpiration;
            options.Cookie.Name = ".ManageFreeSchoolProjects.Session";
            options.Cookie.IsEssential = true;
        });
        services.AddHttpContextAccessor();

        services.AddHsts(options => {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        services.AddAuthorization(options => { options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build(); });

        services.AddMicrosoftIdentityWebAppAuthentication(Configuration);
        services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
           options =>
           {
               options.AccessDeniedPath = "/access-denied";
               options.Cookie.Name = ".ManageFreeSchoolProjects.Login";
               options.Cookie.HttpOnly = true;
               options.Cookie.IsEssential = true;
               options.ExpireTimeSpan = _authenticationExpiration;
               options.SlidingExpiration = true;
               if (string.IsNullOrEmpty(Configuration["CI"]))
               {
                   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
               }
           });

        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = Configuration["ApplicationInsights:ConnectionString"];
        });

        services.AddHttpClient("MfspClient", (_, client) =>
        {
            MfspOptions mfspOptions = GetTypedConfigurationFor<MfspOptions>();
            client.BaseAddress = new Uri(mfspOptions.ApiEndpoint);
            client.DefaultRequestHeaders.Add("ApiKey", mfspOptions.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "ManageFreeSchoolProjects/1.0");
        });

        services.AddScoped<ErrorService>();
        services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
        services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

        services.AddAntiforgery(options =>
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    private void SetupDataprotection(IServiceCollection services)
    {
      // Setup basic Data Protection and persist keys.xml to local file system
      var dp = services.AddDataProtection();

      // If a Key Vault Key URI is defined, expect to encrypt the keys.xml
      string kvProtectionKeyUri = Configuration.GetValue<string>("DataProtection:KeyVaultKey");
      if (!string.IsNullOrEmpty(kvProtectionKeyUri))
      {
        dp.PersistKeysToFileSystem(new DirectoryInfo(@"/srv/app/storage"));

        var credentials = new DefaultAzureCredential();
        dp.ProtectKeysWithAzureKeyVault(
          new Uri(kvProtectionKeyUri),
          credentials
        );
      }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        // Ensure we do not lose X-Forwarded-* Headers when behind a Proxy
        var forwardOptions = new ForwardedHeadersOptions {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false
        };
        forwardOptions.KnownNetworks.Clear();
        forwardOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardOptions);

        logger.LogInformation("Feature Flag - Use Academisation API: {usingAcademisationApi}", IsFeatureEnabled("hi"));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Errors");
        }

        app.UseSecurityHeaders(SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment()));
        app.UseHsts();

        app.UseStatusCodePagesWithReExecute("/Errors", "?statusCode={0}");

        app.UseHttpsRedirection();
        app.UseHealthChecks("/health");

        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        const string defaultCulture = "en-GB";

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(new CultureInfo(defaultCulture)),
            SupportedCultures = GetSupportedCultures(),
            SupportedUICultures = GetSupportedCultures(),
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllerRoute("default", "{controller}/{action}/");
        });

        bool IsFeatureEnabled(string flag)
        {
            return (app.ApplicationServices.GetService(typeof(IFeatureManager)) as IFeatureManager)?.IsEnabledAsync(flag).Result ?? false;
        }

        List<CultureInfo> GetSupportedCultures()
        {
            return new List<CultureInfo>
            {
                {  new CultureInfo(defaultCulture) }
            };
        }
    }

    /// <summary>
    ///    Builds Authorization policy
    ///    Ensure authenticated user and restrict roles if they are provided in configuration
    /// </summary>
    /// <returns>AuthorizationPolicyBuilder</returns>
    private AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
    {
        AuthorizationPolicyBuilder policyBuilder = new();
        policyBuilder.RequireAuthenticatedUser();

        string allowedRoles = Configuration.GetSection("AzureAd")["AllowedRoles"];
        if (string.IsNullOrWhiteSpace(allowedRoles) is false)
        {
            policyBuilder.RequireClaim(ClaimTypes.Role, allowedRoles.Split(','));
        }

        return policyBuilder;
    }
}
