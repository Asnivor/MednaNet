using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MednaNetAPIClient;

namespace Chatter
{
    public partial class Form1 : Form
    {
        MednaNetAPIClient.Client client;
        private Dictionary<int, int> lastChannelMessageId = new Dictionary<int, int>();
        private int currentChannel = 0;
        private MednaNetAPIClient.Data.Installs currentInstall = null;
        private int userQueryCount = 3;
        System.Timers.Timer t = null;

        public Form1()
        {
            InitializeComponent();

            t = new System.Timers.Timer(5000);
            t.SynchronizingObject = this; 
            t.AutoReset = true;
            t.Elapsed += T_Elapsed; ;
            
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if(this.currentChannel != 0)
            {
                displayMessages(this.currentChannel);
            }

            if(userQueryCount == 3)
            {
                userQueryCount = 0;
                displayUsers();
            }
            else
            {
                userQueryCount++;
            }
        }

        private async void LoadClient(object sender, EventArgs e)
        {
            this.client = new MednaNetAPIClient.Client("localhost", "24215", "562ad8ef-12c4-4596-ac58-f5021749541b");

            currentInstall = await this.client.Install.GetCurrentInstall("562ad8ef-12c4-4596-ac58-f5021749541b");

            IEnumerable<MednaNetAPIClient.Data.Channels> channels = await this.client.Channels.GetChannels();

            

            List<TreeNode> nodes = new List<TreeNode>();

            foreach(var c in channels)
            {

                var tempNode = new TreeNode(c.channelName);
                tempNode.Tag = c.id;

                nodes.Add(tempNode);
            }

            TreeNode treeNode = new TreeNode("Channels", nodes.ToArray());
            treeView1.Nodes.Add(treeNode);
            treeView1.NodeMouseClick += loadMessages;

            t.Start();
        }

        private void loadMessages(object sender, TreeNodeMouseClickEventArgs e)
        {
           if(e.Node.Tag != null)
            {
                this.currentChannel = (int)e.Node.Tag;
                messageBox.Text = "";
                displayMessages(this.currentChannel);
            }
        }

        private async void displayMessages(int channelId)
        {
            if (lastChannelMessageId.ContainsKey(channelId))
            {

                if(messageBox.Text == "")
                {
                    IEnumerable<MednaNetAPIClient.Data.Messages> messages = await client.Channels.GetChannelMessages(channelId);

                    foreach (var message in messages)
                    {
                        messageBox.AppendText(message.name + " @ " + message.postedOn.ToString() + System.Environment.NewLine);
                        messageBox.AppendText(message.message + System.Environment.NewLine);
                        messageBox.AppendText(System.Environment.NewLine);

                        if (lastChannelMessageId.ContainsKey(channelId))
                        {
                            lastChannelMessageId[channelId] = message.id;
                        }
                        else
                        {
                            lastChannelMessageId.Add(channelId, message.id);
                        }

                    }
                }
                else
                {
                    IEnumerable<MednaNetAPIClient.Data.Messages> messages = await client.Channels.GetChannelMessagesAfterMessageId(channelId, lastChannelMessageId[channelId]);

                    foreach (var message in messages)
                    {
                        messageBox.AppendText(message.name + " @ " + message.postedOn.ToString() + System.Environment.NewLine);
                        messageBox.AppendText(message.message + System.Environment.NewLine);
                        messageBox.AppendText(System.Environment.NewLine);

                        lastChannelMessageId[channelId] = message.id;
                    }
                }

                
            }
            else
            {
                IEnumerable<MednaNetAPIClient.Data.Messages> messages = await client.Channels.GetChannelMessages(channelId);

                foreach (var message in messages)
                {
                    messageBox.AppendText(message.name + " @ " + message.postedOn.ToString() + System.Environment.NewLine);
                    messageBox.AppendText(message.message + System.Environment.NewLine);
                    messageBox.AppendText(System.Environment.NewLine);

                    if (lastChannelMessageId.ContainsKey(channelId))
                    {
                        lastChannelMessageId[channelId] = message.id;
                    }
                    else
                    {
                        lastChannelMessageId.Add(channelId, message.id);
                    }

                }
            }
        }

        private async void displayUsers()
        {
            List<MednaNetAPIClient.Data.Users> userList = await client.Users.GetAllUsers();

            userListTV.Nodes.Clear();

            List<TreeNode> users = new List<TreeNode>();

            foreach(var user in userList)
            {
                var tempNode = new TreeNode(user.username);
                tempNode.Tag = user.userId;
                users.Add(tempNode);
            }

            TreeNode treeNode = new TreeNode("Users", users.ToArray());
            userListTV.Nodes.Add(treeNode);
            userListTV.ExpandAll();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            DateTime postTime = DateTime.Now;

            messageBox.AppendText(currentInstall.username + " @ " + postTime.ToString() + System.Environment.NewLine);
            messageBox.AppendText(message.Text + System.Environment.NewLine);
            messageBox.AppendText(System.Environment.NewLine);

            MednaNetAPIClient.Data.Messages newMessage = await client.Channels.CreateMessage(1, new MednaNetAPIClient.Data.Messages()
            {
                channel = this.currentChannel,
                code = installKey.Text,
                message = message.Text,
                name = "", //This does nothing, the username is install specific and the API takes it from the database.
                postedOn = postTime
            });

            lastChannelMessageId[this.currentChannel] = newMessage.id;
        }
    }
}
