namespace Assessment2_BoilerControllerConsole.Persistence.FileManager
{
    /// <summary>
    /// CSV file manager to load and store the data.
    /// </summary>
    /// <typeparam name="T">Generic type.</typeparam>
    public class CSVFileManager<T>
    {
        private readonly string _filePath;
        private readonly Func<T, string> _toCsv;
        private readonly Func<string, T> _fromCsv;

        /// <summary>
        /// Initializes the csv manager.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        /// <param name="toCsv">Convert to csv format.</param>
        /// <param name="fromCsv">Convert from csv format</param>
        public CSVFileManager(string filePath, Func<T, string> toCsv, Func<string, T> fromCsv)
        {
            _filePath = filePath;
            _toCsv = toCsv;
            _fromCsv = fromCsv;
        }

        /// <summary>
        /// Gets the data from the csv
        /// </summary>
        /// <returns>List of data.</returns>
        public List<T> GetData()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            string[] lines = File.ReadAllLines(_filePath);
            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(_fromCsv)
                .ToList();
        }

        /// <summary>
        /// Saves the data into the log file.
        /// </summary>
        /// <param name="data">Data to be stored.</param>
        /// <param name="header">Header of csv.</param>
        public void SaveData(List<T> data, string header)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            List<string> lines = new List<string>()
            {
                header,
            };
            lines.AddRange(data.Select(_toCsv));
            File.WriteAllLines(_filePath, lines);
        }
    }
}
