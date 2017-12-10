using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Platform specific exchange of files
    /// </summary>
    public interface IShareHandler
    {
        event EventHandler ShareFileRequestReceived;
        TransferFile ReceiveFile();
        Task ProvideFile(TransferFile transferFile);
    }

}
