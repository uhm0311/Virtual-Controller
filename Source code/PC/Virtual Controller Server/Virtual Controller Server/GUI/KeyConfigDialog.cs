using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using VirtualControllerServer.Utils.Keys;

namespace VirtualControllerServer.GUI
{
    public partial class KeyConfigDialog : Form
    {
        private enum Flag
        {
            Create,
            Load,
            Edit,
            Delete
        };
        private Flag flag;

        private KeyConfigManager keyConfigManager;
        private List<TextBox> txtes;

        private int player;
        private string keyConfigName = string.Empty;

        private int scanCode;

        public KeyConfigDialog(KeyConfigManager keyConfigManager)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.keyConfigManager = keyConfigManager;
            this.txtes = buildTextBoxes();

            txtes[2].Select();
        }

        public string createKeyConfig(int player)
        {
            this.player = player;
            flag = Flag.Create;

            for (int i = 0; i < txtes.Count; i++)
                txtes[i].Text = Keys.None.ToString();

            base.ShowDialog();

            return keyConfigName;
        }

        public void loadKeyConfig(int player, string keyConfigName)
        {
            flag = Flag.Load;

            keyConfigManager.load(this.player = player, this.keyConfigName = keyConfigName);
        }

        public void editKeyConfig(int player, string keyConfigName)
        {
            flag = Flag.Edit;

            keyConfigManager.load(this.player = player, this.keyConfigName = keyConfigName);
            Keys[] keys = keyConfigManager.getVirtualKeys(player);

            for (int i = 0; i < keys.Length; i++)
                txtes[i].Text = keys[i].ToString();
            
            base.ShowDialog();
        }

        public void deleteKeyConfig(int player, string keyConfigName)
        {
            flag = Flag.Delete;

            keyConfigManager.delete(this.player = player, this.keyConfigName = keyConfigName);
        }

        private List<TextBox> buildTextBoxes()
        {
            List<TextBox> txtes = new List<TextBox>(Enum.GetNames(typeof(KeyCode)).Length);

            txtes.Add(txtSTART);
            txtes.Add(txtCOIN);
            txtes.Add(txtUP);
            txtes.Add(txtDOWN);
            txtes.Add(txtLEFT);
            txtes.Add(txtRIGHT);
            txtes.Add(txtA);
            txtes.Add(txtB);
            txtes.Add(txtC);
            txtes.Add(txtD);
            txtes.Add(txtL);
            txtes.Add(txtR);

            foreach (TextBox txt in txtes)
            {
                txt.KeyDown += txt_KeyDown;
                txt.KeyPress += txt_KeyPress;
            }

            return txtes;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Equals(btnOK))
            {
                if ((flag == Flag.Create || flag == Flag.Edit) && (InputBox("키 설정 저장", "키 설정 이름을 입력해주십시오.", ref keyConfigName) == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(keyConfigName)))
                {
                    keyConfigManager.save(player, keyConfigName);
                    this.Close();
                }
            }
            else
            {
                keyConfigManager.load(player, keyConfigName);
                this.Close();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (msg.Msg)
            {
                case 0x0100:
                    int lParam = msg.LParam.ToInt32();
                    scanCode = (lParam >> 16) & 0x000000ff;
                    int ext = (lParam >> 24) & 0x00000001;

                    if (ext == 1)
                        scanCode += 128;

                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            int index = txtes.IndexOf(txt);

            List<TextBox> temp = txtes.FindAll(element => (element.Text == e.KeyCode.ToString() && !element.Equals(txt)));
            foreach (TextBox t in temp)
            {
                t.Text = "None";
                keyConfigManager.setKeyCode(player, (KeyCode)txtes.IndexOf(t), 0);
            }

            txt.Text = e.KeyCode.ToString();
            keyConfigManager.setKeyCode(player, (KeyCode)index, scanCode);
            scanCode = 0;

            if (index >= 11) 
                index = -1;

            txtes[index + 1].Select();
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
