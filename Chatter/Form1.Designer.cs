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
            this.messagesBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupNameText = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.groupsDropdown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.installsDropdown = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.createMessagebox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messagesBox
            // 
            this.messagesBox.Location = new System.Drawing.Point(15, 177);
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "messagesBox";
            this.messagesBox.Size = new System.Drawing.Size(312, 154);
            this.messagesBox.TabIndex = 4;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(362, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Create Group";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.createGroup);
            // 
            // groupNameText
            // 
            this.groupNameText.Location = new System.Drawing.Point(490, 77);
            this.groupNameText.Name = "groupNameText";
            this.groupNameText.Size = new System.Drawing.Size(117, 20);
            this.groupNameText.TabIndex = 6;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(362, 39);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(117, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "Create Install";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.createInstall);
            // 
            // groupsDropdown
            // 
            this.groupsDropdown.FormattingEnabled = true;
            this.groupsDropdown.Location = new System.Drawing.Point(12, 90);
            this.groupsDropdown.Name = "groupsDropdown";
            this.groupsDropdown.Size = new System.Drawing.Size(121, 21);
            this.groupsDropdown.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Groups";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(359, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Admin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Installs";
            // 
            // installsDropdown
            // 
            this.installsDropdown.FormattingEnabled = true;
            this.installsDropdown.Location = new System.Drawing.Point(12, 31);
            this.installsDropdown.Name = "installsDropdown";
            this.installsDropdown.Size = new System.Drawing.Size(121, 21);
            this.installsDropdown.TabIndex = 13;
            this.installsDropdown.SelectedIndexChanged += new System.EventHandler(this.installsDropdown_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Load Messages";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // createMessagebox
            // 
            this.createMessagebox.Location = new System.Drawing.Point(16, 371);
            this.createMessagebox.Name = "createMessagebox";
            this.createMessagebox.Size = new System.Drawing.Size(311, 20);
            this.createMessagebox.TabIndex = 15;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(311, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Create Message";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(139, 29);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "Load Installs";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.populateInstallsDropDown);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(139, 90);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(103, 23);
            this.button6.TabIndex = 18;
            this.button6.Text = "Load Groups";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.populateGroupsDropdown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 461);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.createMessagebox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.installsDropdown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupsDropdown);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.groupNameText);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.messagesBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox messagesBox;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox groupNameText;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox groupsDropdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox installsDropdown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox createMessagebox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button6;
    }
}

