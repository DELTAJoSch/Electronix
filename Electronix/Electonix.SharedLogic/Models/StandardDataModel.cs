using Electonix.SharedLogic.Interfaces;
using Electonix.SharedLogic.LinkLayers;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Components = await Model.GetHardwareComponents();
                await CreateNewComponent(component);
            }
        }

        /// <summary>
        /// This method deletes the specified element from the database
        /// </summary>
        /// <param name="uid">The uid of the element that shall be deleted</param>
        /// <returns></returns>
        public async Task DeleteComponent(string uid)
        {
            if(Components != null)
            {
                if (Components.ContainsKey(uid))
                {
                    await Model.DeleteComponent(uid);
                    Components.Remove(uid);
                }
            }
            else
            {
                Components = await Model.GetHardwareComponents();
                await DeleteComponent(uid);
            }
        }

        /// <summary>
        /// This method fetches the specified component from the database
        /// </summary>
        /// <param name="uid">The uid of the requested component</param>
        /// <returns></returns>
        public async Task<HardwareComponent> FetchComponent(string uid)
        {
            if(Components != null)
            {
                Components.TryGetValue(uid, out var component);
                return component;
            }
            else
            {
                Components = await Model.GetHardwareComponents();
                return await FetchComponent(uid);
            }
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

        /// <summary>
        /// This method searches for a specified value in the specified category.
        /// </summary>
        /// <param name="SearchText">The search parameter</param>
        /// <param name="Options">The category to search in</param>
        /// <returns>An IEnumerable containing all valid components</returns>
        public async Task<IEnumerable<KeyValuePair<string, HardwareComponent>>> SearchFor(string SearchText, DataOptions Options = DataOptions.ComponentName)
        {
            if(Components != null)
            {
                try
                {
                    switch (Options)
                    {
                        case DataOptions.ComponentName:
                            return Components.Where(component => component.Value.componentName.Contains(SearchText));
                        case DataOptions.ComponentAmount:
                            return Components.Where(component => component.Value.componentAmount == Convert.ToInt32(SearchText));
                        case DataOptions.ComponentRack:
                            return Components.Where(component => component.Value.componentRack == Convert.ToInt32(SearchText));
                        case DataOptions.ComponentDrawer:
                            return Components.Where(component => component.Value.componentDrawer == Convert.ToInt32(SearchText));
                        case DataOptions.ComponentNotes:
                            return Components.Where(component => component.Value.componentNotes.Contains(SearchText));
                        case DataOptions.ComponentMinimumOrderWarning:
                            return Components.Where(component => component.Value.componentOrderWarning == Convert.ToInt32(SearchText));
                        case DataOptions.ComponentUID:
                            return Components.Where(component => component.Value.componentUid.Contains(SearchText));
                        default:
                            return Components.Where(component => component.Value.componentName.Contains(SearchText));
                    }
                }
                catch (FormatException)
                {
                    return null;
                }
            }
            else
            {
                Components = await GetHardwareComponents();
                return await SearchFor(SearchText, Options);
            }
        }

        /// <summary>
        /// This method sorts the components for the specified type
        /// </summary>
        /// <param name="Options">The sort parameter</param>
        /// <returns>A sorted IEnumerable</returns>
        public async Task<IEnumerable<KeyValuePair<string, HardwareComponent>>> SortBy(DataOptions Options = DataOptions.ComponentName)
        {
            if(Components != null)
            {
                return Options switch
                {
                    DataOptions.ComponentName => Components.OrderBy(component => component.Value.componentName),
                    DataOptions.ComponentAmount => Components.OrderByDescending(component => component.Value.componentAmount),
                    DataOptions.ComponentRack => Components.OrderBy(component => component.Value.componentRack),
                    DataOptions.ComponentDrawer => Components.OrderBy(component => component.Value.componentDrawer),
                    DataOptions.ComponentNotes => Components.OrderBy(component => component.Value.componentNotes),
                    DataOptions.ComponentMinimumOrderWarning => Components.OrderBy(component => component.Value.componentOrderWarning),
                    DataOptions.ComponentUID => Components.OrderBy(component => component.Value.componentUid),
                    _ => Components.OrderBy(component => component.Value.componentName),
                };
            }
            else
            {
                Components = await Model.GetHardwareComponents();
                return await SortBy(Options);
            }
        }

        /// <summary>
        /// This method updates a already existing component or creates it if it doesn't exist
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public async Task UpdateComponent(HardwareComponent component)
        {
            if(Components != null)
            {
                if (Components.ContainsKey(component.componentUid))
                {
                    await Model.UpdateHardwareComponent(component);
                    Components.Remove(component.componentUid);
                    Components.Add(component.componentUid, component);
                }
                else
                {
                    await Model.AddComponent(component);
                    Components.Add(component.componentUid, component);
                }
            }
            else
            {
                Components = await Model.GetHardwareComponents();
                await UpdateComponent(component);
            }
        }

        /// <summary>
        /// This method updates all components contained in the list
        /// </summary>
        /// <param name="components">A list of components</param>
        /// <returns></returns>
        public async Task UpdateComponents(List<HardwareComponent> components)
        {
            if(Components != null)
            {
                foreach(HardwareComponent component in components)
                {
                    await UpdateComponent(component);
                }
            }
            else
            {
                Components = await Model.GetHardwareComponents();
                await UpdateComponents(components);
            }
        }

        
    }
}
