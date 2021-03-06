using UnityEngine;
using System.Collections;
using Bjoy;
using NetWorklib;

public class HelloXML : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		//SocketManager.Instance.DoRecvData(1);
    }

    void OnGUI()
    {
        //改变字体大小
        GUI.skin.label.fontSize = 120;
        //定位显示(左边距x, 上边距y, 宽, 高)
        GUI.Label(new Rect(10, 50, 900, 120), "Hello XML!");

		if (GUI.Button(new Rect(100, 120, 300, 150), "Connect"))
		{
			NetMain.MakeConnect();
		}
		
		if (GUI.Button(new Rect(100, 320, 300, 150), "DisConnect"))
		{
			NetMain.CloseConnect();
		}
		
		if (GUI.Button(new Rect(100, 520, 300, 150), "Quit"))
		{
			Application.Quit();
		}
		
		if (GUI.Button(new Rect(500, 20, 200, 100), "0x105"))
		{
		    NetMain.SendLoginRequest();
		}

		if (GUI.Button(new Rect(500, 170, 200, 100), "0x106"))
		{
			NetMain.SendSyncDataRequest("111222");
		}

		if (GUI.Button(new Rect(500, 320, 200, 100), "0x107"))
		{
			NetMain.SendUpdateUserDataRequest();
		}
		
		if (GUI.Button(new Rect(500, 470, 200, 100), "0x108"))
		{
			NetMain.SendUserBuyNuJianRequest();
		}
    }
}