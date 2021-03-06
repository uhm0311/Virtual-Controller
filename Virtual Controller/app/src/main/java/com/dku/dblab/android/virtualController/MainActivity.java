package com.dku.dblab.android.virtualController;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Rect;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.text.format.Formatter;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.MobileAnarchy.Android.Widgets.Joystick.JoystickMovedListener;
import com.MobileAnarchy.Android.Widgets.Joystick.JoystickView;
import com.dku.dblab.android.virtualController.utils.connections.ConnectionManager;
import com.dku.dblab.android.virtualController.utils.QRCodeReader;
import com.dku.dblab.android.virtualController.utils.connections.ParameterizedRunnable;
import com.dku.dblab.android.virtualController.utils.keys.KeyState;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;

import java.util.HashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends Activity 
{
	private static final int originalWidth = 540;
	private static final int originalHeight = 960;
	
	private static final HashMap<String, Rect> originalViewRects = new HashMap<>();

	static {
		originalViewRects.put("Joystick", new Rect(169, 162, 169 + 200, 162 + 200));

		originalViewRects.put("Coin", new Rect(40, 460, 40 + 100, 460 + 60));
		originalViewRects.put("Start", new Rect(40, 540, 40 + 100, 540 + 60));

		originalViewRects.put("A", new Rect(197, 525, 197 + 118, 525 + 118));
		originalViewRects.put("B", new Rect(70, 652, 70 + 118, 652 + 118));
		originalViewRects.put("C", new Rect(197, 780, 196 + 118, 780 + 118));
		originalViewRects.put("D", new Rect(321, 652, 321 + 118, 652 + 118));

		//originalViewRects.put("E", new Rect(276, 632, 276 + 118, 632 + 118));
		// originalViewRects.put("F", new Rect(276, 762, 276 + 118, 762 + 118));

		//originalViewRects.put("E", new Rect(0, 0, 0, 0));
		//originalViewRects.put("F", new Rect(0, 0, 0, 0));

		originalViewRects.put("L", new Rect(400, 0, 437 + 103, 0 + 150));
		originalViewRects.put("R", new Rect(400, 809, 437 + 103, 809 + 150));
	}
	private RelativeLayout layout = null;
	
	private double widthRatio;
	private double heightRatio;

	private ConnectionManager connectionManager = new ConnectionManager();
	private ExecutorService threadPool = Executors.newCachedThreadPool();

	private Runnable startUSBConnection, closeConnection;
	private ParameterizedRunnable startWiFiConnection, sendKey;

	@SuppressWarnings("deprecation")
	@SuppressLint("NewApi")
	@Override
	protected void onCreate(Bundle savedInstanceState) 
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		setTheme(android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);

		DisplayMetrics displayInfo = new DisplayMetrics();
		MainActivity.this.getWindowManager().getDefaultDisplay().getMetrics(displayInfo);

		widthRatio = (double)displayInfo.widthPixels / (double)originalWidth;
		heightRatio = (double)displayInfo.heightPixels / (double)originalHeight;

		initViews();
		initRunnables();

		showConnectionDialog();
	}

	private void initViews() {
		layout = (RelativeLayout) findViewById(R.id.main_layout);

		final int sdk = android.os.Build.VERSION.SDK_INT;
		if(sdk < android.os.Build.VERSION_CODES.JELLY_BEAN)
			layout.setBackgroundDrawable(getResources().getDrawable(R.drawable.control) );
		else layout.setBackground(getResources().getDrawable(R.drawable.control));

		for (final String viewName : originalViewRects.keySet())
		{
			if (!viewName.equals("Joystick"))
				addView(setOnButtonTouchListener(new Button(this), viewName), viewName);
		}
		addView(setOnJoystickMovedListener(new JoystickView(this)), "Joystick");
	}

	private void initRunnables() {
		startUSBConnection = new Runnable() {
			@Override
			public void run() {
				try { connectionManager.startUSBConnection(); }
				catch (Exception e) { e.printStackTrace(); }
			}
		};

		closeConnection = new Runnable() {
			@Override
			public void run() {
				try { connectionManager.dispose(); }
				catch (Exception e) { e.printStackTrace(); }
			}
		};
	}

	private void showConnectionDialog() {
		final String[] connections = { "USB", "WI-FI" };
		new AlertDialog.Builder(this).setTitle("Select Your Connection Type").setSingleChoiceItems(connections, -1, new DialogInterface.OnClickListener()
		{
			public void onClick(DialogInterface dialog, int item)
			{
				Toast.makeText(getApplicationContext(), "Connection : " + connections[item], Toast.LENGTH_SHORT).show();

				if (item == 1)
				{
					startQRCodeReader();
				}
				else
				{
					new AlertDialog.Builder(MainActivity.this).setTitle("USB Connection").setMessage("Ready to connect?").setCancelable(false)
							.setPositiveButton("확인", new DialogInterface.OnClickListener()
							{
								public void onClick(DialogInterface dialog, int whichButton)
								{
									startUSBConnection();
								}
							}).setNegativeButton("취소", new DialogInterface.OnClickListener()
					{
						public void onClick(DialogInterface dialog, int whichButton)
						{
							finish();
						}
					}).create().show();
				}
				dialog.cancel();
			}
		}).create().show();
	}

	private void startQRCodeReader() {
		new IntentIntegrator(MainActivity.this).initiateScan();
	}

	private void startUSBConnection() {
		threadPool.execute(startUSBConnection);
	}

	private void startWiFiConnection(String ipAddresses) {
		startWiFiConnection = new ParameterizedRunnable(ipAddresses) {
			@Override
			public void run() {
				try { connectionManager.startWiFiConnection(parameter.toString()); }
				catch (Exception e) { e.printStackTrace(); }
			}
		};

		threadPool.execute(startWiFiConnection);
	}

	private void closeConnection() {
		threadPool.execute(closeConnection);
	}

	private void sendKey(String keyName, KeyState keyState) {
		sendKey = new ParameterizedRunnable(new Object[] { keyName, keyState }) {
			@Override
			public void run() {
				Object[] params = (Object[])parameter;

				try { connectionManager.sendKey(params[0].toString(), (KeyState)params[1]); }
				catch (Exception e) { e.printStackTrace(); }
			}
		};

		threadPool.execute(sendKey);
	}

	private void sendKeyUp(String keyName) {
		sendKey(keyName, KeyState.Up);
	}

	private void sendKeyDown(String keyName) {
		sendKey(keyName, KeyState.Down);
	}

    @Override
    public void onDestroy()
    {
		closeConnection();

        super.onDestroy();
    }

	protected void onActivityResult(int requestCode, int resultCode, Intent data)
	{
		if (data != null) {
			IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data);

			WifiManager wifiManager = (WifiManager) getApplicationContext().getSystemService(WIFI_SERVICE);
			WifiInfo wifiInfo = wifiManager.getConnectionInfo();
			int ip = wifiInfo.getIpAddress();

			QRCodeReader qrReader = new QRCodeReader(result, Formatter.formatIpAddress(ip));
			startWiFiConnection(qrReader.toString());
		}
	}

	private Button setOnButtonTouchListener(Button btn, final String viewName)
	{
		if (!viewName.equals("Joystick"))
		{
			btn.setOnTouchListener(new View.OnTouchListener() {
				public boolean onTouch(View v, MotionEvent event) {
					switch (event.getAction()) {
						case MotionEvent.ACTION_DOWN:
							sendKeyDown(viewName);
							break;

						case MotionEvent.ACTION_UP:
							sendKeyUp(viewName);
							break;
					}

					return true;
				}
			});

			btn.setAlpha(0);
		}

		return btn;
	}

	private JoystickView setOnJoystickMovedListener(JoystickView joystick)
	{
		joystick.setOnJoystickMovedListener(new JoystickMovedListener()
		{
			public void OnMoved(int pan, int tilt)
			{
				//pan = y, tilt = x
				if (pan >= 8)
				{
					sendKeyDown("Up");
				}
				else if (pan <= -8)
				{
					sendKeyDown("Down");
				}
				else
				{
					sendKeyUp("Up");
					sendKeyUp("Down");
				}

				if (tilt >= 8)
				{
					sendKeyDown("Right");
				}
				else if (tilt <= -8)
				{
					sendKeyDown("Left");
				}
				else
				{
					sendKeyUp("Right");
					sendKeyUp("Left");
				}
			}
			public void OnReleased()
			{
				sendKeyUp("Up");
				sendKeyUp("Down");
				sendKeyUp("Right");
				sendKeyUp("Left");
			}
		});

		return joystick;
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
