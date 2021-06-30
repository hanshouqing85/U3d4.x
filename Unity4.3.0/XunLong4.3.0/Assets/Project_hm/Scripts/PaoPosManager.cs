using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/PaoPosManeger")]
public class PaoPosManager : MonoBehaviour {
	public List <GameObject> BallistaOnPos = new List<GameObject>();
	public Transform[] PaoPos;
	public Transform CurrentPos;
	public GameObject CurrentBall;
   //public List <GameObject> CurrentBalls = new List<GameObject>();
	//public List <Player> player = new List<Player>();
	//protected  List <Transform> Ballista = new List<Transform>();
	public List <GameObject[]> Ballistas = new List<GameObject[]>();
	public GameObject[] Ballista;
 //protected Transform m_transform;
	public int Hooknum;
	public int PaoPosindex;
	public bool[] PaoPosvacate;

	public GameObject CurrentIndicate;
	public List <GameObject> cardIndicatePos = new List<GameObject>();
	public int CurrentIndex;
	public List <Transform> Posvacate= new List<Transform>();
	public bool[] BallistaPosvacate;

	//public List <int> CurrentLocation=new List<int>();
	public int[] CurrentLocation;
	protected int k=0;
	protected int i=0;

	//public GameObject[] bgArro;
	public List <GameObject> bgArro= new List<GameObject>();
	public List <GameObject> bgArrosub= new List<GameObject>();

//	public List <GameObject> bglight= new List<GameObject>();
	public GameObject[] bglight1;
	public GameObject[] bglight2;
	public GameObject[] bglight3;
	public GameObject[] bglight4;
	public GameObject[] bglight5;
	public GameObject[] bglight6;
	public GameObject[] bglight7;
	public GameObject[] bglight8;

	public GameObject[] bgArr1;
	public GameObject[] bgArr2;
	public GameObject[] bgArr3;
	public GameObject[] bgArr4;
	public GameObject[] bgArr5;
	public GameObject[] bgArr6;
	public GameObject[] bgArr7;
	public GameObject[] bgArr8;

	public List <GameObject[]> SelectedUI = new List<GameObject[]>();

	// Use this for initialization
	void Awake()
	{
		for (int i=0; i<PaoPosvacate.Length; i++) {
			PaoPosvacate[i]=true;
		}
		for (int i=0; i<BallistaPosvacate.Length; i++) {
			BallistaPosvacate[i]=true;
		}

		for (int i=0; i<PaoPos.Length; i++) {
			Posvacate.Add(PaoPos[i]);
		}
	}
	
	void Start () {
	    //PaoPos = GameObject.Find("Script").GetComponent<MainScript>().PaoPos;
		//Ballista = GameObject.Find ("Script").GetComponent<Player> ().Ballista;
	   //Ballista = GameObject.Find("Script").GetComponent<GameManager>().Players[0].Ballista;
		//Hooknum = GameObject.Find ("Script").GetComponent<Player> ().Hooknum;
		CurrentPos = PaoPos [0];
		//m_transform = this.transform;


	}
	
