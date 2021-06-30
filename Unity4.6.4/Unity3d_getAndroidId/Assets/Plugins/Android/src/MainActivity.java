package com.bailianlong.t;

import android.os.Bundle;
//import android.app.Activity;
//import android.view.Menu;
import android.util.Log;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.telephony.TelephonyManager;
import com.unity3d.player.UnityPlayerActivity;
import com.unity3d.player.UnityPlayer;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Iterator;
import android.hardware.usb.UsbConstants;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbDeviceConnection;
import android.hardware.usb.UsbEndpoint;
import android.hardware.usb.UsbInterface;
import android.hardware.usb.UsbManager;
import android.hardware.usb.UsbRequest;
import android.os.Handler;
import android.os.Message;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.IntentFilter;

public class MainActivity extends UnityPlayerActivity {

    public static MainActivity inst;

	private boolean bUsbDeviceDetached;
	private boolean stopCheckJoystickThread;
	private boolean[] ReadFlag={false,false};	
	private UsbManager manager; // USB管理器
	private UsbDevice[] mUsbDevice={null,null}; // 找到的USB设备
	//private ListView lsv1; // 显示USB信息的
	private UsbInterface[] mInterface={null,null,null,null,null,null,null,null};// 每个设备4个接口
	private UsbDeviceConnection[] mDeviceConnection={null,null};
	private PendingIntent mPermissionIntent;
	private static final String ACTION_USB_PERMISSION = "com.bailianlong.t.USB_PERMISSION";
	private final int m_VendorID = 4300;//0X10CC
	private final int m_ProductID =21762;//0X5502
    private final int m_ProductID2 =21772;//0X550C
	private static String m_UnityObjName = "Main Camera";
	private static String m_UnityFuncName = "OnReadJoyData";

	public static void SetUnityObjName(String str)
	{
		m_UnityObjName=str;
	}

	public static void ChangeReadFlag()
	{
		inst.changeReadFlag();
	}

	public void changeReadFlag()
	{
		ReadFlag[0]=!ReadFlag[0];
		ReadFlag[1]=!ReadFlag[1];
	}

	/** Called when the activity is first created. */
	@Override
		public void onCreate(Bundle savedInstanceState) 
		{
			super.onCreate(savedInstanceState);
            inst = this;

			//filter.addAction(Intent.ACTION_UMS_CONNECTED);
			//filter.addAction(Intent.ACTION_UMS_DISCONNECTED);
			filter.addAction(Intent.ACTION_POWER_CONNECTED);

			filter.addAction("android.intent.action.MEDIA_MOUNTED");
			filter.addAction("android.intent.action.MEDIA_EJECT");

			filter.addAction(UsbManager.ACTION_USB_DEVICE_ATTACHED);
			filter.addAction(UsbManager.ACTION_USB_DEVICE_DETACHED);
			filter.addAction(UsbManager.ACTION_USB_ACCESSORY_ATTACHED);
			filter.addAction(UsbManager.ACTION_USB_ACCESSORY_DETACHED);
			filter.addAction(UsbManager.EXTRA_PERMISSION_GRANTED);
			filter.addAction(UsbManager.EXTRA_DEVICE);
			filter.addAction(UsbManager.EXTRA_ACCESSORY);
			filter.addAction(ACTION_USB_PERMISSION);

			mPermissionIntent = PendingIntent.getBroadcast(this, 0,new Intent(ACTION_USB_PERMISSION), 0);
			//IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
			registerReceiver(receiver, filter);
			registerReceiver(mUsbReceiver, filter);
        
			//UsbManager mUsbManager = (UsbManager) getSystemService(Context.USB_SERVICE);
			// 获取USB设备
			manager = (UsbManager) getSystemService(Context.USB_SERVICE);
			if (manager == null) 
			{
				return;
			} 
			else 
			{
				//Log.i(TAG, "usb设备：" + String.valueOf(manager.toString()));
			}
			HashMap<String, UsbDevice> deviceList = manager.getDeviceList();
			//Log.i(TAG, "usb设备：" + String.valueOf(deviceList.size()));
			//tv.setText(tv.getText() + "usb设备：" + String.valueOf(deviceList.size()));
			Iterator<UsbDevice> deviceIterator = deviceList.values().iterator();
			int mUsbDeviceCount=0;
			while (deviceIterator.hasNext()) 
			{
				UsbDevice device = deviceIterator.next();
  
				// 在这里添加处理设备的代码mUsbDevice==null && 
				/*if((device.getVendorId() == m_VendorID && device.getProductId() == m_ProductID)) 
				{
					mUsbDeviceCount++;
					if(mUsbDeviceCount==1||mUsbDeviceCount==2)
					{
						mUsbDevice[mUsbDeviceCount-1] = device;
						ReadFlag[mUsbDeviceCount-1]=false;
						manager.requestPermission(mUsbDevice[mUsbDeviceCount-1], mPermissionIntent);
						//Log.i(TAG, "找到设备");
						//tv.setText(tv.getText() + "找到设备,DeviceId=" + String.valueOf(device.getDeviceId()));
					}
				}*/
				if((device.getVendorId() == m_VendorID && (device.getProductId() == m_ProductID||device.getProductId() == m_ProductID2))) 
				{
					mUsbDeviceCount++;
					if(mUsbDeviceCount==1||mUsbDeviceCount==2)
					{
						if(device.getProductId() == m_ProductID)
						{
							mUsbDevice[0] = device;
							ReadFlag[0]=false;
							manager.requestPermission(mUsbDevice[0], mPermissionIntent);
						}
						else
						{
							mUsbDevice[1] = device;
							ReadFlag[1]=false;
							manager.requestPermission(mUsbDevice[1], mPermissionIntent);
						}
						//tv.setText(tv.getText() + "找到设备,DeviceId=" + String.valueOf(device.getDeviceId()));
					}
				}

			}
			new Thread(ListenerDeviceRunnable).start();
		}

