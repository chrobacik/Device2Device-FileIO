using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;
using Droid = global::Android;

namespace Device2DeviceFileIO.Android.Services
{
    [Droid.App.Service]
    public class FileService : Droid.App.Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override Droid.App.StartCommandResult OnStartCommand(Intent intent, Droid.App.StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    var service = new Device2DeviceFileIO.Classes.FileIOFileService();
                    service.UploadFile(_cts.Token).Wait();
                }
                catch (Droid.OS.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        var message = new CancelledMessage();

                        Device.BeginInvokeOnMainThread(() => MessagingCenter.Send(message, "CancelledMessage"));
                    }
                }

            }, _cts.Token);

            return Droid.App.StartCommandResult.Sticky;
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
