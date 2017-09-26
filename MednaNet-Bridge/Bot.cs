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
    public partial class Bot : Form
    {
        private DiscordSocketClient client;
        private MednaNetAPIClient.Client apiClient;
        private List<Data.MonitoredChannel> monitoredChannels = new List<Data.MonitoredChannel>();
        private DateTime lastMessageUpdateFrom = DateTime.Now;
        private bool isUpdating = false;
        private bool isFirst = true;
        private string botInstallKey = System.Configuration.ConfigurationManager.AppSettings["botInstallKey"];
        private string mednaNetAPIUrl = System.Configuration.ConfigurationManager.AppSettings["mednaNetAPIUrl"];
        private string mednaNetAPIPort = System.Configuration.ConfigurationManager.AppSettings["mednaNetAPIPort"];
        private string botDiscordId = System.Configuration.ConfigurationManager.AppSettings["botDiscordId"];
        private string botToken = System.Configuration.ConfigurationManager.AppSettings["botToken"];
        private string guildId = System.Configuration.ConfigurationManager.AppSettings["guildId"];

        private int getUserCount = 0;

        public void SetupTimer()
        {
            System.Timers.Timer t = new System.Timers.Timer(5000);
            t.AutoReset = true;
            t.Elapsed += T_Elapsed;
            t.SynchronizingObject = this;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetMessages();

            if(getUserCount == 3)
            {
                getUserCount = 0;
                GetUsers();
                RefreshChannels();
            }
            else
            {
                getUserCount += 1;        
            }
        }

        public Bot()
        {
            InitializeComponent();
        }

        private async void GetUsers()
        {
            textBox1.AppendText("Getting users." + System.Environment.NewLine);

            SocketGuild guild = client.GetGuild(Convert.ToUInt64(guildId));
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

                userList.Add(new MednaNetAPIClient.Models.Users()
                {
                    discordId = user.Id.ToString(),
                    username = (user.Nickname == null) ? user.Nickname : user.Username,
                    isOnline = temp

                });
            }
            await apiClient.Users.AddDiscordUsers(userList);
        }

        private async void RefreshChannels()
        {
            textBox1.AppendText("Refreshing Channels..." + System.Environment.NewLine);

            var channels = await this.apiClient.Channels.GetChannels();

            foreach (var channel in channels)
            {
                var localLookup = monitoredChannels.Where(a => a.channelId == channel.id).FirstOrDefault();

                if (localLookup == null)
                {
                    monitoredChannels.Add(new Data.MonitoredChannel()
                    {
                        channelName = channel.channelName,
                        channelId = channel.id,
                        discordChannelId = channel.discordId,
                        lastMessageId = 0
                    });
                }                
            }
        }

        private async void GetMessages()
        {
            textBox1.AppendText("Getting messages." + System.Environment.NewLine);

            if (!this.isUpdating)
            {
                textBox1.AppendText("Not updating, get messages." + System.Environment.NewLine);

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
                                await ch.SendMessageAsync("**" + message.user.username + "**: " + message.message);

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

            label1.Text = "BOT RUNNING!!";

            textBox1.AppendText("BOT Started..." + System.Environment.NewLine);

            button2.Enabled = false;

            this.apiClient = new MednaNetAPIClient.Client(this.mednaNetAPIUrl, this.mednaNetAPIPort, this.botInstallKey);

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

            string token = this.botToken;
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
            try
            {
                if (message.Author.Id != Convert.ToUInt64(botDiscordId))
                {
                    if (monitoredChannels.Exists(x => x.channelName == message.Channel.Name))
                    {

                        var channel = monitoredChannels.Where(x => x.channelName == message.Channel.Name).FirstOrDefault();

                        if (channel != null)
                        {
                            string attachmentURLS = "";
                            string messagecontent = message.Content;

                            if (message.Source == MessageSource.Webhook)
                            {
                                foreach (var embed in message.Embeds)
                                {
                                    messagecontent = "";
                                    messagecontent += (embed.Author.HasValue) ? embed.Author + System.Environment.NewLine : "";
                                    messagecontent += (embed.Title.Length > 0) ? embed.Title + System.Environment.NewLine : "";
                                    messagecontent += (embed.Description.Length > 0) ? embed.Description + System.Environment.NewLine : "";
                                    messagecontent += (embed.Url.Length > 0) ? embed.Url + System.Environment.NewLine : "";
                                    messagecontent += (embed.Image.HasValue) ? embed.Image.Value.Url : "";

                                    await this.apiClient.Channels.CreateMessage(channel.channelId, new MednaNetAPIClient.Models.Messages()
                                    {
                                        channel = channel.channelId,
                                        code = this.botInstallKey,
                                        message = messagecontent,
                                        postedOn = message.CreatedAt.LocalDateTime,
                                        user = new MednaNetAPIClient.Models.Users()
                                        {
                                            discordId = message.Author.Id.ToString(),
                                            username = message.Author.Username
                                        }
                                    });
                                }
                            }
                            else
                            {
                                if (message.Attachments.Count > 0)
                                {
                                    foreach (var attachment in message.Attachments)
                                    {
                                        attachmentURLS += attachment.Url + ", ";
                                    }

                                    if (attachmentURLS.EndsWith(", "))
                                    {
                                        attachmentURLS = attachmentURLS.Substring(0, attachmentURLS.Length - 2);
                                    }

                                    messagecontent += " " + attachmentURLS;
                                }

                                await this.apiClient.Channels.CreateMessage(channel.channelId, new MednaNetAPIClient.Models.Messages()
                                {
                                    channel = channel.channelId,
                                    code = this.botInstallKey,
                                    message = messagecontent,
                                    postedOn = message.CreatedAt.LocalDateTime,
                                    user = new MednaNetAPIClient.Models.Users()
                                    {
                                        discordId = message.Author.Id.ToString(),
                                        username = message.Author.Username
                                    }
                                });
                            }




                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Do nothing...
            }
        }

            
     }

        
}

