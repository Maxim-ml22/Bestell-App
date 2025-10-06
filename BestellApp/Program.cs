using BestellApp.Components;
using System;
using System.Linq;
using BestellApp;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<BestellDbContext>(options =>
    options.UseSqlite($"Data Source=BestellApp.db"));

builder.Services.AddScoped<BestellDbService>();
builder.Services.AddScoped<User>();


/*
// Configure Microsoft Identity
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));*/

/*// Register GraphService
builder.Services.AddScoped<GraphServiceClient>(sp =>
{
    var accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
    return new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
    {
        var result = await accessTokenProvider.AccessTokenAsync();
        if (result.TryGetToken(out var token))
        {
            requestMessage.Headers.Authorization = new
            System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
        }
    }));
});
*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();