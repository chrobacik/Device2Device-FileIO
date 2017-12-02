using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Platform specific exchange of files
    /// </summary>
    public interface IShareHandler
    {
        event EventHandler ShareFileRequestReceived;
        TransferFile ReceiveFile();
        void ProvideFile(TransferFile transferFile);
    }

}
