using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIT_Server.Resources
{
    public partial class ControlPanel : Form
    {
        Timer _updater;
        public ControlPanel()
        {
            InitializeComponent();
            
        }

        private void _updater_Tick(object sender, EventArgs e)
        {
            var gos = Lobby.scene.GameObjects.Keys.ToList();
            object[] arr = new object[listBox1.Items.Count];
            listBox1.Items.CopyTo(arr, 0);
            var list = new List<string>(arr.Select(x=>(string)x));
            foreach (var go in gos)
            {
                if (!list.Contains(go.ToString()))
                    listBox1.Items.Add(go.ToString());
            }
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(Lobby.scene.GameObjects.Count.ToString());
            _updater = new Timer();
            _updater.Interval = 200;
            _updater.Tick += _updater_Tick;
            _updater.Start();
        }
    }
}
