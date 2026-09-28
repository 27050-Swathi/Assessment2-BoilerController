namespace Assessment2_BoilerControllerConsole.Model.Enums
{
    /// <summary>
    /// Represents the status of the boiler.
    /// </summary>
    public enum BoilerStatus
    {
        /// <summary>
        /// Represents the initial state.
        /// </summary>
        Lockout = 1,

        /// <summary>
        /// Represents the state when the switch is closed.
        /// </summary>
        Ready,

        /// <summary>
        /// Represents the pre purge state.
        /// </summary>
        PrePurge,

        /// <summary>
        /// Represents the ignition state.
        /// </summary>
        Ignition,

        /// <summary>
        /// Represents the operational state.
        /// </summary>
        Operational,
    }
}
