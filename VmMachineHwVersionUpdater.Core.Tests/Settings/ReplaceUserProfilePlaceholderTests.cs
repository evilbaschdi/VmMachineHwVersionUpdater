namespace VmMachineHwVersionUpdater.Core.Tests.Settings;

public class ReplaceUserProfilePlaceholderTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReplaceUserProfilePlaceholder).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(ReplaceUserProfilePlaceholder sut)
    {
        sut.Should().BeAssignableTo<IReplaceUserProfilePlaceholder>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ReplaceUserProfilePlaceholder).GetMethods().Where(method => !method.IsAbstract));
    }
}