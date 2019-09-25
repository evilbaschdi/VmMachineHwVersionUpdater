using NSubstitute;
using System;
using Xunit;
using System.Linq;
using AutoFixture.Idioms;
using FluentAssertions;
using AutoFixture.Xunit2;

namespace VmMachineHwVersionUpdater.Core.Tests
{
    public class GuestOsOutputStringMappingTests
    {
        [Theory, AutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GuestOsOutputStringMapping).GetConstructors());
        }

        [Theory, AutoData]
        public void Constructor_ReturnsInterfaceName(GuestOsOutputStringMapping sut)
        {
            sut.Should().BeAssignableTo<IGuestOsOutputStringMapping>();
        }

        [Theory, AutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GuestOsOutputStringMapping).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory]
        [AutoData]
        public void ValueFor_Windows9srv64_ReturnsWindowsServer2016OrLaterX64(            
            GuestOsOutputStringMapping sut)
        {
            // Arrange

            // Act
            var result = sut.ValueFor("windows9srv-64");

            // Assert

            result.Should().Be("Windows Server 2016 or later x64");
        }

        [Theory]
        [AutoData]
        public void ValueFor_Windows964_ReturnsWindows10X64(
            GuestOsOutputStringMapping sut)
        {
            // Arrange

            // Act
            var result = sut.ValueFor("windows9-64");

            // Assert

            result.Should().Be("Windows 10 x64");
        }
    }
}