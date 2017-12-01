using System;
namespace Device2DeviceFileIO.Classes
{
    public class QRCode
    {
        
        public String Url { get; set; }
        public String FileName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte[] Key { get; set; }

        public QRCode()
        {
        }
    }
}
