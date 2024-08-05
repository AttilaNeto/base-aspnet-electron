using ElectronNET.API;
using ElectronNET.API.Entities;

namespace base_aspnet_electron.Services
{
    public class ElectronService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public ElectronService(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async Task CreateWindowAsync()
        {
            var windowOptions = new BrowserWindowOptions
            {
                Width = 800,
                Height = 600,
                Title = "Projeto Base Electron",
                Icon = "caminho/para/seu/icone.png",
                AutoHideMenuBar = true
            };

            var window = await Electron.WindowManager.CreateWindowAsync(windowOptions);

            if (_environment.IsDevelopment())
            {
                var clientAppUrl = _configuration.GetValue<string>("ClientApp:Url");

                if (!string.IsNullOrEmpty(clientAppUrl))
                {
                    window.LoadURL(clientAppUrl);
                }
                else
                {
                    Console.WriteLine("CLIENT_APP_URL não está definida.");
                    window.Close();
                }
            }

            window.OnClosed += () =>
            {
                Electron.App.Quit();
            };
        }
    }
}