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
        void Upload();
        void Download();
    }
}
