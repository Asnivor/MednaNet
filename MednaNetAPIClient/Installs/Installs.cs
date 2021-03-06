﻿using System;
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
        public async Task<Models.Installs> CreateNewInstall()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/installs", "");
            Models.Installs install = null;

            if (response.IsSuccessStatusCode)
            {
                install = await response.Content.ReadAsAsync<Models.Installs>();
            }
            else
            {
                throw new System.Exception(response.StatusCode.ToString() + ": " + response.Content.ToString());
            }

            return install;
        }

        /// <summary>
        /// Returns an Install specified by installKey. If the install key is passed as a blank string a new install will be created. 
        /// </summary>
        public async Task<Models.Installs> GetCurrentInstall(string installKey)
        {
            if(installKey == "")
            {
                return await CreateNewInstall();
            }
            else
            {
                HttpResponseMessage response = await client.GetAsync("api/v1/installs");
                Models.Installs install = null;

                if (response.IsSuccessStatusCode)
                {
                    install = await response.Content.ReadAsAsync<Models.Installs>();
                }
                else
                {
                    throw new System.Exception(response.StatusCode.ToString() + ": " + response.Content.ToString());
                }

                return install;
            }
        }

        /// <summary>
        /// Updates the username for the install. Currently only username can be updated, all other values will be ignored.
        /// </summary>
        /// <param name="install"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUsername(Models.Installs install)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("api/v1/installs", install);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new System.Exception(response.StatusCode.ToString() + ": " + response.Content.ToString());
            }
        }
    }
}
