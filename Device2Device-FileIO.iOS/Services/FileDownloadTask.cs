using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Device2DeviceFileIO.iOS.Services
{
    public class FileDownloadTask
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("FileDownloadTask", OnExpiration);

            try
            {
                await App.GetCloudFileService().DownloadFileAsync(_cts);
            }
            catch (OperationCanceledException)
            {
                _cts.Cancel();
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}
