package com.dku.dblab.android.virtualController.utils.connections;

import com.dku.dblab.android.virtualController.utils.keys.KeyCode;
import com.dku.dblab.android.virtualController.utils.keys.KeyManager;
import com.dku.dblab.android.virtualController.utils.keys.KeyState;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.Charset;
import java.util.Random;
import java.util.Scanner;

/**
 * Created by cobur_000 on 2015-11-23.
 */
public class ConnectionManager {
    private static final int clientPort = 8731;
    private static final int serverPort = 39123;

    private ServerSocket serverSocket;
    private Socket socket;
    private OutputStream os;

    private KeyManager keyManager = new KeyManager();

    public void dispose() throws IOException {
        if (os != null) {
            os.write(new byte[] { (byte)KeyState.Close.ordinal(), 0 });

            os.close();
            os = null;
        }

        if (socket != null) {
            socket.close();
            socket = null;
        }

        if (serverSocket != null) {
            serverSocket.close();
            serverSocket = null;
        }
    }

    public void startUSBConnection() throws IOException {
        serverSocket = new ServerSocket(serverPort);
        socket = serverSocket.accept();
        socket.setSendBufferSize(10240);
        os = socket.getOutputStream();
    }

    public void startWiFiConnection(String ipAddresses) throws IOException {
        Scanner sc = new Scanner(ipAddresses);

        while(sc.hasNextLine()) {
            validate(new Socket(sc.nextLine(), clientPort));
        }

        sc.close();
    }

    private void validate(Socket soc)
    {
        if (soc != null)
        {
            try
            {
                int validationNumber = new Random().nextInt(1000000000);

                soc.setSoTimeout(2000);

                PrintWriter writer = new PrintWriter(soc.getOutputStream(), true);
                BufferedReader reader = new BufferedReader(new InputStreamReader(soc.getInputStream(), Charset.forName("EUC-KR")));

                writer.println("vc?");
                writer.println(validationNumber);

                String response = reader.readLine();
                int num = Integer.parseInt(reader.readLine());

                if (response.toUpperCase().trim().equals("VC.") && num == ~validationNumber)
                {
                    writer.println("good.");
                    socket = soc;
                    socket.setSendBufferSize(10240);
                    os = socket.getOutputStream();

                    return;
                }
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }

            try { soc.close(); soc = null; }
            catch (Exception ex) { }
        }
    }

    private void sendKey(final KeyCode keyCode, final KeyState keyState) throws IOException {
        if (!keyManager.getKeyState(keyCode).equals(keyState))
        {
            keyManager.setKeyState(keyCode, keyState);
            os.write(new byte[] { (byte)keyState.ordinal(), (byte)keyCode.ordinal() });
        }
    }

    public void sendKey(String keyName, KeyState keyState) throws IOException {
        sendKey(keyManager.getKeyCode(keyName), keyState);
    }
}
