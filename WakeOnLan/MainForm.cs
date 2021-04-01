using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WakeOnLan
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (toolStrip1.Items.IndexOf(e.ClickedItem))
            {
                case 0:
                    ListViewItem item = new ListViewItem(new[] { "1", "2", "3"});
                    listView1.Items.Add(item);
                    break;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var localIP = Program.GetLocalIP();
            var localMask = Program.GetLocalMask(localIP);
            var networkIP = Program.GetNetworkIP(localIP, localMask);

            var n = Program.GetMaxHostNumber(localMask);
            var ip = networkIP;

            for (int i = 0; i < n; i++)
            {
                ip = Program.GetNextIp(ip);

                if (Program.IsIpReachable(ip))
                {
                    ListViewItem item = new ListViewItem(new[] { ip.ToString() });
                    listView1.Items.Add(item);
                }
            }
        }
    }
}
