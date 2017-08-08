using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MednaNetAPIClient.Channels
{
    public class Channels
    {
        private HttpClient client = null;

        public Channels(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Data.Channels>> GetChannels()
        {
            List<Data.Channels> channels = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels");

            if (response.IsSuccessStatusCode)
            {
                channels = await response.Content.ReadAsAsync<List<Data.Channels>>();
            }

            return channels;
        }

        public async Task<Data.Channels> GetChannelById(int channelId)
        {
            Data.Channels channel = null;

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString());

            if (response.IsSuccessStatusCode)
            {
                channel = await response.Content.ReadAsAsync<Data.Channels>();
            }

            return channel;
        }


        public async Task<IEnumerable<Data.Messages>> GetChannelMessages(int channelId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages");
            IEnumerable<Data.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Data.Messages>>();
            }

            return messages;
        }

        public async Task<Data.Messages> GetChannelLastMessage(int channelId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/last");
            Data.Messages message = null;

            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsAsync<Data.Messages>();
            }

            return message;
        }

        public async Task<IEnumerable<Data.Messages>> GetChannelMessagesFrom(int channelId, DateTime from)
        {
            string urlSafeDateString = from.ToUniversalTime().ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/from/" + urlSafeDateString);
            IEnumerable<Data.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Data.Messages>>();
            }

            return messages;
        }

        public async Task<IEnumerable<Data.Messages>> GetChannelMessagesAfterMessageId(int channelId, int messageId)
        {
            

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/after/" + messageId.ToString());
            IEnumerable<Data.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Data.Messages>>();
            }

            return messages;
        }

        public async Task<Data.Messages> CreateMessage(int channelId, Data.Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages", message);
            response.EnsureSuccessStatusCode();

            Data.Messages newMessage = null;

            if (response.IsSuccessStatusCode)
            {
                newMessage = await response.Content.ReadAsAsync<Data.Messages>();
            }

            return newMessage;
            
        }

    }
}
