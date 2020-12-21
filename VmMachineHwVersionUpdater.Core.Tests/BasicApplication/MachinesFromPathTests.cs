using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication
{
    public class MachinesFromPathTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MachinesFromPath).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(MachinesFromPath sut)
        {
            sut.Should().BeAssignableTo<IMachinesFromPath>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MachinesFromPath).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}