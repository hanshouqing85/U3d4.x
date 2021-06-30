using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("testgame/MessageManager")]
public class MessageManager : MonoBehaviour {
	//public UILabel scores;
//	public UILabel hooknums;
//	public UILabel longcoins;
	public List <Player> Players = new List<Player>();
	public  Player Player01=new Player();
	public  Player Player02=new Player();
	public List<UILabel> scores=new List<UILabel>();
	public List<UILabel> hooknums=new List<UILabel>();
	public List<UILabel> longcoins=new List<UILabel>();
	// Use this for initialization
	void Start () {
	//	GameObject.Find ("Script").GetComponent<GameManager> ();
	//	GameManager.Instance.initMessage (scores,hooknums,longcoins);
		Player01.LongCoinnum = 40;
		Player01.Hooknum = 120;
		Player01.m_score = 0;
		
		Player02.LongCoinnum = 20;
		Player02.Hooknum = 60;
		Player02.m_score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//GameManager.Instance.initMessage (scores,hooknums,longcoins);

	}

	void OnGUI()
	{  
		//改变字体大小
		GUI.skin.label.fontSize = 120;
		if (GUI.Button (new Rect (100, 120, 300, 150), "Player01")) {
			Players.Add(Player01);
			//	GameObject.Find("Script").GetComponent<GameManager>().initMessage(0);
			initMessage(0);
		}
		if (GUI.Button (new Rect (100, 250, 300, 150), "Player02")) {
			Players.Add(Player02);
			//GameObject.Find("Script").GetComponent<GameManager>().initMessage(1);
			initMessage(1);
		}
	}
	public void initMessage(int index)
	{
		scores[index].text=Players[index].m_score.ToString();
		hooknums[index].text=Players[index].Hooknum.ToString();
		longcoins[index].text=Players[index].LongCoinnum.ToString();
	}
}
