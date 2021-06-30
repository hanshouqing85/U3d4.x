using UnityEngine;
using System.Collections;

public class Const {
#if UNITY_ANDROID && !UNITY_EDITOR
	//#if !USE_NEW_JOY
	#if USE_OLD_JOY
    // 旧手柄：
//	public const KeyCode MyJoystick1Button3 = KeyCode.Joystick1Button18;
//	public const KeyCode MyJoystick2Button3 =KeyCode.Joystick2Button18;
//	public const KeyCode MyJoystick3Button3 = KeyCode.Joystick3Button18;
//	public const KeyCode MyJoystick4Button3 =KeyCode.Joystick4Button18;
//	public const KeyCode MyJoystick5Button3 =KeyCode.Joystick5Button18;
//	public const KeyCode MyJoystick6Button3 =KeyCode.Joystick6Button18;
//	public const KeyCode MyJoystick7Button3 =KeyCode.Joystick7Button18;
//	public const KeyCode MyJoystick8Button3 =KeyCode.Joystick8Button18;
	public const KeyCode MyJoystick1Button3 = KeyCode.Joystick1Button19;
	public const KeyCode MyJoystick2Button3 =KeyCode.Joystick2Button19;
	public const KeyCode MyJoystick3Button3 = KeyCode.Joystick3Button19;
	public const KeyCode MyJoystick4Button3 =KeyCode.Joystick4Button19;


	public const KeyCode MyJoystick1Button0 = KeyCode.Joystick1Button15;
	public const KeyCode MyJoystick1Button1 = KeyCode.Joystick1Button16;
	public const KeyCode MyJoystick1Button2 = KeyCode.Joystick1Button17;

	public const KeyCode MyJoystick2Button0 =KeyCode.Joystick2Button15;	
	public const KeyCode MyJoystick2Button1 =KeyCode.Joystick2Button16;
	public const KeyCode MyJoystick2Button2 =KeyCode.Joystick2Button17;

	public const KeyCode MyJoystick3Button0 =KeyCode.Joystick3Button15;
	public const KeyCode MyJoystick3Button1 =KeyCode.Joystick3Button16;
	public const KeyCode MyJoystick3Button2 =KeyCode.Joystick3Button17;

	public const KeyCode MyJoystick4Button0 =KeyCode.Joystick4Button15;
	public const KeyCode MyJoystick4Button1 =KeyCode.Joystick4Button16;
	public const KeyCode MyJoystick4Button2 =KeyCode.Joystick4Button17;
	
//	public const KeyCode MyJoystick1Button4 =KeyCode.Joystick1Button19;
//	public const KeyCode MyJoystick2Button4 =KeyCode.Joystick2Button19;
//	public const KeyCode MyJoystick3Button4 =KeyCode.Joystick3Button19;
//	public const KeyCode MyJoystick4Button4 =KeyCode.Joystick4Button19;
//	public const KeyCode MyJoystick5Button4 =KeyCode.Joystick5Button19;
//	public const KeyCode MyJoystick6Button4 =KeyCode.Joystick6Button19;
//	public const KeyCode MyJoystick7Button4 =KeyCode.Joystick7Button19;
//	public const KeyCode MyJoystick8Button4 =KeyCode.Joystick8Button19;
	public const KeyCode MyJoystick1Button4 =KeyCode.Joystick1Button18;
	public const KeyCode MyJoystick2Button4 =KeyCode.Joystick2Button18;
	public const KeyCode MyJoystick3Button4 =KeyCode.Joystick3Button18;
	public const KeyCode MyJoystick4Button4 =KeyCode.Joystick4Button18;

	/**/
#else
	// 定制手柄：
	public const KeyCode MyJoystick1Button3 = KeyCode.Joystick1Button2;
	public const KeyCode MyJoystick2Button3 =KeyCode.Joystick2Button2;
	public const KeyCode MyJoystick3Button3 = KeyCode.Joystick3Button2;
	public const KeyCode MyJoystick4Button3 =KeyCode.Joystick4Button2;

	
	public const KeyCode MyJoystick1Button0 = KeyCode.Joystick1Button3;
	public const KeyCode MyJoystick1Button1 = KeyCode.Joystick1Button1;
	public const KeyCode MyJoystick1Button2 = KeyCode.Joystick1Button0;
	
