using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Runtime.InteropServices;
using System.Windows.Forms;

using VirtualControllerServer.GUI;
using VirtualControllerServer.Utils;
using VirtualControllerServer.Utils.Keys;

namespace VirtualControllerServer.Connections
{
    public delegate void USBCheckListener(USBStatus status);
    public enum USBStatus { NoConnection, Ready, TooManyConnections };

    public class ConnectionManager : IDisposable
    {
        private KeyConfigManager keyConfigManager;
        private USBCheckListener listener;

        private bool isWiFiRunning = false;
        private bool isUSBRunning = false;
        private bool isRunning = false;
        private int port;

        private int maxPlayers;

        private TcpListener server;
        private TcpClient client;

        private Dictionary<int, Socket> clients = new Dictionary<int, Socket>();
        private Dictionary<int, bool> isUSBConnection = new Dictionary<int, bool>();

        private Thread acceptThread;
        private Dictionary<int, Thread> receiveThread = new Dictionary<int, Thread>();

        public static readonly int usbPort = 39123;
        private ADBManager adbManager = new ADBManager();

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern int MapVirtualKey(int wCode, int wMapType);

        public ConnectionManager(int port, KeyConfigManager keyConfigManager)
        {
            this.port = port;
            this.keyConfigManager = keyConfigManager;

            for (int i = 1; i <= 4; i++)
            {
                receiveThread.Add(i, null);
                clients.Add(i, null);
                isUSBConnection.Add(i, false);
            }
            isRunning = true;
        }

        public void Dispose()
        {
            isRunning = false;
            adbManager.Dispose();

            for (int i = 1; i <= 4; i++)
            {
                if (clients[i] != null)
                    removeClient(clients[i]);
            }
        }

        public void checkUSB(USBCheckListener listener)
        {
            this.listener = listener;
            new Thread(new ThreadStart(checkUSB)).Start();
        }

        private void checkUSB()
        {
            while (isRunning)
            {
                try
                {
                    int devices = adbManager.getConnectedDevices();

                    if (devices == 1)
                    {
                        try { adbManager.portForward(39123); }
                        catch (Exception e) { string str = e.ToString();
                            Console.WriteLine(str);
                        }
                        listener.Invoke(USBStatus.Ready);
                    }
                    else
                    {
                        if (isUSBRunning)
                            stopUSBConnection();
                        if (devices == 0)
                            listener.Invoke(USBStatus.NoConnection);
                        else listener.Invoke(USBStatus.TooManyConnections);
                    }
                }
                catch { }

                Thread.Sleep(1);
            }
        }

        public bool getWiFiRunning()
        {
            return this.isWiFiRunning;
        }
        public bool getUSBRunning()
        {
            return this.isUSBRunning;
        }

        public void startWiFiConnection(int maxPlayers)
        {
            if (!isWiFiRunning)
            {
                isWiFiRunning = true;
                this.maxPlayers = maxPlayers;

                server = new TcpListener(IPAddress.Any, 8731);
                server.Start(maxPlayers);

                acceptThread = new Thread(new ThreadStart(accept));
                acceptThread.Start();
            }
        }

