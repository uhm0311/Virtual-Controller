package com.dku.dblab.android.virtualController;

import java.io.OutputStream;
import java.net.Socket;
import java.util.HashMap;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.graphics.Rect;
import android.os.Bundle;
import android.text.InputType;
import android.util.DisplayMetrics;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RelativeLayout;

import com.MobileAnarchy.Android.Widgets.Joystick.JoystickMovedListener;
import com.MobileAnarchy.Android.Widgets.Joystick.JoystickView;

public class MainActivity extends Activity 
{
	private static final byte keyDown = 0;
	private static final byte keyUp = 0x0002;
	
	private static final int originalWidth = 540;
	private static final int originalHeight = 960;
	
	private static final HashMap<String, Rect> originalViewRects = new HashMap<String, Rect>();
	private static final HashMap<String, Byte> keyCodes = new HashMap<String, Byte>();
	private static final HashMap<Byte, Byte> keyState = new HashMap<Byte, Byte>();
	
	static
	{
		originalViewRects.put("Joystick", new Rect(169, 65, 169 + 200, 65 + 200));
		
        originalViewRects.put("Coin", new Rect(22, 502, 22 + 103, 502 + 118));
        originalViewRects.put("Start", new Rect(22, 632, 22 + 103, 632 + 118));
        
        originalViewRects.put("A", new Rect(146, 502, 146 + 118, 502 + 118));
        originalViewRects.put("B", new Rect(146, 632, 146 + 118, 632 + 118));
        originalViewRects.put("C", new Rect(146, 762, 146 + 118, 762 + 118));
        originalViewRects.put("D", new Rect(276, 502, 276 + 118, 502 + 118));
        originalViewRects.put("E", new Rect(276, 632, 276 + 118, 632 + 118));
        originalViewRects.put("F", new Rect(276, 762, 276 + 118, 762 + 118));
                
        originalViewRects.put("L", new Rect(437, 0, 437 + 103, 0 + 248));
        originalViewRects.put("R", new Rect(437, 711, 437 + 103, 711 + 248));
        
        keyCodes.put("Start", (byte) 0);
        keyCodes.put("Coin", (byte) 1);
        
        keyCodes.put("Up", (byte) 2);
        keyCodes.put("Down", (byte) 3);
        keyCodes.put("Left", (byte) 4);
        keyCodes.put("Right", (byte) 5);
        
        keyCodes.put("A", (byte) 6);
        keyCodes.put("B", (byte) 7);
        keyCodes.put("C", (byte) 8);
        keyCodes.put("D", (byte) 9);
        keyCodes.put("E", (byte) 10);
        keyCodes.put("F", (byte) 11);
        
        keyCodes.put("L", (byte) 12);
        keyCodes.put("R", (byte) 13);
        
		keyState.put(keyCodes.get("Start"), keyUp);
		keyState.put(keyCodes.get("Coin"), keyUp);
		
		keyState.put(keyCodes.get("Up"), keyUp);
		keyState.put(keyCodes.get("Down"), keyUp);
		keyState.put(keyCodes.get("Left"), keyUp);
		keyState.put(keyCodes.get("Right"), keyUp);
		
		keyState.put(keyCodes.get("A"), keyUp);
		keyState.put(keyCodes.get("B"), keyUp);
		keyState.put(keyCodes.get("C"), keyUp);
		keyState.put(keyCodes.get("D"), keyUp);
		keyState.put(keyCodes.get("E"), keyUp);
		keyState.put(keyCodes.get("F"), keyUp);
		
		keyState.put(keyCodes.get("L"), keyUp);
		keyState.put(keyCodes.get("R"), keyUp);
	}
	private RelativeLayout layout = null;
	
	private double widthRatio;
	private double heightRatio;
	
	private String IPAddress;
	private Socket socket;
	private OutputStream os;

