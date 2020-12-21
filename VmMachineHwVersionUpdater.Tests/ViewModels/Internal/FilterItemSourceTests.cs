using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EvilBaschdi.Testing;
using FluentAssertions;
using NSubstitute;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.ViewModels.Internal;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.ViewModels.Internal
{
    public class FilterItemSourceTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(FilterItemSource).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(FilterItemSource sut)
        {
            sut.Should().BeAssignableTo<IFilterItemSource>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(FilterItemSource).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void RunFor_ForSearchOsText_ReturnsMachinesByFilterOnly(
            [Frozen] IConfigureListCollectionView configureListCollectionView,
            FilterItemSource sut,
            Machine machine0,
            Machine machine1
        )
        {
            // Arrange
            machine0.GuestOs = "Windows 10 x64";
            machine1.GuestOs = "Ubuntu x64";
            configureListCollectionView.Value.Returns(new ListCollectionView(new List<Machine> { machine0, machine1 }));

            // Act
            sut.RunFor("Windows", string.Empty);

            // Assert
            configureListCollectionView.Value.Cast<Machine>().Should().HaveCount(1);
            configureListCollectionView.Value.Cast<Machine>().ElementAt(0).GuestOs.Should().Be("Windows 10 x64");
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void RunFor_ForSearchFilterText_ReturnsMachinesByFilterOnly(
            [Frozen] IConfigureListCollectionView configureListCollectionView,
            FilterItemSource sut,
            Machine machine0,
            Machine machine1
        )
        {
            // Arrange
            machine0.DisplayName = "Windows 10 x64";
            machine1.DisplayName = "Ubuntu x64";
            configureListCollectionView.Value.Returns(new ListCollectionView(new List<Machine> { machine0, machine1 }));

            // Act
            sut.RunFor(string.Empty, "x64");

            // Assert
            configureListCollectionView.Value.Cast<Machine>().Should().HaveCount(2);
            configureListCollectionView.Value.Cast<Machine>().ElementAt(0).DisplayName.Should().Be("Windows 10 x64");
            configureListCollectionView.Value.Cast<Machine>().ElementAt(1).DisplayName.Should().Be("Ubuntu x64");
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void RunFor_ForSearchOsTextAndSearchFilterText_ReturnsMachinesByFilterOnly(
            [Frozen] IConfigureListCollectionView configureListCollectionView,
            FilterItemSource sut,
            Machine machine0,
            Machine machine1
        )
        {
            // Arrange
            machine0.DisplayName = "Windows 10 Pro (2004 DevChannel 21277)";
            machine0.GuestOs = "Windows 10 x64";
            machine1.DisplayName = "Windows Server 2019 Essentials (1809 17763, each VS, Azure DevOps)";
            machine1.GuestOs = "Windows Server 2016 or later x64";
            configureListCollectionView.Value.Returns(new ListCollectionView(new List<Machine> { machine0, machine1 }));

            // Act
            sut.RunFor("Windows 10 x64", "dev");

            // Assert
            configureListCollectionView.Value.Cast<Machine>().Should().HaveCount(1);
            configureListCollectionView.Value.Cast<Machine>().ElementAt(0).DisplayName.Should().Be("Windows 10 Pro (2004 DevChannel 21277)");
        }
    }
}