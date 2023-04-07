using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebHost.Result;

namespace WebHost.Pages
{
    public class ActiveDirectoryModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ActiveDirectoryModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string DevRequest = "https://activedirectoryservice-dev-diplomarbeit.azurewebsites.net/activedirectory/getall";
        private const string ProdRequest = "https://activedirectoryservice-prod-diplomarbeit.azurewebsites.net/activedirectory/getall";

        public ActiveDirectoryModel(
            ILogger<ActiveDirectoryModel> logger, 
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<ActiveDirectoryResultModel> DevEntries { get; set; } = new();

        public List<ActiveDirectoryResultModel> ProdEntries { get; set; } = new();

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

                List<ActiveDirectoryResultModel>? devResult = JsonConvert.DeserializeObject<List<ActiveDirectoryResultModel>>(result);
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

                List<ActiveDirectoryResultModel>? prodResult = JsonConvert.DeserializeObject<List<ActiveDirectoryResultModel>>(result);
                if (prodResult != null)
                {
                    ProdEntries = prodResult;
                }
            }
        }
    }
}