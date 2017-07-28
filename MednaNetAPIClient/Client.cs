using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MednaNetAPIClient
{
    public class Client
    {
        private HttpClient client = new HttpClient();
        private string url = "";
        private string port = "";

        private Groups.Groups groups = null;
        private Installs.Installs install = null;

        public Client(string apiHostname, string apiPort)
        {
            url = apiHostname.Replace("http://", "").Replace("https://", "");
            port = apiPort;

            client.BaseAddress = new Uri("http://" + url + ":" + port + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.install = new Installs.Installs(client);
            this.groups = new Groups.Groups(client);
        }

        public void SetInstallKey(string installKey)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", installKey);
        }

        public async Task<Data.Installs> CreateInstall()
        {
            string installKey = await this.install.CreateNewInstall();
            SetInstallKey(installKey);
            return await this.install.GetCurrentInstall(true);
        }

        public async Task<Data.Installs> GetInstall(bool forceUpdate)
        {
            return await this.install.GetCurrentInstall(forceUpdate);
        }
     
        public async Task<IEnumerable<Data.Groups>> GetAllGroups()
        {
            return await this.groups.GetGroups();
        }

        

        public async Data.Groups GetGroupById(int groupId)
        {

        }



        public async Task<Uri> CreateMessage(Data.Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/messages", message);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        
    }
}
