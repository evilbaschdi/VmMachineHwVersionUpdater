using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using VmMachineHwVersionUpdater.Core.Settings;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.Settings;

public class VmPoolsTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmPools).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(VmPools sut)
    {
        Assert.IsAssignableFrom<IVmPools>(sut);
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(VmPools).GetMethods().Where(method => !method.IsAbstract));
    }
}