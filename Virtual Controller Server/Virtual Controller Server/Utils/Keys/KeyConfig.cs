using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualControllerServer.Utils.Keys
{
    public class KeyConfig
    {
        private static readonly string[] keyNames = Enum.GetNames(typeof(KeyCode));
        private static readonly int numberOfKeys = keyNames.Length;

        private Dictionary<int, int> keyConfigs = new Dictionary<int, int>();
        private Dictionary<int, int> virtualKeyConfigs = new Dictionary<int, int>();

        private string configName;
        private int player;

        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(int wCode, int wMapType);

        public KeyConfig(int player)
        {
            this.player = player;

            keyConfigs = createDefaultKeyConfigs();
            virtualKeyConfigs = createDefaultKeyConfigs();
        }

        public void load(string keyConfigFilePath)
        {
            if (File.Exists(keyConfigFilePath))
            {
                keyConfigs = convertStringToKeyConfig(File.ReadAllText(keyConfigFilePath));
                configName = Path.GetFileName(keyConfigFilePath);

                mapScanCodesToVirtualKeyes();
            }
            else keyConfigs = createDefaultKeyConfigs();
        }

        public void save(string keyConfigFilePath)
        {
            if (!File.Exists(keyConfigFilePath))
                File.Create(keyConfigFilePath).Close();

            File.WriteAllText(keyConfigFilePath, this.ToString());
            mapScanCodesToVirtualKeyes();
        }

        public void setKeyCode(KeyCode keyCode, int keyValue)
        {
            keyConfigs[(int)keyCode] = keyValue;
        }

        private void mapScanCodesToVirtualKeyes()
        {
            for (int i = 0; i < numberOfKeys; i++)
            {
                int scanCode = keyConfigs[i];

                if (scanCode > 128)
                    scanCode -= 128;
                virtualKeyConfigs[i] = MapVirtualKey(scanCode, 1);
            }
        }

        private Dictionary<int, int> convertStringToKeyConfig(string keyConfigs)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(keyConfigs))
                {
                    Dictionary<int, int> keyConfig = new Dictionary<int, int>();
                    StringReader reader = new StringReader(keyConfigs);
                    string line;

                    if (reader.ReadLine() != null)
                    {
                        for (int i = 0; i < numberOfKeys; i++)
                        {
                            line = reader.ReadLine();
                            if (!(string.IsNullOrWhiteSpace(line) || line.StartsWith(";")))
                            {
                                int keyValue = int.Parse(line.Substring(line.IndexOf('=') + 1));
                                keyConfig[i] = keyValue;
                            }
                            else throw new DataException();
                        }
                        return keyConfig;
                    }
                    else throw new DataException();
                }
                else return createDefaultKeyConfigs();
            }
            catch (DataException)
            {
                return createDefaultKeyConfigs();
            }
        }

        private Dictionary<int, int> createDefaultKeyConfigs()
        {
            return convertStringToKeyConfig(createDefaultKeyConfigsString());
        }

        private string createDefaultKeyConfigsString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(";P" + player);

            for (int j = 0; j < numberOfKeys; j++)
                str.AppendLine("Key_" + keyNames[j] + "=0");
            str.AppendLine();

            return str.ToString();
        }

        public int getScanCode(KeyCode keyCode)
        {
            return keyConfigs[(int)keyCode];
        }

        public int getVirtualKeyCode(KeyCode keyCode)
        {
            
            return virtualKeyConfigs[(int)keyCode];
        }

        public System.Windows.Forms.Keys[] getVirtualKeys()
        {
            List<System.Windows.Forms.Keys> keys = new List<System.Windows.Forms.Keys>();

            for (int j = 0; j < numberOfKeys; j++)
            {
                System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)MapVirtualKey(this.keyConfigs[j], 1);
                if ((int)key == 0 && this.keyConfigs[j] > 128)
                    key = (System.Windows.Forms.Keys)MapVirtualKey(this.keyConfigs[j] - 128, 1);

                keys.Add(key);
            }

            return keys.ToArray();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            int i;

            str.AppendLine(";P" + player);
            for (i = 0; i < numberOfKeys; i++)
                str.AppendLine("Key_" + keyNames[i] + "=" + (int)keyConfigs[i]);
            str.AppendLine();

            return str.ToString();
        }
    }
}
