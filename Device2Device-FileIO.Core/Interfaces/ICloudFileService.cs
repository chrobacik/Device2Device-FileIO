using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Abstraction of cloud based file storage provider
    /// </summary>
    public interface ICloudFileService
    {
        void Upload(TransferFile file, QRCode qRCode);
        void Download(TransferFile file, QRCode qRCode);
    }
}
