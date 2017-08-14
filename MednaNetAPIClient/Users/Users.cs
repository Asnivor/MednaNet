using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MednaNetAPIClient.Users
{
    public class Users
    {
        private HttpClient client = null;

        public Users(HttpClient client)
        {
            this.client = client;
        }

        //public List<Data.Users> GetAllUsers()
        //{
//
       // }

        public async Task<List<Data.Users>> GetDiscordUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/users");

            List<Data.Users> users = null;

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<Data.Users>>();
            }

            return users;
        }

        public async Task<List<Data.Users>> GetMedLaunchUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/installs/checkedin");

            List<Data.Installs> users = null;

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<Data.Users>>();
            }

            return users;
        }
    }
}
