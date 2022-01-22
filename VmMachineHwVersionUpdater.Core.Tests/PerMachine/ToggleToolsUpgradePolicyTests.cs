using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.PerMachine;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class ToggleToolsUpgradePolicyTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsUpgradePolicy).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ToggleToolsUpgradePolicy sut)
    {
        sut.Should().BeAssignableTo<IToggleToolsUpgradePolicy>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ToggleToolsUpgradePolicy).GetMethods().Where(method => !method.IsAbstract));
    }
}