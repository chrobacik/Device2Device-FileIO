using System;
namespace Device2DeviceFileIO.Classes
{
    public class QRCode
    {
        
        public String Url { get; set; }
        public String FileName { get; set; }
        public int ExpirationDate { get; set; }
        public String Key { get; set; }

        public QRCode()
        {
        }
    }
}