        public bool startUSBConnection(int maxPlayers)
        {
            if (!isUSBRunning)
            {
                isUSBRunning = true;
                this.maxPlayers = maxPlayers;

                try
                {
                    client = new TcpClient("localhost", usbPort);

                    client.GetStream();
                    addClient(client.Client, true);

                    return true;
                }
                catch
                {
                    MessageBox.Show("USB 연결에 실패했습니다. 스마트폰 앱을 먼저 켰는지 확인해주세요.", "USB 연결 시작", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.stopUSBConnection();

                    return false;
                }
            }
            else return false;
        }

        public void stopWiFiConnection()
        {
            if (isWiFiRunning)
            {
                isWiFiRunning = false;

                server.Stop();
                server = null;

                acceptThread.Abort();
                acceptThread = null;

                for (int i = 1; i <= maxPlayers; i++)
                {
                    if (!isUSBConnection[i])
                    {
                        if (clients[i] != null)
                        {
                            clients[i].Close();
                            clients[i] = null;
                        }

                        if (receiveThread[i] != null)
                        {
                            receiveThread[i].Abort();
                            receiveThread[i] = null;
                        }
                    }
                }
            }
        }

        public void stopUSBConnection()
        {
            if (isUSBRunning)
            {
                isUSBRunning = false;
                if(client!=null)
                    client.Close();
                client = null;

                for (int i = 1; i <= maxPlayers; i++)
                {
                    if (!isUSBConnection[i])
                    {
                        if (clients[i] != null)
                        {
                            clients[i].Close();
                            clients[i] = null;
                        }

                        if (receiveThread[i] != null)
                        {
                            receiveThread[i].Abort();
                            receiveThread[i] = null;
                        }
                    }
                }
            }
        }

        private void accept()
        {
            while (isWiFiRunning)
            {
                try { addClient(server.AcceptSocket(), false); }
                catch { }
            }
        }

        private void addClient(Socket client, bool isUSB)
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i] == null)
                {
                    clients[i] = client;
                    this.isUSBConnection[i] = isUSB;
                    client.ReceiveBufferSize = 10240;

                    if (isUSB) 
                    {
                        receiveThread[i] = new Thread(new ParameterizedThreadStart(receive));
                        receiveThread[i].Start(i);
                    }
                    else new Thread(new ParameterizedThreadStart(validate)).Start(i);

                    return;
                }
            }

            client.Close();
            client = null;
        }

        private void validate(object parameter)
        {
            int player = (int)parameter;

            using (NetworkStream stream = new NetworkStream(clients[player]))
            {
                using (StreamReader reader = new StreamReader(stream, ByteConverter.EUCKR))
                {
                    using (StreamWriter writer = new StreamWriter(stream, ByteConverter.EUCKR))
                    {
                        try
                        {
                            if (reader.ReadLine().Trim().ToUpper().Equals("VC?"))
                            {
                                int num = ~(int.Parse(reader.ReadLine().Trim()));
                                StringBuilder response = new StringBuilder();

                                response.AppendLine("VC.");
                                response.Append(num);

                                writer.WriteLine(response.ToString());
                                writer.Flush();

                                if (reader.ReadLine().Trim().ToUpper().Equals("GOOD."))
                                {
                                    receiveThread[player] = new Thread(new ParameterizedThreadStart(receive));
                                    receiveThread[player].Start(player);
                                }
                                else removeClient(clients[player]);
                            }
                            else removeClient(clients[player]);
                        }
                        catch { removeClient(clients[player]); }
                    }
                }
            }
        }

        private void receive(object parameter)
        {
            int player = (int)parameter;
            byte[] buffer = new byte[2];

            uint dwFlags;
            int keyCode, scanCode;

            while (isWiFiRunning || isUSBRunning)
            {
                Thread.Sleep(1);

                try
                {
                    if (clients[player].Connected)
                    {
                        clients[player].Receive(buffer, 2, SocketFlags.None);

                        if (buffer[0] != 1)
                        {
                            dwFlags = buffer[0];
                            scanCode = keyConfigManager.getScanCode(player, (KeyCode)buffer[1]);
                            keyCode = keyConfigManager.getVirtualKeyCode(player, (KeyCode)buffer[1]);

                            if (scanCode > 128)
                                keybd_event((byte)keyCode, 0, dwFlags, (UIntPtr)1);
                            else if (keyCode > 0)
                                keybd_event((byte)keyCode, 0, dwFlags, (UIntPtr)1);

                            if (scanCode > 0)
                                keybd_event(0, (byte)scanCode, dwFlags, (UIntPtr)1);
                        }
                        else break;
                    }
                    else break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    break;
                }
            }
            try { removeClient(clients[player]); }
            catch { }
        }

        private void removeClient(Socket client)
        {
            try
            {
                int player = clients.First(element => element.Value.Equals(client)).Key;

                if (player > 0)
                {
                    if (clients[player] != null)
                    {
                        clients[player].Close();
                        clients[player] = null;
                    }

                    if (receiveThread[player] != null)
                    {
                        receiveThread[player].Abort();
                        receiveThread[player] = null;
                    }
                    isUSBConnection[player] = false;
                }
            }
            catch { }
        }
    }
}
