using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device2DeviceFileIO.Interfaces;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Device2DeviceFileIO.Classes;
using Android.Provider;

namespace Device2DeviceFileIO.Droid.Classes
{
    class ShareHandler : IShareHandler
    {

        private TransferFile File { get; set; }

        public void HandleIntent(Activity activity)
        {
            if (activity.Intent.Action == Intent.ActionSend)
            {
                File = new TransferFile();

                var fileUri = activity.Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                var subject = activity.Intent.GetStringExtra(Intent.ExtraSubject);

                //get file name and type
                var cr = activity.ContentResolver;
                string[] projection = { MediaStore.MediaColumns.DisplayName, MediaStore.MediaColumns.MimeType };
                using (var metadataCursor = cr.Query(fileUri, projection, null, null, null))
                {
                    if (metadataCursor?.MoveToFirst() == true)
                    {
                        File.Name = metadataCursor.GetString(
                            Array.IndexOf(projection, MediaStore.MediaColumns.DisplayName));
                        File.Type = metadataCursor.GetString(
                            Array.IndexOf(projection, MediaStore.MediaColumns.MimeType));
                    }
                }

                //get file data
                var firstClipDataItem = activity.Intent.ClipData.GetItemAt(0);
                var itemStream = cr.OpenInputStream(firstClipDataItem.Uri);

                var localStream = new System.IO.MemoryStream();
                itemStream.CopyTo(localStream);

                File.Content = localStream.ToArray() ?? new byte[] { };
                File.Size = File.Content.Length;


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
        {
            throw new NotImplementedException();
        }

        public TransferFile ReceiveFile()
        {
            return File;
        }

    }
}