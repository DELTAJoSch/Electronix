using Electonix.SharedLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Electonix.SharedLogic.Interfaces
{
    /// <summary>
    /// This interface defines the standard sqlite link layer functions.
    /// </summary>
    public interface ISqliteStorageModel
    {
        /// <summary>
        /// This function should return the values of a specific hardware component.
        /// </summary>
        /// <param name="uid">The uid of the requested component</param>
        /// <returns>The requested hardware component, if available</returns>
        public Task<HardwareComponent> GetHardwareComponent(string uid);

        /// <summary>
        /// This function should delete a specific component if this component exists.
        /// </summary>
        /// <param name="uid">The uid of the requested component</param>
        /// <returns></returns>
        public Task DeleteComponent(string uid);

        /// <summary>
        /// This function should add a specific component to the database.
        /// </summary>
        /// <param name="component">The newly created component</param>
        /// <returns></returns>
        public Task AddComponent(HardwareComponent component);

        /// <summary>
        /// This function should return all saved hardware components.
        /// </summary>
        /// <returns>A Dictionary of all hardware components</returns>
        public Task<Dictionary<string, HardwareComponent>> GetHardwareComponents();

        /// <summary>
        /// This method should update a specific hardware component.
        /// </summary>
        /// <param name="component">The new values of the component</param>
        /// <returns></returns>
        public Task UpdateHardwareComponent(HardwareComponent component);
    }
}