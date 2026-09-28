namespace Assessment2_BoilerControllerConsole.Model
{
    /// <summary>
    /// Notification class to store the notifications.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a notification class.
        /// </summary>
        /// <param name="message"></param>
        public Notification(string message)
        {
            Message = message;
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the TimeStamp.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
