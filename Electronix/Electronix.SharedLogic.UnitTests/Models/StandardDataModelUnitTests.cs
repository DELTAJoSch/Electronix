using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.Models;
using Electronix.SharedLogic.UnitTests.Fakes_Mocks_Stubs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Electronix.SharedLogic.UnitTests.Models
{
    [TestFixture]
    class StandardDataModelUnitTests
    {
        InMemoryDatabaseSubstitute databaseSubstitute;

        [SetUp]
        public async Task SetUp()
        {
            databaseSubstitute = new InMemoryDatabaseSubstitute();
            await databaseSubstitute.AddComponent(new HardwareComponent("#4", 5, 2, 3, "A", "Z"));
        }

        [Test]
        public async Task StandardDataModel_GetComponents_ReturnsComponents()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);
            var components = await dataModel.GetHardwareComponents();

            Assert.AreEqual(2, components.Count);
        }

        [Test]
        public async Task StandardDataModel_AddComponent_AddsComponentToDatabase()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            await dataModel.CreateNewComponent(new HardwareComponent("#1", 10, 5, 100, "B", "empty", 60));

            var components = await dataModel.GetHardwareComponents();
            Assert.AreEqual(3, components.Count);
        }

        [Test]
        public async Task StandardDataModel_AddComponent_AddsComponentToDictionaryWithoutNewRead()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);
            var components = await dataModel.GetHardwareComponents();

            await dataModel.CreateNewComponent(new HardwareComponent("#1", 10, 5, 100, "B", "empty", 60));
            
            Assert.AreEqual(3, components.Count);
        }

        [Test]
        public async Task StandardDataModel_AddComponentAlreadyExists_OverwritesComponent()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);
            var components = await dataModel.GetHardwareComponents();

            await dataModel.CreateNewComponent(new HardwareComponent("#2", 10, 5, 100, "B", "empty", 60));

            components.TryGetValue("#2", out var component);

            Assert.AreEqual("B", component.componentName);
        }

        [Test]
        public async Task StandardDataModel_CreateUID_ReturnsDifferentUids()
        {
            IDataModel dataModel = new StandardDataModel(databaseSubstitute);

            var hashOne = await dataModel.CreateUID();

            await Task.Delay(10);

            var hashTwo = await dataModel.CreateUID();

            Assert.AreNotEqual(hashOne, hashTwo, $"Hash one: {hashOne}, hash two: {hashTwo}\n");
        }

        [TearDown]
        public void TearDown()
        {
            databaseSubstitute.Dispose();
        }
    }
}
