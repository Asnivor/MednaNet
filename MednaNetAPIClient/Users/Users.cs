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

        /// <summary>
        /// Returns a list of both MedLaunch installs and Discord users. MedLaunch installs can be differentiated by having a value of 0 for the discordId.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a list of all currently online Discord Users.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Adds Discord Users to the database. THe specified list will replace all the current users in the database. Its not possible to partially update the list. This method will only work if it is being called by an install using the MednaNet-Bridge installKey.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a list of all MedLaunch installs that have checked in within the last 10 minutes. MedLaunch installs will check in every time they make a request through the client API.
        /// </summary>
        /// <returns></returns>
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