	@SuppressWarnings("deprecation")
	@SuppressLint("NewApi")
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		setTheme(android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		
		layout = (RelativeLayout) findViewById(R.id.main_layout);
		
		final int sdk = android.os.Build.VERSION.SDK_INT;
		if(sdk < android.os.Build.VERSION_CODES.JELLY_BEAN) 
		    layout.setBackgroundDrawable(getResources().getDrawable(R.drawable.controller_lr_rotate) );
		else layout.setBackground(getResources().getDrawable(R.drawable.controller_lr_rotate));
		
        DisplayMetrics displayInfo = new DisplayMetrics();
        MainActivity.this.getWindowManager().getDefaultDisplay().getMetrics(displayInfo);
        
        widthRatio = (double)displayInfo.widthPixels / (double)originalWidth;
        heightRatio = (double)displayInfo.heightPixels / (double)originalHeight;
        
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("아이피 주소를 입력하세요.");

        // Set up the input
        final EditText input = new EditText(this);
        // Specify the type of input expected; this, for example, sets the input as a password, and will mask the text
        input.setInputType(InputType.TYPE_CLASS_TEXT);
        builder.setView(input);

        // Set up the buttons
        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() 
        { 
            @Override
            public void onClick(DialogInterface dialog, int which) 
            {
                IPAddress = input.getText().toString();
                WiFiConnectionStart();
            }
        });
        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() 
        {
            @Override
            public void onClick(DialogInterface dialog, int which) 
            {
                dialog.cancel();
                MainActivity.this.finish();
            }
        });
        builder.show();
        
        
        JoystickView joystick = new JoystickView(this);
        joystick.setOnJostickMovedListener(new JoystickMovedListener()
        {
			public void OnMoved(int pan, int tilt) 
			{
				//pan = y, tilt = x
				if (pan >= 8)
				{
					sendKeyDown(keyCodes.get("Up"));
				}
				else if (pan <= -8)
				{
					sendKeyDown(keyCodes.get("Down"));
				}
				else
				{
					sendKeyUp(keyCodes.get("Up"));
					sendKeyUp(keyCodes.get("Down"));
				}
				
				if (tilt >= 8)
				{
					sendKeyDown(keyCodes.get("Right"));
				}
				else if (tilt <= -8)
				{
					sendKeyDown(keyCodes.get("Left"));
				}
				else
				{
					sendKeyUp(keyCodes.get("Right"));
					sendKeyUp(keyCodes.get("Left"));
				}
			}
			public void OnReleased() 
			{
				sendKeyUp(keyCodes.get("Up"));
				sendKeyUp(keyCodes.get("Down"));
				sendKeyUp(keyCodes.get("Right"));
				sendKeyUp(keyCodes.get("Left"));
			}
        });
        addView(joystick, "Joystick");
        
        for (final String viewName : originalViewRects.keySet())
        {
        	if (!viewName.equals("Joystick"))
        	{
	        	Button btn = new Button(this);
	        	btn.setOnTouchListener(new OnTouchListener() 
	            {
	    			public boolean onTouch(View v, MotionEvent event) 
	    			{
	    				switch (event.getAction() ) 
	    				{
	    				    case MotionEvent.ACTION_DOWN:
	    				    	sendKeyDown(keyCodes.get(viewName));
	    				    	break;
	    				    case MotionEvent.ACTION_UP:
	    				    	sendKeyUp(keyCodes.get(viewName));
	    				    	break;
	    				}
	    				return true;
	    			}
	    		});
	            addView(btn, viewName);
        	}
        }
	}
	
	private void WiFiConnectionStart()
	{
		new Thread(new Runnable() 
		{
			public void run() 
			{
				try
				{
					socket = new Socket(IPAddress, 8731);
					socket.setSendBufferSize(10240);
					os = socket.getOutputStream();
				}
				catch (Exception e)
				{
					e.printStackTrace();
				}
			}
		}).start();
	}
	
	private void sendKeyDown(byte keyCode)
	{
		try
		{
			if (keyState.get(keyCode).equals(keyUp))
			{
				keyState.put(keyCode, keyDown);
				os.write(new byte[] { keyDown, keyCode });
			}
		}
		catch (Exception e)
		{
			
		}
	}
	
	private void sendKeyUp(byte keyCode)
	{
		try
		{
			if (keyState.get(keyCode).equals(keyDown))
			{
				keyState.put(keyCode, keyUp);
				os.write(new byte[] { keyUp, keyCode });
			}
		}
		catch (Exception e)
		{
			
		}
	}
	
	private void addView(View view, String viewName)
	{
		Rect originalViewRect = originalViewRects.get(viewName);
		
		RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams((int) Math.ceil(originalViewRect.width() * widthRatio), (int) Math.ceil(originalViewRect.height() * heightRatio));
        lp.leftMargin = (int) (originalViewRect.left * widthRatio);
        lp.topMargin = (int) (originalViewRect.top * heightRatio);
        
        layout.addView(view, lp);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) 
	{
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) 
	{
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings) 
		{
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
}
