using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.ViewModels.Internal;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.ViewModels.Internal
{
    public class OpenWithCodeDefaultCommandTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(OpenWithCodeDefaultCommand).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(OpenWithCodeDefaultCommand sut)
        {
            sut.Should().BeAssignableTo<IOpenWithCodeDefaultCommand>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(OpenWithCodeDefaultCommand).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}