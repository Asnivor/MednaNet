using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetplayAPIClient;

namespace Chatter
{
    public partial class Form1 : Form
    {
        NetplayAPIClient.Client client = new NetplayAPIClient.Client("localhost", "24215");
        NetplayAPIClient.Installs currentInstall = null;

        public Form1()
        {
            InitializeComponent();
            client.start("");
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var message = new NetplayAPIClient.Messages();
            message.code = "test";
            message.message = createMessagebox.Text;
            
           

            await client.CreateMessage(message);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var group = new NetplayAPIClient.Groups();
            group.groupDescription = "Group description";
            group.groupName = "This is the group Name";
            group.groupOwner = 2;

            await client.CreateGroup(group);
        }

        

        private async void createInstall(object sender, EventArgs e)
        {
            string installKey = await client.CreateInstall();

            MessageBox.Show(installKey);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private async void populateInstallsDropDown(object sender, EventArgs e)
        {
            List<NetplayAPIClient.Installs> installs =  await client.GetInstalls();

            installsDropdown.Items.Clear();

            foreach (var install in installs)
            {
                installsDropdown.Items.Add(install.code);
            }
        }

        private async void populateGroupsDropdown(object sender, EventArgs e)
        {
            List<Groups> groups = await client.GetGroups();

            groupsDropdown.Items.Clear();

            foreach (var group in groups)
            {
                groupsDropdown.Items.Add(group.groupName);
            }
        }

        private async void createGroup(object sender, EventArgs e)
        {
            //Get install


            var group = new NetplayAPIClient.Groups();
            group.groupDescription = "";
            group.groupName = groupNameText.Text;
            group.groupOwner = currentInstall.id;

            await client.CreateGroup(group);
        }

        private async void installsDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentInstall = await client.GetInstall(installsDropdown.Text);
            client = null;
            client = new NetplayAPIClient.Client("localhost", "24215");
            client.start(currentInstall.code);
        }
    }
}
