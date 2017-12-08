using System;
using System.IO;
using System.Windows.Input;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Device2DeviceFileIO.UI.View;
using Device2DeviceFileIO.Interfaces;

namespace Device2DeviceFileIO.UI.ViewModel
{
    public class TransferFileUploadVm : BindableBase
    {
        const double _defaultxpirationInDay = 1;

        public TransferFileUploadVm(TransferFile uploadTransferFile) {
            UploadTransferFile = uploadTransferFile;
            _expirationDate = DateTime.Now.AddDays(_defaultxpirationInDay);

            ((App)Application.Current).ShareHandler.ShareFileRequestReceived += (object sender, System.EventArgs e) => {
                UploadTransferFile = ((IShareHandler)sender).ReceiveFile();
            };
        }

        public INavigation Navigation { get; set; }

        private TransferFile _uploadTransferFile;
        public TransferFile UploadTransferFile
        {
            get { return _uploadTransferFile; }
            set { SetProperty(ref _uploadTransferFile, value); }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { SetProperty(ref _expirationDate, value); }
        }

        // lazy instantiation
        private ICommand _startUploadCommand;
        public ICommand StartUploadCommand => _startUploadCommand ?? (_startUploadCommand = new Command(() => StartUpload()));

        async public void StartUpload()
        {
            // Nur zum Testen
            // var stream = this.GetType().Assembly.GetManifestResourceStream("Device2DeviceFileIO.Resources.EarthLarge.jpg");

            // var ms = new MemoryStream();
            // stream.CopyTo(ms);
            //UploadTransferFile.Content = ms.ToArray();

            App.GetCloudFileService().UploadFinished += (object sender, FileOperation.UploadFinishedEventArgs e) => {
                // edtDownloadLink.Text = e.Code.Url;
                // Console.Write("Finished ...");
                // UploadTransferFile.Name = "meine_bild.png";
            };

            App.GetCloudFileService().Upload(UploadTransferFile);
            await Navigation.PopAsync();
        }
    }
}
