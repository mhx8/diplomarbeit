using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebHost.Result;

namespace WebHost.Pages
{
    public class BackendModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BackendModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string DevRequest = "https://backendservice-dev-diplomarbeit.azurewebsites.net/weatherforecast/getall";
        private const string ProdRequest = "https://backendservice-prod-diplomarbeit.azurewebsites.net/weatherforecast/getall";

        public BackendModel(
            ILogger<BackendModel> logger, 
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<BackendResultModel> DevEntries { get; set; } = new();

        public List<BackendResultModel> ProdEntries { get; set; } = new();

        public async Task OnGetAsync()
        {
            HttpClient httpClient;
            if (_webHostEnvironment.IsDevelopment())
            {
                httpClient = _httpClientFactory.CreateClient("DevelopmentClient");
            }
            else
            {
                httpClient = _httpClientFactory.CreateClient("AzureClient");
            }

            HttpRequestMessage httpRequestMessageDev = new(HttpMethod.Get, DevRequest);
            HttpResponseMessage httpResponseMessageDev = await httpClient.SendAsync(httpRequestMessageDev);

            if (httpResponseMessageDev.IsSuccessStatusCode)
            {
                string result =
                    await httpResponseMessageDev.Content.ReadAsStringAsync();

                List<BackendResultModel>? devResult = JsonConvert.DeserializeObject<List<BackendResultModel>>(result);
                if(devResult != null)
                {
                    DevEntries = devResult;
                }
            }

            HttpRequestMessage httpRequestMessageProd = new(HttpMethod.Get, ProdRequest);
            HttpResponseMessage httpResponseMessageProd = await httpClient.SendAsync(httpRequestMessageProd);

            if (httpResponseMessageProd.IsSuccessStatusCode)
            {
                string result =
                  await httpResponseMessageProd.Content.ReadAsStringAsync();

                List<BackendResultModel>? prodResult = JsonConvert.DeserializeObject<List<BackendResultModel>>(result);
                if (prodResult != null)
                {
                    ProdEntries = prodResult;
                }
            }
        }
    }
}