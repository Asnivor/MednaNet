using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NetplayAPIClient
{
    public class Client
    {
        private HttpClient client = new HttpClient();
        private string url = "";
        private string port = "";

        

        public Client(string apiHostname, string apiPort)
        {
            url = apiHostname.Replace("http://", "").Replace("https://", "");
            port = apiPort;
        }


        public void start(string installKey)
        {
            if(installKey == "")
            {
                installKey = "supersecret";

            }

            client.BaseAddress = new Uri("http://" + url + ":" + port + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", installKey);
        }

        public async Task<List<Data.Groups>> GetGroups()
        {
            List<Data.Groups> groups = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/groups");

            if (response.IsSuccessStatusCode)
            {
                groups = await response.Content.ReadAsAsync<List<Data.Groups>>();
            }

            return groups;
        }

        /*public async Task<List<Messa>> GetGroups(string installKey)
        {
            List<Groups> groups = null;
            HttpResponseMessage response = await client.GetAsync("api/chat/groups?installKey=" + installKey);

            if (response.IsSuccessStatusCode)
            {
                groups = await response.Content.ReadAsAsync<List<Groups>>();
            }

            return groups;
        }*/

        public async Task<List<Data.Installs>> GetInstalls()
        {
            List<Data.Installs> installs = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/installs");

            if (response.IsSuccessStatusCode)
            {
                installs = await response.Content.ReadAsAsync<List<Data.Installs>>();
            }

            return installs;
        }

        public async Task<Data.Installs> GetInstall(string installKey)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/installs/" + installKey);
            Data.Installs install = null;

            if (response.IsSuccessStatusCode)
            {
                install = await response.Content.ReadAsAsync<Data.Installs>();
            }

            return install;
        }

        public async Task<string> CreateInstall()
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/installs", "");
            string installKey = "";

            if (response.IsSuccessStatusCode)
            {
                installKey = await response.Content.ReadAsAsync<string>();
            }

            return installKey;
        }


        public async Task<Data.Messages> GetMessages(string groupId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/groups/" + groupId.ToString() + "/messages");
            Data.Messages messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<Data.Messages>();
            }

            return messages;
        }


        public async Task<Uri> CreateMessage(Data.Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/messages", message);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<Uri> CreateGroup(Data.Groups group)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/groups", group);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
    }
}
