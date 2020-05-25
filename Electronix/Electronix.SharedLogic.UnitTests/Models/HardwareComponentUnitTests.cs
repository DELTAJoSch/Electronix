using Electonix.SharedLogic.Models;
using NUnit.Framework;

namespace Electronix.SharedLogic.UnitTests.Models
{
    [TestFixture]
    internal class HardwareComponentUnitTests
    {
        [Test]
        public void HardwareComponent_ConstructorArgumentsPassedIn_SavedCorrectly()
        {
            string name = "LM324";
            string uid = "#001";
            int orderWarningLimit = 10;
            int quantity = 3;
            int rack = 50;
            int drawer = 2;
            string notes = "Great component! 10/10";

            HardwareComponent component = new HardwareComponent(componentName: name, componentNotes: notes, componentAmount: quantity, componentDrawer: drawer, componentOrderWarning: orderWarningLimit, componentRack: rack, componentUid: uid);

            Assert.AreEqual(component.componentUid, uid);
            Assert.AreEqual(component.componentRack, rack);
            Assert.AreEqual(component.componentOrderWarning, orderWarningLimit);
            Assert.AreEqual(component.componentNotes, notes);
            Assert.AreEqual(component.componentName, name);
            Assert.AreEqual(component.componentDrawer, drawer);
            Assert.AreEqual(component.componentAmount, quantity);
        }
    }
}