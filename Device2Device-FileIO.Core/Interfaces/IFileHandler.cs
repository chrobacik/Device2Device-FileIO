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
        void Load();
        void Save();
    }
}
