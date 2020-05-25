using Electonix.SharedLogic.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Electronix.SharedLogic.UnitTests.Fakes_Mocks_Stubs.UnitTests
{
    [TestFixture]
    class InMemoryDatabaseUnitTests
    {
        [Test]
        public void InMemoryDataBase_ConstructorCalled_DoesNotCrash()
        {
            Assert.DoesNotThrow(() => { InMemoryDatabaseSubstitute inMemoryDatabase = new InMemoryDatabaseSubstitute(); });
        }

        [Test]
        public async Task InMemoryDataBase_GetHardwareComponents_ReturnsComponents()
        {
            InMemoryDatabaseSubstitute substitute = CreateSubstitute();

            var values = await substitute.GetHardwareComponents();

            foreach(KeyValuePair<string, HardwareComponent> componentPair in values)
            {
                Assert.AreEqual(componentPair.Key, "#2");
                Assert.AreEqual(componentPair.Value.componentAmount, 3);
                Assert.AreEqual(componentPair.Value.componentDrawer, 2);
                Assert.AreEqual(componentPair.Value.componentName, "TEST");
                Assert.AreEqual(componentPair.Value.componentNotes, "10/10 would recommend");
                Assert.AreEqual(componentPair.Value.componentOrderWarning, 20);
                Assert.AreEqual(componentPair.Value.componentRack, 4);
                Assert.AreEqual(componentPair.Value.componentUid, "#2");
            }
        }

        [Test]
        public async Task InMemoryDataBase_GetHardwareComponent_ReturnsCorrectComponent()
        {
            InMemoryDatabaseSubstitute substitute = CreateSubstitute();

            var value = await substitute.GetHardwareComponent("#2");

            Assert.AreEqual(value.componentUid, "#2");
            Assert.AreEqual(value.componentAmount, 3);
            Assert.AreEqual(value.componentDrawer, 2);
            Assert.AreEqual(value.componentName, "TEST");
            Assert.AreEqual(value.componentNotes, "10/10 would recommend");
            Assert.AreEqual(value.componentOrderWarning, 20);
            Assert.AreEqual(value.componentRack, 4);
        }

        [Test]
        public async Task InMemoryDataBase_AddHardwareComponent_AddsCorrectComponent()
        {
            InMemoryDatabaseSubstitute substitute = CreateSubstitute();

            await substitute.AddComponent(new HardwareComponent("#3", 50, 10, 200, "Core i9-9900K", "An overpriced processor", 1));

            var value = await substitute.GetHardwareComponent("#3");

            Assert.AreEqual(value.componentUid, "#3");
            Assert.AreEqual(value.componentAmount, 50);
            Assert.AreEqual(value.componentDrawer, 200);
            Assert.AreEqual(value.componentName, "Core i9-9900K");
            Assert.AreEqual(value.componentNotes, "An overpriced processor");
            Assert.AreEqual(value.componentOrderWarning, 1);
            Assert.AreEqual(value.componentRack, 10);
        }

        [Test]
        public async Task InMemoryDataBase_Delete_DeletesCorrectRecord()
        {
            InMemoryDatabaseSubstitute substitute = CreateSubstitute();

            await substitute.AddComponent(new HardwareComponent("#3", 50, 10, 200, "Core i9-9900K", "An overpriced processor", 1));

            await substitute.DeleteComponent("#2");

            var value = await substitute.GetHardwareComponent("#3");
            var deletedValue = await substitute.GetHardwareComponent("#2");

            Assert.AreEqual(value.componentUid, "#3");
            Assert.AreEqual(value.componentAmount, 50);
            Assert.AreEqual(value.componentDrawer, 200);
            Assert.AreEqual(value.componentName, "Core i9-9900K");
            Assert.AreEqual(value.componentNotes, "An overpriced processor");
            Assert.AreEqual(value.componentOrderWarning, 1);
            Assert.AreEqual(value.componentRack, 10);

            Assert.IsNull(deletedValue);
        }

        private InMemoryDatabaseSubstitute CreateSubstitute()
        {
            return new InMemoryDatabaseSubstitute();
        }
    }
}
