using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace VmMachineHwVersionUpdater.Tests
{
    public class MainWindowTests
    {
        [Theory, AutoData]
        public void Constructor_HasNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MainWindow).GetConstructors());
        }

        [Theory, AutoData]
        public void Constructor_ReturnsMetroWindow(MainWindow sut)
        {
            Assert.IsAssignableFrom<MetroWindow>(sut);
        }

        [Theory, AutoData]
        public void Methods_HaveNullGuards(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(MainWindow).GetMethods().Where(method => !method.IsAbstract));
        }
    }
}
