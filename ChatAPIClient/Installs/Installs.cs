using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace MednaNetAPIClient.Installs
{
    class Installs
    {
        private Data.Installs install = null;
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
        public async Task<Data.Installs> GetCurrentInstall(bool queryServer)
        {
            if(this.install == null || queryServer == true)
            {
                HttpResponseMessage response = await client.GetAsync("api/v1/installs");
                Data.Installs install = null;

                if (response.IsSuccessStatusCode)
                {
                    install = await response.Content.ReadAsAsync<Data.Installs>();
                }

                this.install = install;
            }

            return this.install;
        }

        /// <summary>
        /// Returns the current install. If this is the first time GetCurrentInstall has been called it will query the web service, otherwise
        /// the cached group list will be returned.
        /// </summary>
        /// <returns></returns>
        public async Task<Data.Installs> GetCurrentInstall()
        {
            return await GetCurrentInstall(false);
        }

        public void CheckinInstall()
        {

        }
    }
}
