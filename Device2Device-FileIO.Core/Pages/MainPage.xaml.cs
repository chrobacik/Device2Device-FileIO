using System;
using System.IO;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Pages;
using Xamarin.Forms;

namespace Device2DeviceFileIO
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Title = "Device2Device File.IO";

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

            App.GetCloudFileService().DownloadFinished += Handle_DownloadFinished;
            App.GetCloudFileService().DownloadProgress += Handle_DownloadProgress;
            App.GetCloudFileService().UploadProgress += Handle_UploadProgress;
            App.GetCloudFileService().UploadFinished += Handle_UploadFinished;
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
