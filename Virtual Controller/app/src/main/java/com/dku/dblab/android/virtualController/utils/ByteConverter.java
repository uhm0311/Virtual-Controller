package com.dku.dblab.android.virtualController.utils;

/**
 * Created by cobur_000 on 2015-11-23.
 */
import java.nio.charset.Charset;


class ByteConverter
{
    public final static Charset EUCKR = Charset.forName("EUC-KR");

    private ByteConverter() { }

    public static String byteArrayToHex(byte[] stream)
    {
        if (stream == null || stream.length == 0)
            return null;

        StringBuffer sb = new StringBuffer(stream.length * 2);
        String hexNumber;
        for (int x = 0; x < stream.length; x++)
        {
            hexNumber = "0" + Integer.toHexString(0xff & stream[x]);
            sb.append(hexNumber.substring(hexNumber.length() - 2));
        }
        return sb.toString().toUpperCase();
    }
    private static byte[] hexToByteArray(String hex)
    {
        if (hex == null || hex.length() == 0)
            return null;

        byte[] stream = new byte[hex.length() / 2];
        for (int i = 0; i < stream.length; i++)
            stream[i] = (byte) Integer.parseInt(hex.substring(2 * i, 2 * i + 2), 16);

        return stream;
    }
    private static byte[] stringToByteArray(String str)
    {
        return str.getBytes(EUCKR);
    }

    public static String stringToHex(String str)
    {
        return byteArrayToHex(stringToByteArray(str));
    }
    public static String hexToString(String hex)
    {
        return new String(hexToByteArray(hex), EUCKR);
    }
}
