namespace ReelerGameSimulator.Rng.Anvil
{
    public interface IRandom
    {
        byte[] GetBytes(int length);

        void FillBytes(byte[] bytes);
    }
}
