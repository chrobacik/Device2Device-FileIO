using System;
using Device2DeviceFileIO.UI.ViewModel;

namespace Device2DeviceFileIO.Classes
{
    public class TransferFile : BindableBase
    {
        private String _name;
        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private long _size;
        public long Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }

        private String _type;
        public String Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }
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

        public override string ToString()
        {
            return $"{this.GetType().ToString()}:: Name: {Name}, Type: {Type}, Content: {Content}, Size: {Size}, StoragePath: {StoragePath}, Status: {Status}";
        }
    }
}
