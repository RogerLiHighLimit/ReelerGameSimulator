namespace ReelerGameSimulator.DataOutput.ConsoleWriter
{
    public class ConsoleWriter : ILogWriter
    {
        public string Name { get; }

        public LoggingFlags Flags { get; }

        public ConsoleWriter()
        {
            Name = "Console";
            Flags = LoggingFlags.Writer | LoggingFlags.Console;
        }

        public void Write(char c) => Console.Write(c);

        public void Write(string str) => Console.Write(str);

        public void Write(ILogEmitter emitter) => emitter.ToConsole();

        public void WriteLine() => Console.WriteLine();

        public void WriteLine(string str) => Console.WriteLine(str);

        public override string ToString() => Name;
    }
}
