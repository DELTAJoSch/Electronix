using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Electonix.SharedLogic.LinkLayers
{
    /// <summary>
    /// This class is the standard implementation of the SqliteStorageModel
    /// </summary>
    internal class StandardSqliteStorageModel : ISqliteStorageModel, IDisposable
    {
        private readonly SqliteConnection dbConnection;

        internal StandardSqliteStorageModel(string DatabaseFilePath)
        {
            var absolutePath = Path.GetFullPath(DatabaseFilePath);
            dbConnection = new SqliteConnection($"Data Source = {absolutePath};");
            dbConnection.Open();
            dbConnection.BeginTransaction();
        }

        /// <summary>
        /// This method adds a hardware component to the database
        /// </summary>
        /// <param name="component">The component to be added</param>
        /// <returns></returns>
        public async Task AddComponent(HardwareComponent component)
        {
            var command = dbConnection.CreateCommand();
            command.CommandText =
            $@"INSERT INTO components(
                ComponentAmount,
                ComponentDrawer,
                ComponentName,
                ComponentNotes,
                ComponentOrderWarningMinimum,
                ComponentRack,
                ComponentUID
            )
            VALUES(
                {component.componentAmount},
                {component.componentDrawer},
                '{component.componentName}',
                '{component.componentNotes}',
                {component.componentOrderWarning},
                {component.componentRack},
                '{component.componentUid}'
            )";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// This method deletes a component from the database based on its uid
        /// </summary>
        /// <param name="uid">The uid of the component</param>
        /// <returns></returns>
        public async Task DeleteComponent(string uid)
        {
            var command = dbConnection.CreateCommand();

            command.CommandText =
            $@"DELETE FROM components WHERE ComponentUID='{uid}'";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// This method returns only the requested hardware component.
        /// It fetches the components based on its uid.
        /// </summary>
        /// <param name="uid">The uid of the component</param>
        /// <returns>The requested HardwareComponent</returns>
        public async Task<HardwareComponent> GetHardwareComponent(string uid)
        {
            var command = dbConnection.CreateCommand();
            command.CommandText =
            $@"SELECT * FROM components WHERE ComponentUID='{uid}'";

            var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            var component = new HardwareComponent(reader.GetString(6), reader.GetInt32(3), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(0), reader.GetString(4), reader.GetInt32(5));

            return component;
        }

        /// <summary>
        /// This method returns a dictionary of type string, HardwareComponent.
        /// It reads all available components and returns them.
        /// </summary>
        /// <returns>Dictionary<string, HardwareComponent></returns>
        public async Task<Dictionary<string, HardwareComponent>> GetHardwareComponents()
        {
            var command = dbConnection.CreateCommand();
            command.CommandText =
            $@"SELECT * FROM components";

            var reader = await command.ExecuteReaderAsync();

            Dictionary<string, HardwareComponent> components = new Dictionary<string, HardwareComponent>();

            while(await reader.ReadAsync())
            {
                HardwareComponent component = new HardwareComponent(reader.GetString(6), reader.GetInt32(3), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(0), reader.GetString(4), reader.GetInt32(5));
                components.Add(component.componentUid, component);
            }

            return components;
        }

        /// <summary>
        /// This method updates a specified component
        /// </summary>
        /// <param name="component">The new component information</param>
        /// <returns></returns>
        public async Task UpdateHardwareComponent(HardwareComponent component)
        {
            var command = dbConnection.CreateCommand();
            command.CommandText =
            $@"UPDATE components
               SET ComponentName = '{component.componentName}',
                   ComponentRack = {component.componentRack},
                   ComponentDrawer = {component.componentDrawer},
                   ComponentAmount = {component.componentAmount},
                   ComponentNotes = '{component.componentNotes}',
                   ComponentOrderWarningMinimum = {component.componentOrderWarning}
               WHERE ComponentUID = '{component.componentUid}';
            ";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Disposes of the StandardSqliteStorageModel
        /// </summary>
        public void Dispose()
        {
            dbConnection.Close();
        }
    }
}