using base_aspnet_electron.Services;
using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddElectron();
builder.Services.AddSingleton<ElectronService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapFallbackToFile("index.html");

if (HybridSupport.IsElectronActive)
{
    var electronWindowService = app.Services.GetRequiredService<ElectronService>();
    await electronWindowService.CreateWindowAsync();
}

app.Run();