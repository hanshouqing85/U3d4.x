using UnityEngine;
using System.Collections;
using System;
//using System.Runtime.InteropServices;

public class PluginImport : MonoBehaviour {
	
	private AndroidJavaClass ajc;
	private int iRet = 0;
	string strMacAddr="";
	string strAndroidId="";

	void Start () {
		#if UNITY_ANDROID
			// 通过unity提供的访问java插件的帮助类来找到我们的插件并创建对象
			//ajc = new AndroidJavaClass("com.bailianlong.t.MainActivity");
		    ajc = new AndroidJavaClass("com.LB.UnityAndroid1.CompassActivity");
			iRet =ajc.CallStatic<int>("Max", new object[] { 10, 20 });
			strMacAddr=ajc.CallStatic<string>("getAndroidMacID");
			strAndroidId=ajc.CallStatic<string>("getAndroidId");
		#endif
	}
	
	void OnGUI()
	{
		//改变字体大小
		GUI.skin.label.fontSize = 30;

		GUI.Label(new Rect (10, 10, 500, 60),"iRet="+iRet.ToString());
		GUI.Label(new Rect(10, 80, 500, 60), "MacAddr="+strMacAddr.ToString());
		GUI.Label(new Rect(10, 150, 500, 60), "AndroidId="+strAndroidId.ToString());
		
		if(GUI.Button(new Rect(10, 300, 100, 60), "Quit"))
		{
			Application.Quit();
		}

		if(GUI.Button(new Rect(10,420,200,100), "Max"))  
		{  
		    #if UNITY_ANDROID
			   iRet =ajc.CallStatic<int>("Max", new object[] { 6, 14 });
            #endif
		}

	}
	
}