	// Update is called once per frame
	void Update () {
	/*
		if (Input.GetKeyDown (KeyCode.Alpha1) ) {
			print ("1");
			//Instantiate(Ballista[0],CurrentPos.position,CurrentPos.rotation);
			if (!GameManager.Instance.Players [0].CurrentBall) {
				PaoPosindex = 0;
				CurrentPos = PaoPos [PaoPosindex];
				PaoPosvacate [PaoPosindex] = false;
				Posvacate.Remove(PaoPos[PaoPosindex]);
				//	CurrentIndicate=Myutils.makeGameObject (GameManager.Instance.Players[0].cardIndicate, cardIndicatePos[0].transform);
				//	GameManager.Instance.Players[0].CurrentIndicate=CurrentIndicate;
					
				GameManager.Instance.Players[0].CurrentPos=CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista[0], CurrentPos);
					GameManager.Instance.Players[0].CurrentBall=CurrentBall;

				//CurrentBalls.Add(CurrentBall);
				//CurrentBall = Myutils.makeGameObject (Ballistas[0][0], CurrentPos);

                Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
				//标记弩赋值
					BallistaOnScene.id=PaoPosindex;
				BallistaOnScene.Hooknum=GameManager.Instance.Players[0].Hooknum;
				//	print ("BallistaOnScene.Hooknum="+BallistaOnScene.Hooknum);

				//BallistaOnScene.name =i.ToString();
				//CurrentBall.transform.localScale=new Vector3(10,10,10);
			}
		}
	

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			print ("2");
			//Instantiate(Ballista[0],CurrentPos.position,CurrentPos.rotation);
			if (!GameManager.Instance.Players [1].CurrentBall) {
				PaoPosindex = 1;
				CurrentPos = PaoPos [PaoPosindex];
				PaoPosvacate [PaoPosindex] = false;
				Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players[1].CurrentPos=CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista[1], CurrentPos);
					GameManager.Instance.Players[1].CurrentBall=CurrentBall;

				//	CurrentBalls.Add(CurrentBall);
				//CurrentBall = Myutils.makeGameObject (Ballistas [0][0], CurrentPos);
				Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
				//标记弩赋值
					BallistaOnScene.id=PaoPosindex;
					BallistaOnScene.Hooknum=GameManager.Instance.Players[1].Hooknum;
			}
		}
		
		 
	

		if (Input.GetKeyDown (KeyCode.Alpha3) ) {
			print ("3");
			if (!GameManager.Instance.Players [2].CurrentBall) {
				PaoPosindex = 2;
				CurrentPos = PaoPos [PaoPosindex];
				PaoPosvacate [PaoPosindex] = false;
				Posvacate.Remove (PaoPos [PaoPosindex]);

				GameManager.Instance.Players [2].CurrentPos = CurrentPos;
				CurrentBall = Myutils.makeGameObject (Ballista [2], CurrentPos);
				GameManager.Instance.Players [2].CurrentBall = CurrentBall;
				//CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
				Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
				//标记弩赋值
				BallistaOnScene.id = PaoPosindex;
				BallistaOnScene.Hooknum = GameManager.Instance.Players [2].Hooknum;
				print ("BallistaOnScene.Hooknum=" + BallistaOnScene.Hooknum);
			}
		}
		

		{
			if (Input.GetKeyDown (KeyCode.Alpha4) ) {
				print ("4");

				if (!GameManager.Instance.Players [3].CurrentBall) {
					PaoPosindex = 3;
					CurrentPos = PaoPos [PaoPosindex];
					PaoPosvacate [PaoPosindex] = false;
					Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players [3].CurrentPos = CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista [0], CurrentPos);
					GameManager.Instance.Players [3].CurrentBall = CurrentBall;
					//CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
					Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id = PaoPosindex;
					BallistaOnScene.Hooknum = GameManager.Instance.Players [3].Hooknum;
				}
			}
		}
			
		{
			if (Input.GetKeyDown (KeyCode.Alpha5)) {

				if (!GameManager.Instance.Players [4].CurrentBall) {
					PaoPosindex = 4;
					CurrentPos = PaoPos [PaoPosindex];
					PaoPosvacate [PaoPosindex] = false;
					Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players [4].CurrentPos = CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista [0], CurrentPos);
					GameManager.Instance.Players [4].CurrentBall = CurrentBall;
					//	CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
					Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id = PaoPosindex;
					BallistaOnScene.Hooknum = GameManager.Instance.Players [4].Hooknum;
				}
			} 
		}
	
		{
			if (Input.GetKeyDown (KeyCode.Alpha6)) {
			
				if (!GameManager.Instance.Players [5].CurrentBall) {
					PaoPosindex = 5;
					CurrentPos = PaoPos [PaoPosindex];
					PaoPosvacate [PaoPosindex] = false;
					Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players [5].CurrentPos = CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista [0], CurrentPos);
					GameManager.Instance.Players [5].CurrentBall = CurrentBall;
					//	CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
					Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id = PaoPosindex;
					BallistaOnScene.Hooknum = GameManager.Instance.Players [5].Hooknum;
				}
			} 
		}
		{
			if (Input.GetKeyDown (KeyCode.Alpha7)) {

				if (!GameManager.Instance.Players [6].CurrentBall) {
					PaoPosindex = 6;
					CurrentPos = PaoPos [PaoPosindex];
					PaoPosvacate [PaoPosindex] = false;
					Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players [6].CurrentPos = CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista [0], CurrentPos);
					GameManager.Instance.Players [6].CurrentBall = CurrentBall;
					//	CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
					Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id = PaoPosindex;
					BallistaOnScene.Hooknum = GameManager.Instance.Players [6].Hooknum;
				}
			} 
		}

		{
			if (Input.GetKeyDown (KeyCode.Alpha8)) {

				if (!GameManager.Instance.Players [7].CurrentBall) {
					PaoPosindex = 7;
					CurrentPos = PaoPos [PaoPosindex];
					PaoPosvacate [PaoPosindex] = false;
					Posvacate.Remove (PaoPos [PaoPosindex]);

					GameManager.Instance.Players [7].CurrentPos = CurrentPos;
					CurrentBall = Myutils.makeGameObject (Ballista [0], CurrentPos);
					GameManager.Instance.Players [7].CurrentBall = CurrentBall;
					//CurrentBall =Myutils.makeGameObject (Ballistas[0][0], CurrentPos);
					Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id = PaoPosindex;
					BallistaOnScene.Hooknum = GameManager.Instance.Players [7].Hooknum;
				}
			} 
		}*/



		/*
		if (Input.GetKeyDown (KeyCode.Keypad1)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 0;
				GoIndex1 (CurrentIndex);
				print ("Keypad1");
			}
		}

		if (Input.GetKeyDown (KeyCode.Keypad2)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 1;
				GoIndex1 (CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad3)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 2;
				GoIndex1 (CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad4)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 3;
				GoIndex1 (CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad5)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 4;
				GoIndex1 (CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad6)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 5;
				GoIndex1 (CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad7)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 6;
				GoIndex1(CurrentIndex);
				//print ("Keypad2");
			}
		}
		if (Input.GetKeyDown (KeyCode.Keypad8)) {
			if (!GameManager.Instance.Players [0].CurrentBall) {
				//当前指示器
				CurrentIndex = 7;
				GoIndex1 (CurrentIndex);
				print ("Keypad8");
			}
		}*/
	}
	public void  changePaoDefalt(int id,int CurrentIndex)
	{
		if (GameManager.Instance.Players[id].CurrentBall) {
			Destroy(GameManager.Instance.Players[id].CurrentBall);
		}

		CurrentBall = Myutils.makeGameObject (Ballista[0], PaoPos [CurrentIndex]);
		GameManager.Instance.Players[id].CurrentBall=CurrentBall;
		Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
		//标记弩赋值
		BallistaOnScene.id=id;
		//BallistaOnScene.CurrentIndex=CurrentIndex;
		//GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
		BallistaOnScene.Hooknum=GameManager.Instance.Players[id].Hooknum;
	}
		
