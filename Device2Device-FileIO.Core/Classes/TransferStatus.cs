using System;

namespace Device2DeviceFileIO.Classes
{
    public class TransferStatus
    {
        public enum TypeState
        {
            Pending,
            Transfering,
            Completed,
            Aborted,
            ReceivedFromOS,
            Failed
        };

        public TypeState State { get; set; }
        public float Percentage { get; set; }
        public String ErrorMessage { get; set; }
        public Exception Exception { get; set; }
    }
}
