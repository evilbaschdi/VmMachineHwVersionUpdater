using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

namespace VmMachineHwVersionUpdater.Core.Tests.BasicApplication;

public class GuestOsesInUseTests
{
    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsesInUse).GetConstructors());
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Constructor_ReturnsInterfaceName(GuestOsesInUse sut)
    {
        sut.Should().BeAssignableTo<IGuestOsesInUse>();
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(GuestOsesInUse).GetMethods().Where(method => !method.IsAbstract));
    }

    [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
    public void Value_ForProvidedMappings_ReturnsOsesSplit(
        [Frozen] IGuestOsStringMapping guestOsStringMapping,
        [Frozen] ILoad load,
        GuestOsesInUse sut,
        IConfigurationRoot dummConfigurationRoot,
        IConfiguration dummyConfiguration
    )
    {
        // Arrange
        var dummyDictionary = new ConcurrentDictionary<string, bool>();
        dummyDictionary.TryAdd("Windows 10 x64", true);
        dummyDictionary.TryAdd("Ubuntu x64", true);

        load.Value.Returns(new LoadHelper
                           {
                               SearchOsItems = dummyDictionary
                           }
        );

        var configurationSections = new List<IConfigurationSection>
                                    {
                                        new ConfigurationSection(dummConfigurationRoot, "1")
                                        {
                                            Value = "Windows 7"
                                        },
                                        new ConfigurationSection(dummConfigurationRoot, "2")
                                        {
                                            Value = "Windows 10 x64"
                                        },
                                        new ConfigurationSection(dummConfigurationRoot, "3")
                                        {
                                            Value = "Windows Server 2022 x64"
                                        },
                                        new ConfigurationSection(dummConfigurationRoot, "4")
                                        {
                                            Value = "Ubuntu x64"
                                        }
                                    };

        dummyConfiguration.GetChildren().Returns(configurationSections);
        guestOsStringMapping.Value.Returns(dummyConfiguration);

        // Act
        var result = sut.Value;

        // Assert
        result.Should().HaveCount(2);
        result.Should().HaveElementAt(0, new("Ubuntu", true));
        result.Should().HaveElementAt(1, new("Windows", true));
    }
}