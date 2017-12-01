using System;
namespace Device2DeviceFileIO.Classes
{
    public class File
    {

        public String Name { get; set; }
        public long Size { get; set; }
        public String Type { get; set; }
        public byte[] Content { get; set; }

        public File()
        {
        }
    }
}
