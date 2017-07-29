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

namespace MednaNet_Bridge
{
    public partial class Form1 : Form
    {
        private DiscordSocketClient client;
        private Dictionary<string, int> monitoredChannels = new Dictionary<string, int>();
        private MednaNetAPIClient.Client apiClient;

        System.Timers.Timer r = new System.Timers.Timer(1000);
        public Form1()
        {
            InitializeComponent();
        }

        private async void StartBot(object sender, EventArgs e)
        {
            await startBot();

            monitoredChannels.Add("development", 1);

           // monitoredChannels.Add("development");
            this.apiClient = new MednaNetAPIClient.Client("localhost", "24215", "botInstallKey");
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
               // MednaNetAPIClient.Data.Messages messages = this.apiClient.
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
