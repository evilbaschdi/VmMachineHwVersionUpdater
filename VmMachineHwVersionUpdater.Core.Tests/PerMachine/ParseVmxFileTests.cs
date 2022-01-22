using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.PerMachine;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine
{
    public class ParseVmxFileTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ParseVmxFile).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ParseVmxFile sut)
        {
            sut.Should().BeAssignableTo<IParseVmxFile>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ParseVmxFile).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}