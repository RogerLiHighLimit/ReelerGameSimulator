using System.Text;

namespace ReelerGameSimulator.DataOutput.ConsoleWriter
{
    public static class Logger
    {
        private class Instance : TextWriter
        {
            public override Encoding Encoding => _encoding;

            public override void Write(char value)
            {
                Logger.Write(value);
            }
        }

        private static readonly List<ILogWriter> _writers = new List<ILogWriter>();

        private static readonly Encoding _encoding = Encoding.UTF8;

        private static LoggingFlags _flags = LoggingFlags.None;

        public static TextWriter TextWriter { get; } = new Instance();


        public static void Write(char value)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                _writers[i].Write(value);
            }
        }

        public static bool Configure()
        {
            return Configure(LoggingFlags.Trace | LoggingFlags.Console);
        }

        public static bool Configure(LoggingFlags flags)
        {
            if (!_flags.Equals(LoggingFlags.None))
            {
                return false;
            }

            _flags = flags;
            if (flags.HasFlag(LoggingFlags.Console))
            {
                _writers.Add(new ConsoleWriter());
            }

            if (flags.HasFlag(LoggingFlags.Trace))
            {
                _writers.Add(new TraceWriter());
            }

            if (flags.HasFlag(LoggingFlags.File))
            {
                _writers.Add(new FileWriter());
            }

            return true;
        }

        public static void Register(ILogWriter writer)
        {
            _writers.Add(writer);
        }

        public static bool Remove(ILogWriter writer)
        {
            return _writers.Remove(writer);
        }

        public static bool Remove(string name)
        {
            for (int num = _writers.Count - 1; num >= 0; num--)
            {
                if (_writers[num].Name.Equals(name))
                {
                    _writers.RemoveAt(num);
                    return true;
                }
            }

            return false;
        }

        public static void Write(string str)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                logWriter.Write(str);
            }
        }

        public static void Write(ILogEmitter emitter)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                logWriter.Write(emitter);
            }
        }

        public static void WriteLine(string str)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                logWriter.WriteLine(str);
            }
        }

        public static void WriteLine()
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                logWriter.WriteLine();
            }
        }

        public static void Write(string str, LoggingFlags flags)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                if (flags == (flags & logWriter.Flags))
                {
                    logWriter.Write(str);
                }
            }
        }

        public static void WriteLine(string str, LoggingFlags flags)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                if (flags == (flags & logWriter.Flags))
                {
                    logWriter.WriteLine(str);
                }
            }
        }

        public static void WriteLine(LoggingFlags flags)
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                ILogWriter logWriter = _writers[i];
                if (flags == (flags & logWriter.Flags))
                {
                    logWriter.WriteLine();
                }
            }
        }
    }
}
