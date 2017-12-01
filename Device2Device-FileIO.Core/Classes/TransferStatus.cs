using System;
namespace Device2DeviceFileIO.Classes
{
    public class TransferStatus
    {

        enum TypeState
        {
            Pending,
            Transfering,
            Completed,
            Aborted,
        };

        public int percentage { get; set; }

        public TransferStatus()
        {
            
        }
    }
}
