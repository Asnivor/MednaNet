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

        public async Task<List<Models.Users>> GetAllUsers()
        {
            List<Models.Users> allUsers = new List<Models.Users>();

            var discordUsers = await GetDiscordUsers();
            var medLaunchUsers = await GetMedLaunchUsers();

            if (discordUsers != null)
            {
                allUsers.AddRange(discordUsers);
            }

            if (medLaunchUsers != null)
            {
                allUsers.AddRange(medLaunchUsers);
            }

            

            return allUsers;
        }

        public async Task<List<Models.Users>> GetDiscordUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/users");

            List<Models.Users> users = null;

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<Models.Users>>();
            }

            return users;
        }

        public async Task<bool> AddDiscordUsers(List<MednaNetAPIClient.Models.Users> users)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/discord/users", users);
            response.EnsureSuccessStatusCode();

            bool usersAdded = false;

            if (response.IsSuccessStatusCode)
            {
                usersAdded = true;
            }
            else
            {
                usersAdded = false;
            }

            return usersAdded;
        }

        public async Task<List<Models.Users>> GetMedLaunchUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/users");

            List<Models.Users> users = null;

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<Models.Users>>();
            }

            return users;
        }
    }
}
