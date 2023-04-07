using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebHost.Result;

namespace WebHost.Pages
{
    public class UserModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const string DevRequest = "https://userservice-dev-diplomarbeit.azurewebsites.net/user/getall";
        private const string ProdRequest = "https://userservice-prod-diplomarbeit.azurewebsites.net/user/getall";

        public UserModel(
            ILogger<UserModel> logger, 
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<UserResultModel> DevEntries { get; set; } = new();

        public List<UserResultModel> ProdEntries { get; set; } = new();

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

                List<UserResultModel>? devResult = JsonConvert.DeserializeObject<List<UserResultModel>>(result);
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

                List<UserResultModel>? prodResult = JsonConvert.DeserializeObject<List<UserResultModel>>(result);
                if (prodResult != null)
                {
                    ProdEntries = prodResult;
                }
            }
        }
    }
}