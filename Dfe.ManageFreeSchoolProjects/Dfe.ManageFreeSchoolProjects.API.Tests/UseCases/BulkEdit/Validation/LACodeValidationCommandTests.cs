﻿using Dfe.ManageFreeSchoolProjects.API.UseCases.BulkEdit.Validations;
using Dfe.ManageFreeSchoolProjects.API.UseCases.LocalAuthority;
using System.Collections.Generic;

namespace Dfe.ManageFreeSchoolProjects.API.Tests.UseCases.BulkEdit.Validation
{
    public class LACodeValidationCommandTests
    {
        public class TestCache(List<LocalAuthorityCacheItem> cache) : ILocalAuthorityCache
        {
            public List<LocalAuthorityCacheItem> GetLocalAuthorities()
            {
                return cache;
            }
        }

        [Fact]
        public void Execute_WhenValidLACode_ReturnsValidResult()
        {

            var localAuthorityCache = new TestCache(new List<LocalAuthorityCacheItem>()
            {
                new LocalAuthorityCacheItem()
                {
                    LACode = "123",
                    Name = "Local Authority 1",
                    GeographicRegion = "Geographic Region 1",
                },
            });

            var command = new LACodeValidationCommand(localAuthorityCache);

            // Act
            var result = command.Execute(new() { Data = null, Value = "123"});

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Execute_WhenInvalidLACode_ReturnsInvalidResult()
        {
            var localAuthorityCache = new TestCache(new List<LocalAuthorityCacheItem>()
            {
                new LocalAuthorityCacheItem()
                {
                    LACode = "123",
                    Name = "Local Authority 1",
                    GeographicRegion = "Geographic Region 1",
                },
            });

            var command = new LACodeValidationCommand(localAuthorityCache);
            
            // Act
            var result = command.Execute(new() { Data = null, Value = "456" });

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Be("Enter an existing local authority");

        }

        [Fact]
        public void Execute_WhenInvalidShortLACode_ReturnsInvalidResult()
        {
            var localAuthorityCache = new TestCache(new List<LocalAuthorityCacheItem>()
            {
                new LocalAuthorityCacheItem()
                {
                    LACode = "123",
                    Name = "Local Authority 1",
                    GeographicRegion = "Geographic Region 1",
                },
            });

            var command = new LACodeValidationCommand(localAuthorityCache);

            // Act
            var result = command.Execute(new() { Data = null, Value = "12" });

            // Assert
            result.IsValid.Should().BeFalse();
            result.ErrorMessage.Should().Be("Local authority must be 3 numbers or more");

        }
    }
}
