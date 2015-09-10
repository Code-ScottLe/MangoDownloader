using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace HttpDownloader
{
    public static class HttpDownloadHelper
    {
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
