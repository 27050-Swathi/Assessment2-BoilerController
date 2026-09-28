using Assessment2_BoilerControllerConsole.Model.Enums;

namespace Assessment2_BoilerControllerConsole.Model
{
    /// <summary>
    /// Represents the switch class that acts as the start of the application.
    /// </summary>
    public class Switch
    {
        /// <summary>
        /// Initializes a switch class that sets the default values.
        /// </summary>
        public Switch()
        {
            SwitchStatus = SwitchStatus.Open;
        }

        /// <summary>
        /// Gets or sets the status of the switch.
        /// </summary>
        public SwitchStatus SwitchStatus { get; set; }
    }
}
