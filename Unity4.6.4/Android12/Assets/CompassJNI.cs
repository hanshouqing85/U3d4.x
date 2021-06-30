using UnityEngine;
using System.Collections;
using System;

public class CompassJNI : MonoBehaviour 
{
	static float 	xValue=0;
	static float	yValue=0;
	static float	zValue=0;
#if UNITY_ANDROID && !UNTIY_EDITOR
	private AndroidJavaClass ajc;
#endif
	// Use this for initialization
	void Start () 
	{
#if UNITY_ANDROID && !UNTIY_EDITOR
        // 找到我们的插件并创建对象
		//ajc=new AndroidJavaClass("com.LB.UnityAndroid1.CompassActivity");//OK
		ajc=new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = ajc.GetStatic<AndroidJavaObject>("currentActivity");
		//jo.Call("StartActivity0","第一个Activity");
		xValue = jo.CallStatic<float>("getX");
		yValue = jo.CallStatic<float>("getY");
		zValue = jo.CallStatic<float>("getZ");
//		xValue = ajc.CallStatic<float>("getX");
//		yValue = ajc.CallStatic<float>("getY");
//		zValue = ajc.CallStatic<float>("getZ");
		/*
		using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
		{
			
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
				
				AndroidJavaClass cls_CompassActivity = new AndroidJavaClass("com.LB.UnityAndroid1.CompassActivity");
				
				cls_CompassActivity.CallStatic("Init", obj_Activity);
				
				xValue = cls_CompassActivity.CallStatic<float>("getX");
				yValue = cls_CompassActivity.CallStatic<float>("getY");
				zValue = cls_CompassActivity.CallStatic<float>("getZ");
				
			}
		}
		*/
#endif
	}

	void Update() {
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void OnGUI() 
	{
		GUI.Label(new Rect(Screen.width / 2 -200, Screen.height / 2, 400,100), "xmag = " + xValue.ToString() + " ymag = " + yValue.ToString() + " zmag = " + zValue.ToString());
		if(GUI.Button(new Rect(Screen.width-120,Screen.height-40,120,30),"Click to YUHUA!")) 
		{
			Application.OpenURL("http://blog.csdn.net/libeifs");
		}
	}
}