using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.iOS.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms.iOS;

namespace Device2DeviceFileIO.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            Platform.Init();

            LoadApplication(new App());

            MessagingCenter.Subscribe<FileOperation.UploadMessage>(this, FileOperation.UPLOAD, async message => {

                await new FileUploadTask().Start();
            });

            MessagingCenter.Subscribe<FileOperation.DownloadMessage>(this, FileOperation.DOWNLOAD, async message => {

                await new FileDownloadTask().Start();
            });

            return base.FinishedLaunching(app, options);
        }

    }
}
