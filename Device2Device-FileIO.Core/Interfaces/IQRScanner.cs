using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// presents a QR Scanner and returns the data
    /// </summary>
    public interface IQRScanner
    {
        QRCode StartScan();
    }
}
