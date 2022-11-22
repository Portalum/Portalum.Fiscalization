using Portalum.Fiscalization.Docker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var countryCode = ConfigurationManager.AppSettings["CountryCode"];
            Task.Run(async () => await this.StartDockerAsync(countryCode));
        }

        private async Task StartDockerAsync(string countryCode)
        {
            await DockerHelper.CleanupAsync();

            if (countryCode.Equals("at", StringComparison.OrdinalIgnoreCase))
            {
                await DockerHelper.StartEfstaEfrAsync("efstait/efsta_at");
            }
            else if (countryCode.Equals("de", StringComparison.OrdinalIgnoreCase))
            {
                await DockerHelper.StartEfstaEfrAsync("efstait/efsta_de");

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Sign_require", "TSE_Sim"),
                    //new KeyValuePair<string, string>("Offline", ""),
                    //new KeyValuePair<string, string>("Badge", ""),
                    //new KeyValuePair<string, string>("Proxy", ""),
                    //new KeyValuePair<string, string>("TaxId", ""),
                    //new KeyValuePair<string, string>("Sign_Cfg", ""),
                    //new KeyValuePair<string, string>("RN_TT", ""),
                    //new KeyValuePair<string, string>("Password", ""),
                    //new KeyValuePair<string, string>("Update_disable", ""),
                    //new KeyValuePair<string, string>("HttpServer_Port", "5620"),
                    //new KeyValuePair<string, string>("RootPath", ""),
                    //new KeyValuePair<string, string>("DiskQuota", "1000"),
                    //new KeyValuePair<string, string>("Attributes", ""),
                    //new KeyValuePair<string, string>("Notes", ""),
                    //new KeyValuePair<string, string>("@save", "save")
                });

                await Task.Delay(2000);

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://localhost:5618/profile?RN=def", formContent);
            }
        }
    }
}
