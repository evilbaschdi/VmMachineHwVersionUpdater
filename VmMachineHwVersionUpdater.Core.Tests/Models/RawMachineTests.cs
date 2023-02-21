using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.Tests.Models;

public class RawMachineTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RawMachine).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RawMachine).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
    }
}