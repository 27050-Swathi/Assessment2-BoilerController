using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Model.Enums;
using Assessment2_BoilerControllerConsole.Persistence;

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
        private readonly LoggerRepository _loggerRepository;
        private readonly List<Notification> _notifications = new List<Notification>();
        public event EventHandler<List<Notification>>? NotificationRaised;
        private readonly object _notificationLock = new object();

        public SwitchService(LoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }

        /// <summary>
        /// Toggle the switch
        /// </summary>
        public void Toggle()
        {
            if (State == SwitchStatus.Open)
            {
                State = SwitchStatus.Closed;
                AddNotification($"Interlock Switch toggled to {State}.");
            }
            else
            {
                State = SwitchStatus.Open;
                AddNotification($"Interlock Switch toggled to {State}.");
            }
        }

        /// <summary>
        /// Check if the switch is closed.
        /// </summary>
        /// <returns><true>If the switch is closed.</true></returns>
        public bool IsClosed()
        {
            return State == SwitchStatus.Closed;
        }

        /// <summary>
        /// Adds notification and logging of the operations.
        /// </summary>
        /// <param name="message">Notification Message.</param>
        public void AddNotification(string message)
        {
            Notification notification = new Notification(message);
            List<Notification> snapshot;
            lock (_notificationLock)
            {
                _notifications.Add(notification);
                snapshot = new List<Notification>(_notifications);
            }
            _loggerRepository.SaveData(new Logger("Event Raised:", message));
            NotificationRaised?.Invoke(this, snapshot);
        }
    }
}
