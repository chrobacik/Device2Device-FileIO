﻿using System;
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
            ReceivedFromOS,

        };

        public double percentage { get; set; }

        public TransferStatus()
        {
            
        }
    }
}
