using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bjoy;
//using UnityEngine.UI;

[AddComponentMenu("testgame/GameManager")]
public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	public List<UILabel> scores=new List<UILabel>();
	public List<UILabel> hooknums=new List<UILabel>();
	public List<UILabel> longcoins=new List<UILabel>();	
//	public List<UISprite> Scoreszhu=new List<UISprite>();


	public List<GameObject> PlayersbousUI=new List<GameObject>();

	public List<GameObject> PlayersKaUI=new List<GameObject>();
	public int CurrentIndex;
	public List<GameObject> Scoreszhu=new List<GameObject>();
	public float Scoreszhutp;
	//public  UILabel CountTimetext;

	public GameObject PaihangUI;
	//public int currentTime;
	public float currentTime1;
	public int Count =10;
	public int indexnum = 1;
	public  float LastoverTime;

	//总分
	public static int m_scoresum =0;
	//总经验
	public int m_expsum = 0;
	//积分
	public  int m_score =0;
	//经验
	public  int m_exp = 0;
	//奖池常量s
	public static float N=24;
	//奖池最小数
	public static float min=0;
	//奖池最大数
	public static float max=20000;
	//奖池积分
	public  float bous=min;
	//轮次数
	public static int m_turnnum = 1;
	//轮次时间
	public float turnTime=300;
	//本机已发射弩箭总数量
	public static int m_missleshootsum;
	//本机已经发射弩箭数量
	public  int  m_localmissleshootsum;
	//龙数量限制
	public int Longnum=20;
	//boss龙数量限制
	public int Bossnum=1;
	//记录
	public static int m_hiscore =0;
	//背景音乐
	public AudioClip m_musicClip;
	//声音源
	protected AudioSource m_Audio;
	protected int Hooknum;
	protected int LongCoinnum;
	public int HooknumInter=10;
	public bool debugtxt=false;
	//玩家
	public List <Player> Players = new List<Player>();
	//public List <GameObject> Players = new List<GameObject>();
	public List <Player> PlayersActive = new List<Player>();

	public List <int> Playerscore = new List<int>();
	public List <int> Playerscoresub = new List<int>();
	public List <float>Scorepercent = new List<float>();
	public List <int> Playerscoreid = new List<int>();

	public float calmaxLength = 1f;
	public float calPercent;

	public float	Playerscoretp=1f;
	public Transform HitMissle;

	//public List <int>Playerscoreindex = new List<int>();
	public int Playerscoreindex;
	public bool Captured;
	public bool NoBoss=true;
	public bool CanSpawn=true;
	public bool CanBoss=true;
	public bool TurnTimeCount=true;
	public bool Shootenble=true;
	private bool StartTimeCount = false;

	void Awake()
	{
		//Debuger.EnableLog = false;
		Instance = this;
//		PlayersData.Clear ();
		PaihangUI.SetActive (false);
		//CountTimetext.text=Count.ToString();
//	       text.text=Count.ToString();
	}
	// Use this for initialization
	void Start () {
		m_Audio = this.GetComponent<AudioSource>();

		/*
		Players[0].LongCoinnum = 40;
		Players[0].Hooknum = 30;
		Players[0].m_score = 0;
		Players[1].LongCoinnum = 20;
		Players[1].Hooknum = 10;
		Players[1].m_score = 0;
       */
		//获取玩家
		//GameObject obj = GameObject.FindGameObjectWithTag ("Player");
	/*	GameObject obj = GameObject.Find ("Script");
		if (obj != null) {
			m_player=obj.GetComponent<Player>();
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		//	LongCoinnum = (int)m_player.LongCoinnum;
	//	Hooknum = (int)m_player.Hooknum;
	//	intiPlayer ();

		//CountTimetext.text=Count.ToString();
//		text.text=Count.ToString();
	//	currentTime =Mathf.CeilToInt(Time.fixedTime);

		if (Players != null) {
			for (int i=0; i<Players.Count; i++) {
				//	if (Players[i].LongCoinnum != 0) {
				//		 LongCoinnum= Players[i].LongCoinnum;
				//	}
				if (Players [i].Hooknum != 0) {
					Hooknum = Players [i].Hooknum;
				} else {
					//提示买弩箭，否则30秒后断开连接
					print ("Hook Out");
				}
			}
		}


		//出现BOSS
		if ((m_missleshootsum != 0) &&(m_missleshootsum >=HooknumInter)&& (m_missleshootsum % HooknumInter == 0)) 
		{	
			print("Bosssssss  Out");
			//概率20%
		//	if(Random.Range(1,6)%5==0)
			{  
				NoBoss=false;
				print("Bosssssss  Out22222");

			
			}
		}

			//循环播放音乐
		if (!m_Audio.isPlaying) {
			m_Audio.clip =m_musicClip;
			m_Audio.Play();
		}

		// 暂停游戏
		if (Time.timeScale > 0 && Input.GetKeyDown (KeyCode.Escape)) {
			Time.timeScale =0;
		}
	}
	
	void OnGUI()
	{  
		if (TurnTimeCount) {
			turnTime -= Time.deltaTime;
		}
		Paihang ();

		if (Time.timeScale == 0) {
		if (GUI.Button(new Rect(Screen.width*0.5f-50,Screen.height*0.4f,100,30),"继续游戏"))
			{
				Time.timeScale=1;
			}
		if (GUI.Button (new Rect (Screen.width * 0.5f - 50, Screen.height * 0.6f, 100, 30), "Quit Game")) 
			{
				Application.Quit();
			}
		}
		if (turnTime<=0)
		{   
			turnTime=300;
			CanSpawn=false;
			Shootenble=false;
			//更新玩家数据到服务器
	//	NetMain.SendUpdateUserDataRequest1(this);                   

		//	GUI.skin.label.fontSize =50;
		//	GUI.skin.label.alignment =TextAnchor.LowerCenter;
		//	GUI.Label(new Rect (0,Screen.height*0.2f,Screen.width,60),"本轮游戏结束");
		//	GUI.skin.label.fontSize =20;
		//	if (GUI.Button(new Rect(Screen.width*0.5f-50,Screen.height*0.5f,100,30),"再来一轮"))
		//	{  
		//		Shootenble=true;
		//		m_turnnum ++;
		//		Application.LoadLevel(Application.loadedLevelName);
		//	}
			LastoverTime=Time.realtimeSinceStartup;
			//currentTime =Mathf.CeilToInt(Time.fixedTime);
			StartTimeCount = true;
			PlayersScores();
		
		}

		if (Players != null) {
		//	Hooknum = (int)m_player.Hooknum;
		} else {
			//game over
			GUI.skin.label.fontSize =50;
			GUI.skin.label.alignment =TextAnchor.LowerCenter;
			GUI.Label(new Rect (0,Screen.height*0.2f,Screen.width,60),"游戏结束");

			GUI.skin.label.fontSize =20;
			if (GUI.Button(new Rect(Screen.width*0.5f-50,Screen.height*0.5f,100,30),"再来一轮"))
			{
					Application.LoadLevel(Application.loadedLevelName);
			}
		}
		if (debugtxt) {
			GUI.skin.label.fontSize = 20;
			GUI.Label (new Rect (50, 10, 100, 30), "得分" + m_score);
			GUI.Label (new Rect (50, 30, 100, 30), "总积分" + m_scoresum);
			GUI.Label (new Rect (50, 50, 100, 30), "经验" + m_expsum);
			GUI.Label (new Rect (50, 70, 100, 30), "发射数" + m_localmissleshootsum);
			GUI.Label (new Rect (50, 90, 100, 30), "轮数" + m_turnnum);
			GUI.Label (new Rect (50, 110, 200, 30), "发射总数" + m_missleshootsum);
			/*
			GUI.skin.label.fontSize = 120;
			if (GUI.Button (new Rect (100, 120, 300, 150), "Player01")) {
				//Players.Add(Player01);
				GameManager.Instance.initUIMessage(0);
			}
			if (GUI.Button (new Rect (100, 250, 300, 150), "Player02")) {
				//Players.Add(Player02);
				//GameObject.Find("Script").GetComponent<GameManager>().initMessage(1);
				GameManager.Instance.initUIMessage(1);
			}*/

		}
	}
	public void Paihang()
	{
		if (StartTimeCount)
		{  
			TurnTimeCount=false;
			currentTime1 =Time.realtimeSinceStartup;
			if ((currentTime1-LastoverTime>=indexnum)&&indexnum!=12)
			{	
				Count--;
				//CountTimetext.text=Count.ToString();
//				text.text=Count.ToString();
				indexnum++;
			 }
			else if (indexnum==12)
			{
				GameRest();
				TurnTimeCount=true;
			}
		}
	}

	public void GameRest()
	{ 	    
		    initPlayerPerTurn ();
		    StartTimeCount = false;
			Shootenble=true;
		    CanSpawn = true;
			m_turnnum ++;
		PaihangUI.SetActive(false);
		indexnum = 1;
		Count = 10;

		//	Application.LoadLevel(Application.loadedLevelName);
		}
	
	public void PlayersScoresSort()
	{
		Playerscore.Clear ();
	//	Playerscoreindex.Clear ();
		Playerscoresub.Clear ();
		for (int i=0; i<Players.Count; i++) {
			Playerscore.Add (Players [i].m_score);
			Playerscoreid.Add(i);
			//print ("Playerscore000" + "[" + i + "]=" + Playerscore [i]);
		}
		Playerscoresub.AddRange (Playerscore);
		for (int i=0; i<Playerscoresub.Count; i++) {
			print ("Playerscoresub" + "[" + i + "]=" + Playerscoresub [i]);
		}

	//    Playerscore.Sort ();
	//	Playerscore.Reverse ();
//玩家分数排序
		BubbleSort (Playerscore,Playerscoreid);

//		for (int i=0; i<PlayersActive.Count; i++) {
//		PlayersUI[i].text=indexCheck(i).ToString();
//		}


		for (int i=0;i<Playerscore.Count;i++)
		{
//			PlayersUI[i].text=(Playerscoreid[i]+1).ToString();
		}

		print ("indexof=" + indexCheck (0));
		print ("Players.Count=" + Players.Count);
		print ("Playerscore.Count=" + Playerscore.Count);

		if (PlayersActive.Count < 4) {
			PlayersbousUI[0].SetActive(false);
			PlayersbousUI[1].SetActive(false);
			PlayersbousUI[2].SetActive(false);
		}

		if ((PlayersActive.Count >= 4) && (PlayersActive.Count <= 5)) {
			Players[indexCheck(0)].ex_score=100;
			PlayersbousUI[0].SetActive(true);
			PlayersbousUI[1].SetActive(false);
			PlayersbousUI[2].SetActive(false);
		}

		if ((PlayersActive.Count >= 6) && (PlayersActive.Count <= 7)) {
			Players[indexCheck(0)].ex_score=100;
			Players[indexCheck(1)].ex_score=50;
			PlayersbousUI[0].SetActive(true);
			PlayersbousUI[1].SetActive(true);
			PlayersbousUI[2].SetActive(false);
		}

		if (PlayersActive.Count == 8)  {
			Players[indexCheck(0)].ex_score=100;
			Players[indexCheck(1)].ex_score=50;
			Players[indexCheck(2)].ex_score=20;
			PlayersbousUI[0].SetActive(true);
			PlayersbousUI[1].SetActive(true);
			PlayersbousUI[2].SetActive(true);
		}


		/*
		for (int i=0; i<Playerscore.Count; i++) {
			print ("Playerscore111"+"["+i+"]="+Playerscore[i]);
			Playerscoreindex.Add( Playerscore.IndexOf (Players [i].m_score));
			print ("Playerscoreindex" + "[" + i + "]=" + Playerscoreindex [i]);
		}*/
	}


	public void BubbleSort(List<int> Scores,List<int> Scoresid)
	{
		bool exchange ;
		int temp,tempid;
		
		for (int i=0; i<Scores.Count-1; i++) {
			exchange = false;
			
			for (int j=Scores.Count-2;j>=i;j--)
				
				if (Scores[j+1]>Scores[j])
			{	
				temp=Scores[j+1];
				Scores[j+1]=Scores[j];
				Scores[j]=temp;
				exchange=true;
				tempid=Scoresid[j+1];
				Scoresid[j+1]=Scoresid[j];
				Scoresid[j]=tempid;
			
			//	max=Scores[j+1];
			}
			if (!exchange)
				return;
			
		}
	}



	public void PlayersScores()
	{
		Playerscore.Clear ();
		Scorepercent.Clear ();
		PlayersScoresSort ();
		//calmaxLength=Scoreszhu [0].transform.localScale.x;
		//calmaxLength=1;
      	Playerscoretp = (Playerscore [0] == 0 )? 1 : Playerscore [0];
		print ("Playerscoretp="+Playerscoretp);
		for (int i=0; i<Players.Count; i++) {
			calPercent=Playerscore[i]/Playerscoretp;
			print ("Playerscore"+"["+i+"]="+Playerscore[i]);
			Scorepercent.Add(calPercent);
			print ("Scorepercent"+"["+i+"]="+Scorepercent[i]);
		   }
		for (int i=0; i<Players.Count; i++) {
			Scoreszhu [i].transform.localScale=new Vector3(Scorepercent[i],1,1);
		//	print ("Scorepercent"+"["+i+"]="+Scorepercent[i]);
		//	print ("SIze.x=" + 	Scoreszhu [i].transform.localScale.x);
		}
		PaihangUI.SetActive(true);
	}
	
	public int indexCheck(int index)
	{
		for (int j=0; j<Playerscoresub.Count; j++)
			if ((Playerscoresub [j] == Playerscore [index]) && (Playerscore [index] != 0)) {
				Playerscoreindex = j;
				break;
			} else {
			if (Playerscoresub [j] == Playerscore [index]) 
				Playerscoreindex=index;
		}
		return Playerscoreindex+1;
	}



	public void initPlayerPerTurn()
	{
		for (int i=0; i<Players.Count; i++) {
			Players[i].m_scoreadd=0;
			Players[i].Hooknumadd=0;
			Players[i].m_expadd=0;
		}
		
	}


	public void AddScore(int point,int index)
	{
		Players [index].m_score += point;

	   Players[index].m_scoreadd+=point;
	
		//	m_score += point;
    // m_scoresum += point;
	}

	public void AddExp(int point,int index)
	{
		Players [index].m_exp += point;
		Players [index].m_expsum += point;
		Players[index].m_expadd+=point;
	}

	//添加奖池积分与射击数目
	public void AddShootNum()
	{
		//m_localmissleshootsum += point;
		m_missleshootsum ++;
		if (bous <= max)
			bous+=N;
	}



	public void UpdateHooknumAdd(int index)
	{
		Players [index].Hooknumadd--;
	}

