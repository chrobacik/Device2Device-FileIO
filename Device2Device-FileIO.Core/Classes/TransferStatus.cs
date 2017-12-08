using System;
using Device2DeviceFileIO.UI.ViewModel;

namespace Device2DeviceFileIO.Classes
{
    public class TransferStatus : BindableBase
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

        private TypeState _state;
        public TypeState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private float _percentage;
        public float Percentage
        {
            get { return _percentage; }
            set { SetProperty(ref _percentage, value); }
        }
        public String ErrorMessage { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().ToString()}:: State: {State}, Percentage: {Percentage}, ErrorMessage: {ErrorMessage}, {Exception?.Message}";
        }
    }
}