	private final BroadcastReceiver mUsbReceiver = new BroadcastReceiver() 
	{

		public void onReceive(Context context, Intent intent) 
		{
			String action = intent.getAction();

			//tv.setText(tv.getText() + "mUsbReceiver.onReceive = " + action + "\n");

			if (ACTION_USB_PERMISSION.equals(action)) 
			{
				synchronized (this) 
				{
					UsbDevice device = (UsbDevice) intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);

					if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false))
					{
						if (device != null) 
						{
							//tv.setText(tv.getText()+"permission for device-------------------------- ");
							//tv.setText(tv.getText() + "device.DeviceId=" + String.valueOf(device.getDeviceId()));
							if(mUsbDevice[0]!=null)
							{
								//tv.setText(tv.getText() + "mUsbDevice[0].DeviceId=" + String.valueOf(mUsbDevice[0].getDeviceId()));
								findIntfAndEpt(0);
								ReadFlag[0]=true;
							}
							if(mUsbDevice[1]!=null)
							{
								//tv.setText(tv.getText() + "mUsbDevice[1].DeviceId=" + String.valueOf(mUsbDevice[1].getDeviceId()));
								findIntfAndEpt(1);
								ReadFlag[1]=true;
							}
						}
					} 
					else 
					{
						//tv.setText(tv.getText()+"permission denied for device " + device);
					}
				}
			}
		}
	};

	@Override
		public boolean onKeyUp(int keyCode, KeyEvent event) 
		{
			//tv.setText(tv.getText() + "keyUp " + keyCode + "\n");
			InputDevice device = event.getDevice();
			if (device != null && device.getSources() == 0x1000511 && keyCode == 4) 
			{
				return true;
			}
			return super.onKeyUp(keyCode, event);
		}

	@Override
		public boolean onKeyLongPress(int keyCode, KeyEvent event) 
		{
			//tv.setText(tv.getText() + "keyLongPress " + keyCode + "\n");
			return super.onKeyLongPress(keyCode, event);
		}

	@Override
		public boolean onKeyDown(int keyCode, KeyEvent event) 
		{
			//tv.setText(tv.getText() + "keyDown keyCode = " + keyCode+ ", scanCode = " + event.getScanCode() + "\n");

			InputDevice device = event.getDevice();
			if (device != null && device.getSources() == 0x1000511 && keyCode == 4) 
			{
				return true;
			}

			return super.onKeyDown(keyCode, event);
		}

	@Override
		public boolean onGenericMotionEvent(MotionEvent event) 
		{
			float x = event.getX();
			float y = event.getY();

			//tv.setText(tv.getText() + " onGenericMotionEvent x = " + event.getX()+ " y = " + event.getY() + "\n");

			if ((x == -1 && y == -1) || (x == 1 && y == 1) || (x == 1 && y == -1)
				|| (x == -1 && y == 1)) 
			{
				return true;
			} 
			else 
			{
				return super.onGenericMotionEvent(event);
			}
		}

	@Override
		public boolean onTouchEvent(MotionEvent event) 
		{
			//System.out.println("onTouchEvent");
			//System.out.println("x = " + event.getX());
			//System.out.println("y = " + event.getY());
			// tv.setText(tv.getText() + " onTouchEvent x = " + event.getX());
			return super.onTouchEvent(event);
		}

	@Override
		protected void onResume() 
		{
			this.registerReceiver(receiver, filter);
			registerReceiver(mUsbReceiver, filter);
			super.onResume();
		}

	@Override
		protected void onStop() 
		{
			this.unregisterReceiver(receiver);
			this.unregisterReceiver(mUsbReceiver);
			super.onStop();
		}

	private IntentFilter filter = new IntentFilter();

	private BroadcastReceiver receiver = new BroadcastReceiver() 
	{

		@Override
			public void onReceive(Context arg0, Intent arg1) 
			{
				String action = arg1.getAction();
				//Log.d(TAG, "onReceive()   action = " + action);
				//tv.setText(tv.getText() + "  receiver.onReceive = " + action + "\n");

				if (action.equals(UsbManager.ACTION_USB_DEVICE_ATTACHED)) 
				{
					//tv.setText(tv.getText() + "  usb connected");
				} 
				else if (action.equals(UsbManager.ACTION_USB_DEVICE_DETACHED)) 
				{
					//tv.setText(tv.getText() + "  usb disconnected");
					bUsbDeviceDetached = true;
				}

				if (action.equalsIgnoreCase("android.hardware.usb.action.USB_DEVICE_DETACHED")) 
				{
					//usb 设备拔出事件
					bUsbDeviceDetached = true;
				}

			}
	};

	
	/**
	 * 这个线程在启动程序的时候启动，每隔0.2秒读一下USB HID设备
	 */
	Runnable ListenerDeviceRunnable = new Runnable() 
	{
		@Override
			public void run() 
			{
				UsbManager manager = (UsbManager) getSystemService(Context.USB_SERVICE);
				while (!stopCheckJoystickThread) 
				{
					if(ReadFlag[0] && mDeviceConnection[0]!=null)
					{
						ReadData(0);
						ReadData(1);
						ReadData(2);
						ReadData(3);					
					}
					if(ReadFlag[1] && mDeviceConnection[1]!=null)
					{
						ReadData(4);
						ReadData(5);
						ReadData(6);
						ReadData(7);					
					}				
					try 
					{
						Thread.sleep(200);
					} 
					catch (InterruptedException e) 
					{
						e.printStackTrace();
					}

				}
			}

	};

	private Handler _handler = new Handler() 
	{
		@Override
			public void handleMessage(Message msg) 
			{
				int what = msg.what;
				if(what>=0&&what<8){
                   //OnCheckJoyindex(what,msg.obj.toString());
				   // Unity手柄号Joyindex不一定等于端口号idx
                   UnityPlayer.UnitySendMessage(m_UnityObjName,m_UnityFuncName,msg.obj.toString());
				}
				super.handleMessage(msg);
			}
	};

	@Override
		protected void onDestroy() 
		{
			stopCheckJoystickThread = true;
			if (mDeviceConnection[0]!= null) 
			{
				synchronized (mDeviceConnection[0])
				{
					mDeviceConnection[0].releaseInterface(mInterface[0]);
					mDeviceConnection[0].releaseInterface(mInterface[1]);
					mDeviceConnection[0].releaseInterface(mInterface[2]);
					mDeviceConnection[0].releaseInterface(mInterface[3]);				
					mDeviceConnection[0].close();
					mDeviceConnection[0]=null;
				}
			}
			if (mDeviceConnection[1]!= null) 
			{
				synchronized (mDeviceConnection[1])
				{
					mDeviceConnection[1].releaseInterface(mInterface[4]);
					mDeviceConnection[1].releaseInterface(mInterface[5]);
					mDeviceConnection[1].releaseInterface(mInterface[6]);
					mDeviceConnection[1].releaseInterface(mInterface[7]);				
					mDeviceConnection[1].close();
					mDeviceConnection[1]=null;
				}
			}		
			super.onDestroy();
		}
	
	// 寻找接口和分配结点,int Interfaceindex
	private void findIntfAndEpt(int Devindex) 
	{
		if (mUsbDevice[Devindex] == null) 
		{
			//tv.setText(tv.getText() + "mUsbDevice == null没有找到设备");
			return;
		}
		int nInterfaceCount=mUsbDevice[Devindex].getInterfaceCount();
		//tv.setText(tv.getText() + "接口个数:" + String.valueOf(nInterfaceCount));
		for (int i = 0; i < nInterfaceCount;i++) 
		{
			// 获取设备接口，一般都是一个接口，你可以打印getInterfaceCount()方法查看接口的个数，在这个接口上有两个端点，OUT 和 IN
			UsbInterface intf = mUsbDevice[Devindex].getInterface(i);
			mInterface[Devindex*nInterfaceCount+i] = intf;
			//String str="Id:" + intf.getId()+"InterfaceClass:" + intf.getInterfaceClass()+ "InterfaceSubclass:" + intf.getInterfaceSubclass()+ "InterfaceProtocol:" + intf.getInterfaceProtocol();
			//tv.setText(tv.getText() +"第"+String.valueOf(i)+ "个接口：" + str+"\n");
			if (intf != null) 
			{
				// 判断是否有权限
				if (manager.hasPermission(mUsbDevice[Devindex])) 
				{
					if (mDeviceConnection[Devindex]== null) 
					{
						// 打开设备，获取 UsbDeviceConnection 对象，连接设备，用于后面的通讯
						mDeviceConnection[Devindex] = manager.openDevice(mUsbDevice[Devindex]);
					}
					if (mDeviceConnection[Devindex]== null) 
					{
						return;
					}
					if (mDeviceConnection[Devindex].claimInterface(intf, true)) 
					{
						//tv.setText(tv.getText() + "找到接口");
						// 用UsbDeviceConnection 与 UsbInterface 进行端点设置和通讯
						getEndpoint(mDeviceConnection[Devindex], intf,Devindex*nInterfaceCount+i);
					} 
					else 
					{
						mDeviceConnection[Devindex].close();
					}
				} 
				else 
				{
					//tv.setText(tv.getText() + "没有权限");
				}
			} 
			else 
			{
				//tv.setText(tv.getText() + "没有找到接口");
			}
            
		}
	}

	private UsbEndpoint[] epIn={null,null,null,null,null,null,null,null};
 
	// 用UsbDeviceConnection 与 UsbInterface 进行端点设置和通讯
	private void getEndpoint(UsbDeviceConnection connection, UsbInterface intf,int idx) 
	{
		//getDirection值为0是输出，128是输入的 ;getInterfaceClass=3
		if (intf.getEndpoint(0) != null) 
		{
			epIn[idx] = intf.getEndpoint(0);
			//tv.setText(tv.getText() + "endpoint0 direction-->" + epIn[idx].getDirection() + "," + "class-->" + intf.getInterfaceClass()+ "\n"); 
		}
		else
		{
			//tv.setText(tv.getText() + "epIn[idx] =intf.getEndpoint(0) == null");
		}       
	}
     
	void ReadData(int idx)
	{
		int inMax = epIn[idx].getMaxPacketSize();  
		ByteBuffer byteBuffer = ByteBuffer.allocate(inMax);    
		UsbRequest usbRequest = new UsbRequest();    
		usbRequest.initialize(mDeviceConnection[idx<4?0:1],epIn[idx]);    
		usbRequest.queue(byteBuffer, inMax);
		//String str="idx=" + String.valueOf(idx)+","+"inMax=" + String.valueOf(inMax)+"\n";
		String str="";
		if(mDeviceConnection[idx<4?0:1].requestWait() == usbRequest)
		{    
			byte[] retData= byteBuffer.array();
			//str=str+Bytes2String(retData)+ "\n";
			str=Bytes2HexString(retData);
		}
		if(str.length()>36)
		{
           str=str.substring(0,36);
		}
		Message msg = new Message();
		msg.what = idx;
		msg.obj = str;
		_handler.sendMessage(msg);	
	}
    
	public static String Bytes2String(byte[] b) 
	{
		String ret = "";
		for (int i = 0; i < b.length; i++) 
		{
			String hex = Integer.toHexString(b[i]& 0xFF);
			ret += hex.toUpperCase()+"-";
		}
		return ret;
	}
 
	public static String Bytes2HexString(byte[] b) 
	{
		String ret = "";
		for (int i = 0; i < b.length; i++) 
		{
			String hex = Integer.toHexString(b[i] & 0xFF);
			if (hex.length() == 1) 
			{
				hex = '0' + hex;
			}
			ret += hex.toLowerCase();
		}
		return ret;
	}
       
	public static byte uniteBytes(byte src0, byte src1) 
	{
		byte _b0 = Byte.decode("0x" + new String(new byte[]{src0})).byteValue();
		_b0 = (byte)(_b0 << 4);
		byte _b1 = Byte.decode("0x" + new String(new byte[]{src1})).byteValue();
		byte ret = (byte)(_b0 ^ _b1);
		return ret;
	}
    
	public static byte[] HexString2Bytes(String src)
	{
		byte[] ret = new byte[8];
		byte[] tmp = src.getBytes();
		for(int i=0; i<8; i++)
		{
			ret[i] = uniteBytes(tmp[i*2], tmp[i*2+1]);
		}
		return ret;
	}

    
	public static String getAndroidMacID()
	{
	   
	    String str = null;   
	   
	    WifiManager wifi = (WifiManager)inst.getSystemService(Context.WIFI_SERVICE);     
	    WifiInfo info = wifi.getConnectionInfo();     
	    str = info.getMacAddress();     
	   
	    if(str==null)
	    {   
	        Log.e("获取android mac地址失败", "0000000");	
	    }   
	    Log.e("获取android mac地址 "+str, "00000000");   
	   
	    return str;
	}
	
	public static String getIMSI()
    {
	    return ((TelephonyManager)inst.getSystemService(Context.TELEPHONY_SERVICE)).getSubscriberId();
	}
	
	public static String getIMEI()
    {
	    return ((TelephonyManager)inst.getSystemService(Context.TELEPHONY_SERVICE)).getDeviceId();
	}
	
	public static String getAndroidId()
    {
	    return android.provider.Settings.Secure.getString(inst.getContentResolver(),android.provider.Settings.Secure.ANDROID_ID);
	}
	
	public static String getSimSerialNumber()
    {
	    return ((TelephonyManager)inst.getSystemService(Context.TELEPHONY_SERVICE)).getSimSerialNumber();
	}

	public static String getSerialNumber1()
    {
	    return android.os.Build.SERIAL;
	}
	
	/**

	* getSerialNumber

	* @return result is same to getSerialNumber1()

	*/
	/*public static String getSerialNumber()
	{
	    String serial = null;
	    try {
	    Class<?> c =Class.forName("android.os.SystemProperties");
	       Method get =c.getMethod("get", String.class);
	       serial = (String)get.invoke(c, "ro.serialno");
	    } catch (Exception e) {
	       e.printStackTrace();
	    }
	    return serial;
	}*/
	
	public static String getuniqueId()
	{
        TelephonyManager tm = (TelephonyManager)inst.getSystemService(Context.TELEPHONY_SERVICE);
        String imei=tm.getDeviceId();
        String simSerialNumber=tm.getSimSerialNumber();
        String androidId=android.provider.Settings.Secure.getString(inst.getContentResolver(),android.provider.Settings.Secure.ANDROID_ID);
        java.util.UUID deviceUuid =new java.util.UUID(androidId.hashCode(),((long)imei.hashCode() << 32) |simSerialNumber.hashCode());
        String uniqueId=deviceUuid.toString();
        return uniqueId;
    }
	

    //判断大小  
	public static int Max(int a,int b)  
    {  
        if(a>b)  
            return a;  
        return b;  
    }

	public static void OnCheckJoyindex(int idx,String arg)
	{    
		String arg1="OnCheckJoyindex"+String.valueOf(idx);
		UnityPlayer.UnitySendMessage(m_UnityObjName,arg1,arg); 
	}
	
    public static void SendMsgToUnity(String arg1,String arg2)
    {    
        UnityPlayer.UnitySendMessage(m_UnityObjName,arg1,arg2);   
    }

    public static void AndroidFunc1(String arg)
    {    
        UnityPlayer.UnitySendMessage(m_UnityObjName,"UnityFunc1",arg);   
    }
	
    public static void AndroidFunc2()
    {    
        UnityPlayer.UnitySendMessage(m_UnityObjName,"UnityFunc1","--AndroidFunc2--");   
    }
    
}
