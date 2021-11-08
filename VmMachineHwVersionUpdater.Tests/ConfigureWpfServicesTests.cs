﻿using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.Testing;
using FluentAssertions;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels.Internal;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests
{
    public class ConfigureWpfServicesTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureWpfServices).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ConfigureWpfServices sut)
        {
            sut.Should().BeAssignableTo<IConfigureWpfServices>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConfigureWpfServices).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void RunFor_ForProvidedServiceCollection_ReturnsServiceCollectionWithInstances(
            ConfigureWpfServices sut
        )
        {
            // Arrange
            IServiceCollection dummyServiceCollection = new ServiceCollection();

            // Act
            sut.RunFor(dummyServiceCollection);

            // Assert
            dummyServiceCollection.Should().HaveCount(10);
            dummyServiceCollection.Should().HaveService<IConfigureListCollectionView>().WithImplementation<ConfigureListCollectionView>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ICopyDirectoryWithFilesWithProgress>().WithImplementation<CopyDirectoryWithFilesWithProgress>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ICopyDirectoryWithProgress>().WithImplementation<CopyDirectoryWithProgress>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ICopyProgress>().WithImplementation<CopyProgress>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ICurrentItemSource>().WithImplementation<CurrentItemSource>().AsSingleton();
            dummyServiceCollection.Should().HaveService<IFilterItemSource>().WithImplementation<FilterItemSource>().AsSingleton();
            dummyServiceCollection.Should().HaveService<IInitDefaultCommands>().WithImplementation<InitDefaultCommands>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ILoadSearchOsItems>().WithImplementation<LoadSearchOsItems>().AsSingleton();
            dummyServiceCollection.Should().HaveService<IProcessByPath>().WithImplementation<ProcessByPath>().AsSingleton();
            dummyServiceCollection.Should().HaveService<ITaskbarItemProgressState>().WithImplementation<CurrentTaskbarItemProgressState>().AsSingleton();
        }
    }
}