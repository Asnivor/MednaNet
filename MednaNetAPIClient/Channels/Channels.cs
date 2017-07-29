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


        public async Task<IEnumerable<Data.Messages>> GetChannelMessages(int groupId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + groupId.ToString() + "/messages");
            IEnumerable<Data.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Data.Messages>>();
            }

            return messages;
        }

        public async Task<IEnumerable<Data.Messages>> GetChannelMessagesFrom(int groupId, DateTime from)
        {
            string urlSafeDateString = from.ToUniversalTime().ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + groupId.ToString() + "/messages/from/" + urlSafeDateString);
            IEnumerable<Data.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Data.Messages>>();
            }

            return messages;
        }
    }
}
