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

                var stream = this.GetType().Assembly.GetManifestResourceStream("Device2DeviceFileIO.Resources.MyFile.txt");

                var transferFile = new TransferFile
                {
                    Name = "MyFile.txt"
                };

                var ms = new MemoryStream();
                stream.CopyTo(ms);
                transferFile.Content = ms.ToArray();

                App.GetCloudFileService().Upload(transferFile, new QRCode());
            };

            btnDownloadFile.Clicked += (object sender, System.EventArgs e) =>
            {
                App.GetCloudFileService().Download(new TransferFile(), new QRCode { Url = edtDownloadLink.Text });
            };

            btnShareFile.Clicked += BtnShareFile_Clicked;


            App.GetCloudFileService().DownloadFinished += Handle_DownloadFinished;
            App.GetCloudFileService().OperationProgress += Handle_OperationProgress;
            App.GetCloudFileService().OperationCanceled += Handle_OperationCanceled;
            App.GetCloudFileService().UploadFinished += Handle_UploadFinished;
            App.GetCloudFileService().OperationFailed += Handle_OperationFailed;
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

        public void Handle_DownloadFinished(object sender, FileOperation.DownloadFinishedMessage e)
        {
            Console.WriteLine($"File downloaded: {e.Content}");
        }

        public void Handle_OperationProgress(object sender, FileOperation.ProgressMessage e)
        {
            prgUploadFile.Progress = e.Percentage;
        }

        public void Handle_OperationCanceled(object sender, FileOperation.CanceledMessage e)
        {
            prgUploadFile.Progress = 0;
        }

        public void Handle_UploadFinished(object sender, FileOperation.UploadFinishedMessage e)
        {
            edtDownloadLink.Text = e.Result;
            
            Console.WriteLine($"File uploaded: {e.Result}");
        }

        public void Handle_OperationFailed(object sender, FileOperation.FailedMessage e)
        {
            Console.WriteLine($"File operation failed: {e.Error}");
        }
    }
}
