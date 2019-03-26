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
using System.Net.NetworkInformation;

using System.Runtime.InteropServices;

using VirtualController.Connections;

using BusinessRefinery.Barcode;


namespace VirtualController.GUI
{
    public partial class VirtualControllerForm : Form
    {
        private KeyConfigDialog keyConfigDialog = new KeyConfigDialog();
        private List<string> keyConfigNames = new List<string>();

        private ConnectionManager manager;
        private USBStatus usbStatus;
        private GUI.QRCode QR = null;

        public VirtualControllerForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void VirtualControllerForm_Load(object sender, EventArgs e)
        {
            maxPlayers.SelectedIndex = 2;

            manager = new ConnectionManager(8731, keyConfigDialog);
            manager.checkUSB(new USBCheckListener(checkUSBListen));
        }

        private void btnWiFi_Click(object sender, EventArgs e)
        {
            if (manager.getWiFiRunning())
            {
                if (MessageBox.Show("WiFi 연결을 중단하시겠습니까?", "WiFi 연결 중단", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    stopWiFiConnection();
            }
            else
            {
                if (MessageBox.Show("WiFi 연결을 시작하시겠습니까?", "WiFi 연결 시작", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    startWiFiConnection();
            }
        }

        private void startWiFiConnection()
        {
            manager.startWiFiConnection(int.Parse(maxPlayers.SelectedItem.ToString()));

            if (manager.getUSBRunning() || manager.getWiFiRunning())
                maxPlayers.Enabled = false;
            btnWiFi.Text = "WiFi 연결 중단";

            StringBuilder code = new StringBuilder();
            foreach (IPAddress addr in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                string ipAddress = addr.ToString();

                if (!ipAddress.Contains(":"))
                    code.AppendLine(ipAddress.ToString());
            }

            if (QR == null)
            {
                QR = new QRCode(Utils.QRCodeGenerator.generate(code.ToString(), QRCodeECL.L));
                QR.Show();
            }
        }

        private void stopWiFiConnection()
        {
            manager.stopWiFiConnection();

            if (!manager.getUSBRunning() && !manager.getWiFiRunning())
                maxPlayers.Enabled = true;
            btnWiFi.Text = "WiFi 연결 시작";

            if (QR != null)
            {
                QR.Close(); QR.Dispose();
                QR = null;
            }
        }

        private void btnUSB_Click(object sender, EventArgs e)
        {
            if (manager.getUSBRunning())
            {
                if (MessageBox.Show("USB 연결을 중단하시겠습니까?", "USB 연결 중단", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    stopUSBConnection();
            }
            else if (usbStatus.Equals(USBStatus.Ready))
            {
                if (MessageBox.Show("USB 연결을 시작하시겠습니까?", "USB 연결 시작", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    startUSBConnection();
            }
            else if (usbStatus.Equals(USBStatus.NoConnection)) 
                MessageBox.Show("연결된 스마트폰이 없습니다.", "USB 연결 시작", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (usbStatus.Equals(USBStatus.TooManyConnections))
                MessageBox.Show("하나의 스마트폰만 연결해 주시기 바랍니다.", "USB 연결 시작", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        private void startUSBConnection()
        {
            if (manager.startUSBConnection(int.Parse(maxPlayers.SelectedItem.ToString())))
            {
                if (manager.getUSBRunning() || manager.getWiFiRunning())
                    maxPlayers.Enabled = false;
                btnUSB.Text = "USB 연결 중단";
            }
        }

        private void stopUSBConnection()
        {
            manager.stopUSBConnection();

            if (!manager.getUSBRunning() && !manager.getWiFiRunning())
                maxPlayers.Enabled = true;
            btnUSB.Text = "USB 연결 시작";

            if (usbStatus.Equals(USBStatus.NoConnection))
                MessageBox.Show("연결된 스마트폰이 없습니다.", "USB 연결 중단", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (usbStatus.Equals(USBStatus.TooManyConnections))
                MessageBox.Show("하나의 스마트폰만 연결해 주시기 바랍니다.", "USB 연결 중단", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void checkUSBListen(USBStatus status)
        {
            if (!this.usbStatus.Equals(status))
            {
                this.usbStatus = status;

                if (!this.usbStatus.Equals(USBStatus.Ready))
                    this.stopUSBConnection();
            }
        }

        private void VirtualControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (manager.getWiFiRunning())
            {
                this.stopWiFiConnection();
                manager.stopWiFiConnection();
            }

            if (manager.getUSBRunning())
            {
                this.stopUSBConnection();
                manager.stopUSBConnection();
            }
            manager.Dispose();
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
