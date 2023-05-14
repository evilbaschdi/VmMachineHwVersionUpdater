using ReactiveUI;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Tests.ViewModels;

public class ViewModelBaseTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ViewModelBase).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ViewModelBase sut)
    {
        sut.Should().BeAssignableTo<ReactiveObject>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ViewModelBase).GetMethods().Where(method => !method.IsAbstract));
    }
}