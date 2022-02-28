using System.Linq;
using System.Windows.Data;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Resources;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.Resources;

public class GuestOsFormatterTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsFormatter).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GuestOsFormatter sut)
    {
        sut.Should().BeAssignableTo<IValueConverter>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsFormatter).GetMethods().Where(method => !method.IsAbstract));
    }
}