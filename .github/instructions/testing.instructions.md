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
    assertion.Verify(typeof(ClassUnderTest).GetMethods().Where(method => !method.IsAbstract));
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

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Method_WithNullParameter_ThrowsArgumentNullException(ClassUnderTest sut)
{
    // Act & Assert
    sut.Invoking(x => x.Method(null!))
        .Should().Throw<ArgumentNullException>()
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

## Mocking with [Frozen] and NSubstitute

Use `[Frozen]` to freeze a mock dependency so the same instance is injected into the SUT by AutoFixture. This allows verifying interactions on that dependency.

### Setting Up Mock Return Values
```csharp
[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Parse_WithVmxExtension_CallsParseVmxFile(
    [Frozen] IParseVmxFile parseVmxFile,
    MachineParserStrategy sut,
    string fileName,
    RawMachine expectedResult)
{
    // Arrange
    var filePath = $"{fileName}.vmx";
    parseVmxFile.ValueFor(filePath).Returns(expectedResult);

    // Act
    var result = sut.Parse(filePath);

    // Assert
    result.Should().Be(expectedResult);
    parseVmxFile.Received(1).ValueFor(filePath);
}
```

### Verifying Method Calls
```csharp
[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Start_WhenCalled_StartsFileWatcher(
    [Frozen] IVmFileWatcher vmFileWatcher,
    VmFileChangeHandler sut)
{
    // Act
    sut.Start();

    // Assert
    vmFileWatcher.Received(1).Start();
}
```

Key rules:
- `[Frozen]` parameters MUST appear before `sut` in the parameter list
- AutoFixture-generated parameters (e.g. `string fileName`, `RawMachine expectedResult`) appear after `sut`
- Use `.Returns()` to configure mock return values
- Use `.ReturnsNull()` for null returns (from `NSubstitute.ReturnsExtensions`)
- Use `.Received(1)` to verify a method was called exactly once
- Use `.DidNotReceive()` to verify a method was not called

### CancellationToken in Assertions
When you explicitly pass a `CancellationToken` in the Arrange/Act block, use `TestContext.Current.CancellationToken` to verify the same token was forwarded:

```csharp
// When you PASS a CancellationToken to the method under test:
var cancellationToken = TestContext.Current.CancellationToken;
await sut.RunForAsync(machine, "newDir", cancellationToken);
await dependency.Received(1)
    .RunForAsync(Arg.Any<string>(), Arg.Any<string>(), cancellationToken);

// When you do NOT pass a CancellationToken (uses default):
await sut.RunForAsync(machine, "newDir");
await dependency.DidNotReceive()
    .RunForAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
```

## Methods_HaveNullGuards Filter Variants

For simple classes (no property setters or events):
```csharp
assertion.Verify(typeof(ClassUnderTest).GetMethods().Where(method => !method.IsAbstract));
```

For models/POCOs with property setters:
```csharp
assertion.Verify(typeof(ClassUnderTest).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
```

For ViewModels and classes with property setters/events (Avalonia/ReactiveUI):
```csharp
assertion.Verify(typeof(ClassUnderTest).GetMethods().Where(method => !method.IsAbstract
                                                                      & !method.Name.StartsWith("set_")
                                                                      & !method.Name.StartsWith("add_")
                                                                      & !method.Name.StartsWith("remove_")));
```

> **Note**: Use bitwise `&` (not `&&`) for these filters — this is the established convention.

## Constructor_ReturnsInterfaceName Variants

For classes implementing an interface:
```csharp
sut.Should().BeAssignableTo<IClassUnderTest>();
```

For classes inheriting a base class (e.g. ViewModels):
```csharp
sut.Should().BeAssignableTo<ReactiveObject>();
```

For model/POCO classes without an interface:
```csharp
sut.Should().NotBeNull();
sut.Should().BeOfType<VmFileChangedEventArgs>();
```

## Static Class Testing
For static classes, modify the Constructor_ReturnsInterfaceName test:
```csharp
[Fact]
public void Constructor_ReturnsInterfaceName()
{
    // Static class cannot be instantiated, verify it's static
    typeof(StaticClass).Should().NotBeNull();
}
```

## Dependency Injection Tests

For testing DI service registration (static extension method classes), use the fluent `ServiceCollection` assertions from `EvilBaschdi.Testing` (available via global using `FluentAssertions.Microsoft.Extensions.DependencyInjection`).

DI tests MUST use `[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]` with `IServiceCollection` injected by AutoFixture:

```csharp
[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(IServiceCollection serviceCollection)
{
    // Act
    serviceCollection.AddCoreServices();

    // Assert
    serviceCollection.Should().HaveCount(39);
    serviceCollection.Should().HaveService<IGoToCommand>()
                              .WithImplementation<GoToCommand>()
                              .AsSingleton();
    serviceCollection.Should().HaveService<IReloadCommand>()
                              .WithImplementation<ReloadCommand>()
                              .AsSingleton();
}
```

### Available Fluent DI Assertions
- `.HaveCount(int)` — Assert total number of registrations
- `.HaveService<TService>()` — Assert a service type is registered
- `.WithImplementation<TImpl>()` — Assert specific implementation type
- `.WithFactory()` — Assert registration uses a factory function
- `.WithCount(int)` — Assert a service is registered N times
- `.AsSingleton()` / `.AsScoped()` / `.AsTransient()` — Assert lifetime
- `.And()` — Chain multiple assertions

> **Note**: Do NOT use `BuildServiceProvider()` + `GetService<>()` to verify registrations. Use the fluent `services.Should().HaveService<>()` pattern instead.

## When to use `[Fact]`

Use `[Fact]` ONLY for tests that do not instantiate anything — typically tests on static classes or static methods where AutoFixture cannot inject anything useful:

```csharp
[Fact]
public void Constructor_ReturnsInterfaceName()
{
    // Static class cannot be instantiated, verify it's static
    typeof(StaticClass).Should().NotBeNull();
}

[Fact]
public void AddCommandServices_WithNullServices_ThrowsArgumentNullException()
{
    // Act & Assert
    ((Action)(() => ConfigureCommandServices.AddCommandServices(null!)))
        .Should().Throw<ArgumentNullException>();
}
```

All other tests MUST use `[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]`.

## IDisposable Testing

```csharp
[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Dispose_WhenCalled_CanBeCalledMultipleTimes(ClassUnderTest sut)
{
    // Act
    var act = () =>
    {
        sut.Dispose();
        sut.Dispose();
    };

    // Assert
    act.Should().NotThrow();
}

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void Dispose_WhenCalled_ImplementsIDisposable(ClassUnderTest sut)
{
    // Assert
    sut.Should().BeAssignableTo<IDisposable>();
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