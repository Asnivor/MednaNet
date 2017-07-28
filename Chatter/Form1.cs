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
        MednaNetAPIClient.Client client = new MednaNetAPIClient.Client("localhost", "24215");
        MednaNetAPIClient.Data.Installs currentInstall = null;

        public Form1()
        {
            InitializeComponent();
            /*client.start("");*/

        }



        private void CreateInstall(object sender, EventArgs e)
        {

        }
    }
}
