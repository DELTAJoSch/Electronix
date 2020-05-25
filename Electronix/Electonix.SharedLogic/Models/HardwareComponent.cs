namespace Electonix.SharedLogic.Models
{
    /// <summary>
    /// This class is the standardized container for all components.
    /// All properties of a component are stored here.
    /// </summary>
    public class HardwareComponent
    {
        /// <summary>
        /// The component UID is a unique identifier of this specific row (components may have non-unique names)
        /// </summary>
        public readonly string componentUid;

        /// <summary>
        /// This integer contains the amount available at the current time (inventory quantity).
        /// </summary>
        public int componentAmount;

        /// <summary>
        /// This integer contains the id of the rack the component is stored in.
        /// </summary>
        public int componentRack;

        /// <summary>
        /// This integer contains the id of the drawer the component is stored in.
        /// </summary>
        public int componentDrawer;

        /// <summary>
        /// This string contains the display-name of the component.
        /// </summary>
        public string componentName;

        /// <summary>
        /// This integer contains the minimum number of available components before a re-stock warning should be issued.
        /// </summary>
        public int componentOrderWarning;

        /// <summary>
        /// This string contains additional notes. These are left up to the user.
        /// </summary>
        public string componentNotes;

        public HardwareComponent(string componentUid, int componentAmount, int componentRack, int componentDrawer, string componentName, string componentNotes, int componentOrderWarning = 5)
        {
            this.componentUid = componentUid;
            this.componentAmount = componentAmount;
            this.componentDrawer = componentDrawer;
            this.componentName = componentName;
            this.componentNotes = componentNotes;
            this.componentOrderWarning = componentOrderWarning;
            this.componentRack = componentRack;
        }
    }
}