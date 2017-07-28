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


        public Groups.Groups Group
        {
            get
            {
                return this.groups;
            }
        }

        public Installs.Installs Install
        {
            get
            {
                return this.install;
            }
        }

        public Client(string apiHostname, string apiPort, string installKey)
        {
            url = apiHostname.Replace("http://", "").Replace("https://", "");
            port = apiPort;

            client.BaseAddress = new Uri("http://" + url + ":" + port + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", installKey);

            this.install = new Installs.Installs(client);
            this.groups = new Groups.Groups(client);
        }
    }
}
