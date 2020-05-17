package com.dku.dblab.android.virtualController.utils;

import com.google.zxing.integration.android.IntentResult;

import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.StringTokenizer;

/**
 * Created by cobur_000 on 2015-11-23.
 */
public class QRCodeReader
{
    private IntentResult result;
    private String deviceIPAddress;

    public QRCodeReader(IntentResult result, String deviceIPAddress)
    {
        this.result = result;
        this.deviceIPAddress = deviceIPAddress;
    }

    public String toString()
    {
        String contents = result.getContents();
        StringBuilder builder = new StringBuilder();

        List<String> firstPriorities = new ArrayList<String>();
        List<String> secondPriorities = new ArrayList<String>();
        List<String> thirdPriorities = new ArrayList<String>();

        Scanner scanner = new Scanner(ByteConverter.hexToString(contents));
        String line;

        while (scanner.hasNextLine())
        {
            line = scanner.nextLine();

            if (line != null && line.trim().length() > 0)
            {
                StringTokenizer ipTokenzier1 = new StringTokenizer(new String(deviceIPAddress), ".");
                StringTokenizer ipTokenzier2 = new StringTokenizer(new String(line), ".");
                int priority = 4;

                while (ipTokenzier1.countTokens() > 1 && ipTokenzier2.countTokens() > 1)
                {
                    if (ipTokenzier1.nextToken().equals(ipTokenzier2.nextToken()))
                        priority--;
                }

                switch (priority)
                {
                    case 1: firstPriorities.add(line); break;
                    case 2: secondPriorities.add(line); break;
                    case 3: thirdPriorities.add(line); break;
                }
            }
            else break;
        }

        for (String ipAddress : firstPriorities)
            builder.append(ipAddress + "\r\n");

        for (String ipAddress : secondPriorities)
            builder.append(ipAddress + "\r\n");

        for (String ipAddress : thirdPriorities)
            builder.append(ipAddress + "\r\n");

        return builder.toString();
    }
}
