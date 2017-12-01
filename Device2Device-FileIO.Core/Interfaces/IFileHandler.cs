using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Platform inpependent local file store provider
    /// </summary>
    interface IFileHandler
    {
        void Load(TransferFile transfer);
        void Save(TransferFile transfer);
    }
}
