using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.ViewModels.Internal;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.ViewModels.Internal;

public class StartDefaultCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartDefaultCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(StartDefaultCommand sut)
    {
        sut.Should().BeAssignableTo<IStartDefaultCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(StartDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}