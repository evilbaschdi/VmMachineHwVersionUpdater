using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.DependencyInjection;
using EvilBaschdi.Testing;
using FluentAssertions;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests
{
    public class ConfigureDelegateForConfigureServicesTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureDelegateForConfigureServices).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ConfigureDelegateForConfigureServices sut)
        {
            sut.Should().BeAssignableTo<IConfigureDelegateForConfigureServices>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureDelegateForConfigureServices).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}