	public const KeyCode MyJoystick2Button0 =KeyCode.Joystick2Button3;	
	public const KeyCode MyJoystick2Button1 =KeyCode.Joystick2Button1;
	public const KeyCode MyJoystick2Button2 =KeyCode.Joystick2Button0;
	
	public const KeyCode MyJoystick3Button0 =KeyCode.Joystick3Button3;
	public const KeyCode MyJoystick3Button1 =KeyCode.Joystick3Button1;
	public const KeyCode MyJoystick3Button2 =KeyCode.Joystick3Button0;
	
	public const KeyCode MyJoystick4Button0 =KeyCode.Joystick4Button3;
	public const KeyCode MyJoystick4Button1 =KeyCode.Joystick4Button1;
	public const KeyCode MyJoystick4Button2 =KeyCode.Joystick4Button0;

	public const KeyCode MyJoystick1Button4 =KeyCode.Joystick1Button2;
	public const KeyCode MyJoystick2Button4 =KeyCode.Joystick2Button2;
	public const KeyCode MyJoystick3Button4 =KeyCode.Joystick3Button2;
	public const KeyCode MyJoystick4Button4 =KeyCode.Joystick4Button2;

#endif
#else
	#if !USE_NEW_JOY
	//#if USE_OLD_JOY
	// 旧手柄：
	public const KeyCode MyJoystick1Button0 = KeyCode.Joystick1Button0;
	public const KeyCode MyJoystick1Button1 = KeyCode.Joystick1Button1;
	public const KeyCode MyJoystick1Button2 = KeyCode.Joystick1Button2;
	public const KeyCode MyJoystick1Button3 = KeyCode.Joystick1Button3;
	public const KeyCode MyJoystick2Button0 =KeyCode.Joystick2Button0;	
	public const KeyCode MyJoystick2Button1 =KeyCode.Joystick2Button1;
	public const KeyCode MyJoystick2Button2 =KeyCode.Joystick2Button2;
	public const KeyCode MyJoystick2Button3 =KeyCode.Joystick2Button3;
	public const KeyCode MyJoystick3Button0 =KeyCode.Joystick3Button0;
	public const KeyCode MyJoystick3Button1 =KeyCode.Joystick3Button1;
	public const KeyCode MyJoystick3Button2 =KeyCode.Joystick3Button2;
	public const KeyCode MyJoystick3Button3 = KeyCode.Joystick3Button3;
	public const KeyCode MyJoystick4Button0 =KeyCode.Joystick4Button0;
	public const KeyCode MyJoystick4Button1 =KeyCode.Joystick4Button1;
	public const KeyCode MyJoystick4Button2 =KeyCode.Joystick4Button2;
	public const KeyCode MyJoystick4Button3 =KeyCode.Joystick4Button3;

	public const KeyCode MyJoystick1Button5 = KeyCode.Joystick1Button5;
	public const KeyCode MyJoystick1Button6 = KeyCode.Joystick1Button6;
	public const KeyCode MyJoystick1Button7 = KeyCode.Joystick1Button7;
		
	public const KeyCode MyJoystick2Button5 =KeyCode.Joystick2Button5;
	public const KeyCode MyJoystick2Button6 =KeyCode.Joystick2Button6;
	public const KeyCode MyJoystick2Button7 =KeyCode.Joystick2Button7;

	public const KeyCode MyJoystick3Button5 =KeyCode.Joystick3Button5;
	public const KeyCode MyJoystick3Button6 =KeyCode.Joystick3Button6;
	public const KeyCode MyJoystick3Button7 = KeyCode.Joystick3Button7;

	public const KeyCode MyJoystick4Button5 =KeyCode.Joystick4Button5;
	public const KeyCode MyJoystick4Button6 =KeyCode.Joystick4Button6;
	public const KeyCode MyJoystick4Button7 =KeyCode.Joystick4Button7;


