using UnityEngine;
using System.Collections;
using System;

public class CompassJNI : MonoBehaviour 
{
	static String 	xValue="";
	static String	yValue="";
	static String	zValue="";

	// Use this for initialization
	void Start () 
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJNI.AttachCurrentThread();
#else
		
#endif
	}

	void Update() {
#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
		//using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
		using (AndroidJavaClass cls_CompassActivity = new AndroidJavaClass("com.LB.UnityAndroid1.CompassActivity"))	
		{

			//using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				//cls_CompassActivity.CallStatic("Init", obj_Activity);

				xValue = cls_CompassActivity.CallStatic<String>("getAndroidId");
				yValue = cls_CompassActivity.CallStatic<String>("getAndroidMacID");
				zValue = cls_CompassActivity.CallStatic<String>("getIMEI");

			//}
		}
#else
		
#endif		
	}

	void OnGUI() 
	{
		GUI.Label(new Rect(Screen.width / 2 -200, Screen.height / 2,800,300), "getAndroidId = " + xValue.ToString() + "\r\n MacAddr = " + yValue.ToString() + "\r\n IMEI = " + zValue.ToString());
		if(GUI.Button(new Rect(Screen.width-120,Screen.height-40,120,30),"Click to YUHUA!")) 
		{
			Application.OpenURL("http://blog.csdn.net/libeifs");
		}
	}
}