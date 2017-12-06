using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Device2DeviceFileIO.iOS.Services
{
    public class FileUploadTask
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("FileUploadTask", OnExpiration);

            try
            {
                await App.GetCloudFileService().UploadFileAsync(_cts);
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
