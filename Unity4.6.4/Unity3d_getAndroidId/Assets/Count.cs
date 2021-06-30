using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Count : MonoBehaviour {
#if UNITY_ANDROID && !UNITY_EDITOR
	private AndroidJavaClass ajc;
#endif
	private int res = 0;
	int i=0;
	bool bflag=true;
	string strTest="";
	string strTest1="";
	string strTest2="";
	string strTest3="";
	string strTest4="";
	string strTest5="";
	string strTest6="";
	string strTest7="";
	string strTest8="";

	string strIMSI="";
	string strIMEI="";
	string strAndroidId="";
	string strSimSerialNumber="";
	string strSerialNumber1="";
	string strgetuniqueId="";

	List<string> strArrTag=new List<string>();
	List<string> strArrTag2=new List<string>();
	int iTag =-1;
	int iTag2 =-1;
	int iLeft=850;//电视
	//int iLeft=500;//手机

	// Use this for initialization
	void Start () {
		//byte[] bytes=System.Security.Util.Hex.DecodeHexString("80");
		//byte b=System.Byte.Parse("10",System.Globalization.NumberStyles.HexNumber);
		//Debug.Log(b);
		//Debug.Log(0x10 & 0x10);
#if UNITY_ANDROID && !UNITY_EDITOR
		//通过unity提供的访问java插件的帮助类来找到我们的插件并创建对象
		ajc = new AndroidJavaClass("com.bailianlong.t.MainActivity");
		res =ajc.CallStatic<int>("Max", new object[] { 10, 20 });
		strTest=ajc.CallStatic<string>("getAndroidMacID");
		//		strIMSI=ajc.CallStatic<string>("getIMSI");
		//		strIMEI=ajc.CallStatic<string>("getIMEI");
		strAndroidId=ajc.CallStatic<string>("getAndroidId");

		AndroidJavaClass jc = new AndroidJavaClass("org.unity.lib.jacobiN");
		i=jc.CallStatic<int>("Legendre", new object[] {5, 17});

		AndroidJavaObject jo1 = new AndroidJavaObject("org.unity.lib.jacobiN");
		res=jo1.CallStatic<int>("Legendre", new object[] {13, 17});

		AndroidJavaObject jo = new AndroidJavaObject("java.lang.String", "some_string");// 有点费时  
		strTest= jo.Call<int>("hashCode").ToString();//第一次 - 费时
#endif
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Home) ){ 
			Application.Quit(); 
		}
		foreach (KeyCode i in System.Enum.GetValues(typeof(KeyCode))) {
			if(Input.GetKeyDown(i)){
				strIMSI=string.Format("KeyCode={0},{1}",i.ToString(),(int)i);
			}
		}
	}

	void OnGUI(){
		//改变字体大小
		GUI.skin.label.fontSize = 40;
		
//		GUI.Label(new Rect (10, 50, 900, 120),i.ToString());
//		
//		Debug.Log(strTest);
//		GUI.Label(new Rect (100, 50, 900, 120),res.ToString());
//		GUI.Label(new Rect (200, 50, 900, 120),strTest);
		GUI.Label(new Rect(iLeft, 200, 900, 120), strIMSI);
		GUI.Label(new Rect(iLeft, 350, 900, 120), "OnJoyKeyDown="+strIMEI.ToString());
		GUI.Label(new Rect(iLeft, 500, 900, 120), "AndroidId="+strAndroidId.ToString());
		GUI.Label(new Rect(iLeft, 600, 900, 120), "OnJoyKeyUp="+strSimSerialNumber.ToString());
		GUI.Label(new Rect(iLeft, 700, 900, 120), "OnCombJoyKeyDown="+strSerialNumber1.ToString());
		GUI.Label(new Rect(iLeft, 800, 900, 120), "OnCheckJoyAxis="+strgetuniqueId.ToString());

		//定位显示(左边距x, 上边距y, 宽, 高)
		GUI.Label(new Rect(10, 50, 950, 100), "strTest="+strTest);
		GUI.Label(new Rect(10, 200, 950, 100), "strTest2="+strTest2);
		GUI.Label(new Rect(10, 350, 950, 100), "strTest3="+strTest3);
		GUI.Label(new Rect(10, 500, 950, 100), "strTest4="+strTest4);
		GUI.Label(new Rect(10, 600, 950, 100), "strTest5="+strTest5);
		GUI.Label(new Rect(10, 700, 950, 100), "strTest6="+strTest6);
		GUI.Label(new Rect(10, 800, 950, 100), "strTest7="+strTest7);
		GUI.Label(new Rect(10, 900, 950, 100), "strTest8="+strTest8);

		if (GUI.Button(new Rect(10, 270, 200, 100), "Quit"))
		{
			Application.Quit();
		}

		if (GUI.Button(new Rect(300, 270, 200, 100), "strArrTag"))
		{
			strTest="";
			if(strArrTag.Count>0)
			{
				iTag=(iTag+1)%strArrTag.Count;
				strTest=strArrTag[iTag];
			}
		}

		if (GUI.Button(new Rect(600, 270, 200, 100), "strArrTag2"))
		{
			strIMSI="";
			if(strArrTag2.Count>0)
			{
				iTag2=(iTag2+1)%strArrTag2.Count;
				strIMSI=strArrTag2[iTag2];
			}
		}

		if (GUI.Button(new Rect(300, 420, 200, 100), "Clear2"))
		{
			//strArrTag2.Clear();
			//strIMSI="";
			strTest="";
			strTest1="";
			strTest2="";
			strTest3="";
			strTest4="";
			strTest5="";
			strTest6="";
			strTest7="";
			strTest8="";
		}

		if(GUI.Button(new Rect(10,420,200,100), "UnitySendMessage Test"))  
		{  
		#if UNITY_ANDROID && !UNITY_EDITOR
			if(bflag)
			{
				ajc.CallStatic("AndroidFunc2");
			}
			else
			{
				ajc.CallStatic("AndroidFunc1", new string[]{"Call 韩AndroidFunc1 2"});;
			}
			bflag=!bflag;
         #endif
		}

		int sum = 0;
		if (GUILayout.Button ("Count")) {
			for(int j=0;j<5;j++){
				sum+=j;
				print(sum);
			}
		}
	}

	void OnJoyKeyDown(string str)   
	{   
		strIMEI=str;
	}

	void OnJoyKeyUp(string str)   
	{   
		strSimSerialNumber=str;
	}

	private static byte[] m_CombBtnByte = {0x12,0x11,0x18,0x03,0x0a,0x09,0x13,0x1a,0x19,0x0b,0x1b};
	public static string[] m_strCombBtnArr={"组合键0+1","组合键0+2","组合键0+3","组合键1+2","组合键1+3","组合键2+3","组合键0+1+2","组合键0+1+3","组合键0+2+3","组合键1+2+3","组合键0+1+2+3"};
	private byte[] m_BtnByte = {0,0,0,0,0,0,0,0};
	private byte[] m_LHByte = {0,0,0,0,0,0,0,0};
	private byte[] m_LVByte = {0,0,0,0,0,0,0,0};
	private bool[] KeyDownArr0 = {false,false,false,false,false,false,false,false};
	private bool[] KeyDownArr1 = {false,false,false,false,false,false,false,false};
	private bool[] KeyDownArr2 = {false,false,false,false,false,false,false,false};
	private bool[] KeyDownArr3 = {false,false,false,false,false,false,false,false};

	void OnReadJoyData(string str)   
	{   
		if (str.Length != 36)
			return;
		int Joyindex = System.Convert.ToInt32 (str.Substring(35,1))-1;
		string strCardNo = str.Substring (18,16);
		OnCheckJoyindex(Joyindex, strCardNo);
		if (strCardNo == "0000000000000000") {

		} else {
			byte BtnByte=System.Byte.Parse(str.Substring (0,2),System.Globalization.NumberStyles.HexNumber);
			byte LHByte=System.Byte.Parse(str.Substring (6,2),System.Globalization.NumberStyles.HexNumber);
			byte LVByte=System.Byte.Parse(str.Substring (8,2),System.Globalization.NumberStyles.HexNumber);
			OnCheckJoyBtn(Joyindex,BtnByte);
			OnCheckJoyAxis(Joyindex,LHByte,LVByte);
		}
	}

	void OnCheckJoyAxis(int Joyindex,byte LHByte,byte LVByte){
		if (m_LHByte [Joyindex] == LHByte && m_LVByte [Joyindex] == LVByte)
			return;
		if (LHByte == 0 && LVByte == 0x80) {
			OnLeftStickLeft(Joyindex);
		}
		if (LHByte == 0xff && LVByte == 0x80) {
			OnLeftStickRight(Joyindex);
		}
		if (LHByte == 0x80 && LVByte == 0) {
			OnLeftStickUp(Joyindex);
		}
		if (LHByte == 0x80 && LVByte == 0xff) {
			OnLeftStickDown(Joyindex);
		}
		strgetuniqueId=string.Format ("OnCheckJoyAxis Joyindex={0},LHByte={1},LVByte={2}", Joyindex, LHByte,LVByte);
		m_LHByte [Joyindex]=LHByte;
		m_LVByte [Joyindex]=LVByte;
	}

	void OnLeftStickLeft(int Joyindex)
	{
		//Debuger.Log(string.Format("OnLeftStickLeft Joyindex={0}",Joyindex));
	}
	
	void OnLeftStickRight(int Joyindex)
	{
		//Debuger.Log(string.Format("OnLeftStickRight Joyindex={0}",Joyindex));
	}
	
	void OnLeftStickUp(int Joyindex)
	{
		//Debuger.Log(string.Format("OnLeftStickUp Joyindex={0}",Joyindex));
	}
	
	void OnLeftStickDown(int Joyindex)
	{
		//Debuger.Log(string.Format("OnLeftStickDown Joyindex={0}",Joyindex));
	}

	void OnCheckJoyBtn(int Joyindex,byte BtnByte){
		if (m_BtnByte [Joyindex] == BtnByte)
			return;
		if(BtnByte!=0){
			if((BtnByte & 0x10)==0x10){
				OnJoyKeyDown(Joyindex,0);
				KeyDownArr0[Joyindex]=true;
			}
			if((BtnByte & 0x02)==0x02){
				OnJoyKeyDown(Joyindex,1);
				KeyDownArr1[Joyindex]=true;
			}
			if((BtnByte & 0x01)==0x01){
				OnJoyKeyDown(Joyindex,2);
				KeyDownArr2[Joyindex]=true;
			}
			if((BtnByte & 0x08)==0x08){
				OnJoyKeyDown(Joyindex,3);
				KeyDownArr3[Joyindex]=true;
			}
			int iNum=m_CombBtnByte.Length;
			for(int i=0;i<iNum;i++){
				if(m_CombBtnByte[i]==BtnByte){
					OnCombJoyKeyDown(Joyindex,BtnByte);
					break;
				}
			}
		}
		else {
			if(KeyDownArr0[Joyindex]){
				OnJoyKeyUp(Joyindex,0);
				KeyDownArr0[Joyindex]=false;
			}
			if(KeyDownArr1[Joyindex]){
				OnJoyKeyUp(Joyindex,1);
				KeyDownArr1[Joyindex]=false;
			}
			if(KeyDownArr2[Joyindex]){
				OnJoyKeyUp(Joyindex,2);
				KeyDownArr2[Joyindex]=false;
			}
			if(KeyDownArr3[Joyindex]){
				OnJoyKeyUp(Joyindex,3);
				KeyDownArr3[Joyindex]=false;
			}
		}
		m_BtnByte[Joyindex]=BtnByte;
	}

	void OnCombJoyKeyDown(int Joyindex,byte BtnByte)
	{
		// Debuger.Log(string.Format("OnCombJoyKeyDown Joyindex={0},BtnByte={1}",Joyindex,BtnByte));
		strSerialNumber1 = string.Format ("OnCombJoyKeyDown Joyindex={0},BtnByte={1}", Joyindex, BtnByte);
	}

	void OnJoyKeyDown(int Joyindex,int Buttonindex)
	{
		//	Debuger.Log(string.Format("OnJoyKeyDown Joyindex={0},Buttonindex={1}",Joyindex,Buttonindex));
		strIMEI = string.Format ("OnJoyKeyDown Joyindex={0},Buttonindex={1}", Joyindex, Buttonindex);
		switch (Buttonindex)
		{
		case 0:
		{
			break;
		}
		case 1:
		{
			break;
		}
		case 2:		
			break;
		case 3:
			break;
		default:
			break;
		}
	}

	void OnJoyKeyUp(int Joyindex,int Buttonindex)
	{
		//	Debuger.Log(string.Format("OnJoyKeyUp Joyindex={0},Buttonindex={1}",Joyindex,Buttonindex));
		strSimSerialNumber = string.Format ("OnJoyKeyUp Joyindex={0},Buttonindex={1}", Joyindex, Buttonindex);
		switch (Buttonindex)
		{
		case 0:
		{
			break;
		}
		case 1:
		{
			break;
		}
		case 2:		
			break;
		case 3:
			break;
		default:
			break;
		}
	}
	
	void OnCheckJoyindex(int Joyindex,string strCardNo)   
	{   
		switch (Joyindex)
		{
		case 0:
		{
			strTest=strCardNo;
			break;
		}
		case 1:
		{
			strTest2=strCardNo;
			break;
		}
		case 2:
		{
			strTest3=strCardNo;
			break;
		}
		case 3:
		{
			strTest4=strCardNo;
			break;
		}
		case 4:
		{
			strTest5=strCardNo;
			break;
		}
		case 5:
		{
			strTest6=strCardNo;
			break;
		}
		case 6:
		{
			strTest7=strCardNo;
			break;
		}
		case 7:
		{
			strTest8=strCardNo;
			break;
		}
		default:
			break;
		}
	}

	void OnCheckJoyindex0(string str)   
	{   
		strTest=str;
	}

	void OnCheckJoyindex1(string str)   
	{   
		strTest2=str;
	}

	void OnCheckJoyindex2(string str)   
	{   
		strTest3=str;
	}

	void OnCheckJoyindex3(string str)   
	{   
		strTest4=str;
	}

	void OnCheckJoyindex4(string str)   
	{   
		strTest5=str;
	}

	void OnCheckJoyindex5(string str)   
	{   
		strTest6=str;
	}

	void OnCheckJoyindex6(string str)   
	{   
		strTest7=str;
	}

	void OnCheckJoyindex7(string str)   
	{   
		strTest8=str;
	}

	void UnityFunc1(string str)   
	{   
		strTest=str;
	}

	void MissileLauncherActivity(string str)   
	{   
		strArrTag.Add(str);
	}

	void MainActivity(string str)   
	{   
		strArrTag2.Add(str);
	}
}
