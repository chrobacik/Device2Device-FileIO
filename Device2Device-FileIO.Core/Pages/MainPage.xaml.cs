using System;
using System.IO;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Pages;
using Xamarin.Forms;
using Device2DeviceFileIO.Interfaces;

namespace Device2DeviceFileIO
{
    public partial class MainPage : ContentPage
    {
        TransferFile SharedFile { get; set; }


        public MainPage()
        {
            InitializeComponent();
            Title = "Device2Device File.IO";

            ((App)Application.Current).ShareHandler.ShareFileRequestReceived += ShareHandler_ShareFileRequestReceived;
            this.Disappearing += MainPage_Disappearing;


            btnGoToSelectFile.Clicked += async (object sender, System.EventArgs e) =>
            {

                await Navigation.PushAsync(new SendFilePage());
            };

            btnGoToBarcodeScan.Clicked += async (object sender, System.EventArgs e) =>
            {

                await Navigation.PushAsync(new BarcodeScanPage());
            };

            btnGoToBarcodeShow.Clicked += async (object sender, System.EventArgs e) =>
            {

                await Navigation.PushAsync(new BarcodePage());
            };

            btnUploadFile.Clicked += (object sender, System.EventArgs e) => {

                var stream = this.GetType().Assembly.GetManifestResourceStream("Device2DeviceFileIO.Resources.EarthLarge.jpg");
                var transferFile = new TransferFile { Name = "EarthLarge.jpg" };

                var ms = new MemoryStream();
                stream.CopyTo(ms);
                transferFile.Content = ms.ToArray();

                App.GetCloudFileService().Upload(transferFile);
            };

            btnDownloadFile.Clicked += (object sender, System.EventArgs e) =>
            {
                App.GetCloudFileService().Download(new QRCode { Url = edtDownloadLink.Text });
            };

            btnShareFile.Clicked += BtnShareFile_Clicked;


            App.GetCloudFileService().DownloadFinished += Handle_DownloadFinished;
            App.GetCloudFileService().DownloadProgress += Handle_DownloadProgress;
            App.GetCloudFileService().UploadProgress += Handle_UploadProgress;
            App.GetCloudFileService().UploadFinished += Handle_UploadFinished;
        }

        private void MainPage_Disappearing(object sender, EventArgs e)
        {
            ((App)Application.Current).ShareHandler.ShareFileRequestReceived -= ShareHandler_ShareFileRequestReceived;
        }

        private void ShareHandler_ShareFileRequestReceived(object sender, EventArgs e)
        {
            btnShareFile.IsEnabled = true;

            SharedFile = ((IShareHandler)sender).ReceiveFile();
        }

        private void BtnShareFile_Clicked(object sender, EventArgs e)
        {
            if (SharedFile != null)
            {
                ((App)Application.Current).ShareHandler.ProvideFile(SharedFile);
            }
        }

        public void Handle_DownloadFinished(object sender, FileOperation.DownloadFinsihedEventArgs e)
        {
            Console.WriteLine($"File downloaded: {e.File.Name}");
        }

        public void Handle_DownloadProgress(object sender, FileOperation.DownloadProgressEventArgs e)
        {
            prgDownloadFile.Progress = e.File.Status.Percentage;
        }

        public void Handle_UploadProgress(object sender, FileOperation.UploadProgressEventArgs e)
        {
            prgUploadFile.Progress = e.File.Status.Percentage;
        }

        public void Handle_UploadFinished(object sender, FileOperation.UploadFinishedEventArgs e)
        {
            edtDownloadLink.Text = e.Code.Url;
            
            Console.WriteLine($"File uploaded: {e.Code.Url}");
        }
    }
}

