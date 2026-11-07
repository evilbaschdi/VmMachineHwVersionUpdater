using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class LoadTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Load).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(Load sut)
    {
        sut.Should().BeAssignableTo<ILoad>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Load).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Refresh_CreatesFreshSnapshotWithoutReplacingCachedValue(
        [Frozen] IMachinesFromPath machinesFromPath,
        Load sut)
    {
        machinesFromPath.Value.Returns(new ConcurrentBag<Machine>(),
                                       new ConcurrentBag<Machine>());

        var cachedValue = sut.Value;
        var freshValue = sut.Refresh();

        freshValue.Should().NotBeSameAs(cachedValue);
        sut.Value.Should().BeSameAs(cachedValue);

        sut.Value = freshValue;

        sut.Value.Should().BeSameAs(freshValue);
        sut.ResetCache();
        sut.Value.Should().NotBeSameAs(freshValue);
    }
}