	public void changePao(int id, int Currentindex)
	{  
		k++;
		i=k%Ballista.Length;
	
		if (GameManager.Instance.Players [id].CurrentBall) {
			Destroy (GameManager.Instance.Players [id].CurrentBall);
		} 
		CurrentBall = Myutils.makeGameObject (Ballista [i], PaoPos [Currentindex]);
		GameManager.Instance.Players [id].CurrentBall = CurrentBall;
		Ballistabasic BallistaOnScene = CurrentBall.GetComponent<Ballistabasic> ();
		//标记弩赋值
		BallistaOnScene.id = id;
	//	BallistaOnScene.CurrentIndex=CurrentIndex;
		//GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
		BallistaOnScene.Hooknum = GameManager.Instance.Players [id].Hooknum;
		if (k >= Ballista.Length)
			k = 0;
	}

//	int uidIndex = alUserIds.IndexOf(userId);
	public void GoIndex1(int index){

		for (int i=0; i<bgArr1.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr1 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;

			} else 
				if (BallistaPosvacate[i])
			{
				bgArr1 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex2(int index){
	
		for (int i=0; i<bgArr2.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr2 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;
			
			} else 
				if (BallistaPosvacate[i])
			{
				bgArr2 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex3(int index){

		for (int i=0; i<bgArr3.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr3 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;

			} else 
				if (BallistaPosvacate[i])
			{
				bgArr3 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex4(int index){

		for (int i=0; i<bgArr4.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr4 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;
			
			} else 
				if (BallistaPosvacate[i])
			{
				bgArr4 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex5(int index){

		for (int i=0; i<bgArr5.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr5 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;

			} else 
				if (BallistaPosvacate[i])
			{
				bgArr5 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex6(int index){

		for (int i=0; i<bgArr6.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr6 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;

			} else 
				if (BallistaPosvacate[i])
			{
				bgArr6 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex7(int index){

		for (int i=0; i<bgArr7.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr7 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;
			
			} else 
				if (BallistaPosvacate[i])
			{
				bgArr7 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex8(int index){
	
		for (int i=0; i<bgArr8.Length; i++) {
			if ((i == index)&&(BallistaPosvacate[i])){
				bgArr8 [i].SetActive (true);
				bgArro [i].SetActive (false);
				BallistaPosvacate[i]=false;

			} else 
				if (BallistaPosvacate[i])
			{
				bgArr8 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}


	public void GoIndex1NoShow(int index)
	{
		bglight1 [index].SetActive (false);
		}

	public void GoIndex2NoShow(int index)
	{
		bglight2 [index].SetActive (false);
	}

	public void GoIndex3NoShow(int index)
	{
		bglight3 [index].SetActive (false);
	}
	public void GoIndex4NoShow(int index)
	{
		bglight4 [index].SetActive (false);
	}
	public void GoIndex5NoShow(int index)
	{
		bglight5 [index].SetActive (false);
	}
	public void GoIndex6NoShow(int index)
	{
		bglight6 [index].SetActive (false);
	}
	public void GoIndex7NoShow(int index)
	{
		bglight7 [index].SetActive (false);
	}
	public void GoIndex8NoShow(int index)
	{
		bglight8 [index].SetActive (false);
	}

}
	/*
	public void GoIndex1(int index){
		bgArrosub.AddRange (bgArro);
		for (int i=0; i<bgArr1.Length; i++) {
			if (i == index) {
				bgArr1 [i].SetActive (true);
				bgArro [i].SetActive (false);
				bgArrosub.Remove(bgArrosub[i]);
			} else {
				bgArr1 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex2(int index){
		for (int i=0; i<bgArr2.Length; i++) {
			if (i == index) {
				bgArr2 [i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr2 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex3(int index){
		for (int i=0; i<bgArr3.Length; i++) {
			if (i == index) {
				bgArr3 [i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr3 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex4(int index){
		for (int i=0; i<bgArr4.Length; i++) {
			if (i == index) {
				bgArr4 [i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr4 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex5(int index){
		for (int i=0; i<bgArr5.Length; i++) {
			if (i == index) {
				bgArr5 [i].SetActive (true);
				bgArro[i].SetActive (false);
			} else {
				bgArr5 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}

	public void GoIndex6(int index){
		for (int i=0; i<bgArr6.Length; i++) {
			if (i == index) {
				bgArr6[i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr6 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex7(int index){
		for (int i=0; i<bgArr7.Length; i++) {
			if (i == index) {
				bgArr7 [i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr7 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}
	public void GoIndex8(int index){
		for (int i=0; i<bgArr7.Length; i++) {
			if (i == index) {
				bgArr8 [i].SetActive (true);
				bgArro [i].SetActive (false);
			} else {
				bgArr8 [i].SetActive (false);			
				bgArro [i].SetActive (true);					
			}
		}
	}*/
	



