using System;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileUploadVm : BindableBase
    {

        public INavigation Navigation { get; set; }

        private TransferFile _uploadTransferFile;
        public TransferFile UploadTransferFile
        {
            get { return _uploadTransferFile; }
            set { SetProperty(ref _uploadTransferFile, value); }
        }

        // lazy instantiation
        private ICommand _startUploadCommand;
        public ICommand StartUploadCommand => _startUploadCommand ?? (_startUploadCommand = new Command(() => StartUpload()));

        async public void StartUpload()
        {
            await Navigation.PopAsync();
        }
    }
}
