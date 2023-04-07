using Application.Iteration;
using Application.Release;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Net;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        private const string AzureDevOpsOrganizationUrl = "https://dev.azure.com/diplomarbeitrm/";
        private const string Pat = "<PERSONALACCESSTOKEN>";

        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IIterationManager, IterationManager>();
            serviceCollection.AddTransient<IReleaseManager, ReleaseManager>();

            return serviceCollection;
        }

        public static IServiceCollection AddAzureDevopConnection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(_ => new VssConnection(new Uri(AzureDevOpsOrganizationUrl), GetVssHttpMessageHandler(_), null));
            return serviceCollection;
        }

        private static VssHttpMessageHandler GetVssHttpMessageHandler(IServiceProvider serviceProvider)
        {
            IWebHostEnvironment webHostEnvironment = serviceProvider.GetService<IWebHostEnvironment>();

            if (webHostEnvironment.IsDevelopment())
            {
                VssHttpMessageHandler.DefaultWebProxy = new WebProxy(
                     "http://webgw3.swisslife.ch:8080",
                            true,
                     new string[] { "https://dev.azure.com/diplomarbeitrm/" },
                     CredentialCache.DefaultCredentials);
            }

            return new VssHttpMessageHandler(
                new VssBasicCredential(string.Empty, Pat),
                new VssClientHttpRequestSettings() { BypassProxyOnLocal = true, SuppressFedAuthRedirects = true, AllowAutoRedirect = true }
            );
        }
    }
}
