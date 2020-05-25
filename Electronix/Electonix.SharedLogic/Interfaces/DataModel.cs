using Electonix.SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Electonix.SharedLogic.Interfaces
{
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
    }
}
