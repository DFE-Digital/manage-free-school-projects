using Microsoft.AspNetCore.Builder;

namespace Dfe.ManageFreeSchoolProjects.Security
{
	public static class SecurityHeadersDefinitions
	{
		static string GoogleTagManagerUri => "https://www.googletagmanager.com";
		static string GoogleAnalyticsUri => "https://www.google-analytics.com/";
		static string ApplicationInsightsUri => "https://js.monitor.azure.com/";

		public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev)
		{

			var policy = new HeaderPolicyCollection()
				.AddFrameOptionsDeny()
				.AddXssProtectionDisabled()
				.AddContentTypeOptionsNoSniff()
				.AddReferrerPolicyStrictOriginWhenCrossOrigin()
				.RemoveServerHeader()
				.AddCrossOriginOpenerPolicy(builder =>
				{
					builder.SameOrigin();
				})
				.AddCrossOriginEmbedderPolicy(builder =>
				{
					builder.RequireCorp();
				})
				.AddCrossOriginResourcePolicy(builder =>
				{
					builder.SameOrigin();
				})
				.AddContentSecurityPolicy(builder =>
				{
					builder.AddObjectSrc().None();
					builder.AddBlockAllMixedContent();
					builder.AddImgSrc().Self().From("data:").From(GoogleAnalyticsUri)
						.From(GoogleTagManagerUri);
					builder.AddFormAction().Self();
					builder.AddFormAction().OverHttps();
					builder.AddFontSrc().Self();
					builder.AddStyleSrc().Self();
					builder.AddBaseUri().Self();
					builder.AddScriptSrc()
						.From(GoogleTagManagerUri).From(ApplicationInsightsUri)
							.UnsafeInline().WithNonce();
					builder.AddFrameAncestors().None();
				})
				.RemoveServerHeader()
				.AddPermissionsPolicy(builder =>
				{
					builder.AddAccelerometer().None();
					builder.AddAutoplay().None();
					builder.AddCamera().None();
					builder.AddEncryptedMedia().None();
					builder.AddFullscreen().All();
					builder.AddGeolocation().None();
					builder.AddGyroscope().None();
					builder.AddMagnetometer().None();
					builder.AddMicrophone().None();
					builder.AddMidi().None();
					builder.AddPayment().None();
					builder.AddPictureInPicture().None();
					builder.AddSyncXHR().None();
					builder.AddUsb().None();
				});

			return policy;
		}
	}
}
