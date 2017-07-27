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
        private Installs.Installs intsall = null;

        public Client(string apiHostname, string apiPort)
        {
            url = apiHostname.Replace("http://", "").Replace("https://", "");
            port = apiPort;
        }

        public void SetInstallKey(string installKey)
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", installKey);
        }

        public void Connect(string installKey)
        {
            client.BaseAddress = new Uri("http://" + url + ":" + port + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", installKey);
        }


     

        public async Task<Uri> CreateMessage(Data.Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat/messages", message);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        
    }
}
