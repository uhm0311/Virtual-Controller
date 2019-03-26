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

    public void dispose()
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                try { serverSocket.close(); } catch (Exception e) { }
                try { socket.close(); } catch (Exception e) { }
                try { os.close(); } catch (Exception e) { }
            }
        }).start();
    }

    public void startUSBConnection()
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    serverSocket = new ServerSocket(serverPort);
                    socket = serverSocket.accept();
                    socket.setSendBufferSize(10240);
                    os = socket.getOutputStream();
                } catch (Exception e) {

                }
            }
        }).start();
    }

    public void startWiFiConnection(String ipAddresses)
    {
        final Scanner sc = new Scanner(ipAddresses);

        while(sc.hasNextLine()) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    try { validate(new Socket(sc.nextLine(), clientPort)); } catch (Exception e) { }
                }
            }).start();
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
                try { soc.close(); soc = null; }
                catch (Exception ex) { }

                e.printStackTrace();
            }
        }

        try { soc.close(); soc = null; }
        catch (Exception ex) { }
    }

    private void sendKey(final KeyCode keyCode, final KeyState keyState)
    {
        new Thread(new Runnable() {
            @Override
            public void run() {
                try
                {
                    if (!keyManager.getKeyState(keyCode).equals(keyState))
                    {
                        keyManager.setKeyDown(keyCode);
                        os.write(new byte[] { (byte)keyState.ordinal(), (byte)keyCode.ordinal() });
                    }
                }
                catch (Exception e)
                {
                    try { socket.close(); socket = null; }
                    catch (Exception ex) { }
                }
            }
        }).start();
    }

    private void sendKeyDown(KeyCode keyCode)
    {
        sendKey(keyCode, KeyState.Down);
    }

    private void sendKeyUp(KeyCode keyCode)
    {
        sendKey(keyCode, KeyState.Up);
    }

    private void sendKey(String keyName, KeyState keyState) {
        sendKey(keyManager.getKeyCode(keyName), keyState);
    }

    public void sendKeyDown(String keyName) {
        sendKey(keyName, KeyState.Down);
    }

    public void sendKeyUp(String keyName) {
        sendKey(keyName, KeyState.Up);
    }
}
