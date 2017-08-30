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

        /// <summary>
        /// Returns a list of available Discord Channels.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Channels>> GetChannels()
        {
            List<Models.Channels> channels = null;
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels");

            if (response.IsSuccessStatusCode)
            {
                channels = await response.Content.ReadAsAsync<List<Models.Channels>>();
            }

            return channels;
        }

        /// <summary>
        /// Returns Discord Channel specified by the channel id. .
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <returns></returns>
        public async Task<Models.Channels> GetChannelById(int channelId)
        {
            Models.Channels channel = null;

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString());

            if (response.IsSuccessStatusCode)
            {
                channel = await response.Content.ReadAsAsync<Models.Channels>();
            }

            return channel;
        }

        /// <summary>
        /// Returns a list of all message for the specified channel. It is unlikely you will need to use this. Use GetChannelMessagesFrom or GetChannelMessagesAfterMessageId instead.
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Messages>> GetChannelMessages(int channelId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages");
            IEnumerable<Models.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Models.Messages>>();
            }

            return messages;
        }

        /// <summary>
        /// Returns the last message that has been posted in the specified channel.
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <returns></returns>
        public async Task<Models.Messages> GetChannelLastMessage(int channelId)
        {
            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/last");
            Models.Messages message = null;

            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsAsync<Models.Messages>();
            }

            return message;
        }

        /// <summary>
        /// Returns a list of channel messages for a specific channel from a point in time.
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <param name="from">A DateTime representing the point in time that you want the messages to start from. </param>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Messages>> GetChannelMessagesFrom(int channelId, DateTime from)
        {
            string urlSafeDateString = from.ToUniversalTime().ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/from/" + urlSafeDateString);
            IEnumerable<Models.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Models.Messages>>();
            }

            return messages;
        }

        /// <summary>
        /// Returns a list of channel messages that have a higher message ID (so were posted after) the message ID specified.
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <param name="messageId">This is not the Discord Message ID, it is the unique ID for the message in the MednaNetAPI.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Messages>> GetChannelMessagesAfterMessageId(int channelId, int messageId)
        {
            

            HttpResponseMessage response = await client.GetAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages/after/" + messageId.ToString());
            IEnumerable<Models.Messages> messages = null;

            if (response.IsSuccessStatusCode)
            {
                messages = await response.Content.ReadAsAsync<IEnumerable<Models.Messages>>();
            }

            return messages;
        }

        /// <summary>
        /// Submits a new message for the specified channel. Specifying a value for "name" in the message object will do nothing. The name value is taken from the Install details linked to the intstallKey specified when the api client object was created.
        /// </summary>
        /// <param name="channelId">This is not the Discord Channel ID, it is the unique ID for the channel in the MednaNetAPI.</param>
        /// <param name="message">A message object containing the message details</param>
        /// <returns></returns>
        public async Task<Models.Messages> CreateMessage(int channelId, Models.Messages message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/discord/channels/" + channelId.ToString() + "/messages", message);
            response.EnsureSuccessStatusCode();

            Models.Messages newMessage = null;

            if (response.IsSuccessStatusCode)
            {
                newMessage = await response.Content.ReadAsAsync<Models.Messages>();
            }

            return newMessage;
            
        }

    }
}
