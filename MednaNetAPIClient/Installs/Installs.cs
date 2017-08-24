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
        /// Creates a new install and registers it with MednaNet. 
        /// </summary>
        public async Task<Data.Installs> CreateNewInstall()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/installs", "");
            Data.Installs install = null;

            if (response.IsSuccessStatusCode)
            {
                install = await response.Content.ReadAsAsync<Data.Installs>();
            }

            return install;
        }

        /// <summary>
        /// Returns an Install specified by installKey. If the install key is passed as a blank string a new install will be created. 
        /// </summary>
        public async Task<Data.Installs> GetCurrentInstall(string installKey)
        {
            if(installKey == "")
            {
                return await CreateNewInstall();
            }
            else
            {
                HttpResponseMessage response = await client.GetAsync("api/v1/installs");
                Data.Installs install = null;

                if (response.IsSuccessStatusCode)
                {
                    install = await response.Content.ReadAsAsync<Data.Installs>();
                }

                return install;
            }
        }
    }
}
