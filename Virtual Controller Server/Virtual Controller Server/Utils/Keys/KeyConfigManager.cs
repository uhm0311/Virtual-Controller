using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualControllerServer.Utils.Keys
{
    public class KeyConfigManager
    {
        private static readonly string keyConfigsPath = @"Keys\";
        private Dictionary<int, KeyConfig> keyConfigs = new Dictionary<int, KeyConfig>();

        public const int minPlayer = 1;
        public const int maxPlayer = 4;

        public KeyConfigManager()
        {
            if (!Directory.Exists(keyConfigsPath))
                Directory.CreateDirectory(keyConfigsPath);

            for (int i = minPlayer; i <= maxPlayer; i++)
            {
                if (!Directory.Exists(keyConfigsPath + "P" + i))
                    Directory.CreateDirectory(keyConfigsPath + "P" + i);

                keyConfigs.Add(i, new KeyConfig(i));
            }
        }

        private string getKeyConfigFilePath(int player, string keyConfigName)
        {
            return keyConfigsPath + "P" + player + @"\" + (keyConfigName.EndsWith(".ini") ? keyConfigName.Substring(0, keyConfigName.Length - 4) : keyConfigName) + ".ini";
        }

        public void load(int player, string keyConfigName)
        {
            keyConfigs[player].load(getKeyConfigFilePath(player, keyConfigName));
        }

        public void delete(int player, string keyConfigName)
        {
            this.keyConfigs[player] = new KeyConfig(player);
            File.Delete(getKeyConfigFilePath(player, keyConfigName));
        }

        public void save(int player, string keyConfigName)
        {
            keyConfigs[player].save(getKeyConfigFilePath(player, keyConfigName));
        }

        public System.Windows.Forms.Keys[] getVirtualKeys(int player)
        {
            return keyConfigs[player].getVirtualKeys();
        }

        public void setKeyCode(int player, KeyCode keyCode, int keyValue)
        {
            keyConfigs[player].setKeyCode(keyCode, keyValue);
        }

        public string[] getNames(int player)
        {
            string[] namesOfKeyConfigs = Directory.GetFiles(keyConfigsPath + "P" + player);

            for (int i = 0; i < namesOfKeyConfigs.Length; i++)
            {
                namesOfKeyConfigs[i] = namesOfKeyConfigs[i].Substring(0, namesOfKeyConfigs[i].Length - 4);
                namesOfKeyConfigs[i] = namesOfKeyConfigs[i].Substring(namesOfKeyConfigs[i].LastIndexOf('\\') + 1);
            }

            return namesOfKeyConfigs;
        }

        public int getScanCode(int player, KeyCode keyCode)
        {
            return keyConfigs[player].getScanCode(keyCode);
        }

        public int getVirtualKeyCode(int player, KeyCode keyCode)
        {
            return keyConfigs[player].getVirtualKeyCode(keyCode);
        }
    }
}
