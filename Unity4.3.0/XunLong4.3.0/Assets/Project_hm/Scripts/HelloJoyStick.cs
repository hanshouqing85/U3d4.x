using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class HelloJoyStick : MonoBehaviour
{
	string strTest="";
	string strTest1="";
	string strTest2="";
	string strTest3="";
	string strTest4="";
	string strTest5="";
	string strTest6="";
	string strTest7="";
	string strTest8="";
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		foreach (KeyCode i in System.Enum.GetValues(typeof(KeyCode)))
		{
			if(Input.GetKey(i))
			//if(Input.GetKeyDown(i))
			{
				//Debug.Log("KeyCode="+i);
				strTest=string.Format("KeyCode={0},{1}",i.ToString(),(int)i);
			}
		}

//		public const KeyCode Joystick1Button0 = 350;
//		public const KeyCode Joystick1Button1 = 351;
//		public const KeyCode Joystick1Button3 = 353;
//		public const KeyCode Joystick1Button4 = 354;
//		
//		public const KeyCode Joystick2Button0 = 370;
//		public const KeyCode Joystick2Button1 = 371;
//		public const KeyCode Joystick2Button3 = 373;
//		public const KeyCode Joystick2Button4 = 374;
//		
//		public const KeyCode Joystick3Button0 = 390;
//		public const KeyCode Joystick3Button1 = 391;
//		public const KeyCode Joystick3Button3 = 393;
//		public const KeyCode Joystick3Button4 = 394;

		float v=Input.GetAxis("Vertical");
		float h=Input.GetAxis("Horizontal");
		float v1=Input.GetAxisRaw("Vertical");
		float h1=Input.GetAxisRaw("Horizontal");
		strTest1=string.Format("v={0},h={1},v1={2},h1={3}",v,h,v1,h1);
		//Debug.Log(strTest1);
		//strTest1 = Input.inputString;
		string[] sarr=Input.GetJoystickNames();
		//strTest1 = sarr.Length.ToString()+","+sarr[0]+","+sarr[1];

		float v2=Input.GetAxis("Vertical2");
		float h2=Input.GetAxis("Horizontal2");
		float v12=Input.GetAxisRaw("Vertical2");
		float h12=Input.GetAxisRaw("Horizontal2");
		strTest2=string.Format("v2={0},h2={1},v12={2},h12={3}",v2,h2,v12,h12);
		//Debug.Log(strTest2);

		float v3=Input.GetAxis("Vertical3");
		float h3=Input.GetAxis("Horizontal3");
		float v13=Input.GetAxisRaw("Vertical3");
		float h13=Input.GetAxisRaw("Horizontal3");
		strTest3=string.Format("v3={0},h3={1},v13={2},h13={3}",v3,h3,v13,h13);
		//Debug.Log(strTest3);
//		strTest3="第三个手柄的摇杆";

		float v4=Input.GetAxis("Vertical4");
		float h4=Input.GetAxis("Horizontal4");
		float v14=Input.GetAxisRaw("Vertical4");
		float h14=Input.GetAxisRaw("Horizontal4");
		strTest4=string.Format("v4={0},h4={1},v14={2},h14={3}",v4,h4,v14,h14);
		//Debug.Log(strTest4);
//		strTest4="第四个手柄的摇杆";
	}
	
    void OnGUI()
    {
        //改变字体大小
        GUI.skin.label.fontSize =30;
        //定位显示(左边距x, 上边距y, 宽, 高)
		GUI.Label(new Rect(10, 10, 900, 50), strTest);
		GUI.Label(new Rect(10, 70, 900, 50), strTest1);
		GUI.Label(new Rect(10, 130, 900, 50), strTest2);
		GUI.Label(new Rect(10, 190, 900, 50), strTest3);
		GUI.Label(new Rect(10, 250, 900, 50), strTest4);
		GUI.Label(new Rect(10, 310, 900, 50), strTest5);
		GUI.Label(new Rect(10, 370, 900, 50), strTest6);
		GUI.Label(new Rect(10, 430, 900, 50), strTest7);
		GUI.Label(new Rect(10, 490, 900, 50), strTest8);
		if (GUI.Button(new Rect(10, 600, 200, 100), "Quit"))
		{
			Application.Quit();
		}
    }
}