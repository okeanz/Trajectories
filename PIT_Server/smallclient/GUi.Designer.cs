namespace smallclient
{
    partial class GUi
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxConsole = new System.Windows.Forms.ListBox();
            this.ConsoleText = new System.Windows.Forms.TextBox();
            this.ConsoleSend = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(13, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(119, 100);
            this.listBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Clients:";
            // 
            // listBoxConsole
            // 
            this.listBoxConsole.FormattingEnabled = true;
            this.listBoxConsole.ItemHeight = 16;
            this.listBoxConsole.Location = new System.Drawing.Point(13, 168);
            this.listBoxConsole.Name = "listBoxConsole";
            this.listBoxConsole.Size = new System.Drawing.Size(501, 228);
            this.listBoxConsole.TabIndex = 15;
            // 
            // ConsoleText
            // 
            this.ConsoleText.Location = new System.Drawing.Point(15, 403);
            this.ConsoleText.Name = "ConsoleText";
            this.ConsoleText.Size = new System.Drawing.Size(402, 22);
            this.ConsoleText.TabIndex = 16;
            // 
            // ConsoleSend
            // 
            this.ConsoleSend.Location = new System.Drawing.Point(424, 403);
            this.ConsoleSend.Name = "ConsoleSend";
            this.ConsoleSend.Size = new System.Drawing.Size(90, 23);
            this.ConsoleSend.TabIndex = 17;
            this.ConsoleSend.Text = "Send";
            this.ConsoleSend.UseVisualStyleBackColor = true;
            this.ConsoleSend.Click += new System.EventHandler(this.ConsoleSend_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 134);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 27);
            this.button2.TabIndex = 19;
            this.button2.Text = "Add User";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 438);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ConsoleSend);
            this.Controls.Add(this.ConsoleText);
            this.Controls.Add(this.listBoxConsole);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.KeyPreview = true;
            this.Name = "GUi";
            this.Text = "GUi";
            this.Load += new System.EventHandler(this.GUi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxConsole;
        private System.Windows.Forms.TextBox ConsoleText;
        private System.Windows.Forms.Button ConsoleSend;
        private System.Windows.Forms.Button button2;
    }
}