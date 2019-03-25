package com.dku.dblab.android.virtualController.connections;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.Socket;

public class SocketUtility 
{
	private OutputStream outputStream;
	private InputStream inputStream;
	private PrintWriter writer;
	private BufferedReader reader;
	
	public SocketUtility(Socket sender) throws Exception
	{
		outputStream = sender.getOutputStream();
		inputStream = sender.getInputStream();
		writer = new PrintWriter(outputStream, true);
		reader = new BufferedReader(new InputStreamReader(inputStream, "EUC-KR"));
	}
	
	public void dispose() throws Exception
	{
		writer.close();
		reader.close();
		outputStream.close();
		inputStream.close();
	}
	
	public int readInt() throws Exception
	{
		return Integer.parseInt(readLine());
	}
	
	public String readLine() throws Exception
	{
		return reader.readLine();
	}
	
	public void writeInt(int value)
    {
		writeLine(String.valueOf(value));
    }

    public void writeLine(String content)
    {
        writer.println(content);
    }
}