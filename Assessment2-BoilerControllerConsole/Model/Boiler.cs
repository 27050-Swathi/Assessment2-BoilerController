using Assessment2_BoilerControllerConsole.Model.Enums;

namespace Assessment2_BoilerControllerConsole.Model
{
    /// <summary>
    /// Boiler class consisting of a boiler that helps to run the simulation.
    /// </summary>
    public class Boiler
    {
        /// <summary>
        /// Initializes a boiler class and sets the default values.
        /// </summary>
        public Boiler()
        {
            Status = BoilerStatus.Lockout;
        }

        /// <summary>
        /// Gets or sets the status of the boiler.
        /// </summary>
        BoilerStatus Status { get; set; }
    }
}
