using ElectronNET.API;
using ElectronNET.API.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddElectron();

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
    CreateWindow();
}

app.Run();

async void CreateWindow()
{
    var clientAppUrl = builder.Configuration.GetValue<string>("ClientApp:Url");

    if (!string.IsNullOrEmpty(clientAppUrl))
    {
        Console.WriteLine($"ClientApp está rodando em: {clientAppUrl}");
    }
    else
    {
        Console.WriteLine("CLIENT_APP_URL não está definida.");
        Console.WriteLine($"{clientAppUrl}");
    }

    var windowOptions = new BrowserWindowOptions
    {
        Width = 800,
        Height = 600,
        Title = "Projeto Base Electron", // Alterar o título da janela
        Icon = "caminho/para/seu/icone.png", // Alterar o ícone da janela
        AutoHideMenuBar = true // Esconder o menu
    };

    var window = await Electron.WindowManager.CreateWindowAsync(windowOptions);

    window.OnClosed += () =>
    {
        Electron.App.Quit();
    };
}