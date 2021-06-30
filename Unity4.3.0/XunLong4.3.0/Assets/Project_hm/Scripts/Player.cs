using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 //using UnityEngine.UI;

[AddComponentMenu("testgame/player")]
public class Player : MonoBehaviour {
//	public List <Transform> Ballista = new List<Transform>();
	//public List <GameObject> Ballista = new List<GameObject>();
	public GameObject[]  Ballista ;
	public Transform[] PaoPos;
	protected int k=0;
	protected int i=0;
	protected int Ballnum=1;

	public  int LongCoinnum=30;
	public  int Hooknum=10;
	public int Linpiannum=0;
	public int m_score=0;
	public int m_exp=0;
	public int m_expsum=0;
	
	public string PlayerID;

	public int Hooknumadd=0;
	public int m_scoreadd=0;
	public int m_expadd=0;
	public int Linpiannumadd=0;
	public int ex_score = 0;

	public Transform CurrentPos;
	public GameObject CurrentBall;

	public GameObject cardIndicate;
	public GameObject CurrentIndicate;

	public int CurrentIndex=0,NextIndex,PreIndex,NewIndex=0;
	public bool isGetAxisP=false;
	public bool[] bgArroVacated;
	public bool[] PosVacated;
	public int id=-1;
	public List <GameObject> bgArro= new List<GameObject>();
	public List <GameObject> bgArrosub= new List<GameObject>();
	private bool Location = true;

	// 声音
	public AudioClip m_shootClip;
	// 声音源
	protected AudioSource m_audio;

	// Use this for initialization
	void Start () {
	
		Ballista = GameObject.Find ("Script").GetComponent<PaoPosManager> ().Ballista;
		Ballnum = Ballista.Length;
		bgArro = GameObject.Find ("Script").GetComponent<PaoPosManager> ().bgArro;
		bgArroVacated = GameObject.Find ("Script").GetComponent<PaoPosManager> ().BallistaPosvacate;
		PosVacated= GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPosvacate;
		PaoPos = GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPos;
	}
	
