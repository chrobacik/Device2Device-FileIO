using System;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileDownloadVm : BindableBase
    {

        public INavigation Navigation { get; set; }

        private TransferFile _downloadTransferFile;
        public TransferFile DownloadTransferFile
        {
            get { return _downloadTransferFile; }
            set { SetProperty(ref _downloadTransferFile, value); }
        }

        // lazy instantiation
        private ICommand _startDownloadCommand;
        public ICommand StartDownloadCommand => _startDownloadCommand ?? (_startDownloadCommand = new Command(() => StartDownload()));

        async public void StartDownload()
        {
            await Navigation.PopToRootAsync();
        }

    }
}
