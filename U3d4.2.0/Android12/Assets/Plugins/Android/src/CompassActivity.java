// jar插件的包名要与Unity中的Bundle Identifier一致
package com.LB.UnityAndroid1;

import com.unity3d.player.UnityPlayerActivity;
//import com.unity3d.player.UnityPlayer;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.telephony.TelephonyManager;
import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.util.Config;
import android.util.Log;
import android.app.Activity;
import android.view.WindowManager;

public class CompassActivity extends UnityPlayerActivity {
public static CompassActivity inst;

private static final String TAG = "Compass";

private SensorManager mSensorManager;
private Sensor mSensor;

static public float xmag;
static public float x;
static public float ymag;
static public float zmag;

private final SensorEventListener mListener = new SensorEventListener() {
public void onSensorChanged(SensorEvent event) {
if (Config.DEBUG) Log.d(TAG,
"sensorChanged (" + event.values[0] + ", " + event.values[1] + ", " + event.values[2] + ")");

xmag = event.values[0];
ymag = event.values[1];
zmag = event.values[2];
}

public void onAccuracyChanged(Sensor sensor, int accuracy) {
}
};

@Override
protected void onCreate(Bundle icicle) {
super.onCreate(icicle);
getWindow().setFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON, WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
mSensorManager = (SensorManager)getSystemService(Context.SENSOR_SERVICE);
mSensor = mSensorManager.getDefaultSensor(Sensor.TYPE_ORIENTATION);
        //setContentView(R.layout.activity_main);
        inst = this;
}

@Override
protected void onResume()
{
if (Config.DEBUG) Log.d(TAG, "onResume");
super.onResume();

mSensorManager.registerListener(mListener, mSensor,
SensorManager.SENSOR_DELAY_GAME);
}

@Override
protected void onStop()
{
if (Config.DEBUG) Log.d(TAG, "onStop");
mSensorManager.unregisterListener(mListener);
super.onStop();
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
/*	
    public static void AndroidFunc1(String arg)
    {    
        UnityPlayer.UnitySendMessage("Main Camera","UnityFunc1",arg);   
    }
	
    public static void AndroidFunc2()
    {    
        UnityPlayer.UnitySendMessage("Main Camera","UnityFunc1","--AndroidFunc2--");   
    }
*/
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

public static float getX() {
//return xmag;
x+=10.0;
return x;
}

public static float getY() {
return ymag;
}

public static float getZ() {
return zmag;
}
}