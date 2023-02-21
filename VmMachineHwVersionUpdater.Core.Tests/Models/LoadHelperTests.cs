using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class LoadHelperTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadHelper).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(LoadHelper).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}