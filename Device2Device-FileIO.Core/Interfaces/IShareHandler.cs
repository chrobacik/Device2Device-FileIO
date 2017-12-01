using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Platform specific exchange of files
    /// </summary>
    interface  IShareHandler
    {
        void ReceiveFile();
        void ProvideFile();
    }
}
