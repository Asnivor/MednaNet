using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using Discord.WebSocket;
using System.Threading;

namespace MednaNet_Bridge
{
    public partial class Form1 : Form
    {
        private DiscordSocketClient client;
        private Dictionary<string, KeyValuePair<int, string>> monitoredChannels = new Dictionary<string, KeyValuePair<int, string>>();
        private MednaNetAPIClient.Client apiClient;
        private string botInstallKey = "botInstallKey";
        private DateTime lastMessageUpdateFrom = DateTime.Now;
        private bool isUpdating = false;
        
        public Form1()
        {
            InitializeComponent();
            
        }

        void run()
        {
            CancellationTokenSource cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            RepeatActionEvery(() => getMessages(), TimeSpan.FromSeconds(1), cancellation.Token).Wait();
            
        }

        public static async Task RepeatActionEvery(Action action, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                action();
                Task task = Task.Delay(interval, cancellationToken);

                try
                {
                    await task;
                }

                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        private async void getMessages()
        {
            //
            if (!this.isUpdating)
            {
                this.isUpdating = true;
                foreach (var s in monitoredChannels)
                {
                    IEnumerable<MednaNetAPIClient.Data.Messages> messages = await this.apiClient.Channels.GetChannelMessagesFrom(Convert.ToInt32(s.Value), this.lastMessageUpdateFrom);

                    foreach (var message in messages.ToList())
                    {
                        if (message.code != botInstallKey)
                        {
                            var ch = this.client.GetChannel(Convert.ToUInt64(s.Value.Value)) as ISocketMessageChannel;
                            await ch.SendMessageAsync(message.name + ": " + System.Environment.NewLine + message.message);
                        }
                    }

                }

                this.isUpdating = false;
            }
        }

        private async void StartBot(object sender, EventArgs e)
        {
            await startBot();

            monitoredChannels.Add("development", new KeyValuePair<int, string>(1, "335445676227952640"));

           // monitoredChannels.Add("development");
            this.apiClient = new MednaNetAPIClient.Client("localhost", "24215", this.botInstallKey);
        }

        private async Task startBot()
        {
            this.client = new DiscordSocketClient();
            this.client.Log += Log;
            this.client.MessageReceived += MessageReceived;

            string token = botToken.Text;
            await this.client.LoginAsync(TokenType.Bot, token);
            await this.client.StartAsync();
        }

        private async void sendBotMessage(object sender, EventArgs e)
        {
            var ch = this.client.GetChannel(335445676227952640) as ISocketMessageChannel;
            await ch.SendMessageAsync(botMessage.Text);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            //message.Channel.Name

            if (monitoredChannels.ContainsKey(message.Channel.Name))
            {
                //IEnumerable<MednaNetAPIClient.Data.Messages> messages = await this.apiClient.Channels.GetChannelMessages(monitoredChannels[message.Channel.Name]);
                this.apiClient.Channels.CreateMessage(monitoredChannels[message.Channel.Name].Key, new MednaNetAPIClient.Data.Messages()
                {
                    channel = monitoredChannels[message.Channel.Name].Key,
                    code = this.botInstallKey,
                    message = message.Content,
                    name = message.Author.Username,
                    postedOn = message.CreatedAt.LocalDateTime
                });
            }

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {

           /* MedLaunchEntities db = new MedLaunchEntities();
            var currentId = 0;
            while (true)
            {
                // code here




                var result = from q in db.messages
                             where q.id > currentId
                             orderby q.id ascending
                             select q;

                foreach (var s in result)
                {
                    currentId = s.id;

                    var ch = _client.GetChannel(335445676227952640) as ISocketMessageChannel;
                    await ch.SendMessageAsync("<" + s.name + "> " + s.message1);
                }

                Thread.Sleep(5000);
            }*/
        }

       
    }
}
