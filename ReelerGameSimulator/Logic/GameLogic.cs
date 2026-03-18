using ReelerGameSimulator.Config;
using ReelerGameSimulator.Logic.Model;
using ReelerGameSimulator.Rng.Anvil;
using ReelerGameSimulator.Rng.My;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ReelerGameSimulator.Logic
{
    public class GameLogic
    {
        public EngineConfiguration? EngineConfiguration { get; private set; }
        public GameLogicState GameState { get; private set; } = new GameLogicState();

        public IRandom Rng { get; private set; } = new AnvilRng();
        public Rng.My.MyRandomNumberGenerator MyRng { get; private set; } = new Rng.My.MyRandomNumberGenerator();

        public GameLogic(EngineConfiguration engineConfiguration)
        {
            EngineConfiguration = engineConfiguration;
        }

        public void Run()
        {
            long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            
            for (var i = 0; i < 1_000_000_00; i++)
            {
                var test = NumericExtensions.GetUInt32(Rng, 0, 1);
                if (test < 0 || test > 1)
                {
                    throw new Exception("out of range test");
                }
            }

            /*
            for (var i = 0; i < 1_000_000_00; i++)
            {
                var test = RandomNumberGenerator.GetInt32(0, 99 + 1);
                if (test > 100)
                {
                    throw new Exception("out of range test");
                }
            }
            */
            

            /*
            Span<byte> buffer = stackalloc byte[4 * 1024];
            const int range = 2;
            const int max = int.MaxValue - int.MaxValue % range;

            for (int i = 0; i < 1_000_000_00; i += 1024)
            {
                RandomNumberGenerator.Fill(buffer);

                for (int j = 0; j < 1024; j++)
                {
                    int value;
                    do
                    {
                        value = BitConverter.ToInt32(buffer.Slice(j * 4, 4)) & int.MaxValue;
                    } while (value >= max);

                    int test = value % range; // now uniform 0-100
                    if (test < 0 || test > 1)
                    {
                        throw new Exception("out of range test");
                    }
                }
            }
            */

            /*
            for (int i = 0; i < 1_000_000_00; i ++)
            {
                var test = MyRng.GetInt32(100);
                if (test < 0 || test > 100)
                {
                    throw new Exception("out of range test");
                }
            }*/

            long timeEnd = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long timeElapse = timeEnd - timeStart;
            Console.WriteLine(timeElapse);
        }
    }
}
