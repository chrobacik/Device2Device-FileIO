using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Device2DeviceFileIO.Classes;
using Device2DeviceFileIO.Http;
using Device2DeviceFileIO.Interfaces;
using Xamarin.Forms;

namespace Device2DeviceFileIO.FileIO
{
    public class FileService : ICloudFileService
    {
        private const String HOST = "https://file.io";
        private const int DEFAULT_EXPIRATION_IN_DAYS = 14;

        public class TransferOperation
        {
            public TransferFile File { get; set; }
            public QRCode Code { get; set; }

            public TransferOperation(TransferFile file, QRCode code)
            {
                File = file;
                Code = code;
            }

            public bool IsRunning()
            {
                return (File != null && File.Status.State == TransferStatus.TypeState.Transfering);
            }
        }

        public TransferOperation CurrentUpload { get; protected set; }
        public TransferOperation CurrentDownload { get; protected set; }
        protected IFileCryptor FileCryptor { get; set; }

        public event EventHandler<FileOperation.UploadFinishedEventArgs> UploadFinished;
        public event EventHandler<FileOperation.UploadProgressEventArgs> UploadProgress;
        public event EventHandler<FileOperation.DownloadFinsihedEventArgs> DownloadFinished;
        public event EventHandler<FileOperation.DownloadProgressEventArgs> DownloadProgress;

        public FileService() : this(new PlaceboFileCryptor())
        {
        }

        public FileService(IFileCryptor cryptor)
        {
            CurrentUpload = new TransferOperation(new TransferFile(), new QRCode());
            CurrentDownload = new TransferOperation(new TransferFile(), new QRCode());
            FileCryptor = cryptor;
        }

        /// <summary>
        /// Builds upload url with expiraton query paramter
        /// 
        /// Example: https://file.io/?expires=14
        /// </summary>
        /// <returns>The upload URL to send file to</returns>
        protected String BuildUploadURL(QRCode qRCode)
        {
            int days = (qRCode.ExpirationDate - DateTime.Now).Days;

            // Use default expiration when days are negativ
            if (days < 0)
            {
                days = DEFAULT_EXPIRATION_IN_DAYS;
            }
            
            var builder = new UriBuilder(HOST)
            { 
                Query = $"expires={days.ToString()}",
                Port = -1 // This will remove any port number
            };

            return builder.ToString();
        }

        /// <summary>
        /// Verifies the download URL to permit download files from any host 
        /// </summary>
        /// <returns>The download URL to get file from</returns>
        /// <param name="url">URL.</param>
        protected String VerifyDownloadURL(String url)
        {
            if (url.StartsWith(HOST, StringComparison.InvariantCulture) == false)
            {
                throw new ApplicationException($"Invalid URL. Download file from specified host is not allowed.");
            }

            return url;
        }

        /// <summary>
        /// Uploads the specified TransferFile to file.io API
        /// </summary>
        /// <param name="file">TransferFile object to upload</param>
        public void Upload(TransferFile file)
        {
            Upload(file, DateTime.Now.AddDays(Convert.ToDouble(DEFAULT_EXPIRATION_IN_DAYS)));
        }

        /// <summary>
        /// Uploads the specified TransferFile to file.io API
        /// </summary>
        /// <param name="file">TransferFile object to upload</param>
        /// <param name="expiration">Exipration date for uploaded file</param>
        public void Upload(TransferFile file, DateTime expiration)
        {
            // Check if upload is already running
            if (CurrentUpload?.IsRunning() == true)
            {
                throw new ApplicationException("An upload process is already running.");
            }

            if (DateTime.Now.CompareTo(expiration) != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(expiration), expiration, "Expiration date must be greater than actual date.");
            }

            // Create QR code, link will be added when upload has finished
            var qRCode = new QRCode { FileName = file.Name, ExpirationDate = expiration };

            CurrentUpload = new TransferOperation(file, qRCode);

            // Change file state before start upload process
            file.Status.State = TransferStatus.TypeState.Transfering;
            file.Status.Percentage = 0F;

            MessagingCenter.Send(new FileOperation.UploadMessage(), FileOperation.UPLOAD);
        }

        /// <summary>
        /// Cancels the currently running file upload process
        /// </summary>
        public void CancelUpload()
        {
        }

        /// <summary>
        /// Downloads the specified file by QR code from file.io API
        /// </summary>
        /// <param name="qRCode">QR code with information to download file</param>
        public TransferFile Download(QRCode qRCode)
        {
            return Download(qRCode, new TransferFile());
        }

        /// <summary>
        /// Downloads the specified file by QR code from file.io API
        /// </summary>
        /// <param name="qRCode">QR code with information to download file</param>
        /// <param name="file">TransferFile to use for download file content</param>
        public TransferFile Download(QRCode qRCode, TransferFile file)
        {
            // Check if upload is already running
            if (CurrentDownload?.IsRunning() == true)
            {
                throw new ApplicationException("A download process is already running.");
            }

            // Change file state before start download process
            file.Status.State = TransferStatus.TypeState.Transfering;
            file.Status.Percentage = 0F;

            CurrentDownload = new TransferOperation(file, qRCode);

            MessagingCenter.Send(new FileOperation.DownloadMessage(), FileOperation.DOWNLOAD);

            return file;
        }

