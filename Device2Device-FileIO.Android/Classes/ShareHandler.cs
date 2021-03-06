﻿using Android.App;
using Android.Content;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Webkit;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Droid.Classes;
using Device2DeviceFileIO.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(ShareHandler))]
namespace Device2DeviceFileIO.Droid.Classes
{
    class ShareHandler : IShareHandler
    {
        private IFileHandler _FileHandler;
        public IFileHandler FileHandler { get { return _FileHandler ?? (_FileHandler = new FileHandler()); } }

        private TransferFile SharedFile { get; set; }

        private string GetMimeTypeFromUri(Android.Net.Uri uri)
        {
            //this does not work since there is a name clash with getType form Uri and GetType from .Net
            //return uri.GetType();

            return GetMimeTypeFromExtension(uri.ToString());
        }

        private string GetMimeTypeFromExtension(String PathUrl)
        {
            var fileExtension = MimeTypeMap.GetFileExtensionFromUrl(PathUrl);
            if (fileExtension != null)
                return MimeTypeMap.Singleton.GetMimeTypeFromExtension(fileExtension);

            return String.Empty;

        }
        public void HandleShareIntent(Activity currentActivity)
        {
            if (currentActivity.Intent.Action == Intent.ActionSend)
            {
                var fileUri = currentActivity.Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;

                //if fileUri is null, it seems there is no file 
                if (fileUri == null) return;

                var file = new TransferFile();
                var cr = currentActivity.ContentResolver;

                //get file name and type
                //ListMetaData(cr, fileUri);

                string[] projection = { MediaStore.MediaColumns.DisplayName, MediaStore.MediaColumns.MimeType, MediaStore.MediaColumns.Size };
                using (var metadataCursor = cr.Query(fileUri, projection, null, null, null))
                {
                    if (metadataCursor?.MoveToFirst() == true)
                    {

                        var colIndex = metadataCursor.GetColumnIndex(MediaStore.MediaColumns.DisplayName);
                        if (colIndex > 0) file.Name = metadataCursor.GetString(colIndex);

                        colIndex = metadataCursor.GetColumnIndex(MediaStore.MediaColumns.Size);
                        if (colIndex > 0) file.Size = metadataCursor.GetLong(colIndex);

                        //getting type here does not work. Column was empty when tested with jpg from browser
                        //colIndex = metadataCursor.GetColumnIndex(MediaStore.MediaColumns.MimeType);
                        //File.Type = metadataCursor.GetString(colIndex);
                        //using this
                        //cr.GetType(fileUri);
                        file.Type = GetMimeTypeFromUri(fileUri);
                    }

                    
                }

                file.Status = new TransferStatus() { State = TransferStatus.TypeState.ReceivedFromOS, Percentage = 0 };

                //get file data
                var firstClipDataItem = currentActivity.Intent.ClipData.GetItemAt(0);
                if (firstClipDataItem?.Uri != null)
                {

                    var itemStream = cr.OpenInputStream(firstClipDataItem.Uri);

                    var localStream = new System.IO.MemoryStream();
                    itemStream.CopyTo(localStream);

                    file.Content = localStream.ToArray() ?? new byte[] { };

                    if (String.IsNullOrWhiteSpace(file.Name)) file.Name = fileUri.LastPathSegment;
                    if (file.Size <= 0) file.Size = file.Content.Length;

                    FileHandler.Save(file);
                    file.Status.Percentage = 1;

                    SharedFile = file;

                    //Notify Observers
                    OnShareFileRequestReceived();
                }
            }
        }

        public ShareHandler() { }

       private void ListMetaData(ContentResolver resolver, Android.Net.Uri uri)
        {
            using (var metadataCursor = resolver.Query(uri, null, null, null, null))
            {
                if (metadataCursor?.MoveToFirst() == true)
                {
                    String[] columns = metadataCursor.GetColumnNames();
                    foreach (var c in columns)
                    {
                        var data = metadataCursor.GetString(metadataCursor.GetColumnIndex(c));
                        Console.WriteLine($"Metadata-Column: {c} '{data}'");
                    }
                }
            }
        }

        public event EventHandler ShareFileRequestReceived = delegate { };

        private void OnShareFileRequestReceived()
        {
            ShareFileRequestReceived(this, new EventArgs());
        }

        public Task ProvideFile(TransferFile transferFile)
        {
            var context = Android.App.Application.Context;
            
            FileHandler.Save(transferFile);
            
            var sendFileIntent = new Intent(Intent.ActionSend);

            if (string.IsNullOrWhiteSpace(transferFile.Type))
                transferFile.Type = GetMimeTypeFromExtension(transferFile.StoragePath);
 
            var file = new Java.IO.File(transferFile.StoragePath);
            var uri = FileProvider.GetUriForFile(context, context.PackageName, file);

            sendFileIntent.SetType(transferFile.Type);
            sendFileIntent.PutExtra(Intent.ExtraStream, uri);
  
            sendFileIntent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
            sendFileIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.NoHistory);
            sendFileIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);


            sendFileIntent.PutExtra(Intent.ExtraSubject, transferFile.Name);
            sendFileIntent.PutExtra(Intent.ExtraText, "Datei Empfangen von File.IO");

            var chooserIntent = Intent.CreateChooser(sendFileIntent, "Teilen mit...");
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);

            var intent = chooserIntent;

            context.StartActivity(intent);

            return Task.FromResult(true);

        }


        public TransferFile ReceiveFile()
        {
            return SharedFile;
        }

    }
}