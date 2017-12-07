using System;
using Device2DeviceFileIO.Interfaces;

namespace Device2DeviceFileIO.Classes
{
    public class PlaceboFileCryptor : IFileCryptor
    {
        public void Decrypt(TransferFile file, QRCode qRCode)
        {
            // Do nothing
        }

        public void Encrypt(TransferFile file, QRCode qRCode)
        {
            // Do nothing
        }
    }
}