	// Update is called once per frame
	void Update () {
	//	LongCoinnum = GameObject.Find ("Script").GetComponent<InterScript> ().rsp.m_mUserData.m_nUserCoins;
	//	Hooknum = GameObject.Find ("Script").GetComponent<InterScript> ().rsp.m_mXunLongData.m_nNuJian;
		 GameObject.Find ("Script").GetComponent<PaoPosManager> ().BallistaPosvacate=bgArroVacated;
		GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPosvacate=PosVacated;
		PlayerInputManager (id);
			
	}

public void PlayerInputManager(int index)
	{
		switch (index) {
		case 0:
	
			GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex1 (CurrentIndex);
		
			// 左边手把 左键
			if ((Input.GetAxis ("Horizontal")==-1) &&!isGetAxisP&&Location){
		
				for (int i=1;i<8;i++)
				{
					if ((CurrentIndex -i< 0)&&(bgArroVacated[CurrentIndex -i+8]))
				{
						PreIndex =CurrentIndex -i+8;
						print ("PreIndex1="+PreIndex);
						break;
				}
			   else 
						if(((CurrentIndex-i)>=0)&&(bgArroVacated[CurrentIndex-i]))
					{	PreIndex=CurrentIndex-i;
						print ("PreIndex2="+PreIndex);
						break;}
				  }
			
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = PreIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex1 (CurrentIndex);
				bgArroVacated[CurrentIndex]=false;

				//print ("PreIndexPos1");
				isGetAxisP = true;
				StartCoroutine (timeout ());
			}


			// 左边手把 右键
			if( (Input.GetAxis ("Horizontal") == 1) &&(!isGetAxisP)&&Location) {
			
				for (int i=1;i<8;i++)
				{
				if((CurrentIndex+i<=7)&&(bgArroVacated[CurrentIndex+i]))
					{
						NextIndex = CurrentIndex + i;
					print ("NextIndex2="+NextIndex);
						break;
					}
					else if ((CurrentIndex+i>7)&&(bgArroVacated[(CurrentIndex+i)%8]))
					{
						NextIndex=(CurrentIndex+i)%8;
						print ("NextIndex3="+NextIndex);
						break;
					}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = NextIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex1 (CurrentIndex);
				bgArroVacated[CurrentIndex]=false;
				isGetAxisP= true;
				StartCoroutine (timeout ());
			}

			if ( (Input.GetKey (Const.MyJoystick1Button1))||(Input.GetKeyDown (KeyCode.Alpha1) ) ) {
				CurrentPos=	GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPos[CurrentIndex];
				if ((!GameManager.Instance.Players [index].CurrentBall)&&(PosVacated[CurrentIndex])) {

					CurrentBall = Myutils.makeGameObject (Ballista[0], CurrentPos);
					GameManager.Instance.Players[index].CurrentBall=CurrentBall;
					Location=false;
					PosVacated[CurrentIndex]=false;
					GameManager.Instance.PlayersActive.Add(this);

					GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex1NoShow(CurrentIndex);

					Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id=id;
					//BallistaOnScene.CurrentIndex=CurrentIndex;
					GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
					BallistaOnScene.Hooknum=GameManager.Instance.Players[index].Hooknum;
//					GameObject.Find ("Script").GetComponent<PaoPosManager> ().Posvacate.Remove(PaoPos[CurrentIndex]);

					GameManager.Instance.ShowUIMessage(index);
				}
			  }
			break;
		case 1:
		

			GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex2 (CurrentIndex);
			// 左边手把 左键
			if ((Input.GetAxis ("Horizontal2")==-1) &&!isGetAxisP&&Location){
				
				for (int i=1;i<8;i++)
				{
					if ((CurrentIndex -i< 0)&&(bgArroVacated[CurrentIndex -i+8]))
					{
						PreIndex =CurrentIndex -i+8;
						//print ("PreIndex1="+PreIndex);
						break;
					}
					else 
						if(((CurrentIndex-i)>=0)&&(bgArroVacated[CurrentIndex-i]))
					{	PreIndex=CurrentIndex-i;
						//print ("PreIndex2="+PreIndex);
						break;}
				}

				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = PreIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex2(CurrentIndex);
				//print ("PreIndexPos1");
				bgArroVacated[CurrentIndex]=false;
				isGetAxisP = true;
				StartCoroutine (timeout ());
			}
			
			// 左边手把 右键
			if( (Input.GetAxis ("Horizontal2") == 1) &&(!isGetAxisP)&&Location) {
				
				for (int i=1;i<8;i++)
				{
					
					if((CurrentIndex+i<=7)&&(bgArroVacated[CurrentIndex+i]))
					{NextIndex = CurrentIndex + i;
					//	print ("NextIndex2="+NextIndex);
						break;}
					else if ((CurrentIndex+i>7)&&(bgArroVacated[(CurrentIndex+i)%8]))
					{NextIndex=(CurrentIndex+i)%8;
					//	print ("NextIndex3="+NextIndex);
						break;}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = NextIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex2 (CurrentIndex);
				//print ("NextIndexPos1");
				bgArroVacated[CurrentIndex]=false;
				isGetAxisP= true;
				StartCoroutine (timeout ());
			}

			if ( (Input.GetKey (Const.MyJoystick2Button1))||(Input.GetKeyDown (KeyCode.Alpha2))) {
				CurrentPos=	GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPos[CurrentIndex];
				if ((!GameManager.Instance.Players [index].CurrentBall)&&(PosVacated[CurrentIndex])) {
					CurrentBall = Myutils.makeGameObject (Ballista[0], CurrentPos);
					Location=false;
					PosVacated[CurrentIndex]=false;
					GameManager.Instance.PlayersActive.Add(this);

					GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex2NoShow(CurrentIndex);
					Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id=id;
					//BallistaOnScene.CurrentIndex=CurrentIndex;
					GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
					BallistaOnScene.Hooknum=GameManager.Instance.Players[index].Hooknum;
					GameManager.Instance.ShowUIMessage(index);
				}
			}

			break;
		case 2:
	

		
			GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex3 (CurrentIndex);
			// 左边手把 左键
			if ((Input.GetAxis ("Horizontal3")==-1) &&!isGetAxisP&&Location){

				for (int i=1;i<8;i++)
				{
					if ((CurrentIndex -i< 0)&&(bgArroVacated[CurrentIndex -i+8]))
					{
						PreIndex =CurrentIndex -i+8;
						//print ("PreIndex1="+PreIndex);
						break;
					}
					else 
						if(((CurrentIndex-i)>=0)&&(bgArroVacated[CurrentIndex-i]))
					{	PreIndex=CurrentIndex-i;
						//print ("PreIndex2="+PreIndex);
						break;}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = PreIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex3(CurrentIndex);
				//print ("PreIndexPos1");
				bgArroVacated[CurrentIndex]=false;
				isGetAxisP = true;
				StartCoroutine (timeout ());
			}
			
			// 左边手把 右键
			if( (Input.GetAxis ("Horizontal3") == 1) &&(!isGetAxisP)&&Location) {
				
				for (int i=1;i<8;i++)
				{
					
					if((CurrentIndex+i<=7)&&(bgArroVacated[CurrentIndex+i]))
					{NextIndex = CurrentIndex + i;
						//	print ("NextIndex2="+NextIndex);
						break;}
					else if ((CurrentIndex+i>7)&&(bgArroVacated[(CurrentIndex+i)%8]))
					{NextIndex=(CurrentIndex+i)%8;
						//	print ("NextIndex3="+NextIndex);
						break;}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = NextIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex3 (CurrentIndex);
				//print ("NextIndexPos1");
				bgArroVacated[CurrentIndex]=false;

				isGetAxisP= true;
				StartCoroutine (timeout ());
			}

			if ( (Input.GetKey (Const.MyJoystick3Button1))||(Input.GetKeyDown (KeyCode.Alpha3))) {
				CurrentPos=	GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPos[CurrentIndex];
				if ((!GameManager.Instance.Players [index].CurrentBall) &&(PosVacated[CurrentIndex])){
					CurrentBall = Myutils.makeGameObject (Ballista[0], CurrentPos);
					Location=false;
					PosVacated[CurrentIndex]=false;
					GameManager.Instance.PlayersActive.Add(this);

					GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex3NoShow(CurrentIndex);
					Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id=id;
					//BallistaOnScene.CurrentIndex=CurrentIndex;
					GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
					BallistaOnScene.Hooknum=GameManager.Instance.Players[index].Hooknum;
					GameManager.Instance.ShowUIMessage(index);
				}
			}
			break;
		case 3:
		
			
		
			GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex4 (CurrentIndex);
			// 左边手把 左键
			if ((Input.GetAxis ("Horizontal4")==-1) &&!isGetAxisP&&Location){
				
				for (int i=1;i<8;i++)
				{
					if ((CurrentIndex -i< 0)&&(bgArroVacated[CurrentIndex -i+8]))
					{
						PreIndex =CurrentIndex -i+8;
						//print ("PreIndex1="+PreIndex);
						break;
					}
					else 
						if(((CurrentIndex-i)>=0)&&(bgArroVacated[CurrentIndex-i]))
					{	PreIndex=CurrentIndex-i;
						//print ("PreIndex2="+PreIndex);
						break;}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = PreIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex4 (CurrentIndex);
				bgArroVacated[CurrentIndex]=false;
				//print ("PreIndexPos1");
				
				
				isGetAxisP = true;
				StartCoroutine (timeout ());
			}

			
			// 左边手把 右键
			if( (Input.GetAxis ("Horizontal4") == 1) &&(!isGetAxisP)&&Location) {
				
				for (int i=1;i<8;i++)
				{
					
					if((CurrentIndex+i<=7)&&(bgArroVacated[CurrentIndex+i]))
					{NextIndex = CurrentIndex + i;
						//	print ("NextIndex2="+NextIndex);
						break;}
					else if ((CurrentIndex+i>7)&&(bgArroVacated[(CurrentIndex+i)%8]))
					{NextIndex=(CurrentIndex+i)%8;
						//	print ("NextIndex3="+NextIndex);
						break;}
				}
				bgArroVacated[CurrentIndex]=true;
				CurrentIndex = NextIndex;
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex4 (CurrentIndex);
				//print ("NextIndexPos1");
				bgArroVacated[CurrentIndex]=false;

				isGetAxisP= true;
				StartCoroutine (timeout ());
			}

			if ( (Input.GetKey (Const.MyJoystick4Button1))) {
				CurrentPos=	GameObject.Find ("Script").GetComponent<PaoPosManager> ().PaoPos[CurrentIndex];
				if( (!GameManager.Instance.Players [index].CurrentBall)&&(PosVacated[CurrentIndex])) {
					CurrentBall = Myutils.makeGameObject (Ballista[0], CurrentPos);
					Location=false;
					PosVacated[CurrentIndex]=false;
					GameManager.Instance.PlayersActive.Add(this);

					GameObject.Find ("Script").GetComponent<PaoPosManager> ().GoIndex4NoShow(CurrentIndex);
					Ballistabasic BallistaOnScene= CurrentBall.GetComponent<Ballistabasic> ();
					//标记弩赋值
					BallistaOnScene.id=id;
					//BallistaOnScene.CurrentIndex=CurrentIndex;
					GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id]=CurrentIndex;
					BallistaOnScene.Hooknum=GameManager.Instance.Players[index].Hooknum;
					GameManager.Instance.ShowUIMessage(index);
				}
			}
			break;
		}
	}



IEnumerator timeout()
	{
		yield return new WaitForSeconds (1f);
		isGetAxisP=false;
	
	}
}
