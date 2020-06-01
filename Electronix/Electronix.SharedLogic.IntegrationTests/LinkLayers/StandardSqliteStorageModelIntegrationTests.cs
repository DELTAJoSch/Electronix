using Electonix.SharedLogic.LinkLayers;
using Electonix.SharedLogic.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronix.SharedLogic.IntegrationTests.LinkLayers
{
    [TestFixture]
    public class StandardSqliteStorageModelIntegrationTests
    {
        StandardSqliteStorageModel model;

        [SetUp]
        public void SetupDatabase()
        {
            // copy a pre-filled database and open connection
            string test = Directory.GetCurrentDirectory();
            File.Copy("..//..//..//..//electronix.db", "test.db", true);

            model = new StandardSqliteStorageModel("test.db");
        }

        [Test]
        public async Task StandardSqliteStorageModel_GetHardwareComponents_ReturnsComponents()
        {
            var components = await model.GetHardwareComponents();

            Assert.AreEqual(2, components.Count);
        }

        [Test]
        public async Task StandardSqliteStorageModel_GetHardwareComponent_ReturnsCorrectComponent()
        {
            var component = await model.GetHardwareComponent("#2");

            Assert.AreEqual("#2", component.componentUid);
            Assert.AreEqual("TEST", component.componentName);
            Assert.AreEqual(3, component.componentAmount);
            Assert.AreEqual(4, component.componentRack);
            Assert.AreEqual(2, component.componentDrawer);
            Assert.AreEqual("10/10 would recommend", component.componentNotes);
            Assert.AreEqual(20, component.componentOrderWarning);
        }

        [Test]
        public async Task StandardSqliteStorageModel_DeleteComponent_DeletesCorrectComponent()
        {
            await model.DeleteComponent("#1");

            var components = await model.GetHardwareComponents();

            Assert.AreEqual(1, components.Count);
            Assert.AreEqual("#2", components.ElementAt(0).Key);
        }

        [Test]
        public async Task StandardSqliteStorageModel_AddComponent_AddsCorrectComponent()
        {
            HardwareComponent component = new HardwareComponent("#3", 10, 50, 6, "dudeldu", "super cool", 42);

            await model.AddComponent(component);

            var getResultComponent = await model.GetHardwareComponent("#3");
            Assert.AreEqual(component.componentName, getResultComponent.componentName);
        }

        [Test]
        public async Task StandardSqliteStorageModel_UpdateComponent_UpdatesCorrectComponent()
        {
            HardwareComponent component = new HardwareComponent("#2", 10, 50, 6, "dudeldu", "super cool", 42);

            await model.UpdateHardwareComponent(component);

            var getResultComponent = await model.GetHardwareComponent("#2");
            Assert.AreEqual(component.componentName, getResultComponent.componentName);
        }

        [TearDown]
        public void Cleanup()
        {
            model.Dispose();
        }
    }
}
