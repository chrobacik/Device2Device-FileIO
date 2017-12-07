using Android.App;
using Android.Content;
using Android.Provider;
using Android.Webkit;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Droid.Classes;
using Device2DeviceFileIO.Interfaces;
using System;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(ShareHandler))]
namespace Device2DeviceFileIO.Droid.Classes
{
    class ShareHandler : IShareHandler
    {
        public  Activity CurrentActivity { protected get;  set; }
        public IFileHandler FileHandler { protected get; set; }
        private TransferFile SharedFile { get; set; }

        private string GetMimeTypeFromUri(Android.Net.Uri uri)
        {
            //this does not work since there is a name clash with getType form Uri and GetType from .Net
            //return uri.GetType();

            var fileExtension = MimeTypeMap.GetFileExtensionFromUrl(uri.ToString());
            if (fileExtension != null)
                return MimeTypeMap.Singleton.GetMimeTypeFromExtension(fileExtension);

            return String.Empty;
        }

        public void HandleShareIntent()
        {
            if (CurrentActivity.Intent.Action == Intent.ActionSend)
            {
                var fileUri = CurrentActivity.Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;

                //if fileUri is null, it seems there is no file 
                if (fileUri == null) return;

                var file = new TransferFile();
                var cr = CurrentActivity.ContentResolver;

                 //get file name and type
                string[] projection = { MediaStore.MediaColumns.DisplayName, MediaStore.MediaColumns.MimeType, MediaStore.MediaColumns.Size };
                using (var metadataCursor = cr.Query(fileUri, projection, null, null, null))
                {
                    if (metadataCursor?.MoveToFirst() == true)
                    {
                        file.Name = metadataCursor.GetString(
                            Array.IndexOf(projection, MediaStore.MediaColumns.DisplayName));
                        file.Size = metadataCursor.GetLong(
                            Array.IndexOf(projection, MediaStore.MediaColumns.Size));

                        //getting type here does not work. Column was empty when tested with jpg from browser
                        //File.Type = metadataCursor.GetString(
                        //    Array.IndexOf(projection, MediaStore.MediaColumns.MimeType));
                        //using this
                        file.Type = GetMimeTypeFromUri(fileUri);
                    }
                }

                //get file data
                var firstClipDataItem = CurrentActivity.Intent.ClipData.GetItemAt(0);
                var itemStream = cr.OpenInputStream(firstClipDataItem.Uri);

                var localStream = new System.IO.MemoryStream();
                itemStream.CopyTo(localStream);

                file.Content = localStream.ToArray() ?? new byte[] { };
                file.Size = file.Content.Length;

                FileHandler.Save(file);
                SharedFile = file;

                //Notify Observers
                OnShareFileRequestReceived();
            }
        }

        public ShareHandler() { }

        public void SetContext(Activity currentActivity, IFileHandler fileHandler)
        {
            this.CurrentActivity = currentActivity;
            this.FileHandler = fileHandler;
        }

        public ShareHandler(Activity currentActivity, IFileHandler fileHandler)
        {
            SetContext(currentActivity, fileHandler);
        }

        public event EventHandler ShareFileRequestReceived = delegate { };

        private void OnShareFileRequestReceived()
        {
            ShareFileRequestReceived(this, new EventArgs());
        }

        public void ProvideFile(TransferFile transferFile)
        {
            FileHandler.Load(transferFile);

            var sendFileIntent = new Intent(Intent.ActionSend);
            sendFileIntent.SetType(transferFile.Type);
            var file = new Java.IO.File(transferFile.StoragePath);
            var uri = Android.Net.Uri.FromFile(file);
            sendFileIntent.PutExtra(Intent.ExtraStream, uri);

            CurrentActivity.StartActivity(sendFileIntent);
        }

        public TransferFile ReceiveFile()
        {
            return SharedFile;
        }

    }
}