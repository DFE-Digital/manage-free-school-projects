using Dfe.ManageFreeSchoolProjects.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;
using System;

namespace Dfe.ManageFreeSchoolProjects.Tests.Utils
{
    public class UrlValidatorTests
    {
        private readonly IUrlHelper _urlHelper;

        public UrlValidatorTests()
        {
            _urlHelper = Substitute.For<IUrlHelper>();
        }

        #region XSS Attack Prevention - javascript: Protocol

        [Theory]
        [InlineData("javascript:alert(1)")]
        [InlineData("javascript:alert('XSS')")]
        [InlineData("javascript:void(0)")]
        [InlineData("javascript://")]
        [InlineData("JAVASCRIPT:alert(1)")] // Case insensitive
        [InlineData("JavaScript:alert(1)")] // Mixed case
        public void IsValidReturnUrl_ShouldReject_JavascriptProtocol_ForXSS(string maliciousUrl)
        {
            // Act
            var result = UrlValidator.IsValidReturnUrl(maliciousUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("javascript: protocol should be blocked to prevent XSS attacks");
        }

        #endregion

        #region XSS Attack Prevention - data: Protocol

        [Theory]
        [InlineData("data:text/html,<script>alert(1)</script>")]
        [InlineData("data:text/html;<script>alert('XSS')</script>")]
        [InlineData("data:text/htmlashes,<body onload=alert('XSS')>")]
        [InlineData("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==")]
        public void IsValidReturnUrl_ShouldReject_DataProtocol_ForXSS(string maliciousUrl)
        {
            // Act
            var result = UrlValidator.IsValidReturnUrl(maliciousUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("data: protocol should be blocked to prevent XSS attacks");
        }

        #endregion

        #region XSS Attack Prevention - Other Dangerous Protocols

        [Theory]
        [InlineData("vbscript:alert(1)")]
        [InlineData("file://etc/passwd")]
        [InlineData("about:blank")]
        [InlineData("blob:https://evil.com")]
        public void IsValidReturnUrl_ShouldReject_OtherDangerousProtocols_ForXSS(string maliciousUrl)
        {
            // Act
            var result = UrlValidator.IsValidReturnUrl(maliciousUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("dangerous protocols should be blocked");
        }

        #endregion

        #region XSS Attack Prevention - URL Encoded Attacks

        [Theory]
        [InlineData("javascript%3Aalert(1)")] // URL encoded colon
        [InlineData("data%3atext/html,<script>alert(1)</script>")] // URL encoded colon (lowercase)
        [InlineData("javascript%253Aalert(1)")] // Double encoded
        public void IsValidReturnUrl_ShouldReject_URLEncodedProtocols_ForXSS(string encodedUrl)
        {
            // Act
            var result = UrlValidator.IsValidReturnUrl(encodedUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("URL-encoded dangerous protocols should be blocked");
        }

        #endregion

        #region Open Redirect Prevention - External URLs

        [Fact]
        public void IsValidReturnUrl_ShouldReject_ExternalHttpUrl_ForOpenRedirect()
        {
            // Arrange
            var externalUrl = "http://www.qualys.com";
            _urlHelper.IsLocalUrl(externalUrl).Returns(false);

            // Act
            var result = UrlValidator.IsValidReturnUrl(externalUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("external HTTP URLs should be blocked to prevent open redirect");
            _urlHelper.Received().IsLocalUrl(externalUrl);
        }

        [Fact]
        public void IsValidReturnUrl_ShouldReject_ExternalHttpsUrl_ForOpenRedirect()
        {
            // Arrange
            var externalUrl = "https://www.qualys.com";
            _urlHelper.IsLocalUrl(externalUrl).Returns(false);

            // Act
            var result = UrlValidator.IsValidReturnUrl(externalUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("external HTTPS URLs should be blocked to prevent open redirect");
        }

        [Fact]
        public void IsValidReturnUrl_ShouldReject_ProtocolRelativeUrl_ForOpenRedirect()
        {
            // Arrange
            var protocolRelativeUrl = "//evil.com";
            _urlHelper.IsLocalUrl(protocolRelativeUrl).Returns(false);

            // Act
            var result = UrlValidator.IsValidReturnUrl(protocolRelativeUrl, _urlHelper);

            // Assert
            result.Should().BeFalse("protocol-relative URLs should be blocked");
        }

        #endregion

        #region Valid Local URLs

        [Theory]
        [InlineData("/")]
        [InlineData("/home")]
        [InlineData("/projects/123")]
        [InlineData("/public/cookies")]
        [InlineData("~/home")]
        [InlineData("~/Views/Default/Index.html")]
        [InlineData("/?query=value")]
        [InlineData("/page?id=123")]
        public void IsValidReturnUrl_ShouldAccept_ValidLocalUrls(string localUrl)
        {
            // Arrange
            _urlHelper.IsLocalUrl(localUrl).Returns(true);

            // Act
            var result = UrlValidator.IsValidReturnUrl(localUrl, _urlHelper);

            // Assert
            result.Should().BeTrue($"local URL '{localUrl}' should be accepted");
            _urlHelper.Received().IsLocalUrl(localUrl);
        }

        #endregion

        #region Edge Cases

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsValidReturnUrl_ShouldReject_NullOrWhiteSpace(string url)
        {
            // Act
            var result = UrlValidator.IsValidReturnUrl(url, _urlHelper);

            // Assert
            result.Should().BeFalse("null or whitespace URLs should be rejected");
        }

        #endregion

        #region SanitizeReturnUrl Tests

        [Fact]
        public void SanitizeReturnUrl_ShouldReturn_SafeUrl_WhenValidLocalUrl()
        {
            // Arrange
            var validUrl = "/home";
            var defaultUrl = "/";
            _urlHelper.IsLocalUrl(validUrl).Returns(true);

            // Act
            var result = UrlValidator.SanitizeReturnUrl(validUrl, defaultUrl, _urlHelper);

            // Assert
            result.Should().Be(validUrl, "valid local URLs should be returned as-is");
        }

        [Fact]
        public void SanitizeReturnUrl_ShouldReturn_DefaultUrl_WhenJavascriptProtocol()
        {
            // Arrange
            var maliciousUrl = "javascript:alert(1)";
            var defaultUrl = "/";

            // Act
            var result = UrlValidator.SanitizeReturnUrl(maliciousUrl, defaultUrl, _urlHelper);

            // Assert
            result.Should().Be(defaultUrl, "malicious URLs should return default");
        }

        [Fact]
        public void SanitizeReturnUrl_ShouldReturn_DefaultUrl_WhenExternalUrl()
        {
            // Arrange
            var externalUrl = "https://evil.com";
            var defaultUrl = "/";
            _urlHelper.IsLocalUrl(externalUrl).Returns(false);

            // Act
            var result = UrlValidator.SanitizeReturnUrl(externalUrl, defaultUrl, _urlHelper);

            // Assert
            result.Should().Be(defaultUrl, "external URLs should return default");
        }

        [Fact]
        public void SanitizeReturnUrl_ShouldReturn_DefaultUrl_WhenUrlHelperReturnsFalse()
        {
            // Arrange
            var suspiciousUrl = "../evil";
            var defaultUrl = "/";
            _urlHelper.IsLocalUrl(suspiciousUrl).Returns(false);

            // Act
            var result = UrlValidator.SanitizeReturnUrl(suspiciousUrl, defaultUrl, _urlHelper);

            // Assert
            result.Should().Be(defaultUrl, "non-local URLs should return default");
        }

        #endregion

        #region Integration Test - Qualys Identified Vulnerabilities

        [Fact]
        public void IsValidReturnUrl_ShouldBlock_QualysTestPayload_JavascriptAlert()
        {
            // Arrange - Payload from Qualys scan
            var qualysPayload = "javascript%3Aalert(81)%3Bqxss(X140548042796960Y1_1Z)%3B";
            _urlHelper.IsLocalUrl(Arg.Any<string>()).Returns(false);

            // Act
            var result = UrlValidator.IsValidReturnUrl(qualysPayload, _urlHelper);

            // Assert
            result.Should().BeFalse("Qualys identified javascript: payload should be blocked");
        }

        [Fact]
        public void IsValidReturnUrl_ShouldBlock_QualysTestPayload_ExternalRedirect()
        {
            // Arrange - Payload from Qualys scan
            var qualysPayload = "https://www.qualys.com";
            _urlHelper.IsLocalUrl(qualysPayload).Returns(false);

            // Act
            var result = UrlValidator.IsValidReturnUrl(qualysPayload, _urlHelper);

            // Assert
            result.Should().BeFalse("Qualys identified external redirect should be blocked");
        }

        #endregion
    }
}

