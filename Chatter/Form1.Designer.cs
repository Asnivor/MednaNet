namespace Chatter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.installKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.message = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.userListTV = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.currentChannelName = new System.Windows.Forms.Label();
            this.usernameText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(218, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.LoadClient);
            // 
            // installKey
            // 
            this.installKey.Location = new System.Drawing.Point(12, 35);
            this.installKey.Name = "installKey";
            this.installKey.Size = new System.Drawing.Size(218, 20);
            this.installKey.TabIndex = 1;
            this.installKey.Text = "562ad8ef-12c4-4596-ac58-f5021749541b";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Install Key";
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(236, 35);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageBox.Size = new System.Drawing.Size(385, 414);
            this.messageBox.TabIndex = 4;
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(12, 400);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(218, 20);
            this.message.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 426);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(218, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Post Message";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 384);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Message";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 200);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(218, 113);
            this.treeView1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Channels";
            // 
            // userListTV
            // 
            this.userListTV.Location = new System.Drawing.Point(627, 51);
            this.userListTV.Name = "userListTV";
            this.userListTV.Size = new System.Drawing.Size(164, 398);
            this.userListTV.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(628, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Users";
            // 
            // currentChannelName
            // 
            this.currentChannelName.AutoSize = true;
            this.currentChannelName.Location = new System.Drawing.Point(236, 13);
            this.currentChannelName.Name = "currentChannelName";
            this.currentChannelName.Size = new System.Drawing.Size(0, 13);
            this.currentChannelName.TabIndex = 13;
            // 
            // usernameText
            // 
            this.usernameText.Location = new System.Drawing.Point(12, 116);
            this.usernameText.Name = "usernameText";
            this.usernameText.Size = new System.Drawing.Size(218, 20);
            this.usernameText.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Username";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 143);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(218, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "Set user name";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.updateusername);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 461);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.usernameText);
            this.Controls.Add(this.currentChannelName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userListTV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.message);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.installKey);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox installKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView userListTV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label currentChannelName;
        private System.Windows.Forms.TextBox usernameText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
    }
}

