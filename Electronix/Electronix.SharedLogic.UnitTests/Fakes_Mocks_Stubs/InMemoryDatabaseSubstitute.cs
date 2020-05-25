using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Electronix.SharedLogic.UnitTests.Fakes_Mocks_Stubs
{
    internal class InMemoryDatabaseSubstitute : ISqliteStorageModel, IDisposable
    {
        private SqliteConnection inMemDataBase;

        public InMemoryDatabaseSubstitute()
        {
            inMemDataBase = new SqliteConnection("Filename =:memory: ");
            inMemDataBase.Open();
            inMemDataBase.BeginTransaction();
            var command = inMemDataBase.CreateCommand();
            command.CommandText = @"CREATE TABLE 'components' (
                                    'ComponentName' TEXT,
	                                'ComponentRack' INTEGER,
	                                'ComponentDrawer'   INTEGER,
	                                'ComponentAmount'   INTEGER,
	                                'ComponentNotes'    TEXT,
	                                'ComponentOrderWarningMinimum'  INTEGER,
	                                'ComponentUID'  TEXT
                                )";
            command.ExecuteNonQuery();

            command = inMemDataBase.CreateCommand();
            command.CommandText = @"INSERT INTO components(
                                        ComponentAmount,
                                        ComponentDrawer,
                                        ComponentName,
                                        ComponentNotes,
                                        ComponentOrderWarningMinimum,
                                        ComponentRack,
                                        ComponentUID
                                        )
                                        VALUES (
                                        3,
                                        2,
                                        'TEST',
                                        '10/10 would recommend',
                                        20,
                                        4,
                                        '#2'
                                    )";
            command.ExecuteNonQuery();
        }

        public async Task AddComponent(HardwareComponent component)
        {
            var command = inMemDataBase.CreateCommand();
            command.CommandText = $@"INSERT INTO components(

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

            command.ExecuteNonQuery();
        }

        public async Task DeleteComponent(string uid)
        {
            var command = inMemDataBase.CreateCommand();
            command.CommandText = $@"DELETE FROM components WHERE ComponentUID='{uid}'";
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            inMemDataBase.Close();
            inMemDataBase.Dispose();
        }

        public async Task<HardwareComponent> GetHardwareComponent(string uid)
        {
            var command = inMemDataBase.CreateCommand();
            command.CommandText = $@"SELECT * FROM components WHERE ComponentUID='{uid}'";
            var reader = command.ExecuteReader();

            HardwareComponent component = null;

            int count = 0;
            while (reader.Read())
            {
                if (count > 0)
                {
                    return null;
                }
                count++;

                component = new HardwareComponent(reader.GetString(6), reader.GetInt32(3), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(0), reader.GetString(4), reader.GetInt32(5));
            }

            return component;
        }

        public async Task<Dictionary<string, HardwareComponent>> GetHardwareComponents()
        {
            var command = inMemDataBase.CreateCommand();
            command.CommandText = @"SELECT * FROM components";
            var reader = command.ExecuteReader();

            Dictionary<string, HardwareComponent> queryResult = new Dictionary<string, HardwareComponent>();

            while (reader.Read())
            {
                HardwareComponent component = new HardwareComponent(reader.GetString(6), reader.GetInt32(3), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(0), reader.GetString(4), reader.GetInt32(5));
                queryResult.Add(component.componentUid, component);
            }

            return queryResult;
        }

        public async Task UpdateHardwareComponent(HardwareComponent component)
        {
            throw new NotImplementedException();
        }
    }
}