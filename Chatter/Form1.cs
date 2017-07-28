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
        


        public Form1()
        {
            InitializeComponent();
           

        }



        private async void LoadClient(object sender, EventArgs e)
        {
            this.client = new MednaNetAPIClient.Client("localhost", "24215", "562ad8ef-12c4-4596-ac58-f5021749541b");

            IEnumerable<MednaNetAPIClient.Data.Groups> groups = await this.client.Group.GetGroups();

            List<TreeNode> nodes = new List<TreeNode>();

            foreach(var g in groups)
            {
                nodes.Add(new TreeNode(g.id.ToString()));
            }

            TreeNode treeNode = new TreeNode("Groups", nodes.ToArray());
            treeView1.Nodes.Add(treeNode);
            treeView1.NodeMouseClick += loadMessages;
        }

        private async void loadMessages(object sender, TreeNodeMouseClickEventArgs e)
        {
            int number;

            bool result = Int32.TryParse(e.Node.Text, out number);
            if (result)
            {
                IEnumerable<MednaNetAPIClient.Data.Messages> messages = await client.Group.GetGroupMessages(Convert.ToInt32(e.Node.Text));

                foreach (var message in messages)
                {
                    messageBox.Text += message.name + " @ " + message.postedOn.ToString() + System.Environment.NewLine;
                    messageBox.Text += message.message + System.Environment.NewLine;
                    messageBox.Text += System.Environment.NewLine;
                    messageBox.Text += System.Environment.NewLine;
                }
            }

            
        }
    }
}
