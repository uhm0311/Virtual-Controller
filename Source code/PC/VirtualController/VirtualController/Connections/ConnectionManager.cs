using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using System.Runtime.InteropServices;

using VirtualController.GUI;

namespace VirtualController.Connections
{
    public class ConnectionManager
    {
        private KeyConfigDialog keyConfigDialog;

        private bool isRunning = false;
        private int port;

        private int maxPlayers;

        private TcpListener server;
        private Dictionary<int, Socket> clients = new Dictionary<int, Socket>();

        private Thread acceptThread;
        private Dictionary<int, Thread> receiveThread = new Dictionary<int, Thread>();

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public ConnectionManager(int port, KeyConfigDialog keyConfigDialog)
        {
            this.port = port;
            this.keyConfigDialog = keyConfigDialog;
        }

        public bool getIsRunning()
        {
            return this.isRunning;
        }

        public void startWiFiConnection(int maxPlayers)
        {
            isRunning = true;
            this.maxPlayers = maxPlayers;

            server = new TcpListener(8731);
            server.Start(maxPlayers);

            acceptThread = new Thread(new ThreadStart(accept));
            acceptThread.Start();

            for (int i = 1; i <= maxPlayers; i++)
            {
                receiveThread.Add(i, null);
                clients.Add(i, null);
            }
        }

        public void stopWiFiConnection()
        {
            isRunning = false;

            server.Stop();
            server = null;

            acceptThread.Abort();
            acceptThread = null;

            for (int i = 1; i <= maxPlayers; i++)
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
            clients.Clear();
            receiveThread.Clear();
        }

        private void accept()
        {
            while (isRunning)
            {
                try { addClient(server.AcceptSocket()); }
                catch { }
            }
        }

        private void addClient(Socket client)
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i] == null)
                {
                    clients[i] = client;
                    client.ReceiveBufferSize = 10240;

                    switch (i)
                    {
                        case 1:
                            receiveThread[1] = new Thread(new ThreadStart(receiveP1));
                            receiveThread[1].Start();
                            break;
                        case 2:
                            receiveThread[2] = new Thread(new ThreadStart(receiveP2));
                            receiveThread[2].Start();
                            break;
                        case 3:
                            receiveThread[3] = new Thread(new ThreadStart(receiveP3));
                            receiveThread[3].Start();
                            break;
                        case 4:
                            receiveThread[4] = new Thread(new ThreadStart(receiveP4));
                            receiveThread[4].Start();
                            break;
                    }
                    return;
                }
            }
            client.Close();
            client = null;
        }

        private void receiveP1()
        {
            receive(1);
        }

        private void receiveP2()
        {
            receive(2);
        }

        private void receiveP3()
        {
            receive(3);
        }

        private void receiveP4()
        {
            receive(4);
        }

        private void receive(int player)
        {
            byte[] buffer = new byte[2];

            uint dwFlags;
            int keyCode, scanCode;

            while (isRunning)
            {
                try
                {
                    clients[player].Receive(buffer, 2, SocketFlags.None);
                    dwFlags = buffer[0];
                    keyCode = buffer[1];
                    scanCode = keyConfigDialog.getScanCode(player, keyCode);

                    if ((int)scanCode > 0)
                        keybd_event(0, (byte)scanCode, dwFlags, (UIntPtr)1);
                }
                catch
                {
                    break;
                }
            }
            try { removeClient(clients[player]); }
            catch { }
        }

        private void removeClient(Socket client)
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
            }
        }
    }
}
