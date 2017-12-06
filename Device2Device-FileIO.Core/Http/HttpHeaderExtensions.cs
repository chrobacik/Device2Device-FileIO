using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Device2DeviceFileIO.Http
{
    internal static class HttpHeaderExtensions
    {
        public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> header in fromHeaders)
            {
                toHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
