using BlazorApp.Auth;
using BlazorApp.HttpServices;
using BlazorApp.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient for Web API
builder.Services.AddHttpClient("WebAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
});

// Provide HttpClient to SimpleAuthProvider
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("WebAPI"));

// Register User Service
builder.Services.AddScoped<IUserService>(sp =>
    new HttpUserService(sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("WebAPI")));

// Register Post Service
builder.Services.AddScoped<IPostService>(sp =>
    new HttpPostService(sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("WebAPI")));

// Register Comment Service
builder.Services.AddScoped<ICommentService>(sp =>
    new HttpCommentService(sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("WebAPI")));

// Authentication
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();