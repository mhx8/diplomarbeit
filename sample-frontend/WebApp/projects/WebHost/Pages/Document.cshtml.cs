using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebHost.Result;

namespace WebHost.Pages
{
    public class DocumentModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DocumentModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string DevRequest = "https://documentservice-dev-diplomarbeit.azurewebsites.net/document/getall";
        private const string ProdRequest = "https://documentservice-prod-diplomarbeit.azurewebsites.net/document/getall";

        public DocumentModel(
            ILogger<DocumentModel> logger, 
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<DocumentResultModel> DevEntries { get; set; } = new();

        public List<DocumentResultModel> ProdEntries { get; set; } = new();

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

                List<DocumentResultModel>? devResult = JsonConvert.DeserializeObject<List<DocumentResultModel>>(result);
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
                  await httpResponseMessageDev.Content.ReadAsStringAsync();

                List<DocumentResultModel>? prodResult = JsonConvert.DeserializeObject<List<DocumentResultModel>>(result);
                if (prodResult != null)
                {
                    ProdEntries = prodResult;
                }
            }
        }
    }
}