using Device2DeviceFileIO.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Device2DeviceFileIO.Interfaces
{
    /// <summary>
    /// Platform inpependent local file store provider
    /// </summary>
    public interface IFileHandler
    {
        void Load(TransferFile transfer);
        void Save(TransferFile transfer);


    }
}
