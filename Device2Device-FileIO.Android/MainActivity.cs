using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Provider;
using Android.Net;
using Xamarin.Forms;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Droid.Services;
using Android;
using Device2DeviceFileIO.Droid.Classes;

namespace Device2DeviceFileIO.Droid
{


    [IntentFilter(
        new[] { Intent.ActionSend }, 
        Categories = new[] { Intent.CategoryDefault }, 
        DataMimeType = @"*/*")]
    [Activity(Label = "Device2Device-FileIO.Android", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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

            var sh = new ShareHandler();
            sh.HandleIntent(this);

            MessagingCenter.Subscribe<UploadFileMessage>(this, "UploadFileMessage", message => {
                
                var intent = new Intent(this, typeof(FileService));
                StartService(intent);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId && intent != null)
            {
                Console.WriteLine("Intent result: {0}", intent.Data.ToString());

                if (resultCode == Result.Ok)
                {
                    PickImageTaskCompletionSource.SetResult(GetPathToImage(intent.Data));
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/45692514/get-path-of-gallery-image-xamarin/45725693#45725693
        /// </summary>
        /// <returns>The path to image</returns>
        /// <param name="uri">URI of selected image</param>
        protected String GetPathToImage(Android.Net.Uri uri)
        {
            string doc_id = "";
            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                string document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":", StringComparison.InvariantCulture) + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }

            return path;
        }
    }
}
