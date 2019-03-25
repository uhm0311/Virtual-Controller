using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Runtime.InteropServices;

using VirtualController.Connections;

namespace VirtualController.GUI
{
    public partial class VirtualControllerForm : Form
    {
        private KeyConfigDialog keyConfigDialog = new KeyConfigDialog();
        private List<string> keyConfigNames = new List<string>();

        private ConnectionManager server;

        public VirtualControllerForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            server = new ConnectionManager(8731, keyConfigDialog);
            maxPlayers.SelectedIndex = 3;
        }

        private void VirtualControllerForm_Load(object sender, EventArgs e)
        {
            string IPAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

            if (isPrivateIPAddress(IPAddress))
            {
                lblIPAddress.Text += IPAddress;
                maxPlayers.SelectedIndex = 2;
            }
            else gpBoxWiFi.Visible = gpBoxWiFi.Enabled = false;
        }

        private bool isPrivateIPAddress(string IPAddress)
        {
            string[] straryIPAddress = IPAddress.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int[] iaryIPAddress = new int[] { int.Parse(straryIPAddress[0]), int.Parse(straryIPAddress[1]), int.Parse(straryIPAddress[2]), int.Parse(straryIPAddress[3]) };

            if (iaryIPAddress[0] == 10 || (iaryIPAddress[0] == 192 && iaryIPAddress[1] == 168) || (iaryIPAddress[0] == 172 && (iaryIPAddress[1] >= 16 && iaryIPAddress[1] <= 31)))
                return true;  
            else return false;
        }

        private void btnToggler_Click(object sender, EventArgs e)
        {
            if (server.getIsRunning())
            {
                if (MessageBox.Show("연결을 중단하시겠습니까?", "연결 중단", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    server.stopWiFiConnection();

                    maxPlayers.Enabled = true;
                    btnToggler.Text = "연결 시작";
                }
            }
            else
            {
                if (MessageBox.Show("연결을 시작하시겠습니까?", "연결 시작", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    server.startWiFiConnection(int.Parse(maxPlayers.SelectedItem.ToString()));

                    maxPlayers.Enabled = false;
                    btnToggler.Text = "연결 중단";
                }
            }
        }

        

        private void VirtualControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server.getIsRunning())
                server.stopWiFiConnection();
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int player = int.Parse(item.Name.Substring(item.Name.Length - 1));

            string configName = keyConfigDialog.createKeyConfig(player);
            if (configName.Length > 0)
            {
                gpBoxKeyConfigs.Controls["chckBoxP" + player].Text = "Player" + player + " - 키 설정 \'" + configName + "\'";
                ((CheckBox)gpBoxKeyConfigs.Controls["chckBoxP" + player]).Checked = true;
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int player = int.Parse(item.Name.Substring(item.Name.Length - 1));
            string configName = item.Text;

            keyConfigDialog.loadKeyConfig(player, configName);
            gpBoxKeyConfigs.Controls["chckBoxP" + player].Text = "Player" + player + " - 키 설정 \'" + configName + "\'";
            ((CheckBox)gpBoxKeyConfigs.Controls["chckBoxP" + player]).Checked = true;
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int player = int.Parse(item.Name.Substring(item.Name.Length - 1));
            string configName = item.Text;

            keyConfigDialog.editKeyConfig(player, configName);
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말로 삭제하시겠습니까?", "키 설정 삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                int player = int.Parse(item.Name.Substring(item.Name.Length - 1));
                string configName = keyConfigDialog.deleteKeyConfig(player, item.Text);

                ToolStripItemCollection dropdown = ((ToolStripMenuItem)menuKeyConfigK.DropDownItems["menuPlayer" + player]).DropDownItems;

                ToolStripItemCollection open = ((ToolStripMenuItem)dropdown["menuOpenP" + player]).DropDownItems;
                ToolStripItemCollection edit = ((ToolStripMenuItem)dropdown["menuEditP" + player]).DropDownItems;
                ToolStripItemCollection delete = ((ToolStripMenuItem)dropdown["menuDeleteP" + player]).DropDownItems;

                open.Clear();
                edit.Clear();
                delete.Clear();

                keyConfigNames.Clear();

                if (configName.Length == 0)
                {
                    gpBoxKeyConfigs.Controls["chckBoxP" + player].Text = "Player" + player + " - 키 설정 없음";
                    ((CheckBox)gpBoxKeyConfigs.Controls["chckBoxP" + player]).Checked = false;
                }
            }
        }

        private void menuKeyConfigK_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 4; i++)
            {
                ToolStripItemCollection dropdown = ((ToolStripMenuItem)menuKeyConfigK.DropDownItems["menuPlayer" + i]).DropDownItems;

                ToolStripItemCollection open = ((ToolStripMenuItem)dropdown["menuOpenP" + i]).DropDownItems;
                ToolStripItemCollection edit = ((ToolStripMenuItem)dropdown["menuEditP" + i]).DropDownItems;
                ToolStripItemCollection delete = ((ToolStripMenuItem)dropdown["menuDeleteP" + i]).DropDownItems;

                List<ToolStripMenuItem> openItems = new List<ToolStripMenuItem>();
                List<ToolStripMenuItem> editItems = new List<ToolStripMenuItem>();
                List<ToolStripMenuItem> deleteItems = new List<ToolStripMenuItem>();

                string[] keyConfigs = keyConfigDialog.getKeyConfigs(i);
                foreach (string keyConfig in keyConfigs)
                {
                    if (!keyConfigNames.Contains(keyConfig))
                    {
                        ToolStripMenuItem openItem = new ToolStripMenuItem();
                        ToolStripMenuItem editItem = new ToolStripMenuItem();
                        ToolStripMenuItem deleteItem = new ToolStripMenuItem();

                        openItem.Text = editItem.Text = deleteItem.Text = keyConfig;
                        openItem.Name = editItem.Name = deleteItem.Name = "menu" + keyConfig + "P" + i;

                        openItem.Click += menuOpen_Click;
                        editItem.Click += menuEdit_Click;
                        deleteItem.Click += menuDelete_Click;

                        openItems.Add(openItem);
                        editItems.Add(editItem);
                        deleteItems.Add(deleteItem);

                        keyConfigNames.Add(keyConfig);
                    }
                }
                open.AddRange(openItems.ToArray());
                edit.AddRange(editItems.ToArray());
                delete.AddRange(deleteItems.ToArray());
            }
        }
    }
}
