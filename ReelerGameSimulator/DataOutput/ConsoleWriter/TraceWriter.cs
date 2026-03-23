using System.Diagnostics;

namespace ReelerGameSimulator.DataOutput.ConsoleWriter
{
    public class TraceWriter : ILogWriter
    {
        public string Name { get; }

        public LoggingFlags Flags { get; }

        public TraceWriter()
        {
            Name = "Trace";
            Flags = LoggingFlags.Writer | LoggingFlags.Trace;
        }

        public void Write(char c) => Trace.Write(c);

        public void Write(string str) => Trace.Write(str);

        public void Write(ILogEmitter emitter) => Trace.Write(emitter.Emit());

        public void WriteLine() => Trace.WriteLine(string.Empty);

        public void WriteLine(string str) => Trace.WriteLine(str);

        public override string ToString() => Name;
    }
}