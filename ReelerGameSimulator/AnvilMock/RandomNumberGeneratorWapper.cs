using System.Security.Cryptography;

namespace ReelerGameSimulator.AnvilMock
{
    public class RandomNumberGeneratorWapper
    {
        private const int IntBufferSize = 1024;
        private const int RngSize = 4;

        private int IntBufferOffset { get; set; } = IntBufferSize;
        private byte[] IntBuffer { get; set; } = new byte[IntBufferSize * RngSize];

        public int GetInt32(int range)
        {
            //return RandomNumberGenerator.GetInt32(0, range);

            if (IntBufferOffset >= IntBufferSize)
            {
                RefreshBuffer();
            }

            int max = int.MaxValue - int.MaxValue % range;
            int value;
            do
            {
                value = BitConverter.ToInt32(IntBuffer, IntBufferOffset) & int.MaxValue;
                IntBufferOffset += RngSize;
                if (IntBufferOffset >= IntBufferSize)
                {
                    RefreshBuffer();
                }

            } while (value >= max);

            int returnValue = value % range;
            return returnValue;
        }

        private void RefreshBuffer()
        {
            Span<byte> buffer = stackalloc byte[IntBufferSize * RngSize];
            RandomNumberGenerator.Fill(buffer);
            buffer.CopyTo(IntBuffer);
            IntBufferOffset = 0;
        }
    }
}
