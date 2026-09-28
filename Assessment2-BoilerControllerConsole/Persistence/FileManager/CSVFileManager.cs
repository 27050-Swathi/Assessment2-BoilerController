namespace Assessment2_BoilerControllerConsole.Persistence.FileManager
{
    public class CSVFileManager<T>
    {
        private readonly string _filePath;
        private readonly Func<T, string> _toCsv;
        private readonly Func<string, T> _fromCsv;

        public CSVFileManager(string filePath, Func<T, string> toCsv, Func<string, T> fromCsv)
        {
            _filePath = filePath;
            _toCsv = toCsv;
            _fromCsv = fromCsv;
        }

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
