using Xamarin.Forms;
using Device2DeviceFileIO.UI.ViewModel;
using Device2DeviceFileIO.Classes;

namespace Device2DeviceFileIO.UI.View
{
    public partial class TransferFileOverviewPage : ContentPage
    {
        const uint _animationInterval = 250;
        private Easing _easing = Easing.Linear;

        public TransferFileOverviewVm ViewModel { get; set; }

        public TransferFileOverviewPage()
        {
            InitializeComponent();

            Title = "Device2Device FileIO";

            ViewModel = new TransferFileOverviewVm();
            ViewModel.Navigation = Navigation;

            BindingContext = ViewModel;

            prgUploadFile.Progress = 0.0;
            App.GetCloudFileService().UploadProgress += (object sender, FileOperation.UploadProgressEventArgs e) => {
                prgUploadFile.ProgressTo(e.File.Status.Percentage, _animationInterval, _easing);
            };
            App.GetCloudFileService().UploadFinished += (object sender, FileOperation.UploadFinishedEventArgs e) => {
                // prgUploadFile.ProgressTo(e.File.Status.Percentage, _animationInterval, _easing);
                prgUploadFile.ProgressTo(1.0, _animationInterval, _easing);
            };

            prgDownloadFile.Progress = 0.0;
            App.GetCloudFileService().DownloadProgress += (object sender, FileOperation.DownloadProgressEventArgs e) => {
                prgDownloadFile.ProgressTo(e.File.Status.Percentage, _animationInterval, _easing);
            };
            App.GetCloudFileService().DownloadFinished += (object sender, FileOperation.DownloadFinsihedEventArgs e) => {
                prgDownloadFile.ProgressTo(e.File.Status.Percentage, _animationInterval, _easing);
            };
        }
    }
}
