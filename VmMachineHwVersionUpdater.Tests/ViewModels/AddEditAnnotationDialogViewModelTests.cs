using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.Testing;
using VmMachineHwVersionUpdater.ViewModels;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests.ViewModels
{
    public class AddEditAnnotationDialogViewModelTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AddEditAnnotationDialogViewModel).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(AddEditAnnotationDialogViewModel sut)
        {
            Assert.IsAssignableFrom<ApplicationStyleViewModel>(sut);
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AddEditAnnotationDialogViewModel).GetMethods().Where(method => !method.IsAbstract & !method.Name.StartsWith("set_")));
        }
    }
}