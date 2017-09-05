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
        private List<Data.MonitoredChannel> monitoredChannels = new List<Data.MonitoredChannel>();
        private MednaNetAPIClient.Client apiClient;
        private string botInstallKey = System.Configuration.ConfigurationManager.AppSettings["botInstallKey"];
        private DateTime lastMessageUpdateFrom = DateTime.Now;
        private bool isUpdating = false;
        private bool isFirst = true;

        private int getUserCount = 0;

        public void SetupTimer()
        {
            System.Timers.Timer t = new System.Timers.Timer(5000);
            t.AutoReset = true;
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetMessages();

            if(getUserCount == 3)
            {
                getUserCount = 0;
                GetUsers();
            }
            else
            {
                getUserCount += 1;        
            }
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private async void GetUsers()
        {
            SocketGuild guild = client.GetGuild(Convert.ToUInt64(System.Configuration.ConfigurationManager.AppSettings["botDiscordId"]));
            await guild.DownloadUsersAsync();
            var users = guild.Users;

            List<MednaNetAPIClient.Models.Users> userList = new List<MednaNetAPIClient.Models.Users>();
            
            foreach(var user in users)
            {

                bool temp = false;

                if(user.Status != UserStatus.Offline)
                {
                    temp = true;
                }

                // user.Nickname;
                userList.Add(new MednaNetAPIClient.Models.Users()
                {
                    discordId = user.Id.ToString(),
                    username = user.Username,
                    isOnline = temp

                });

                
            }
            await apiClient.Users.AddDiscordUsers(userList);
        }

        private async void GetMessages()
        {
            if (!this.isUpdating)
            {
                this.isUpdating = true;


                if (isFirst)
                {
                    isFirst = false;
                    foreach (var s in monitoredChannels)
                    {
                        MednaNetAPIClient.Models.Messages message = await this.apiClient.Channels.GetChannelLastMessage(Convert.ToInt32(s.channelId));

                        if(message != null)
                        {
                            s.lastMessageId = message.id;
                        }
                        
                    }
                }
                else
                {
                    foreach (var s in monitoredChannels)
                    {
                        IEnumerable<MednaNetAPIClient.Models.Messages> messages = await this.apiClient.Channels.GetChannelMessagesAfterMessageId(s.channelId, s.lastMessageId);

                        foreach (var message in messages.ToList())
                        {
                            if (message.code != botInstallKey)
                            {
                                var ch = this.client.GetChannel(Convert.ToUInt64(s.discordChannelId)) as ISocketMessageChannel;
                                await ch.SendMessageAsync("**" + message.name + "**: " + message.message);

                                s.lastMessageId = message.id;
                            }
                        }

                    }
                }

                this.isUpdating = false;
            }
        }

        private async void StartBot(object sender, EventArgs e)
        {

            await startBot();

            this.apiClient = new MednaNetAPIClient.Client("localhost", "24215", this.botInstallKey);

            var channels = await this.apiClient.Channels.GetChannels();

            foreach(var channel in channels)
            {
                monitoredChannels.Add(new Data.MonitoredChannel()
                {
                    channelName = channel.channelName,
                    channelId = channel.id,
                    discordChannelId = channel.discordId,
                    lastMessageId = 0
                });
            }

            SetupTimer();
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

        private async void SendBotMessage(object sender, EventArgs e)
        {
            //var ch = this.client.GetChannel(335445676227952640) as ISocketMessageChannel;
            //await ch.SendMessageAsync(botMessage.Text);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            


            //Don't add the message if is a message that the bot has posted.
            if (message.Author.Id != Convert.ToUInt64(System.Configuration.ConfigurationManager.AppSettings["botDiscordId"])) //This is the Bots ID
            {
                if (monitoredChannels.Exists(x => x.channelName == message.Channel.Name))
                {
                    var channel = monitoredChannels.Where(x => x.channelName == message.Channel.Name).FirstOrDefault();

                    if (channel != null)
                    {
                        await this.apiClient.Channels.CreateMessage(channel.channelId, new MednaNetAPIClient.Models.Messages()
                        {
                            channel = channel.channelId,
                            code = this.botInstallKey,
                            message = message.Content,
                            name = message.Author.Username,
                            postedOn = message.CreatedAt.LocalDateTime
                        });
                    }
                }
            }
        }
    }
}
