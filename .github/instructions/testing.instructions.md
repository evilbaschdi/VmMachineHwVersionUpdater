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

## Comprehensive Test Coverage Requirements

### Coverage Goals
- **Target: 90%+ line coverage** for all production code
- **Target: 80%+ branch coverage** for all conditional logic
- All public methods MUST have both positive and negative test cases
- All error conditions and exception paths MUST be tested

### Positive Test Cases (Happy Path)
For each public method, include tests for:
- **Valid inputs**: Normal expected values
- **Boundary values**: Min/max valid ranges
- **Edge cases**: Empty collections, zero values, boundary conditions
- **Multiple scenarios**: Different valid input combinations

Example:
```csharp
[Theory]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(1.0, 1073741824.0)]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(0.0, 0.0)]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(0.5, 536870912.0)]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(double.MaxValue, double.MaxValue * 1073741824d)]
public void Method_WithValidInput_ReturnsExpectedResult(double input, double expected)
{
    // Act
    var result = input.Method();
    
    // Assert
    result.Should().BeApproximately(expected, 0.001);
}
```

### Negative Test Cases (Error Conditions)
For each public method, include tests for:
- **Invalid inputs**: Null parameters, negative values where not allowed
- **Out of range**: Values beyond acceptable limits
- **Exception scenarios**: All thrown exceptions with correct parameters
- **Malformed data**: Invalid formats, corrupted input

Example:
```csharp
[Theory]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(-1.0)]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(double.MinValue)]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(double.NaN)]
public void Method_WithInvalidInput_ThrowsArgumentException(double input)
{
    // Act & Assert
    var act = () => input.Method();
    act.Should().Throw<ArgumentOutOfRangeException>()
        .WithParameterName("input");
}

[Fact]
public void Method_WithNullParameter_ThrowsArgumentNullException()
{
    // Act & Assert
    var act = () => sut.Method(null!);
    act.Should().Throw<ArgumentNullException>()
        .WithParameterName("parameter");
}
```

### Special Value Testing
Always test these special values where applicable:
- **Null values**: For reference types
- **Empty collections**: Arrays, lists, dictionaries
- **Zero values**: Numeric types
- **Boundary values**: Min/Max for numeric types
- **Special doubles**: NaN, PositiveInfinity, NegativeInfinity
- **Cultural differences**: Different CultureInfo for formatting/parsing

### Branch Coverage Testing
Ensure every conditional branch is tested:
```csharp
// For if-else statements, test both branches
[Theory]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(true, "expected_when_true")]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(false, "expected_when_false")]
public void Method_WithCondition_ReturnsBranchSpecificResult(bool condition, string expected)

// For switch statements, test all cases including default
[Theory]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(Status.Active, "Active")]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData(Status.Inactive, "Inactive")]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData((Status)999, "Unknown")] // Default case
public void Method_WithAllStatuses_ReturnsCorrectText(Status status, string expected)
```

### Test Organization
Group related tests using regions:
```csharp
#region MethodName Tests

[Theory]
// Positive tests
public void MethodName_WithValidInput_ReturnsExpected() { }

[Theory]  
// Negative tests
public void MethodName_WithInvalidInput_ThrowsException() { }

#endregion
```

## Testing Framework Requirements

- Use xUnit v3 with `[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]` for all tests
- Use FluentAssertions for all assertions: `result.Should().BeOfType<ExpectedType>()`
- Use AutoFixture for parameter injection with `sut` (System Under Test) as the class parameter name
- Test class naming: `{ClassUnderTest}Tests`
- Test method naming: `{MethodUnderTest}_{Scenario}_{ExpectedBehavior}`

## Static Class Testing
For static classes, modify the Constructor_ReturnsInterfaceName test:
```csharp
[Fact]
public void Constructor_ReturnsInterfaceName()
{
    // Static class cannot be instantiated, verify it's static
    typeof(StaticClass).Should().BeStatic();
}
```

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

## Coverage Analysis

Run coverage analysis regularly:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

Prioritize testing for:
1. **Low coverage areas** (under 80% line coverage)
2. **Complex business logic** with multiple branches
3. **Error handling and validation** methods
4. **Public API surfaces** that external code depends on
5. **Utility methods** and extension methods