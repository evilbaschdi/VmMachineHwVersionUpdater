using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Core.Tests.Commands;

public class OpenWithCodeCommandTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeCommand).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(OpenWithCodeCommand sut)
    {
        sut.Should().BeAssignableTo<IOpenWithCodeCommand>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(OpenWithCodeCommand).GetMethods().Where(method => !method.IsAbstract));
    }
}