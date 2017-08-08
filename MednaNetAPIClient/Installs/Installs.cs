using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace MednaNetAPIClient.Installs
{
    public class Installs
    {
        private HttpClient client = null;

        public Installs(HttpClient client)
        {
            this.client = client;
        }
        
        /// <summary>
        /// Will register a new install with the web service and return its Install Key.
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateNewInstall()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/installs", "");
            string installKey = "";

            if (response.IsSuccessStatusCode)
            {
                installKey = await response.Content.ReadAsAsync<string>();
            }

            return installKey;
        }

        /// <summary>
        /// Returns current install.
        /// </summary>
        /// <param name="queryServer">If set to false the cached install will be returned. If set to true then the web service will be queried for 
        /// the most up to date install information.</param>
        /// <returns></returns>
        public async Task<Data.Installs> GetCurrentInstall()
        {
            
            HttpResponseMessage response = await client.GetAsync("api/v1/installs");
            Data.Installs install = null;

            if (response.IsSuccessStatusCode)
            {
                install = await response.Content.ReadAsAsync<Data.Installs>();
            }

            return install;
        }

        public async void CheckinInstall()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/installs/checkin");
        }
    }
}
