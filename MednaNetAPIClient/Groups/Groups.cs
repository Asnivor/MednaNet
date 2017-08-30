using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace MednaNetAPIClient.Groups
{
    public class Groups
    {
        
        
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
        public async Task<IEnumerable<Models.Groups>> GetGroups()
        {
            List<Models.Groups> groups = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/groups");

            if (response.IsSuccessStatusCode)
            {
                groups = await response.Content.ReadAsAsync<List<Models.Groups>>();
            }
            
            return groups;
        }

        public async Task<Models.Groups> GetGroupById(int groupId)
        {
            Models.Groups group = null;

            HttpResponseMessage response = await client.GetAsync("api/v1/groups/" + groupId.ToString());

            if (response.IsSuccessStatusCode)
            {
                group = await response.Content.ReadAsAsync<Models.Groups>();
            }

            return group;
        }

        /// <summary>
        /// Creates a new Group. Regardless of the value of group.groupOwner the owner of the group will always be the current install.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<Uri> CreateGroup(Models.Groups group)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/groups", group);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<IEnumerable<Models.Messages>> GetGroupMessages(int groupId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/groups/" + groupId.ToString() + "/messages");
            IEnumerable<Models.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Models.Messages>>();
            }

            return messages;
        }

        public async Task<IEnumerable<Models.Messages>> GetGroupMessagesFrom(int groupId, DateTime from)
        {
            string urlSafeDateString = from.ToUniversalTime().ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("api/v1/groups/" + groupId.ToString() + "/messages/from/" + urlSafeDateString);
            IEnumerable<Models.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Models.Messages>>();
            }

            return messages;
        }
    }
}
