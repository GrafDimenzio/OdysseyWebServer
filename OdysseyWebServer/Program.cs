using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using OdysseyWebServer;
using OdysseyWebServer.Components;
using OdysseyWebServer.ServerCommunication;

var builder = WebApplication.CreateBuilder(args);

Configuration.LoadConfig(builder.Configuration);
if (string.IsNullOrEmpty(Configuration.Server))
{
    Console.WriteLine("Please configure a hostname or ip that connects to a SMOO Server and restart the application.");
    while (true)
    {
        Thread.Sleep(10000);
    }
}

Console.WriteLine($"Trying to establish communication with Super Mario Odyssey Online Server: {Configuration.Server}:{Configuration.Port}");
Cache.Initialize().GetAwaiter().GetResult();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromDays(1);
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

if (File.Exists("/.dockerenv"))
{
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo("/data/DataProtection-Keys"))
        .SetApplicationName("OdysseyWebServer"); 
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();