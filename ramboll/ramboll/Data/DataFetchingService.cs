
using Newtonsoft.Json;
using ramboll.Models;

namespace ramboll.Data
{
    public class DataFetchingService : IDataFetchingServicecs
    {
        private readonly HttpClient httpClient;
        private const string url = "https://localhost:7112/Data";
        public DataFetchingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<DataResponse> GetDataAsync(List<string> parameters)
        {
            var newUrl = url;

            if (parameters.Any())
            {
                newUrl += GenerateParameterUrl(parameters);
            }

            var response = await httpClient.GetStringAsync(newUrl);

            var desereilizedResponse = DeserializeResponse(response);

            return desereilizedResponse;
        }

        private DataResponse DeserializeResponse(string result)
        {
            var response = JsonConvert.DeserializeObject<DataResponse>(result);

            return response;
        }

        private string GenerateParameterUrl(List<string> parameters)
        {
            var paramtersUrl = string.Empty;
            for (var index = 0; index<parameters.Count; index++)
            {
                if (index==0)
                {
                    paramtersUrl += $"?{parameters[index]}";
                }
                else
                {
                    paramtersUrl += $"&{parameters[index]}";
                }
            }
            return paramtersUrl;
        }
    }
}