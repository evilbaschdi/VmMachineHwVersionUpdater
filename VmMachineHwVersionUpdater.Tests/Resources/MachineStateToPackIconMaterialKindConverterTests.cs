using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Core.Enums;
using VmMachineHwVersionUpdater.Resources;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.Resources
{
    public class MachineStateToPackIconMaterialKindConverterTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MachineStateToPackIconMaterialKindConverter).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(MachineStateToPackIconMaterialKindConverter sut)
        {
            sut.Should().BeAssignableTo<IValueConverter>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MachineStateToPackIconMaterialKindConverter).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Convert_MachineStateOff_ReturnsPackIconMaterialKindPower(
            MachineStateToPackIconMaterialKindConverter sut
        )
        {
            // Arrange

            // Act
            var result = sut.Convert(MachineState.Off, typeof(PackIconMaterialKind), "none", CultureInfo.GetCultureInfo(1033));

            // Assert
            result.Should().Be(PackIconMaterialKind.Power);
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Convert_MachineStatePaused_ReturnsPackIconMaterialKindPower(
            MachineStateToPackIconMaterialKindConverter sut
        )
        {
            // Arrange

            // Act
            var result = sut.Convert(MachineState.Paused, typeof(PackIconMaterialKind), "none", CultureInfo.GetCultureInfo(1033));

            // Assert
            result.Should().Be(PackIconMaterialKind.Pause);
        }
    }
}