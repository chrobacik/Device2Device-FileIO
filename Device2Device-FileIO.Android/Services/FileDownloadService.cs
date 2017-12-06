using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace Device2DeviceFileIO.Droid.Services
{
    [Service]
    public class FileDownloadService : Service
    {
        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(async () => {
                try
                {
                    await App.GetCloudFileService().DownloadFileAsync(_cts);
                }
                catch (Android.OS.OperationCanceledException)
                {
                    if (_cts != null)
                    {
                        _cts.Cancel();
                    }
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }

            base.OnDestroy();
        }
    }
}
