using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Model.Enums;
using Assessment2_BoilerControllerConsole.Persistence;
using Assessment2_BoilerControllerConsole.Service;
using System.Threading;

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
    private readonly object _boilerLock = new object();
    private Timer? _timer;
    private int _secondsRemaining;
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

//using Assessment2_BoilerControllerConsole.Model;

//using Assessment2_BoilerControllerConsole.Persistence;

//using System.Threading;



//namespace Assessment2_BoilerControllerConsole.Service

//{

//    public class BoilerService : IDisposable

//    {

//        private readonly SwitchService _switchService;

//        private readonly LoggerRepository _loggerRepository;



//        private readonly object _boilerLock = new object();



//        private Timer? _timer;



//        private int _remainingSeconds;



//        public BoilerStatus Status { get; private set; }



//        public BoilerService(

//        SwitchService switchService,

//        LoggerRepository loggerRepository)

//        {

//            _switchService = switchService;

//            _loggerRepository = loggerRepository;



//            Status = BoilerStatus.Lockout;

//        }



//        public void Start()

//        {

//            lock (_boilerLock)

//            {

//                if (Status != BoilerStatus.Ready)

//                {

//                    Console.WriteLine(

//                    "Boiler must be in Ready state.");



//                    return;

//                }



//                if (!_switchService.IsClosed())

//                {

//                    Console.WriteLine(

//                    "Run Interlock switch must be Closed.");



//                    return;

//                }



//                Status = BoilerStatus.PrePurge;

//                _remainingSeconds = 10;



//                Log(

//                "Pre-Purge started",

//                "");



//                Console.WriteLine(

//                "Pre-Purge started.");



//                StartTimer();

//            }

//        }



//        private void StartTimer()

//        {

//            _timer?.Dispose();



//            _timer = new Timer(

//            ProcessBoilerSequence,

//            null,

//             TimeSpan.Zero,

//            TimeSpan.FromSeconds(1));

//        }



//        private void ProcessBoilerSequence(object? state)

//        {

//            lock (_boilerLock)

//            {

//                if (Status != BoilerStatus.PrePurge &&

//                Status != BoilerStatus.Ignition)

//                {

//                    return;

//                }



//                Console.WriteLine(

//                $"{Status}: {_remainingSeconds} seconds remaining");



//                _remainingSeconds--;



//                if (_remainingSeconds > 0)

//                {

//                    return;

//                }



//                if (Status == BoilerStatus.PrePurge)

//                {

//                    CompletePrePurge();

//                }

//                else if (Status == BoilerStatus.Ignition)

//                {

//                    CompleteIgnition();

//                }

//            }

//        }



//        private void CompletePrePurge()

//        {

//            Log(

//            "Pre-Purge completed.",

//            "");



//            Status = BoilerStatus.Ignition;



//            _remainingSeconds = 10;



//            Log(

//            "Ignition phase started.",

//            "");



//            Console.WriteLine(

//            "Ignition phase started.");

//        }



//        private void CompleteIgnition()

//        {

//            Log(

//            "Ignition phase completed.",

//            "");



//            Status = BoilerStatus.Operational;



//            Log(

//            "Boiler now operational.",

//            "");



//            Console.WriteLine(

//            "Boiler now operational.");



//            StopTimer();

//        }



//        public bool ResetLockout()

//        {

//            lock (_boilerLock)

//            {

//                if (!_switchService.IsClosed())

//                {

//                    Console.WriteLine(

//                    "Run Interlock switch must be Closed.");



//                    return false;

//                }



//                if (Status != BoilerStatus.Lockout)

//                {

//                    Console.WriteLine(

//                    "Boiler is not in Lockout.");



//                    return false;

//                }



//                Status = BoilerStatus.Ready;



//                Log(

//                "Boiler Status changed to Ready",

//                "");



//                return true;

//            }

//        }



//        public void Stop()

//        {

//            lock (_boilerLock)

//            {

//                if (Status != BoilerStatus.Operational)

//                {

//                    Console.WriteLine(

//                    "Boiler is not Operational.");



//                    return;

//                }



//                Status = BoilerStatus.Lockout;



//                StopTimer();



//                Log(

//                "Boiler stopped",

//                "");

//            }

//        }



//        public void SimulateError()

//        {

//            lock (_boilerLock)

//            {

//                if (Status != BoilerStatus.Operational)

//                {

//                    Console.WriteLine(

//                    "Error simulation is only allowed " +

//                    "when boiler is Operational.");



//                    return;

//                }



//                Status = BoilerStatus.Lockout;



//                StopTimer();



//                Console.WriteLine(

//                "Error: Simulated boiler failure. " +

//                "System in Lockout.");



//                Log(

//                "Boiler Error",

//                "Simulated boiler failure");

//            }

//        }



//        private void StopTimer()

//        {

//            _timer?.Dispose();

//            _timer = null;

//        }



//        private void Log(

//        string eventName,

//        string eventData)

//        {

//            Logger logger =

//            new Logger(eventName, eventData);



//            _loggerRepository.SaveLog(logger);

//        }



//        public void Dispose()

//        {

//            lock (_boilerLock)

//            {

//                StopTimer();

//            }

//        }

//    }

//}