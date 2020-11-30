using System.Linq;
using AutoFixture.Idioms;
using EvilBaschdi.Testing;
using FluentAssertions;
using VmMachineHwVersionUpdater.Core.PerMachine;
using Xunit;

namespace VmMachineHwVersionUpdater.Core.Tests.PerMachine
{
    public class ConvertAnnotationLineBreaksTests
    {
        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConvertAnnotationLineBreaks).GetConstructors());
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Constructor_ReturnsInterfaceName(ConvertAnnotationLineBreaks sut)
        {
            sut.Should().BeAssignableTo<IConvertAnnotationLineBreaks>();
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConvertAnnotationLineBreaks).GetMethods().Where(method => !method.IsAbstract));
        }

        [Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
        public void Value_ForStringFromVmx_ReturnsCorrectString(
            ConvertAnnotationLineBreaks sut)
        {
            // Arrange
            const string vmxAnnotationValue = "Windows Insider|0D|0ADev Channel|0D|0A|0D|0ABuild 20180";
            const string expectedString = "Windows Insider\r\nDev Channel\r\n\r\nBuild 20180";

            // Act
            var result = sut.ValueFor(vmxAnnotationValue);

            // Assert
            result.Should().Be(expectedString);
        }
    }
}