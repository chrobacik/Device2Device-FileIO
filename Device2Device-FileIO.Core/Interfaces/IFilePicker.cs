using System;
using System.Threading.Tasks;

namespace Device2DeviceFileIO.Interfaces
{
    public interface IFilePicker
    {
        Task<String> GetFilePathAsync();
    }
}
