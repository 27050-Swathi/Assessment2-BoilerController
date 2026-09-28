namespace Assessment2_BoilerControllerConsole.Model
{
    /// <summary>
    /// Logger class that helps in logging the events into a CSV file.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Initializes a logger class.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventData">Details of the event.</param>
        public Logger(string eventName, string eventData)
        {
            TimeStamp = DateTime.Now;
            EventName = eventName;
            EventData = eventData;
        }

        /// <summary>
        /// Gets or sets the time of occurrence of the event.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the data of the event.
        /// </summary>
        public string EventData { get; set; }
    }
}
