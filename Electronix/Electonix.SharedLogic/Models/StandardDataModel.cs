using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.LinkLayers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Electonix.SharedLogic.Models
{
    public class StandardDataModel : IDataModel
    {
        private readonly ISqliteStorageModel Model;
        private Dictionary<string, HardwareComponent> Components;

        /// <summary>
        /// This constructor can only be used by internal methods and unit tests
        /// </summary>
        /// <param name="Model"></param>
        internal StandardDataModel(ISqliteStorageModel Model)
        {
            this.Model = Model;
        }

        /// <summary>
        /// The standard constructor for this class.
        /// </summary>
        /// <param name="DatabasePath">The path to the database file</param>
        public StandardDataModel(String DatabasePath)
        {
            Model = new StandardSqliteStorageModel(DatabasePath);
        }

        /// <summary>
        /// This method adds the component to the database
        /// Overwrites the component if a component with the uid already exists.
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns></returns>
        public async Task CreateNewComponent(HardwareComponent component)
        {
            if(Components != null)
            {
                if (!Components.ContainsKey(component.componentUid))
                {
                    Components.Add(component.componentUid, component);
                    await Model.AddComponent(component);
                }
                else
                {
                    Components.Remove(component.componentUid);
                    Components.Add(component.componentUid, component);

                    await Model.UpdateHardwareComponent(component);
                }
            }
            else
            {
                Components = new Dictionary<string, HardwareComponent>
                {
                    { component.componentUid, component }
                };
                await Model.AddComponent(component);
            }
        }

        public async Task DeleteComponent(string uid)
        {
            throw new NotImplementedException();
        }

        public async Task<HardwareComponent> FetchComponent(string uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns all hardware components in the database
        /// </summary>
        /// <returns>A Dictionary of all UIDs and their HardwareComponents</returns>
        public async Task<Dictionary<string, HardwareComponent>> GetHardwareComponents()
        {
            Components = await Model.GetHardwareComponents();
            return Components;
        }

        public async Task<Dictionary<string, HardwareComponent>> SearchFor(string SearchText, DataOptions Options = DataOptions.ComponentName)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, HardwareComponent>> SortBy(DataOptions Options = DataOptions.ComponentName)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComponent(HardwareComponent component)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComponents(List<HardwareComponent> components)
        {
            throw new NotImplementedException();
        }

        
    }
}
