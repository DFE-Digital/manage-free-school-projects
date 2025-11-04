using Dfe.ManageFreeSchoolProjects.Configuration;
using Dfe.ManageFreeSchoolProjects.Models;
using Dfe.ManageFreeSchoolProjects.Services;
using Dfe.ManageFreeSchoolProjects.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Dfe.ManageFreeSchoolProjects.Pages.Public
{
	public class Cookies : PageModel
	{
		public bool? Consent { get; set; }
		public bool PreferencesSet { get; set; } = false;
		public string returnPath { get; set; }
		private readonly ILogger<Cookies> _logger;
		private readonly IAnalyticsConsentService _analyticsConsentService;

		public Cookies(ILogger<Cookies> logger, IAnalyticsConsentService analyticsConsentService)
		{
			_logger = logger;
			_analyticsConsentService = analyticsConsentService;
		}

		public string TransfersCookiesUrl { get; set; }

		public ActionResult OnGet(bool? consent, string returnUrl)
		{
			// Validate and sanitize returnUrl to prevent XSS and Open Redirect attacks
			var sanitizedReturnUrl = UrlValidator.SanitizeReturnUrl(returnUrl, "/", Url);
			
			// Log suspicious attempts for security monitoring
			if (!string.IsNullOrEmpty(returnUrl) && returnUrl != sanitizedReturnUrl)
			{
				_logger.LogWarning("Suspicious returnUrl detected and blocked: {ReturnUrl}", returnUrl);
			}
			
			returnPath = sanitizedReturnUrl;

			Consent = _analyticsConsentService.ConsentValue();

            if (consent.HasValue)
			{
				PreferencesSet = true;

				ApplyCookieConsent(consent.Value);

				if (!string.IsNullOrEmpty(sanitizedReturnUrl))
				{
					return Redirect(sanitizedReturnUrl);
				}

				return RedirectToPage(Links.Public.CookiePreferences);
			}

			return Page();
		}

		public IActionResult OnPost(bool? consent, string returnUrl)
		{
			// Validate and sanitize returnUrl to prevent XSS and Open Redirect attacks
			var sanitizedReturnUrl = UrlValidator.SanitizeReturnUrl(returnUrl, "/", Url);
			
			// Log suspicious attempts for security monitoring
			if (!string.IsNullOrEmpty(returnUrl) && returnUrl != sanitizedReturnUrl)
			{
				_logger.LogWarning("Suspicious returnUrl detected and blocked in POST: {ReturnUrl}", returnUrl);
			}
			
			returnPath = sanitizedReturnUrl;

            Consent = _analyticsConsentService.ConsentValue();

            if (consent.HasValue)
			{
				Consent = consent;
				PreferencesSet = true;

				ApplyCookieConsent(consent.Value);
				return Page();
			}

			return Page();
		}

		private void ApplyCookieConsent(bool consent)
		{
			if (consent) { 
				_analyticsConsentService.AllowConsent();
			}
			else
			{
				_analyticsConsentService.DenyConsent();
			}
		}
	}
}