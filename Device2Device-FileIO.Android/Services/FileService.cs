using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Droid.Services
{
    [Service]
    public class FileService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    var operation = intent.GetStringExtra("operation");

                    if (operation == FileOperation.UPLOAD)
                    {
                        var fileName = intent.GetStringExtra("file");
                        var stream = new MemoryStream(intent.GetByteArrayExtra("content"));
                        var service = App.GetCloudFileService();

                        Task<String> task = service.UploadFileAsync(fileName, stream, _cts.Token);
                        task.Wait();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(new FileOperation.UploadFinishedMessage
                            {
                                Result = task.Result

                            }, FileOperation.UPLOAD_FINISHED);
                        });
                    }
                    else if (operation == FileOperation.DOWNLOAD)
                    {
                        var link = intent.GetStringExtra("link");
                        var service = App.GetCloudFileService();

                        Task<byte[]> task = service.DownloadFileAsync(link, _cts.Token);
                        task.Wait();

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(new FileOperation.DownloadFinishedMessage
                            {
                                Content = task.Result

                            }, FileOperation.DOWNLOAD_FINISHED);
                        });
                    }
                }
                catch (Android.OS.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            MessagingCenter.Send(new FileOperation.CanceledMessage(), FileOperation.CANCELED);
                        });
                    }
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }

            base.OnDestroy();
        }
    }
}
