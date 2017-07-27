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

        public async Task<List<Groups>> GetGroups()
        {
            List<Groups> groups = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/groups");

            if (response.IsSuccessStatusCode)
            {
                groups = await response.Content.ReadAsAsync<List<Groups>>();
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

        public async Task<List<Installs>> GetInstalls()
        {
            List<Installs> installs = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/installs");

            if (response.IsSuccessStatusCode)
            {
                installs = await response.Content.ReadAsAsync<List<Installs>>();
            }

            return installs;
        }

        public async Task<Installs> GetInstall(string installKey)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/installs/" + installKey);
            Installs install = null;

            if (response.IsSuccessStatusCode)
            {
                install = await response.Content.ReadAsAsync<Installs>();
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

        public async Task<Uri> CreateMessage(Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/messages", message);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<Uri> CreateGroup(Groups group)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/groups", group);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }
    }
}
