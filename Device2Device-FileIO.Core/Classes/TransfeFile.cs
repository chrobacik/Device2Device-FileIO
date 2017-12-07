using System;

namespace Device2DeviceFileIO.Classes
{
    public class TransferFile
    {
        public String Name { get; set; }
        public long Size { get; set; }
        public String Type { get; set; }
        public byte[] Content { get; set; }
        public TransferStatus Status { get; set; }
        public String StoragePath { get; set; }
        public TransferFile()
        {
            Status = new TransferStatus
            {
                State = TransferStatus.TypeState.Pending,
                Percentage = 0F
            };
        }
    }
}
