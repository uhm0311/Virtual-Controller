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

using VirtualControllerServer.Connections;

using BusinessRefinery.Barcode;
using VirtualControllerServer.Utils.Keys;


namespace VirtualControllerServer.GUI
{
    public partial class MainForm : Form
    {
        private KeyConfigManager keyConfigManager = new KeyConfigManager();
        private KeyConfigDialog keyConfigDialog = null;

        private ConnectionManager connectionManager;
        private USBStatus usbStatus;
        private GUI.ImageViewer qrCode = null;

        public MainForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void VirtualControllerForm_Load(object sender, EventArgs e)
        {
            maxPlayers.SelectedIndex = 2;

            connectionManager = new ConnectionManager(8731, keyConfigManager);
            connectionManager.checkUSB(new USBCheckListener(checkUSBListen));

            keyConfigDialog = new KeyConfigDialog(keyConfigManager);
        }

        private void btnWiFi_Click(object sender, EventArgs e)
        {
            if (connectionManager.getWiFiRunning())
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
            connectionManager.startWiFiConnection(int.Parse(maxPlayers.SelectedItem.ToString()));

            if (connectionManager.getUSBRunning() || connectionManager.getWiFiRunning())
                maxPlayers.Enabled = false;
            btnWiFi.Text = "WiFi 연결 중단";

            StringBuilder code = new StringBuilder();
            foreach (IPAddress addr in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                string ipAddress = addr.ToString();

                if (!ipAddress.Contains(":"))
                    code.AppendLine(ipAddress.ToString());
            }

            if (qrCode == null)
            {
                qrCode = new ImageViewer(Utils.QRCodeGenerator.generate(code.ToString(), QRCodeECL.L));
                qrCode.Show();
            }
        }

        private void stopWiFiConnection()
        {
            connectionManager.stopWiFiConnection();

            if (!connectionManager.getUSBRunning() && !connectionManager.getWiFiRunning())
                maxPlayers.Enabled = true;
            btnWiFi.Text = "WiFi 연결 시작";

            if (qrCode != null)
            {
                qrCode.Close(); qrCode.Dispose();
                qrCode = null;
            }
        }

        private void btnUSB_Click(object sender, EventArgs e)
        {
            if (connectionManager.getUSBRunning())
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
            if (connectionManager.startUSBConnection(int.Parse(maxPlayers.SelectedItem.ToString())))
            {
                if (connectionManager.getUSBRunning() || connectionManager.getWiFiRunning())
                    maxPlayers.Enabled = false;
                btnUSB.Text = "USB 연결 중단";
            }
        }

        private void stopUSBConnection()
        {
            connectionManager.stopUSBConnection();

            if (!connectionManager.getUSBRunning() && !connectionManager.getWiFiRunning())
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
            if (connectionManager.getWiFiRunning())
            {
                this.stopWiFiConnection();
                connectionManager.stopWiFiConnection();
            }

            if (connectionManager.getUSBRunning())
            {
                this.stopUSBConnection();
                connectionManager.stopUSBConnection();
            }
            connectionManager.Dispose();
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

            keyConfigDialog.loadKeyConfig(player,configName);
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
                keyConfigDialog.deleteKeyConfig(player, item.Text);

                ToolStripItemCollection dropdown = ((ToolStripMenuItem)menuKeyConfigK.DropDownItems["menuPlayer" + player]).DropDownItems;

                ToolStripItemCollection open = ((ToolStripMenuItem)dropdown["menuOpenP" + player]).DropDownItems;
                ToolStripItemCollection edit = ((ToolStripMenuItem)dropdown["menuEditP" + player]).DropDownItems;
                ToolStripItemCollection delete = ((ToolStripMenuItem)dropdown["menuDeleteP" + player]).DropDownItems;

                open.Clear();
                edit.Clear();
                delete.Clear();

                gpBoxKeyConfigs.Controls["chckBoxP" + player].Text = "Player" + player + " - 키 설정 없음";
                ((CheckBox)gpBoxKeyConfigs.Controls["chckBoxP" + player]).Checked = false;
            }
        }

        private void menuKeyConfigK_Click(object sender, EventArgs e)
        {
            for (int i = KeyConfigManager.minPlayer; i <= KeyConfigManager.maxPlayer; i++)
            {
                ToolStripItemCollection dropdown = ((ToolStripMenuItem)menuKeyConfigK.DropDownItems["menuPlayer" + i]).DropDownItems;

                ToolStripItemCollection open = ((ToolStripMenuItem)dropdown["menuOpenP" + i]).DropDownItems;
                ToolStripItemCollection edit = ((ToolStripMenuItem)dropdown["menuEditP" + i]).DropDownItems;
                ToolStripItemCollection delete = ((ToolStripMenuItem)dropdown["menuDeleteP" + i]).DropDownItems;

                open.Clear();
                edit.Clear();
                delete.Clear();

                string[] namesOfKeyConfigs = keyConfigManager.getNames(i);
                foreach (string nameOfKeyConfig in namesOfKeyConfigs)
                {
                    ToolStripMenuItem openItem = new ToolStripMenuItem();
                    ToolStripMenuItem editItem = new ToolStripMenuItem();
                    ToolStripMenuItem deleteItem = new ToolStripMenuItem();

                    openItem.Text = editItem.Text = deleteItem.Text = nameOfKeyConfig;
                    openItem.Name = editItem.Name = deleteItem.Name = "menu" + nameOfKeyConfig + "P" + i;

                    openItem.Click += menuOpen_Click;
                    editItem.Click += menuEdit_Click;
                    deleteItem.Click += menuDelete_Click;

                    open.Add(openItem);
                    edit.Add(editItem);
                    delete.Add(deleteItem);
                }
            }
        }
    }
}
