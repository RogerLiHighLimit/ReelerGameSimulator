namespace ReelerGameSimulator.View
{
    [Flags]
    public enum LoggingFlags
    {
        None = 0,
        Writer = 1,
        Trace = 2,
        Console = 4,
        File = 8,
        Custom = 0x10
    }
}