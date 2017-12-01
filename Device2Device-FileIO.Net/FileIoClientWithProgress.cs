using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Device2DeviceFileIO.net.progress;

namespace Device2DeviceFileIO.net
{
    public class FileIoClientWithProgress
    {
        /// <summary>
        /// Occurs every time the client sending data is making progress.
        /// (is internally linked to the event of the same name inside <see cref="ProgressMessageHandler" />)
        /// </summary>
        public event EventHandler<HttpProgressEventArgs> HttpSendProgress;
       


        public async Task<string> UploadFile(string fullPath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var name = System.IO.Path.GetFileName(fullPath);
            var dataContent = new MultipartFormDataContent();
            var streamContent = new ByteArrayContent(fileBytes);
            dataContent.Add(streamContent, "file", name);

            return await UploadFileInternal(dataContent).ConfigureAwait(false);
        }

        public async Task<string> UploadFileFromStream(Stream stream, string name)
        {
            var dataContent = new MultipartFormDataContent();
            var streamContent = new StreamContent(stream);
            dataContent.Add(streamContent, "file", name);

            return await UploadFileInternal(dataContent).ConfigureAwait(false);
        }

        private async Task<string> UploadFileInternal(HttpContent content)
        {
            var handler = new ProgressMessageHandler();
            
            handler.HttpSendProgress += (o, a) =>
            {
                this.HttpSendProgress?.Invoke(o, a);
            };

            var client = new HttpClient(handler);
            var response = await client.PostAsync("https://file.io", content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
