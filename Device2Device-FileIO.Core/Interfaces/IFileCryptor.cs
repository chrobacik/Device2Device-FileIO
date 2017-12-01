using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Encrypt given file and returns key and encrpted file
    /// Decrypt is doing the opposite.
    /// </summary>
    public interface IFileCryptor
    {
        void Encrypt();
        void Decrypt();
    }
}
