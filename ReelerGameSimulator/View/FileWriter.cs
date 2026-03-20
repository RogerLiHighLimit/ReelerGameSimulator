namespace ReelerGameSimulator.View
{
    public class FileWriter : ILogWriter, IDisposable
    {
        private readonly StreamWriter _writer;
        private bool _isDisposed;

        public string Name { get; }

        public LoggingFlags Flags { get; }

        public FileWriter()
        {
            this.Name = FileWriter.GenerateFileName("Output\\Log");
            this.Flags = LoggingFlags.Writer | LoggingFlags.File;
            this._writer = new StreamWriter((Stream)File.OpenWrite(this.Name));
            this._writer.AutoFlush = true;
        }

        public void Write(char c) => this._writer.Write(c);

        public void Write(string str) => this._writer.Write(str);

        public void Write(ILogEmitter emitter) => this._writer.Write(emitter.Emit());

        public void WriteLine() => this._writer.WriteLine();

        public void WriteLine(string str) => this._writer.WriteLine(str);

        public override string ToString() => this.Name;

        private static string GenerateFileName(string path)
        {
            string str1 = Path.Combine(Directory.GetCurrentDirectory(), path);
            Directory.CreateDirectory(str1);
            string str2 = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(str1, str2 + ".log");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;
            if (disposing)
            {
                this._writer?.Flush();
                this._writer?.Dispose();
            }
            this._isDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }
    }
}