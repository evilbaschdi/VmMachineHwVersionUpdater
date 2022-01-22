using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.PerMachine;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine;

public class SetDisplayNameTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetDisplayName).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(SetDisplayName sut)
    {
        sut.Should().BeAssignableTo<ISetDisplayName>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SetDisplayName).GetMethods().Where(method => !method.IsAbstract));
    }
}