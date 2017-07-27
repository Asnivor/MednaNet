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

        
    }
}
