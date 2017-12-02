using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Device2DeviceFileIO.Classes;
using Xamarin.Forms;


namespace Device2DeviceFileIO.Droid.Services
{
    [Android.App.ServiceAttribute]
    public class FileService : Android.App.Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override Android.App.StartCommandResult OnStartCommand(Intent intent, Android.App.StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    var service = new Device2DeviceFileIO.Classes.FileIOFileService();
                    service.UploadFile(_cts.Token).Wait();
                }
                catch (Android.OS.OperationCanceledException)
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

            return Android.App.StartCommandResult.Sticky;
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
