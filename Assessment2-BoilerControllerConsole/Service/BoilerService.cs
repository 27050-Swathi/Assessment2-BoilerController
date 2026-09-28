using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Model.Enums;
using Assessment2_BoilerControllerConsole.Persistence;
using Assessment2_BoilerControllerConsole.Service;

/// <summary>
/// Boiler service class for the simulation of boiler.
/// </summary>
public class BoilerService
{
    private readonly SwitchService _switchService;
    private readonly LoggerRepository _loggerRepository;
    private readonly List<Notification> _notifications = new List<Notification>();
    public event EventHandler<List<Notification>>? NotificationRaised;
    private readonly object _notificationLock = new object();
    public BoilerStatus Status { get; private set; }
    
    /// <summary>
    /// Initializes a boiler with constructor.
    /// </summary>
    /// <param name="switchService"></param>
    /// <param name="loggerRepository"></param>
    public BoilerService(SwitchService switchService, LoggerRepository loggerRepository)
    {
        _switchService = switchService;
        _loggerRepository = loggerRepository;
        Status = BoilerStatus.Lockout;
    }

    /// <summary>
    /// Reset the lockout and make the boiler ready.
    /// </summary>
    /// <returns><true>if boiler is ready.</true></returns>
    public bool ResetLockOut()
    {
        if (!_switchService.IsClosed())
        {
            AddNotification("Interlock switch must be closed.");
            return false;
        }
        Status = BoilerStatus.Ready;
        AddNotification("Boiler status is set to ready.");
        return true;
    }

    /// <summary>
    /// Start the boiler simulation.
    /// </summary>
    /// <returns>Boiler task</returns>
    public async Task StartBoilerAsync()
    {
        if (!_switchService.IsClosed())
        {
            AddNotification("Switch must be closed in order to start the operation. Toggle the switch.");
            return;
        }

        Status = BoilerStatus.Ready;

        if (Status != BoilerStatus.Ready)
        {
            AddNotification("Boiler must be in ready state to start function.");
            return;
        }

        try
        {
            Status = BoilerStatus.PrePurge;
            AddNotification("Boiler in Pre Purge state");
            await RunPhaseAsync("PrePurge", 3);
            AddNotification("Pre Purge has been completed.");

            Status = BoilerStatus.Ignition;
            AddNotification("Boiler in Ignition state");
            await RunPhaseAsync("Ignition", 5);
            AddNotification("Ignition has been completed.");

            Status = BoilerStatus.Operational;
            AddNotification("Boiler is now in operational state.");

        }
        catch(Exception ex)
        {
            Status = BoilerStatus.Lockout;
            AddNotification($"{ex.Message} - System in Lockout");
        }
    }

    /// <summary>
    /// Shows the countdown for the execution.
    /// </summary>
    /// <param name="phase"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private async Task RunPhaseAsync(string phase, int seconds)
    {
        for(int i = seconds; i > 0; i--)
        {
            AddNotification($"Time remaining for {phase} is {i}");
            await Task.Delay(1000);
        }
    }

    /// <summary>
    /// Stop the boiler from running.
    /// </summary>
    public void StopBoiler()
    {
        if(Status != BoilerStatus.Operational)
        {
            AddNotification("Boiler is not operational.");
            return;
        }

        Status = BoilerStatus.Lockout;
        AddNotification("Boiler process has been stopped.");
        _switchService.Toggle();
    }

    /// <summary>
    /// Simulate an error while running.
    /// </summary>
    public void SimulateBoilerError()
    {
        if(Status != BoilerStatus.Operational)
        {
            AddNotification("Error simulation can only happen when the boiler is in operational state.");
            return;
        }

        try
        {
            throw new InvalidOperationException();
        }
        catch (InvalidOperationException)
        {
            Status = BoilerStatus.Lockout;
            AddNotification("Boiler error simulated.System is in Lockout.");
        }
    }

    /// <summary>
    /// Adds notification and logging of the operations.
    /// </summary>
    /// <param name="message"></param>
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