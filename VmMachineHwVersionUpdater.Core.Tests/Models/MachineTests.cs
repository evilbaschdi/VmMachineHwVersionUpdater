using System.ComponentModel;
using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.Models;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class MachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Machine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Machine sut)
    {
        sut.Should().BeAssignableTo<INotifyPropertyChanged>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Machine).GetMethods()
                                        .Where(method => !method.IsAbstract &
                                                         !method.Name.StartsWith("set_") &
                                                         !method.Name.StartsWith("add_") &
                                                         !method.Name.StartsWith("remove_")));
    }
}