        /// <summary>
        /// Cancels the currently running file download process
        /// </summary>
        public void CancelDownload()
        {
        }

        /// <summary>
        /// Uploads the file content asynchron to file.io API
        /// </summary>
        /// <param name="cancellation">Async task cancellation token</param>
        public async Task UploadFileAsync(CancellationTokenSource cancellation)
        {
            try
            {
                // Create custom HTTP handler to get upload progress
                var handler = new ProgressMessageHandler();

                // Handle upload progress while uploading file
                handler.HttpSendProgress += (sender, e) =>
                {
                    CurrentUpload.File.Status.Percentage = e.GetProgressPercentage();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UploadProgress?.Invoke(sender, new FileOperation.UploadProgressEventArgs
                        {
                            File = CurrentUpload.File
                        });
                    });
                };

                // Create HttpClient and MultipartFormDataContent and add our file stream
                var client = new HttpClient(handler);
                var dataContent = new MultipartFormDataContent();

                // Enrypt file content for upload
                FileCryptor.Encrypt(CurrentUpload.File, CurrentUpload.Code);

                var streamContent = new StreamContent(new MemoryStream(CurrentUpload.File.Content));
                dataContent.Add(streamContent, "file", CurrentUpload.File.Name);

                // Upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(BuildUploadURL(CurrentUpload.Code), dataContent, cancellation.Token);

                // Ensure reponse status code is 200, else throw an HttpRequestException
                response.EnsureSuccessStatusCode();

                // Read response result as a string async into json var
                var result = FileUploadResult.ParseJson(response.Content.ReadAsStringAsync().Result);

                // Set link to QR code
                CurrentUpload.Code.Url = result.Link;
                CurrentUpload.File.Status.State = TransferStatus.TypeState.Completed;
            }
            catch (HttpRequestException httpEx)
            {
                CurrentUpload.File.Status.ErrorMessage = $"HTTP error: {httpEx.Message}";
                CurrentUpload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            catch (TaskCanceledException)
            {
                CurrentUpload.File.Status.State = TransferStatus.TypeState.Aborted;
            }
            catch (Exception ex)
            {
                CurrentUpload.File.Status.ErrorMessage = $"Unknown error: {ex.Message}";
                CurrentUpload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UploadFinished?.Invoke(this, new FileOperation.UploadFinishedEventArgs
                    {
                        File = CurrentUpload.File,
                        Code = CurrentUpload.Code
                    });
                });
            }
        }

        /// <summary>
        /// Downloads the file content asynchron from file.io API
        /// </summary>
        /// <param name="cancellation">Async task cancellation token</param>
        public async Task DownloadFileAsync(CancellationTokenSource cancellation)
        {
            try
            {
                // Create custom HTTP handler to get upload progress
                var handler = new ProgressMessageHandler();

                // Handle upload progress while uploading file
                handler.HttpReceiveProgress += (sender, e) =>
                {
                    CurrentDownload.File.Status.Percentage = e.GetProgressPercentage();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DownloadProgress?.Invoke(sender, new FileOperation.DownloadProgressEventArgs
                        {
                            File = CurrentDownload.File
                        });
                    });
                };

                // Create HttpClient to download file
                var client = new HttpClient(handler);

                // Download file content as byte array
                var response = await client.GetAsync(VerifyDownloadURL(CurrentDownload.Code.Url), cancellation.Token);

                // Handle HTTP 404 status code when file was not found or already downloaded
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new FileNotFoundException(response.Content.ReadAsStringAsync().Result);
                }

                // Ensure reponse status code is 200, else throw an HttpRequestException
                response.EnsureSuccessStatusCode();

                // Request was successful, read file byte array and header informations
                CurrentDownload.File.Content = response.Content.ReadAsByteArrayAsync().Result;

                // Try to get header values for file name and size
                CurrentDownload.File.Name = response.Content.Headers.ContentDisposition?.FileName;
                CurrentDownload.File.Size = response.Content.Headers.ContentLength ?? 0;

                // Decrypt file content after download
                FileCryptor.Decrypt(CurrentDownload.File, CurrentDownload.Code);

                // Everything works fine, change state to completed
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Completed;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                CurrentDownload.File.Status.ErrorMessage = fileNotFoundEx.Result.ErrorMessage;
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            catch (HttpRequestException httpEx)
            {
                CurrentDownload.File.Status.ErrorMessage = $"HTTP error: {httpEx.Message}";
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            catch (TaskCanceledException)
            {
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Aborted;
            }
            catch (ApplicationException appEx)
            {
                CurrentDownload.File.Status.ErrorMessage = appEx.Message;
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            catch (Exception ex)
            {
                CurrentDownload.File.Status.ErrorMessage = $"Unknown error: {ex.Message}";
                CurrentDownload.File.Status.State = TransferStatus.TypeState.Failed;
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DownloadFinished?.Invoke(this, new FileOperation.DownloadFinsihedEventArgs
                    {
                        File = CurrentDownload.File,
                        Code = CurrentDownload.Code
                    });
                });
            }
        }
    }
}
