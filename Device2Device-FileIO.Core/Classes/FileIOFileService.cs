using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Device2DeviceFileIO.Interfaces;
using Xamarin.Forms;

namespace Device2DeviceFileIO.Classes
{
    public class FileIOFileService : IFileService
    {
        public async Task<String> UploadFileAsync(Stream stream)
        {
            var url = "https://file.io";
            var result = "";

            try
            {
                // Create new HttpClient and MultipartFormDataContent and add our file stream
                var client = new HttpClient();
                var dataContent = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                dataContent.Add(streamContent, "file", "file.txt");

                // Upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, dataContent);

                // Read response result as a string async into json var
                result = response.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(result);

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Caught: " + e.ToString());
            }

            return result;

        }

        public async Task UploadFile(CancellationToken token)
        {
            await Task.Run(async () => {

                for (long i = 1; i < 100; i++)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(250);
                    var message = new FileProgressMessage()
                    {
                        Percentage = (i / 100F)
                    };

                    Device.BeginInvokeOnMainThread(() => {
                        
                        MessagingCenter.Send<FileProgressMessage>(message, "FileProgressMessage");
                    });
                }
            }, token).ContinueWith((arg1) => {

                MessagingCenter.Send<CancelledMessage>(new CancelledMessage(), "CancelledMessage");
            });
        }
    }
}
