using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication
{
    public class GuestOsesInUseTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GuestOsesInUse).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(GuestOsesInUse sut)
        {
            sut.Should().BeAssignableTo<IGuestOsesInUse>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(GuestOsesInUse).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}