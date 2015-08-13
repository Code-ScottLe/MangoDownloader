using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MangoEngine.Helpers
{
    public static class MangoWebHelpers
    {
        /*Methods*/
        public static string GetCompression(HttpResponseMessage responseMessage)
        {
            /*GEt the Compression type of the Website fro the HttpResponseMessage*/
            if (responseMessage.Content.Headers.ContentEncoding.Count != 0)
            {
                return responseMessage.Content.Headers.ContentEncoding.ElementAt(0);
            }

            else
            {
                return string.Empty;
            }
        }

        public static async Task<Stream> GetStreamAsync(HttpClient myClient, string url, TimeSpan retryInterval, int retryCount = 3)
        {
            /*Get the stream to the Website with automatic retry*/
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    var stream = await myClient.GetStreamAsync(url);

                    return stream;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await Task.Delay(retryInterval);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}
