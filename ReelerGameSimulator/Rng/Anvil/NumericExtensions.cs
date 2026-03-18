namespace ReelerGameSimulator.Rng.Anvil
{
    public static class NumericExtensions
    {
        private const int BYTES_32 = 4;
        private const int BYTES_64 = 8;

        public static int GetInt32(this IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));
            byte[] bytes = new byte[4];
            rng.FillBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static long GetInt64(this IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));
            byte[] bytes = new byte[8];
            rng.FillBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public static uint GetUInt32(this IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));
            byte[] bytes = new byte[4];
            rng.FillBytes(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static ulong GetUInt64(this IRandom rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));
            byte[] bytes = new byte[8];
            rng.FillBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static int GetInt32(this IRandom rng, int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), string.Format("{0}, {1}", min, max));
            if (min == max)
                return min;
            return min == int.MinValue && max == int.MaxValue ? rng.GetInt32() : ScaleInt32Value(rng, min, max);
        }

        public static uint GetUInt32(this IRandom rng, uint min, uint max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), string.Format("{0} > {1}", min, max));
            if ((int)min == (int)max)
                return min;
            return min == 0U && max == uint.MaxValue ? rng.GetUInt32() : ScaleUInt32Value(rng, min, max);
        }

        public static ulong GetUInt64(this IRandom rng, ulong min, ulong max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), string.Format("{0} > {1}", min, max));
            if ((long)min == (long)max)
                return min;
            return min == 0UL && max == ulong.MaxValue ? rng.GetUInt64() : ScaleUInt64Value(rng, min, max);
        }

        private static int ScaleInt32Value(IRandom rng, int min, int max)
        {
            uint num1 = (uint)(max - min + 1);
            uint num2 = uint.MaxValue / num1 * num1;
            uint uint32;
            do
            {
                uint32 = rng.GetUInt32();
            }
            while (uint32 >= num2);
            return (int)(uint32 % num1 + min);
        }

        private static uint ScaleUInt32Value(IRandom rng, uint min, uint max)
        {
            uint num1 = (uint)((int)max - (int)min + 1);
            uint num2 = uint.MaxValue / num1 * num1;
            uint uint32;
            do
            {
                uint32 = rng.GetUInt32();
            }
            while (uint32 >= num2);
            return uint32 % num1 + min;
        }

        private static ulong ScaleUInt64Value(IRandom rng, ulong min, ulong max)
        {
            ulong num1 = max - min + 1UL;
            ulong num2 = ulong.MaxValue / num1 * num1;
            ulong uint64;
            do
            {
                uint64 = rng.GetUInt64();
            }
            while (uint64 >= num2);
            return uint64 % num1 + min;
        }
    }
}
