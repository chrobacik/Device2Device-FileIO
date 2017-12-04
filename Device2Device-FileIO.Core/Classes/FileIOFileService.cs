using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Device2DeviceFileIO.Interfaces;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Classes
{
    public class FileIOFileService : ICloudFileService
    {
        protected String mURL = "https://file.io";
        protected String mExpiration = "14";
        protected KeyValuePair<TransferFile, QRCode> mCurrentUpload { get; set; }
        protected KeyValuePair<TransferFile, QRCode> mCurrentDownload { get; set; }

        public event EventHandler<FileOperation.DownloadFinishedMessage> DownloadFinished;
        public event EventHandler<FileOperation.CanceledMessage> OperationCanceled;
        public event EventHandler<FileOperation.ProgressMessage> OperationProgress;
        public event EventHandler<FileOperation.FailedMessage> OperationFailed;
        public event EventHandler<FileOperation.UploadFinishedMessage> UploadFinished;

        public FileIOFileService()
        {
            mCurrentUpload = new KeyValuePair<TransferFile, QRCode>(new TransferFile(), new QRCode());
            mCurrentDownload = new KeyValuePair<TransferFile, QRCode>(new TransferFile(), new QRCode());

            MessagingCenter.Subscribe<FileOperation.ProgressMessage>(this, FileOperation.PROGRESS, HandleProgressOperation);
            MessagingCenter.Subscribe<FileOperation.CanceledMessage>(this, FileOperation.CANCELED, HandleCanceledOperation);
            MessagingCenter.Subscribe<FileOperation.FailedMessage>(this, FileOperation.FAILED, HandleFailedOperation);
            MessagingCenter.Subscribe<FileOperation.UploadFinishedMessage>(this, FileOperation.UPLOAD_FINISHED, HandleUploadFinishedOperation);
            MessagingCenter.Subscribe<FileOperation.DownloadFinishedMessage>(this, FileOperation.DOWNLOAD_FINISHED, HandleDownloadFinishedOperation);
        }

        protected void HandleProgressOperation(FileOperation.ProgressMessage obj)
        {
            OperationProgress?.Invoke(this, obj);
        }

        protected void HandleCanceledOperation(FileOperation.CanceledMessage obj)
        {
            OperationCanceled?.Invoke(this, obj);
        }

        protected void HandleFailedOperation(FileOperation.FailedMessage obj)
        {
            OperationFailed?.Invoke(this, obj);
        }

        protected void HandleUploadFinishedOperation(FileOperation.UploadFinishedMessage obj)
        {
            /*
             * Parse API response and fill QRCode
             * 
             * Sample response: "{\"success\":true,\"key\":\"TwqLtU\",\"link\":\"https://file.io/TwqLtU\",\"expiry\":\"14 days\"}"
             */
            var jsonResult = JsonValue.Parse(obj.Result) as JsonObject;

            mCurrentUpload.Key.Status = new TransferStatus
            {
                State = TransferStatus.TypeState.Completed,
                Percentage = 0F
            };

            mCurrentUpload.Value.Url = jsonResult["link"];

            UploadFinished?.Invoke(this, obj);
        }

        protected void HandleDownloadFinishedOperation(FileOperation.DownloadFinishedMessage obj)
        {
            mCurrentDownload.Key.Content = obj.Content;
            mCurrentDownload.Key.Status = new TransferStatus
            {
                State = TransferStatus.TypeState.Completed,
                Percentage = 0F
            };

            DownloadFinished?.Invoke(this, obj);
        }

        /// <summary>
        /// Build upload url with expiraton query paramter, like https://file.io/?expires=14
        /// </summary>
        /// <returns>The upload URL to send file to</returns>
        protected String BuildUploadUrl()
        {
            var builder = new UriBuilder(mURL)
            {
                Query = $"expires={mExpiration}"
            };

            return builder.ToString();
        }

        /// <summary>
        /// Upload the specified TransferFile to file.io API and updates QRCode
        /// </summary>
        /// <param name="file">TransferFile object to upload</param>
        /// <param name="qrCode">QR code object to update after sucessful upload</param>
        public void Upload(TransferFile file, QRCode qrCode)
        {
            mCurrentUpload = new KeyValuePair<TransferFile, QRCode>(file, qrCode);

            try
            {
                var message = new FileOperation.UploadMessage
                {
                    FileName = file.Name,
                    Content = file.Content
                };

                MessagingCenter.Send(message, FileOperation.UPLOAD);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// Uploads the data stream asynchron to file.io API
        /// </summary>
        /// <returns>The JSON response from file.io API</returns>
        /// <param name="fileName">The name of the file</param>
        /// <param name="content">Data stream to upload</param>
        /// <param name="token">Async task cancellation token</param>
        public async Task<String> UploadFileAsync(String fileName, Stream content, CancellationToken token)
        {
            var result = "";

            try
            {
                // Create HttpClient and MultipartFormDataContent and add our file stream
                var client = new HttpClient();
                var dataContent = new MultipartFormDataContent();
                var streamContent = new StreamContent(content);
                dataContent.Add(streamContent, "file", fileName);

                // Upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(BuildUploadUrl(), dataContent);

                // Read response result as a string async into json var
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.ToString());
            }

            return result;
        }

        public void Download(TransferFile file, QRCode qrCode)
        {
            mCurrentDownload = new KeyValuePair<TransferFile, QRCode>(file, qrCode);

            try
            {
                var message = new FileOperation.DownloadMessage
                {
                    Link = qrCode.Url
                };

                MessagingCenter.Send(message, FileOperation.DOWNLOAD);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
            }
        }

        public async Task<byte[]> DownloadFileAsync(String link, CancellationToken token)
        {
            byte[] result = null;

            try
            {
                // Create HttpClient to download file
                var client = new HttpClient();

                // Download file content as byte array
                result = await client.GetByteArrayAsync(link);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e.ToString());
            }

            return result;
        }
    }
}
