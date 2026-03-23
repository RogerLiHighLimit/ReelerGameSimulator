namespace ReelerGameSimulator.DataOutput.ConsoleWriter
{
    public class FileWriter : ILogWriter, IDisposable
    {
        private readonly StreamWriter _writer;
        private bool _isDisposed;

        public string Name { get; }

        public LoggingFlags Flags { get; }

        public FileWriter()
        {
            Name = GenerateFileName("Output\\Log");
            Flags = LoggingFlags.Writer | LoggingFlags.File;
            _writer = new StreamWriter(File.OpenWrite(Name));
            _writer.AutoFlush = true;
        }

        public void Write(char c) => _writer.Write(c);

        public void Write(string str) => _writer.Write(str);

        public void Write(ILogEmitter emitter) => _writer.Write(emitter.Emit());

        public void WriteLine() => _writer.WriteLine();

        public void WriteLine(string str) => _writer.WriteLine(str);

        public override string ToString() => Name;

        private static string GenerateFileName(string path)
        {
            string str1 = Path.Combine(Directory.GetCurrentDirectory(), path);
            Directory.CreateDirectory(str1);
            string str2 = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(str1, str2 + ".log");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;
            if (disposing)
            {
                _writer?.Flush();
                _writer?.Dispose();
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}