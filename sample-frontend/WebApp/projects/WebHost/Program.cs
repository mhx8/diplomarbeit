using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("DevelopmentClient").ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler()
    {
        Proxy = new WebProxy("http://webgw3.swisslife.ch:8080", true)
        { UseDefaultCredentials = true }
    };
});

builder.Services.AddHttpClient("AzureClient");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
