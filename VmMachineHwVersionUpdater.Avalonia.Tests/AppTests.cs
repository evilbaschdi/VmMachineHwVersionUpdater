using Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class AppTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(App).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(App sut)
    {
        sut.Should().BeAssignableTo<Application>();
    }

    //[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    //public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    //{
    //    assertion.Verify(typeof(App).GetMethods()
    //                                .Where(method => !method.IsAbstract &
    //                                                 !method.IsStatic &
    //                                                 !method.Name.StartsWith("get", StringComparison.OrdinalIgnoreCase) &
    //                                                 !method.Name.StartsWith("set", StringComparison.OrdinalIgnoreCase) &
    //                                                 !method.Name.StartsWith("add", StringComparison.OrdinalIgnoreCase) &
    //                                                 !method.Name.StartsWith("remove", StringComparison.OrdinalIgnoreCase) &
    //                                                 !method.Name.StartsWith("clear", StringComparison.OrdinalIgnoreCase) &
    //                                                 !method.Name.StartsWith("try", StringComparison.OrdinalIgnoreCase)));
    //}
}