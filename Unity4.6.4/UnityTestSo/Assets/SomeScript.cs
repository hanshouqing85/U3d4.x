using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class SomeScript : MonoBehaviour {
	// This tells unity to look up the function FooPluginFunction
	// inside the plugin named "PluginName"
	[DllImport ("PluginName")]
	private static extern float FooPluginFunction ();

	[DllImport ("ASimplePlugin")]
	private static extern int PrintANumber();

	[DllImport ("ASimplePlugin")]
	private static extern IntPtr PrintHello();
	
	//[DllImport ("ASimplePlugin")]
	//private static extern int AddTwoIntegers(int i1,int i2);
	
	[DllImport ("ASimplePlugin")]
	private static extern float AddTwoFloats(float f1,float f2);

	[DllImport ("BjoyUtils")]
	private static extern int AddTwoIntegers(int i1,int i2);

//	[DllImport ("BjoyUtils")]
//	private static extern string stringFromJNI();

	#if UNITY_ANDROID && !UNITY_EDITOR
	    private AndroidJavaClass ajc;
	#endif
	int i=0;
	private int res = 0;
	string strTest="";
	int flag=0;

	void Awake () {
		// Calls the FooPluginFunction inside the PluginName plugin
		// And prints 5 to the console
		//print (FooPluginFunction ());
	}

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID && !UNITY_EDITOR
		    ajc=new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			strTest=Marshal.PtrToStringAuto (PrintHello());
			res=AddTwoIntegers(2,2);
		#endif
		// 错误的为数组赋值
#if false
		int a[2][3];//int a[2][3];　其实就是一个二重指针
		int i,j;
		for(i=0;i<2;i++)
		{
			for(j=0;j<3;j++)        //这其中的　j<3　一不小心就可能被大家写成　j<=3　于是，形成数组越界，而这是初学者会常犯的错误
			{
				a[i][j]=i*3+j;
			}
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		//改变字体大小
		GUI.skin.label.fontSize = 40;
		
		GUI.Label (new Rect (10, 50, 900, 120), i.ToString ());
		GUI.Label (new Rect (100, 50, 900, 120), res.ToString ());
		GUI.Label (new Rect (200, 50, 900, 120), strTest);

		if (GUI.Button (new Rect (10, 270, 200, 100), "Quit")) {
			Application.Quit ();
		}
		
		if (GUI.Button (new Rect (300, 270, 200, 100), "PrintANumber")) {
			#if UNITY_ANDROID && !UNITY_EDITOR
				if(flag==0){
					i = PrintANumber();
					strTest=AddTwoFloats(2.5F,4F).ToString();
				}
				else if(flag==1){
					AndroidJavaObject jo = new AndroidJavaObject("com.example.bjoyjni.BjoyJni");  
					strTest= jo.Call<string>("stringFromJNI").ToString();
				}
				else if(flag==2){
					//strTest=stringFromJNI().ToString();
					AndroidJavaObject act= ajc.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject jo = new AndroidJavaObject("com.example.bjoyjni.BjoyJni");  
					//strTest= jo.CallStatic<string>("getAndroidId").ToString();
					strTest= jo.CallStatic<string>("getAndroidId",act);
				}
			#endif
			flag=(flag+1)%3;
		}
	}
}
