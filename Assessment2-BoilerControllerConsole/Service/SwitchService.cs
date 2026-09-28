using Assessment2_BoilerControllerConsole.Model.Enums;

namespace Assessment2_BoilerControllerConsole.Service
{
    /// <summary>
    /// Switch service consisting of toggling the switch.
    /// </summary>
    public class SwitchService
    {
        /// <summary>
        /// State of the switch.
        /// </summary>
        public SwitchStatus State { get; private set; } = SwitchStatus.Open;

        /// <summary>
        /// Toggle the switch
        /// </summary>
        public void Toggle()
        {
            if (State == SwitchStatus.Open)
            {
                State = SwitchStatus.Closed;
            }
            else
            {
                State = SwitchStatus.Open;
            }
        }

        /// <summary>
        /// Check is the switch is closed.
        /// </summary>
        /// <returns></returns>
        public bool IsClosed()
        {
            return State == SwitchStatus.Closed;
        }
    }
}
