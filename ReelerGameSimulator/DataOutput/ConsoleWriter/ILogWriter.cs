namespace ReelerGameSimulator.DataOutput.ConsoleWriter
{
    public interface ILogWriter
    {
        string Name { get; }

        LoggingFlags Flags { get; }

        void Write(char c);

        void Write(string str);

        void Write(ILogEmitter emitter);

        void WriteLine(string str);

        void WriteLine();
    }
}
