using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;
using Android.Provider;
using Xamarin.Forms;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Droid.Services;
using Device2DeviceFileIO.Droid.Classes;

namespace Device2DeviceFileIO.Droid
{
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"*/*")]
    [Activity(Label = "Device2Device-FileIO.Android", Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<String> PickImageTaskCompletionSource { set; get; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Droid.Resource.Layout.Tabbar;
            ToolbarResource = Droid.Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();

            LoadApplication(new App());

            var appShareHandler = ((App)App.Current).ShareHandler as ShareHandler;
            appShareHandler.HandleShareIntent(this);

            MessagingCenter.Subscribe<FileOperation.UploadMessage>(this, FileOperation.UPLOAD, message => {

                StartService(new Intent(this, typeof(FileUploadService)));
            });

            MessagingCenter.Subscribe<FileOperation.DownloadMessage>(this, FileOperation.DOWNLOAD, message => {

                StartService(new Intent(this, typeof(FileDownloadService)));
            });
        }

        protected override void OnStop()
        {
            base.OnStop();
            MessagingCenter.Unsubscribe<FileOperation.UploadMessage>(this, FileOperation.UPLOAD);
            MessagingCenter.Unsubscribe<FileOperation.DownloadMessage>(this, FileOperation.DOWNLOAD);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
