using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace smallclient
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void ValTest_Click(object sender, EventArgs e)
        {
            GUi form = new GUi((int)Value.Value);
            form.Show();
            this.Hide();
        }
        

        private void Settings_Load(object sender, EventArgs e)
        {

        }
    }
}
