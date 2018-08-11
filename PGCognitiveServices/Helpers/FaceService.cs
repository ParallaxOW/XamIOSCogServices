using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PGCognitiveServices.Helpers
{
    public class FaceService
    {
        HttpClient _client;

        JsonSerializerSettings s_settings;

        public FaceService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("ocp-apim-subscription-key", SettingsService.FaceAPIKey);
            _client.BaseAddress = new Uri(SettingsService.FaceAPIEndpoint);

            s_settings = new JsonSerializerSettings();
        }

        public async Task<Face[]> DetectAsync(Stream imageStream)
        {
            var requestUrl =
              $"{SettingsService.FaceAPIEndpoint}/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=emotion,age,gender";
            return await SendRequestAsync<Stream, Face[]>(HttpMethod.Post, requestUrl, imageStream);
        }

        async Task<TResponse> SendRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string requestUrl, TRequest requestBody)
        {
            var request = new HttpRequestMessage(httpMethod, SettingsService.FaceAPIEndpoint);
            request.RequestUri = new Uri(requestUrl);
            if (requestBody != null)
            {
                if (requestBody is Stream)
                {
                    request.Content = new StreamContent(requestBody as Stream);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                }
                else
                {
                    // If the image is supplied via a URL
                    request.Content = new StringContent(JsonConvert.SerializeObject(requestBody, s_settings), Encoding.UTF8, "application/json");
                }
            }

            HttpResponseMessage responseMessage = await _client.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = null;
                if (responseMessage.Content != null)
                {
                    responseContent = await responseMessage.Content.ReadAsStringAsync();
                }
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    return JsonConvert.DeserializeObject<TResponse>(responseContent, s_settings);
                }
                return default(TResponse);
            }
            else
            {

            }
            return default(TResponse);
        }
    }
}