/*	public void UpdatePlayerHooknum(int index,int sum)
	{  
		if (Players [index].Hooknum >=sum)
			Players [index].Hooknum-=sum;
		else 
			print ("Hooknum not enough,need order");
	}*/

	public void UpdatePlayerHooknum(int index)
	{  
		if (Players [index].Hooknum >0)
			Players [index].Hooknum--;
		else 
			print ("need order");
	}

	public void UpdatePlayerLongCoinnum(int index)
	{ 
		if (Players [index].LongCoinnum > 0)
			Players [index].LongCoinnum--;
		else 
			print ("LongCoin not enough");
	}

	public void initUIMessage(int index)
	{
		//scores[index].text=GameObject.Find("Script").GetComponent<PlayerManager>().Players[index].m_score.ToString();
		scores[index].text=Players[index].m_score.ToString();
		hooknums[index].text=Players[index].Hooknum.ToString();
		longcoins[index].text=Players[index].LongCoinnum.ToString();
	}

	public void UpdateUIscore(int index)
	{
		scores[index].text=Players[index].m_score.ToString();
	}

	public void UpdateUIhooknum(int index)
	{
		hooknums[index].text=Players[index].Hooknum.ToString();
	}

	public void UpdateUIlongcoin(int index)
	{
		longcoins[index].text=Players[index].LongCoinnum.ToString();
	}
	
	public void initPlayer()
	{   
		//print ("PlayersData="+PlayersData.Count);
//		Players [PlayersData.Count-1].LongCoinnum = PlayersData [PlayersData.Count-1].m_mUserData.m_nUserCoins;
//		Players [PlayersData.Count-1].Hooknum = PlayersData [PlayersData.Count-1].m_mXunLongData.m_nNuJian;
//		initUIMessage (PlayersData.Count-1);
	}
	public void Orderlongcoins(int index)
	{  
		if (Players [index].LongCoinnum > 0) {
			Players [index].LongCoinnum--;
			Players [index].Hooknum += 10;
			UpdateUIlongcoin (index);
			UpdateUIhooknum (index);
		}
	}

	

	//玩家数据初始化
	public void initSelectedPlayer()
	{   
		//print ("PlayersData="+PlayersData.Count);
//		Players [PlayersData.Count-1].LongCoinnum = PlayersData [PlayersData.Count-1].m_mUserData.m_nUserCoins;
//		Players [PlayersData.Count-1].Hooknum = PlayersData [PlayersData.Count-1].m_mXunLongData.m_nNuJian;
		//initUIMessage1 (PlayersData.Count-1);
	}
	//玩家数据与炮台UI对应
	public void initUIMessage1(int index,int CurrentIndex)
	{  
		//scores[index].text=GameObject.Find("Script").GetComponent<PlayerManager>().Players[index].m_score.ToString();
//		scoresUI[CurrentIndex].text=Players[index].m_score.ToString();
//		hooknumsUI[CurrentIndex].text=Players[index].Hooknum.ToString();
//		longcoinsUI[CurrentIndex].text=Players[index].LongCoinnum.ToString();
	}
	//显示相应玩家数据
	public void ShowUIMessage(int index)
	{
		CurrentIndex=GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[index];
		initUIMessage1 (index,CurrentIndex);
	}
   //订购龙币
	public void Orderlongcoins1(int index)
	{  
		CurrentIndex=GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[index];
		if (Players [index].LongCoinnum > 0) {
			Players [index].LongCoinnum--;
			Players [index].Hooknum += 10;
			UpdateUIlongcoin1 (index,CurrentIndex);
			UpdateUIhooknum1 (index,CurrentIndex);
		}
	}

	public void UpdateUIscore1(int index,int CurrentIndex)
	{
//		scoresUI[CurrentIndex].text=Players[index].m_score.ToString();
	}
	
	public void UpdateUIhooknum1(int index,int CurrentIndex)
	{
//		hooknumsUI[CurrentIndex].text=Players[index].Hooknum.ToString();
	}
	
	public void UpdateUIlongcoin1(int index,int CurrentIndex)
	{
//		longcoinsUI[CurrentIndex].text=Players[index].LongCoinnum.ToString();
	}

}
