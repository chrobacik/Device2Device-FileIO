using Android.App;
using Android.Content;
using Android.Provider;
using Android.Webkit;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Interfaces;
using System;

namespace Device2DeviceFileIO.Droid.Classes
{
    class ShareHandler : IShareHandler
    {

        private TransferFile File { get; set; }

        private string GetMimeTypeFromUri(Android.Net.Uri uri)
        {
            //this does not work since there is a name clash with getType form Uri and GetType from .Net
            //return uri.GetType();

            var fileExtension = MimeTypeMap.GetFileExtensionFromUrl(uri.ToString());
            if (fileExtension != null)
                return MimeTypeMap.Singleton.GetMimeTypeFromExtension(fileExtension);

            return String.Empty;
        }

        public void HandleIntent(Activity activity)
        {
            if (activity.Intent.Action == Intent.ActionSend)
            {
                File = new TransferFile();
            
                var fileUri = activity.Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                var subject = activity.Intent.GetStringExtra(Intent.ExtraSubject);

                

                //get file name and type
                var cr = activity.ContentResolver;
                string[] projection = { MediaStore.MediaColumns.DisplayName, MediaStore.MediaColumns.MimeType, MediaStore.MediaColumns.Size };
                using (var metadataCursor = cr.Query(fileUri, projection, null, null, null))
                {
                    if (metadataCursor?.MoveToFirst() == true)
                    {
                        File.Name = metadataCursor.GetString(
                            Array.IndexOf(projection, MediaStore.MediaColumns.DisplayName));
                        File.Size = metadataCursor.GetLong(
                            Array.IndexOf(projection, MediaStore.MediaColumns.MimeType));

                        //getting type here doew not work. Column was empty when tested with jpg from browser
                        //File.Type = metadataCursor.GetString(
                        //    Array.IndexOf(projection, MediaStore.MediaColumns.MimeType));
                        //using this
                        File.Type = GetMimeTypeFromUri(fileUri);
                    }
                }


                //get file data
                var firstClipDataItem = activity.Intent.ClipData.GetItemAt(0);
                var itemStream = cr.OpenInputStream(firstClipDataItem.Uri);

                var localStream = new System.IO.MemoryStream();
                itemStream.CopyTo(localStream);

                File.Content = localStream.ToArray() ?? new byte[] { };
                File.Size = File.Content.Length;


                //Notify Observers
                OnShareFileRequestReceived();
            }
        }


        public ShareHandler()
        {        }

        public event EventHandler ShareFileRequestReceived = delegate { };

        private void OnShareFileRequestReceived()
        {
            ShareFileRequestReceived(this, new EventArgs());
        }

        public void ProvideFile(TransferFile transferFile)
        {  /*
            var sendFileIntent = new Intent(Intent.ActionSend);
            sendFileIntent.SetType(transferFile.Type);
            var uri = Android.Net.Uri.FromFile((transferFile.Content);
            sendFileIntent.PutExtra(Intent.ExtraStream, uri);

          
             * 
             * 
             * 
             * 
             * 
            Intent CreateIntent()
            {
                var sendFileIntent = new Intent(Intent.ActionSend);
                sendFileIntent.

                sendPictureIntent.SetType("image/*");
                var uri = Android.Net.Uri.FromFile(GetFileStreamPath("image.png"));
                sendPictureIntent.PutExtra(Intent.ExtraStream, uri);
                return sendPictureIntent;
            }
            */
            throw new NotImplementedException();
        }

        public TransferFile ReceiveFile()
        {
            return File;
        }

    }
}