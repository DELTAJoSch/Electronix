using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.Models;
using Electronix.SharedLogic.UnitTests.Fakes_Mocks_Stubs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public async Task StandardDataModel_DeleteComponent_DeletesComponent()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);
            await dataModel.CreateNewComponent(new HardwareComponent("#1", 10, 5, 100, "B", "empty", 60));
            await dataModel.DeleteComponent("#1");

            var components = await dataModel.GetHardwareComponents();

            Assert.AreEqual(2, components.Count);
        }

        [Test]
        public async Task StandardDataModel_FetchComponent_ReturnsComponent()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var component = await dataModel.FetchComponent("#4");

            Assert.AreEqual("A", component.componentName);
        }

        [Test]
        public async Task StandardDataModel_UpdateComponent_UpdatesComponent()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var component = await dataModel.FetchComponent("#4");
            component.componentName = "Franz";

            await dataModel.UpdateComponent(component);

            component = await dataModel.FetchComponent("#4");

            Assert.AreEqual("Franz", component.componentName);
        }

        [Test]
        public async Task StandardDataModel_UpdateComponents_UpdatesComponents()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var component = await dataModel.FetchComponent("#4");
            component.componentName = "Franz";

            var componentTwo = await dataModel.FetchComponent("#2");
            componentTwo.componentName = "Otto-Hans";

            await dataModel.UpdateComponents(new List<HardwareComponent>() { component, componentTwo});

            component = await dataModel.FetchComponent("#4");
            componentTwo = await dataModel.FetchComponent("#2");

            Assert.AreEqual("Franz", component.componentName);
            Assert.AreEqual("Otto-Hans", componentTwo.componentName);
        }

        [Test]
        public async Task StandardDataModel_SortByAmount_SortsComponents()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var components = await dataModel.SortBy(DataOptions.ComponentAmount);

            Assert.AreEqual(5, components.ElementAt(0).Value.componentAmount);
        }

        [Test]
        public async Task StandardDataModel_SortByName_SortsComponents()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var components = await dataModel.SortBy(DataOptions.ComponentName);

            Assert.AreEqual("A", components.ElementAt(0).Value.componentName);
        }

        [Test]
        public async Task StandardDataModel_SearchForName_SearchesComponents()
        {
            var dataModel = new StandardDataModel(databaseSubstitute);

            var components = await dataModel.SearchFor("TE", DataOptions.ComponentName);

            Assert.AreEqual("TEST", components.ElementAt(0).Value.componentName);
        }

        [TearDown]
        public void TearDown()
        {
            databaseSubstitute.Dispose();
        }
    }
}
