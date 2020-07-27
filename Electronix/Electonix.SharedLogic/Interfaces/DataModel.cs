using Electonix.SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Electonix.SharedLogic.Interfaces
{
    public enum DataOptions
    {
        ComponentName,
        ComponentAmount,
        ComponentRack,
        ComponentDrawer,
        ComponentNotes,
        ComponentMinimumOrderWarning,
        ComponentUID
    }

    /// <summary>
    /// This interface specifies the standard model used to interact with a database and its safed contents.
    /// </summary>
    public interface IDataModel
    {
        /// <summary>
        /// This method creates a new component with the specified details.
        /// </summary>
        /// <param name="component">The component that should be added</param>
        /// <returns></returns>
        public Task CreateNewComponent(HardwareComponent component);

        /// <summary>
        /// This method updates a specific component.
        /// </summary>
        /// <param name="component">The component with the updated values</param>
        /// <returns></returns>
        public Task UpdateComponent(HardwareComponent component);

        /// <summary>
        /// This method deletes a specific component based on tis uid.
        /// </summary>
        /// <param name="uid">The uid of the component</param>
        /// <returns></returns>
        public Task DeleteComponent(string uid);

        /// <summary>
        /// This method fetches the data associated to a specific component.
        /// </summary>
        /// <param name="uid">The uid of the requested component</param>
        /// <returns>The component associated with the uid</returns>
        public Task<HardwareComponent> FetchComponent(string uid);

        /// <summary>
        /// This method should return a dictionary of all available components.
        /// </summary>
        /// <returns>A dictionary of all components and their uids</returns>
        public Task<Dictionary<string, HardwareComponent>> GetHardwareComponents();

        /// <summary>
        /// This method updates all the components passed
        /// </summary>
        /// <param name="components">The components to be updated</param>
        /// <returns></returns>
        public Task UpdateComponents(List<HardwareComponent> components);

        /// <summary>
        /// This method returns a dictionary with HardwareComponents that match the search
        /// </summary>
        /// <param name="SearchText">The search text</param>
        /// <param name="Options">the parameter that it will be searched against</param>
        /// <returns></returns>
        public Task<IEnumerable<KeyValuePair<string, HardwareComponent>>> SearchFor(string SearchText, DataOptions Options = DataOptions.ComponentName);

        /// <summary>
        /// This method sorts the hardware components
        /// </summary>
        /// <param name="Options"></param>
        /// <returns></returns>
        public Task<IEnumerable<KeyValuePair<string, HardwareComponent>>> SortBy(DataOptions Options = DataOptions.ComponentName);

        /// <summary>
        /// This method returns a new uid.
        /// This method has a standard implementation.
        /// </summary>
        /// <returns></returns>
        async Task<String> CreateUID()
        {
            using SHA1Managed sha1 = new SHA1Managed();

            var now = DateTime.Now;

            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{now.Year}{now.Millisecond}{now.Second}{now.DayOfYear}{now.Minute}{now.Hour}"));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
