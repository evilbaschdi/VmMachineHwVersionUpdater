---
applyTo: "**/*Tests.cs"
---

# Unit Testing Instructions

When working with test files in this repository, follow these strict conventions:

## Mandatory Test Structure

Every test class MUST include these three foundational tests:

```csharp
[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
{
    assertion.Verify(typeof(ClassUnderTest).GetConstructors());
}

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Constructor_ReturnsInterfaceName(ClassUnderTest sut)
{
    sut.Should().BeAssignableTo<IClassUnderTest>();
}

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
{
    assertion.Verify(typeof(ClassUnderTest).GetMethods()
        .Where(method => !method.IsAbstract 
                      && !method.Name.StartsWith("set_") 
                      && !method.Name.StartsWith("add_") 
                      && !method.Name.StartsWith("remove_")));
}
```

## Testing Framework Requirements

- Use xUnit v3 with `[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]` for all tests
- Use FluentAssertions for all assertions: `result.Should().BeOfType<ExpectedType>()`
- Use AutoFixture for parameter injection with `sut` (System Under Test) as the class parameter name
- Test class naming: `{ClassUnderTest}Tests`
- Test method naming: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

## Required Global Usings Available

All test files have access to these global usings from Directory.Build.props:
- AutoFixture.Idioms
- AutoFixture.Xunit3
- EvilBaschdi.Testing
- FluentAssertions
- FluentAssertions.Microsoft.Extensions.DependencyInjection
- NSubstitute
- NSubstitute.ReturnsExtensions
- Xunit