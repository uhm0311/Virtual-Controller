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

namespace VirtualController.GUI
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

        private List<TextBox> txtes;
        private Dictionary<int, Dictionary<int, int>> keyConfigs = new Dictionary<int, Dictionary<int, int>>();
        private Dictionary<int, Dictionary<int, int>> virtualKeyConfigs = new Dictionary<int, Dictionary<int, int>>();

        private int player = 1;
        private string configName = "";

        private int scanCode;

        private static readonly string keyConfigsPath = @"Keys\";
        private static readonly int numberOfKeys = 12;
        private static readonly string[] keyNames = { "Start", "Coin", "Up", "Down", "Left", "Right", "A", "B", "C", "D", "L", "R" };

        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(int wCode, int wMapType);

        public KeyConfigDialog()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (!Directory.Exists(keyConfigsPath))
                Directory.CreateDirectory(keyConfigsPath);

            for (int i = 1; i <= 4; i++)
            {
                if (!Directory.Exists(keyConfigsPath + "P" + i))
                    Directory.CreateDirectory(keyConfigsPath + "P" + i);

                virtualKeyConfigs.Add(i, new Dictionary<int, int>());
                virtualKeyConfigs[player = i] = parseKeyConfigs(createDefaultKeyConfigs());

                keyConfigs.Add(i, new Dictionary<int, int>());
                keyConfigs[i] = parseKeyConfigs(createDefaultKeyConfigs());
            }
            this.txtes = buildTextBoxes();

            txtes[2].Select();

        }

        public string[] getKeyConfigs(int player)
        {
            string[] keyConfigs = Directory.GetFiles(keyConfigsPath + "P" + player);

            for (int i = 0; i < keyConfigs.Length; i++)
            {
                keyConfigs[i] = keyConfigs[i].Substring(0, keyConfigs[i].Length - 4);
                keyConfigs[i] = keyConfigs[i].Substring(keyConfigs[i].LastIndexOf('\\') + 1);
            }

            return keyConfigs;
        }

        public int getScanCode(int player, int keyCode)
        {
            return this.keyConfigs[player][keyCode];
        }
        public int getVirtualKey(int player, int keyCode)
        {
            return this.virtualKeyConfigs[player][keyCode];
        }

        public string createKeyConfig(int player)
        {
            flag = Flag.Create;
            this.configName = "";

            this.player = player;
            if (player >= 1 && player <= 4)
            {
                for (int j = 0; j < numberOfKeys; j++)
                    txtes[j].Text = ((Keys)0).ToString();
            }
            base.ShowDialog();

            return configName;
        }

        public void loadKeyConfig(int player, string configName)
        {
            string keyConfigFile = keyConfigsPath + "P" + player + @"\" + (configName.EndsWith(".ini") ? configName.Substring(0, configName.Length - 4) : configName) + ".ini";

            flag = Flag.Load;
            this.configName = configName;
            this.keyConfigs[player] = loadKeyConfig(keyConfigFile);

            parseScanCodeToVKey();
        }

        private void parseScanCodeToVKey()
        {
            for (int player = 1; player <= 4; player++)
            {
                for (int i = 0; i < numberOfKeys; i++)
                {
                    int scanCode = keyConfigs[player][i];

                    if (scanCode > 128)
                        scanCode -= 128;
                    virtualKeyConfigs[player][i] = MapVirtualKey(scanCode, 1);
                }
            }
        }

        private Dictionary<int, int> loadKeyConfig(string keyConfigFile)
        {
            if (File.Exists(keyConfigFile))
                return parseKeyConfigs(File.ReadAllText(keyConfigFile));
            else return parseKeyConfigs(createDefaultKeyConfigs());
        }

        public void editKeyConfig(int player, string configName)
        {
            string keyConfigFile = keyConfigsPath + "P" + player + @"\" + (configName.EndsWith(".ini") ? configName.Substring(0, configName.Length - 4) : configName) + ".ini";

            flag = Flag.Edit;
            this.configName = configName;
            this.keyConfigs[player] = loadKeyConfig(keyConfigFile);

            this.player = player;
            if (player >= 1 && player <= 4)
            {
                for (int j = 0; j < numberOfKeys; j++)
                {
                    Keys key = (Keys)MapVirtualKey(this.keyConfigs[player][j], 1);
                    if ((int)key == 0 && this.keyConfigs[player][j] > 128)
                        key = (Keys)MapVirtualKey(this.keyConfigs[player][j] - 128, 1);
                    txtes[j].Text = key.ToString();
                }
            }
            base.ShowDialog();
        }

        public string deleteKeyConfig(int player, string configName)
        {
            string keyConfigFile = keyConfigsPath + "P" + player + @"\" + (configName.EndsWith(".ini") ? configName.Substring(0, configName.Length - 4) : configName) + ".ini";

            flag = Flag.Delete;
            if (this.configName == configName)
            {
                this.keyConfigs[player] = parseKeyConfigs(createDefaultKeyConfigs());
                this.configName = "";
            }
            File.Delete(keyConfigFile);

            return this.configName;
        }

        private void saveKeyConfig()
        {
            string keyConfigFile = keyConfigsPath + "P" + player + @"\" + (configName.EndsWith(".ini") ? configName.Substring(0, configName.Length - 4) : configName) + ".ini";

            if (!File.Exists(keyConfigFile))
                File.Create(keyConfigFile).Close();

            File.WriteAllText(keyConfigFile, parseKeyConfigs(this.keyConfigs));

            parseScanCodeToVKey();
        }

        private List<TextBox> buildTextBoxes()
        {
            List<TextBox> txtes = new List<TextBox>(numberOfKeys);

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

        private Dictionary<int, int> parseKeyConfigs(string keyConfigsStr)
        {
            try
            {
                if (keyConfigsStr.Length > 0)
                {
                    Dictionary<int, int> keyConfigs = new Dictionary<int, int>();
                    StringReader reader = new StringReader(keyConfigsStr);
                    string line;

                    if (reader.ReadLine() != null)
                    {
                        for (int i = 0; i < numberOfKeys; i++)
                        {
                            line = reader.ReadLine();
                            if (!(line == null || line.Trim().Length == 0 || line.StartsWith(";")))
                            {
                                int keyValue = int.Parse(line.Substring(line.IndexOf('=') + 1));
                                keyConfigs[i] = keyValue;
                            }
                            else throw new DataException();
                        }
                        return keyConfigs;
                    }
                    else throw new DataException();
                }
                else return parseKeyConfigs(createDefaultKeyConfigs());
            }
            catch (DataException)
            {
                return parseKeyConfigs(createDefaultKeyConfigs());
            }
        }

        private string parseKeyConfigs(Dictionary<int, Dictionary<int, int>> keyConfigs)
        {
            StringBuilder str = new StringBuilder();
            int i;

            str.AppendLine(";P" + player);
            for (i = 0; i < numberOfKeys; i++)
                str.AppendLine("Key_" + keyNames[i] + "=" + (int)keyConfigs[player][i]);

            str.AppendLine();
            return str.ToString();
        }

        private string createDefaultKeyConfigs()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 1; i <= 4; i++)
            {
                str.AppendLine(";P" + i);
                for (int j = 0; j < numberOfKeys; j++)
                    str.AppendLine("Key_" + keyNames[j] + "=0");

                str.AppendLine();
            }
            return str.ToString();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Equals(btnOK))
            {
                if ((flag == Flag.Create || flag == Flag.Edit) && (InputBox("키 설정 저장", "키 설정 이름을 입력해주십시오.", ref configName) == System.Windows.Forms.DialogResult.OK && configName.Length > 0))
                {
                    saveKeyConfig();
                    this.Close();
                }
            }
            else
            {
                this.keyConfigs[player] = loadKeyConfig(keyConfigsPath + "P" + player + @"\" + (configName.EndsWith(".ini") ? configName.Substring(0, configName.Length - 4) : configName) + ".ini");
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

            txt.Text = e.KeyCode.ToString();
            keyConfigs[player][index] = scanCode;
            scanCode = 0;

            TextBox temp = txtes.Find(element => (element.Text == e.KeyCode.ToString() && !element.Equals(txt)));
            if (temp != null)
            {
                temp.Text = "None";
                keyConfigs[player][txtes.IndexOf(temp)] = 0;
            }

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
