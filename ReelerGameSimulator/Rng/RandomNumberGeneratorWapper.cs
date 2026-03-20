using System.Security.Cryptography;

namespace ReelerGameSimulator.Rng
{
    public class RandomNumberGeneratorWapper
    {
        private const int IntBufferSize = 1024;
        private int IntBufferOffset { get; set; } = IntBufferSize;
        private byte[] IntBuffer { get; set; } = new byte[IntBufferSize * 4];

        public int GetInt32(int range)
        {
            //return RandomNumberGenerator.GetInt32(0, range);

            if (IntBufferOffset >= IntBufferSize)
            {
                Span<byte> buffer = stackalloc byte[4 * 1024];
                RandomNumberGenerator.Fill(buffer);
                buffer.CopyTo(IntBuffer);
                IntBufferOffset = 0;
            }

            int max = int.MaxValue - int.MaxValue % range;
            int value;
            do
            {
                value = BitConverter.ToInt32(IntBuffer, IntBufferOffset) & int.MaxValue;
                IntBufferOffset += 4;
            } while (value >= max);

            int returnValue = value % range;
            return returnValue;
        }
    }
}
