namespace smallclient
{
    partial class Settings
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
            this.NetTest = new System.Windows.Forms.Button();
            this.Self2 = new System.Windows.Forms.Button();
            this.Value = new System.Windows.Forms.NumericUpDown();
            this.ValTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).BeginInit();
            this.SuspendLayout();
            // 
            // NetTest
            // 
            this.NetTest.Enabled = false;
            this.NetTest.Location = new System.Drawing.Point(13, 13);
            this.NetTest.Name = "NetTest";
            this.NetTest.Size = new System.Drawing.Size(257, 35);
            this.NetTest.TabIndex = 0;
            this.NetTest.Text = "3+1 Net Test";
            this.NetTest.UseVisualStyleBackColor = true;
            // 
            // Self2
            // 
            this.Self2.Enabled = false;
            this.Self2.Location = new System.Drawing.Point(12, 54);
            this.Self2.Name = "Self2";
            this.Self2.Size = new System.Drawing.Size(257, 35);
            this.Self2.TabIndex = 1;
            this.Self2.Text = "2C Self";
            this.Self2.UseVisualStyleBackColor = true;
            // 
            // Value
            // 
            this.Value.Location = new System.Drawing.Point(13, 96);
            this.Value.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.Value.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(79, 22);
            this.Value.TabIndex = 2;
            this.Value.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ValTest
            // 
            this.ValTest.Location = new System.Drawing.Point(99, 96);
            this.ValTest.Name = "ValTest";
            this.ValTest.Size = new System.Drawing.Size(170, 22);
            this.ValTest.TabIndex = 3;
            this.ValTest.Text = "Value Test";
            this.ValTest.UseVisualStyleBackColor = true;
            this.ValTest.Click += new System.EventHandler(this.ValTest_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 127);
            this.Controls.Add(this.ValTest);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.Self2);
            this.Controls.Add(this.NetTest);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Value)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NetTest;
        private System.Windows.Forms.Button Self2;
        private System.Windows.Forms.NumericUpDown Value;
        private System.Windows.Forms.Button ValTest;
    }
}