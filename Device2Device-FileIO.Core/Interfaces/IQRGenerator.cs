using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// presents the given QR Code
    /// </summary>
    public interface IQRGenerator
    {
        void ShowQRCode(QRCode qRCode);
    }
}
