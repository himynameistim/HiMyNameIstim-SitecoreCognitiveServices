using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HiMyNameIstim_SitecoreCognitiveServices
{
    public class CognitiveServicesHelper
    {
        //TODO: Get <Subscription Key> from Sitecore settings item.
        const string subscriptionKey = "";

        //TODO: Get uri base from Sitecore settings item
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/analyze";

        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file to analyze.</param>
        public static JToken MakeAnalysisRequest(byte[] imageByteData)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. A third optional parameter is "details".
                string requestParameters =
                    "visualFeatures=Categories,Description,Color";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(imageByteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = client.PostAsync(uri, content).Result;
                }

                // Get the JSON response.
                string contentString = response.Content.ReadAsStringAsync().Result;

                // Return the results
                return JToken.Parse(contentString).ToString();
            }
            catch (Exception e)
            {
                //TODO: Add exception logging

                throw;
            }
        }
    }
}
