using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MednaNetAPIClient.Groups
{
    class Groups
    {
        private string installKey = "";
        private List<Data.Groups> installGroups = new List<Data.Groups>();
        private HttpClient client = null;

        public Groups(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Returns a list of Groups associated with install.
        /// </summary>
        /// <param name="queryServer">If set to false the cached group list will be returned. If set to true then the web service will be queried for 
        /// the most up to date group list.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Data.Groups>> GetGroups(bool queryServer)
        {
            List<Data.Groups> groups = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/groups");

            if (response.IsSuccessStatusCode)
            {
                groups = await response.Content.ReadAsAsync<List<Data.Groups>>();
            }
            installGroups = groups;

            return installGroups;
        }

        /// <summary>
        /// Returns a list of Groups associated with install. If this is the first time GetGroups has been called it will query the web service, otherwise
        /// the cached group list will be returned.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Data.Groups>> GetGroups()
        {
            return await GetGroups(false);
        }

        /// <summary>
        /// Creates a new Group. Regardless of the value of group.groupOwner the owner of the group will always be the current install.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<Uri> CreateGroup(Data.Groups group)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/groups", group);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

    }
}
