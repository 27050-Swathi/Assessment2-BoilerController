using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Model.Enums;
using Assessment2_BoilerControllerConsole.Persistence;
using Assessment2_BoilerControllerConsole.Service;
using Assessment2_BoilerControllerConsole.View;

namespace Assessment2_BoilerControllerConsole.Controller
{
    /// <summary>
    /// Boiler controller class that coordinates between the service and view.
    /// </summary>
    public class BoilerController
    {
        private readonly SwitchService _switchService;
        private readonly BoilerService _boilerService;
        private readonly LoggerRepository _loggerRepository;
        private readonly ConsoleView _consoleView;

        /// <summary>
        /// Initializes a boiler controller class.
        /// </summary>
        /// <param name="service">Instance of switch service.</param>
        /// <param name="boilerService">Instance of boiler service.</param>
        /// <param name="loggerRepository">Instance of logger repository.</param>
        /// <param name="consoleView">Instance of console view.</param>
        public BoilerController(SwitchService service, BoilerService boilerService, LoggerRepository loggerRepository, ConsoleView consoleView)
        {
            _switchService = service;
            _boilerService = boilerService;
            _loggerRepository = loggerRepository;
            _consoleView = consoleView;
        }

        /// <summary>
        /// Start method to start the simulation of boiler.
        /// </summary>
        /// <returns>Task</returns>
        public async Task Start()
        {
            _loggerRepository.SaveData(new Logger("Boiler Initialized", ""));

            _boilerService.NotificationRaised += OnNotificationRaised;
            bool isRunning = true;
            while (isRunning)
            {
                MenuOptions? option = _consoleView.GetMenuOptions();
                switch (option)
                {
                    case MenuOptions.StartBoiler:
                        await _boilerService.StartBoilerAsync();
                        break;
                    case MenuOptions.StopBoiler:
                        _boilerService.StopBoiler();
                        break;
                    case MenuOptions.SimulateBoilerError:
                        _boilerService.SimulateBoilerError();
                        break;
                    case MenuOptions.ToggleRun:
                        _switchService.Toggle();
                        break;
                    case MenuOptions.ResetLockout:
                        _boilerService.ResetLockOut();
                        break;
                    case MenuOptions.ViewEventLog:
                        HandleViewEventLog();
                        break;
                    case MenuOptions.Exit:
                        isRunning = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the log to the console.
        /// </summary>
        private void HandleViewEventLog()
        {
            List<Logger> logs = _loggerRepository.GetData();
            List<Logger> recentOrders = logs.TakeLast(10).ToList();
            _consoleView.DisplayLogDetails(recentOrders);
        }

        /// <summary>
        /// Notification subscription method.
        /// </summary>
        /// <param name="sender">The object from where notification is from.</param>
        /// <param name="notifications">List of notifications.</param>
        private void OnNotificationRaised(object? sender, List<Notification> notifications)
        {
            _consoleView.ShowNotifications(notifications);
        }
    }
}