	public const KeyCode MyJoystick1Button4 = KeyCode.Joystick1Button4;
	public const KeyCode MyJoystick2Button4 =KeyCode.Joystick2Button4;
	public const KeyCode MyJoystick3Button4 =KeyCode.Joystick3Button4;
	public const KeyCode MyJoystick4Button4 =KeyCode.Joystick4Button4;

#else
	// 定制手柄：
	public const KeyCode MyJoystick1Button0 = KeyCode.Joystick1Button4;
	public const KeyCode MyJoystick1Button1 = KeyCode.Joystick1Button1;
	public const KeyCode MyJoystick1Button2 = KeyCode.Joystick1Button0;
	public const KeyCode MyJoystick1Button3 = KeyCode.Joystick1Button3;
	public const KeyCode MyJoystick2Button0 =KeyCode.Joystick2Button4;	
	public const KeyCode MyJoystick2Button1 =KeyCode.Joystick2Button1;
	public const KeyCode MyJoystick2Button2 =KeyCode.Joystick2Button0;
	public const KeyCode MyJoystick2Button3 =KeyCode.Joystick2Button3;
	public const KeyCode MyJoystick3Button0 =KeyCode.Joystick3Button4;
	public const KeyCode MyJoystick3Button1 =KeyCode.Joystick3Button1;
	public const KeyCode MyJoystick3Button2 =KeyCode.Joystick3Button0;
	public const KeyCode MyJoystick3Button3 = KeyCode.Joystick3Button3;
	public const KeyCode MyJoystick4Button0 =KeyCode.Joystick4Button4;
	public const KeyCode MyJoystick4Button1 =KeyCode.Joystick4Button1;
	public const KeyCode MyJoystick4Button2 =KeyCode.Joystick4Button0;
	public const KeyCode MyJoystick4Button3 =KeyCode.Joystick4Button3;

	
	public const KeyCode MyJoystick1Button5 = KeyCode.Joystick1Button5;
	public const KeyCode MyJoystick1Button6 = KeyCode.Joystick1Button6;
	public const KeyCode MyJoystick1Button7 = KeyCode.Joystick1Button7;
	
	public const KeyCode MyJoystick2Button5 =KeyCode.Joystick2Button5;
	public const KeyCode MyJoystick2Button6 =KeyCode.Joystick2Button6;
	public const KeyCode MyJoystick2Button7 =KeyCode.Joystick2Button7;
	
	public const KeyCode MyJoystick3Button5 =KeyCode.Joystick3Button5;
	public const KeyCode MyJoystick3Button6 =KeyCode.Joystick3Button6;
	public const KeyCode MyJoystick3Button7 = KeyCode.Joystick3Button7;
	
	public const KeyCode MyJoystick4Button5 =KeyCode.Joystick4Button5;
	public const KeyCode MyJoystick4Button6 =KeyCode.Joystick4Button6;
	public const KeyCode MyJoystick4Button7 =KeyCode.Joystick4Button7;
	

	
	public const KeyCode MyJoystick1Button4 = KeyCode.Joystick1Button3;
	public const KeyCode MyJoystick2Button4 =KeyCode.Joystick2Button3;
	public const KeyCode MyJoystick3Button4 =KeyCode.Joystick3Button3;
	public const KeyCode MyJoystick4Button4 =KeyCode.Joystick4Button3;

#endif
#endif
	public static string[] strLHArr={"Horizontal","Horizontal2","Horizontal3","Horizontal4","Horizontal5","Horizontal6","Horizontal7","Horizontal8"};
	public static string[] strLVArr={"Vertical","Vertical2","Vertical3","Vertical4","Vertical5","Vertical6","Vertical7","Vertical8"};

	public static int GetButtonindex(int i,int My)
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		int Buttonindex =i - My;
		int[] delta={0,-2,-3,-1};
		for(int j=0;j<4;j++){
			if(Buttonindex==delta[j])
				return j;
		}
		return Buttonindex;
#else
	#if USE_OLD_JOY
		int Buttonindex =i - My;
		return Buttonindex;
    #else
		int Buttonindex =i - My;
		int[] delta={0,-3,-4,-1};
		for(int j=0;j<4;j++){
			if(Buttonindex==delta[j])
				return j;
		}
		return Buttonindex;
    #endif
#endif
	}
}
