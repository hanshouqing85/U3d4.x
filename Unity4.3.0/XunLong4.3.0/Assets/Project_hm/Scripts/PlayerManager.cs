using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/PlayerManager")]
public class PlayerManager : MonoBehaviour { 
public List <string> PlayerCardIDs = new List<string>();


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	   //刷卡一次添加一个帐号
		//存于PlayersPre list
		//PlayerCardIDs.Add (CardID);
		//PlayerCardIDs.Add ("111222");
       
	}
	
	void OnGUI()
	{  

	}
}
