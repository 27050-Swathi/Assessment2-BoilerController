namespace Assessment2_BoilerControllerConsole.Model.Enums
{
    /// <summary>
    /// Represents the menu options.
    /// </summary>
    public enum MenuOptions
    {
        /// <summary>
        /// Menu option to start the boiler.
        /// </summary>
        StartBoiler = 1,

        /// <summary>
        /// Menu option to stop the boiler.
        /// </summary>
        StopBoiler,

        /// <summary>
        /// Menu option to simulate an error.
        /// </summary>
        SimulateBoilerError,

        /// <summary>
        /// Menu option to toggle the switch.
        /// </summary>
        ToggleRun,

        /// <summary>
        /// Menu option to reset lockout.
        /// </summary>
        ResetLockout,

        /// <summary>
        /// Menu option to view the event log.
        /// </summary>
        ViewEventLog,

        /// <summary>
        /// Menu option to exit.
        /// </summary>
        Exit,
    }
}
