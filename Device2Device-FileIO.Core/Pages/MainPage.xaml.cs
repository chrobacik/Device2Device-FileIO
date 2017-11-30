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

            btnUploadFile.Clicked += (object sender, System.EventArgs e) =>
            {

                MessagingCenter.Send(new UploadFileMessage(), "UploadFileMessage");
            };

            btnDownloadFile.Clicked += (object sender, System.EventArgs e) =>
            {

                MessagingCenter.Send(new DownloadFileMessage(), "DownloadFileMessage");
            };

            MessagingCenter.Subscribe<FileProgressMessage>(this, "FileProgressMessage", HandleProgressMessage);

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", HandleCancelledMessage);
        }

        public void HandleProgressMessage(FileProgressMessage message)
        {
            prgUploadFile.Progress = message.Percentage;
        }

        public void HandleCancelledMessage(CancelledMessage message)
        {
            prgUploadFile.Progress = 0;
        }
    }
}
