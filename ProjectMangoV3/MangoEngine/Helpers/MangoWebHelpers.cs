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

        public static async Task<KeyValuePair<Stream,string>> GetCompressedStreamAsync(HttpClient myClient, string url, TimeSpan retryInterval, int retryCount = 3)
        {
            /*Get a stream from source that will be compressed*/

            //List of exceptions
            List<Exception> exceptions = new List<Exception>();

            //Place holder.
            HttpResponseMessage myRespond = null;

            //Loop with the given amount of trials
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    //Make a GET request to the given URL
                    myRespond = await myClient.GetAsync(url);

                    //if success, get out of the loop
                    break;

                }

                catch(Exception ex)
                {
                    //exception was caught, add to the list 
                    exceptions.Add(ex);

                    //Wait until try again
                    await Task.Delay(retryInterval);
                }
            }

            //check if the respond is still null.
            if (myRespond == null)
            {
                //is null, throw the entire list of exceptions.
                throw new AggregateException(exceptions);

            }

            //Got the request, get the compression Type
            string compressed = GetCompression(myRespond);

            //Get the stream
            Stream sourceStream = await myRespond.Content.ReadAsStreamAsync();

            //return the pair.
            return new KeyValuePair<Stream, string>(sourceStream, compressed);
            
        }
    }
}
