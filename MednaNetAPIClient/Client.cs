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
        private Channels.Channels channels = null;
        private Users.Users users = null;


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

        public Channels.Channels Channels
        {
            get
            {
                return this.channels;
            }
        }

        public Users.Users Users
        {
            get
            {
                return this.users;
            }
        }

        public Client(string apiHostname, string apiPort, string installKey)
        {
            SetupClient(apiHostname, apiPort, installKey);
        }

        public Client(string apiHostname, string apiPort)
        {
            SetupClient(apiHostname, apiPort, "");
        }

        private void SetupClient(string apiHostname, string apiPort, string installKey)
        {
            url = apiHostname.Replace("http://", "").Replace("https://", "");
            port = apiPort;

            this.client.BaseAddress = new Uri("http://" + url + ":" + port + "/"); //This needs to be https
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.DefaultRequestHeaders.Add("Authorization", installKey);

            this.install = new Installs.Installs(this.client);

            this.groups = new Groups.Groups(this.client);
            this.channels = new Channels.Channels(this.client);
            this.users = new Users.Users(this.client);
        }
    }
}
