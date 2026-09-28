using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Persistence.FileManager;

namespace Assessment2_BoilerControllerConsole.Persistence
{
    /// <summary>
    /// Logger repository that helps to get and save the log data.
    /// </summary>
    public class LoggerRepository
    {
        private readonly string _filePath = "Data/boiler_log.csv";
        private readonly CSVFileManager<Logger> _csvFileManager;

        private readonly object _loggerLock = new object();

        /// <summary>
        /// Initializes a logger repository.
        /// </summary>
        public LoggerRepository()
        {
            _csvFileManager = new CSVFileManager<Logger>(
                _filePath,

               log =>
               $"{log.TimeStamp}," +
               $"{log.EventName}," +
               $"{log.EventData}",

               line =>
               {
                   string[] values = line.Split(',');
                   Logger logger = new Logger(values[1], values[2]);
                   logger.TimeStamp = DateTime.Parse(values[0]);
                   return logger;
               });
        }

        /// <summary>
        /// Gets the list of log details.
        /// </summary>
        /// <returns></returns>
        public List<Logger> GetData()
        {
            lock (_loggerLock)
            {
                return _csvFileManager.GetData() ?? new List<Logger>();
            }
        }

        /// <summary>
        /// Saves the log information.
        /// </summary>
        /// <param name="logger"></param>
        public void SaveData(Logger logger)
        {
            lock (_loggerLock)
            {
                List<Logger> list = GetData();
                list.Add(logger);
                _csvFileManager.SaveData(list, "TimeStamp,EventName,EventData");
            }
        }
    }
}
