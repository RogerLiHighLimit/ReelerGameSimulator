using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReelerGameSimulator.Rng.Anvil
{
    public class AnvilRng : IRandom, IDisposable
    {
        private readonly RandomNumberGenerator _rng;
        private bool _disposed;

        public AnvilRng()
        {
            this._rng = RandomNumberGenerator.Create();
            this._disposed = false;
        }

        public void FillBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("byte array is null or of zero length");
            this._rng.GetBytes(bytes);
        }

        public byte[] GetBytes(int length)
        {
            byte[] data = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof(length), string.Format("length: {0} <= 0", (object)length));
            this._rng.GetBytes(data);
            return data;
        }

        public void Dispose()
        {
            GC.SuppressFinalize((object)this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
                return;
            if (disposing)
                this._rng.Dispose();
            this._disposed = true;
        }
    }
}
