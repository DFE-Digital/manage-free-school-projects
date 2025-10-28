using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dfe.ManageFreeSchoolProjects.Utils
{
    /// <summary>
    /// Provides URL validation to prevent XSS and Open Redirect attacks.
    /// </summary>
    public static class UrlValidator
    {
        // Dangerous URL protocols that can execute JavaScript or cause other security issues
        private static readonly string[] DangerousProtocols = new[]
        {
            "javascript:",
            "data:",
            "vbscript:",
            "file:",
            "about:",
            "blob:"
        };

        /// <summary>
        /// Validates that a URL is safe for use in redirects and href attributes.
        /// Blocks dangerous protocols (javascript:, data:, etc.) and external URLs.
        /// </summary>
        /// <param name="url">The URL to validate</param>
        /// <param name="urlHelper">Optional IUrlHelper for additional validation</param>
        /// <returns>True if the URL is safe (local and non-malicious), false otherwise</returns>
        public static bool IsValidReturnUrl(string url, IUrlHelper urlHelper = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            // Decode the URL to catch encoded attacks like javascript%3Aalert(1)
            var decodedUrl = System.Web.HttpUtility.UrlDecode(url);
            
            // Check for dangerous protocols (case-insensitive)
            if (ContainsDangerousProtocol(decodedUrl))
            {
                return false;
            }

            // Check if it's a local URL using ASP.NET Core's validation if available
            if (urlHelper != null && !urlHelper.IsLocalUrl(url))
            {
                return false;
            }

            // Additional check: URL should start with / or ~/ for local paths
            // or be relative without a protocol
            if (!IsLocalPath(decodedUrl))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the URL contains any dangerous protocols.
        /// </summary>
        private static bool ContainsDangerousProtocol(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var lowerUrl = url.ToLowerInvariant().Trim();
            
            // Check if URL starts with any dangerous protocol
            return DangerousProtocols.Any(protocol => lowerUrl.StartsWith(protocol));
        }

        /// <summary>
        /// Validates that a URL is a local path (relative URL).
        /// </summary>
        private static bool IsLocalPath(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var trimmedUrl = url.Trim();

            // Allow paths starting with / or ~/
            if (trimmedUrl.StartsWith("/") || trimmedUrl.StartsWith("~/"))
            {
                // Ensure it's not a protocol-relative URL like //evil.com
                if (trimmedUrl.StartsWith("//"))
                {
                    return false;
                }
                return true;
            }

            // Check if it contains a protocol (http://, https://, etc.)
            // If it does, it's not a local path
            if (trimmedUrl.Contains("://") || trimmedUrl.Contains(":"))
            {
                return false;
            }

            // Allow relative paths without leading slash (e.g., "page.html")
            // But be conservative - if uncertain, reject
            return !trimmedUrl.Contains("//");
        }

        /// <summary>
        /// Sanitizes a return URL by validating it and returning a safe default if invalid.
        /// </summary>
        /// <param name="url">The URL to sanitize</param>
        /// <param name="defaultUrl">Default URL to return if validation fails (defaults to "/")</param>
        /// <param name="urlHelper">Optional IUrlHelper for additional validation</param>
        /// <returns>The original URL if valid, or the default URL if invalid</returns>
        public static string SanitizeReturnUrl(string url, string defaultUrl = "/", IUrlHelper urlHelper = null)
        {
            if (IsValidReturnUrl(url, urlHelper))
            {
                return url;
            }

            return defaultUrl;
        }
    }
}


