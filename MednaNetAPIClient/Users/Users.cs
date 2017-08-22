﻿using System;
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

        public async Task<List<Data.Users>> GetAllUsers()
        {
            List<Data.Users> allUsers = new List<Data.Users>();

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

        public async Task<bool> AddDiscordUsers(List<MednaNetAPIClient.Data.Users> users)
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

        public async Task<List<Data.Users>> GetMedLaunchUsers()
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/users");

            List<Data.Users> users = null;

            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<Data.Users>>();
            }

            return users;
        }
    }
}
