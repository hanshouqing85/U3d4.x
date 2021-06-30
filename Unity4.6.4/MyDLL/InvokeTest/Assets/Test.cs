using UnityEngine;
using System.Collections;
using DLLTest;

public class Test : MonoBehaviour {

	string strTest1="";
	string strTest2="";

	// Use this for initialization
	void Start () {
	
		MyUtilities utils = new MyUtilities();
		utils.AddValues(2, 3);
		print("2 + 3 = " + utils.c);
		strTest1 = utils.c.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		print(MyUtilities.GenerateRandom(0, 100));
		strTest2 = MyUtilities.GenerateRandom(0, 100).ToString();
	}

	void OnGUI(){
		//改变字体大小
		GUI.skin.label.fontSize = 120;
		//定位显示(左边距x, 上边距y, 宽, 高)
		GUI.Label(new Rect(10, 50, 900, 120),strTest1);
		GUI.Label(new Rect(10, 200, 900, 120),strTest2);
		
		if (GUI.Button(new Rect(100, 400, 300, 150), "Quit"))
		{
			Application.Quit();
		}
	}
}
