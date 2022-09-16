using SignalR.Hubs;
using SignalR.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();
builder.Services.AddSignalR().AddJsonProtocol();
builder.Services.AddSingleton<Account>(new Account("John"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<AccountHub>("/accountHub");

app.Run();
