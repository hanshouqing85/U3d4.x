using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/Ballistabasic")]
public class Ballistabasic : MonoBehaviour {
	public delegate void KeyDown(int id);
	public event KeyDown keydown;

	public int id=-1; 
//	public GameObject hookgb=null;
	public float autoShoot_timer = 100f;
	public float autoShoot_timervalue=100f;
	public float rotAngleLMin=95f;
	public float rotAngleRMax=260f;
	public float rotAngleLyouMin=30f;
	public float rotAngleRyouMax=190f;
	public float rotAngleLzuoMin=170f;
	public float rotAngleRzuoMax=330f;
	//旋转速度
	public float rotateSpeed = 20;
	//弩钩发射频率
	public float hookRatevalue=1f;
    public float hookRate=1f;
	public GameObject Hook;
	public float m_timer=1f;
	public GameObject HookPos;
	//动画组件
	Animator m_ani;
	public float shangxianRate=1f;
	public float shangxianRatevalue=1f;
	//public GameObject OnlyRope;
	//弩钩
	//public Transform  m_Hook ;
	//public GameObject  m_Hook ;
	//public Transform transform;
	public  int Hooknum=0;
	//public List<GameObject> LaserLine = new List<GameObject>(); 

	//public GameObject jian;

	public List<GameObject> Hookgun = new List<GameObject>();
	public List<Transform> m_transform = new List<Transform >();
	public List<GameObject> Hookgb = new List<GameObject>();
	public List<Hookbasic> Hookgbsc = new List<Hookbasic>();
	public int Hookgunnum;
	protected int PaoPosindex;

	// 声音shoot
	public AudioClip m_shootClip;
	// 声音源
	protected AudioSource m_audio;

	// 声音change
	public AudioClip m_changeClip;

	public bool shoot = false;
	public bool Ready=false;
	private bool Action=false;
	private bool Shootenble=true;
	private bool isButtonDown = false;

	private GameObject hookgb;
	private  AnimatorStateInfo stateInfo;

	private float distance =1f;
	private Quaternion rot,rL,rR,r0,r1,r;
	private Vector3 f0, f1,f2;
	public int CurrentIndex=-1;

	void Awake()
	{
		//GameObject.Find ("Script").GetComponent<InterScript> ().LaserLine=LaserLine ;
	}
	
	// Use this for initialization
	void Start () {
	//	transform = this.transform;
	//	transform.Rotate (Vector3.left,10,Space.World);
	
		rL = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-80f,transform.rotation.eulerAngles.z);
		rR = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y+80f,transform.rotation.eulerAngles.z);

		r = transform.rotation;
		f0=(transform.position-(r*Vector3.forward)*distance);
		r0 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-80f,transform.rotation.eulerAngles.z);
		r1 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y+80f,transform.rotation.eulerAngles.z);
		f1 = (transform.position-(r0*Vector3.forward)*distance);
		f2 =(transform.position-(r1*Vector3.forward)*distance);

	//	print ("r="+r);
	//	print ("rL="+rL);
	//	print ("rR="+rR);

		//获取组件
		m_ani = this.GetComponent<Animator>();
		PaoPosindex = GameObject.Find("Script").GetComponent<PaoPosManager>().PaoPosindex;
		//Hooknum=GameObject.Find ("Script").GetComponent<PaoPosManager> ().Hooknum;

		m_audio = this.GetComponent<AudioSource>();
		Hookgunnum = Hookgun.Count;
		for (int i=0; i<Hookgun.Count; i++) {
			m_transform.Add(Hookgun[i].transform);
		}
	}


	
	// Update is called once per frame
	void Update () {
		rot = transform.rotation;
		Shootenble = GameManager.Instance.Shootenble;
		//获取当前动画状态
		 stateInfo = m_ani.GetCurrentAnimatorStateInfo (0);

	   BallistaInput(id);
		Debug.DrawLine (transform.position,f0,Color.red);
		Debug.DrawLine (transform.position,f1,Color.red);
		Debug.DrawLine (transform.position,f2,Color.red);
		Debug.DrawLine (f0,f1,Color.red);
		Debug.DrawLine (f0,f2,Color.red);
	/*
		//按A键分中右左三类位置讨论
		if (Input.GetKey (KeyCode.A)) {
			if ((PaoPosindex<4)&&(this.transform.eulerAngles.y>=rotAngleLMin)){ 
			
				print ("rotL="+rot);
				//LeanTween.rotateAround( gameObject, Vector3.up, -10f, 0.01f);
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*-rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
		//	if (((PaoPosindex>=4)&&(PaoPosindex<=5))&&(this.transform.eulerAngles.y>=rotAngleLyouMin)){
				if ((PaoPosindex>=4)&&(rot.y > rL.y))	{

				print ("rotL="+rot);
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
			//	print ("RotateAnglesA="+Vector3.up*Time.deltaTime*-rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
			if (((PaoPosindex>=6)&&(PaoPosindex<=7))&&(this.transform.eulerAngles.y>=rotAngleLzuoMin)){
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*-rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
		}
		//按D键分中右左三类位置讨论
	 else if (Input.GetKey(KeyCode.D))
		{  
		if ((PaoPosindex<4)&&(this.transform.eulerAngles.y<=rotAngleRMax)){
				
				print ("rotR="+rot);
			this.transform.Rotate(Vector3.up*Time.deltaTime*rotateSpeed);
			//print ("RotateAnglesD="+Vector3.up*Time.deltaTime*rotateSpeed);
			//print ("euleranglesright="+this.transform.eulerAngles);
			}
		//	if (((PaoPosindex>=4)&&(PaoPosindex<=5))&&(this.transform.eulerAngles.y<=rotAngleRyouMax)){
				if ((PaoPosindex>=4)&&(rot.y < rR.y))	{
				print ("rotR="+rot);
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*rotateSpeed);
			//	print ("euleranglesleft="+this.transform.eulerAngles);
			}
			if (((PaoPosindex>=6)&&(PaoPosindex<=7))&&(this.transform.eulerAngles.y<=rotAngleRzuoMax)){
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
		}*/
	}

public void BallistaInput(int index)
	{
		switch (index) {                                            
		case 0:
			print ("00000000000000");
			CurrentIndex = GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation [index];
			if ((CurrentIndex >= 0) && (CurrentIndex <= 3)) {
				if (Input.GetAxis ("Horizontal") == -1) {
					print ("Joystick0Lx");
					// 左边手把 左键
					if ((this.transform.eulerAngles.y >= rotAngleLMin)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal") == 1) {
					print ("Joystick0Rx");
					// 左边手把 右键
					if ((this.transform.eulerAngles.y <= rotAngleRMax)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}

			if ((CurrentIndex >= 4) && (CurrentIndex <= 7)) {
				if (Input.GetAxis ("Horizontal") == -1) {
					print ("Joystick0L");
					// 左边手把 左键
					if (rot.y > rL.y) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal") == 1) {
					print ("Joystick0R");
					// 左边手把 右键
					if ((rot.y < rR.y)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}

			}

			if (Input.GetKeyDown (Const.MyJoystick1Button0)) {   
				isButtonDown = true;
				print ("Joystick1Button0ChangPao");
				// 播放换炮声音
				m_audio.PlayOneShot (m_changeClip);
				//CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
				GameObject.Find ("Script").GetComponent<PaoPosManager> ().changePao (id, CurrentIndex);
			} else if (Input.GetKeyUp (Const.MyJoystick1Button0)) {
				isButtonDown = false;
			}

			if (Input.GetKeyDown (Const.MyJoystick1Button2)) {
				isButtonDown = true;

				//GameManager.Instance.Orderlongcoins(id);
				GameManager.Instance.Orderlongcoins1 (id);
			} else if (Input.GetKeyUp (Const.MyJoystick1Button2)) {
				isButtonDown = false;
			}

			break;

		case 1:
			print ("1111111111111");
			CurrentIndex = GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation [index];
			if ((CurrentIndex >= 0) && (CurrentIndex <= 3))
			{
			if (Input.GetAxis ("Horizontal2") == -1) {
				// 左边手把 左键
				if ((this.transform.eulerAngles.y >= rotAngleLMin)) {
					this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				}
			}
			if (Input.GetAxis ("Horizontal2") == 1) {
				// 左边手把 右键
				if ((this.transform.eulerAngles.y <= rotAngleRMax)) {
					this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				}
			}
		}
			if ((CurrentIndex >= 4) && (CurrentIndex <= 7)) {
				if (Input.GetAxis ("Horizontal2") == -1) {
					print ("Joystick1L");
					// 左边手把 左键
					if (rot.y > rL.y) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal2") == 1) {
					print ("Joystick1R");
					// 左边手把 右键
					if ((rot.y < rR.y)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}

			if (Input.GetKeyDown (Const.MyJoystick2Button0))
			{   
				isButtonDown=true;
				print ("Joystick2Button0ChangPao");
				// 播放换炮声音
				m_audio.PlayOneShot (m_changeClip);
				GameObject.Find("Script").GetComponent<PaoPosManager>().changePao(id,CurrentIndex);
			}
			else if(Input.GetKeyUp (Const.MyJoystick2Button0))
			{
				isButtonDown=false;
			}

			if (Input.GetKeyDown (Const.MyJoystick2Button2))
			{
				isButtonDown=true;
				//GameManager.Instance.Orderlongcoins(id);
				GameManager.Instance.Orderlongcoins1(id);
			}
			else if  (Input.GetKeyUp (Const.MyJoystick2Button2))
			{
				isButtonDown=false;
			}
			break;

		case 2:
			print ("2222222222222");
			CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
			if ((CurrentIndex >= 0) && (CurrentIndex <= 3))
			{
				if (Input.GetAxis ("Horizontal3") == -1) {
					// 左边手把 左键
					if ((this.transform.eulerAngles.y >= rotAngleLMin)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal3") == 1) {
					// 左边手把 右键
					if ((this.transform.eulerAngles.y <= rotAngleRMax)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}
			if ((CurrentIndex >= 4) && (CurrentIndex <= 7)) {
				if (Input.GetAxis ("Horizontal3") == -1) {
					print ("Joystick1L");
					// 左边手把 左键
					if (rot.y > rL.y) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal3") == 1) {
					print ("Joystick1R");
					// 左边手把 右键
					if ((rot.y < rR.y)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}
			if (Input.GetKeyDown (Const.MyJoystick3Button0))
			{   
				isButtonDown=true;
				print ("Joystick3Button0ChangPao");
				// 播放换炮声音
				m_audio.PlayOneShot (m_changeClip);
				GameObject.Find("Script").GetComponent<PaoPosManager>().changePao(id,CurrentIndex);
			}
			else if(Input.GetKeyUp (Const.MyJoystick3Button0))
			{
				isButtonDown=false;
			}
			if (Input.GetKeyDown (Const.MyJoystick3Button2))
			{
				isButtonDown=true;
			//	GameManager.Instance.Orderlongcoins(id);
				GameManager.Instance.Orderlongcoins1(id);
			}
			else if  (Input.GetKeyUp (Const.MyJoystick3Button2))
			{
				isButtonDown=false;
			}
			break;

		case 3:
			print ("3333333333333333");
			CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
			if ((CurrentIndex >= 0) && (CurrentIndex <= 3))
			{
				if (Input.GetAxis ("Horizontal4") == -1) {
					// 左边手把 左键
					if ((this.transform.eulerAngles.y >= rotAngleLMin)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal4") == 1) {
					// 左边手把 右键
					if ((this.transform.eulerAngles.y <= rotAngleRMax)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}
			if ((CurrentIndex >= 4) && (CurrentIndex <= 7)) {
				if (Input.GetAxis ("Horizontal4") == -1) {
					//print ("Joystick1L");
					// 左边手把 左键
					if (rot.y > rL.y) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
					}
				}
				if (Input.GetAxis ("Horizontal4") == 1) {
					//print ("Joystick1R");
					// 左边手把 右键
					if ((rot.y < rR.y)) {
						this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
					}
				}
			}
			if (Input.GetKeyDown (Const.MyJoystick4Button0))
			{   
				isButtonDown=true;
				print ("Joystick4Button0ChangPao");
				// 播放换炮声音
				m_audio.PlayOneShot (m_changeClip);
				GameObject.Find("Script").GetComponent<PaoPosManager>().changePao(id,CurrentIndex);
			}
			else if(Input.GetKeyUp (Const.MyJoystick4Button0))
			{
				isButtonDown=false;
			}
			if (Input.GetKeyDown (Const.MyJoystick4Button2))
			{
				isButtonDown=true;
				//GameManager.Instance.Orderlongcoins(id);
				GameManager.Instance.Orderlongcoins1(id);
			}
			else if  (Input.GetKeyUp (Const.MyJoystick4Button2))
			{
				isButtonDown=false;
			}
			break;


		}//swith

	}

 public void BallistaHit(int index)
	     {
		shangxianRatevalue -= Time.deltaTime;
		hookRatevalue -= Time.deltaTime;
		autoShoot_timer -= Time.deltaTime;

		//如果处于闲置
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Idle") && !m_ani.IsInTransition (0)) 
		 {
			m_ani.SetBool ("Idle", false);

		
		//	if ((Hooknum >= Hookgunnum) && (Hooknum > 0) && (Shootenble))
			if ((Hooknum >= Hookgunnum) && (Hooknum > 0) )
			{
				m_timer = 1;
				m_ani.SetBool ("shangxian", true);
				if (!Ready){
				BallistaReady();
			
				print ("Ready2");
			     }
			  }
		   }
		//如果处于上弦状态
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.shangxian") && !m_ani.IsInTransition (0))
		  {
			m_ani.SetBool("shangxian",false);
		  }
		//如果处于发射状态
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.fashe") && !m_ani.IsInTransition (0)) {
			m_ani.SetBool ("fashe", false);
			//如果发射动画播完，重新进入闲置
			if (stateInfo.normalizedTime >= 1.0f) {
				m_ani.SetBool ("Idle", true);
				//重置定时器
				m_timer = 5;
			}
		}
		if (shangxianRatevalue <= 0) {
			shangxianRatevalue = shangxianRate;

			switch(index)
			{
			case 0:
				CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
				//空格或左键或相应手柄键发射弩钩
				if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) ||(Input.GetKey (Const.MyJoystick1Button3))||(autoShoot_timer <= 0)) 
				    && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble)&&(Ready))
				{
					//print ("Shoooooooooooooooooooooot1");
					autoShoot_timer = autoShoot_timervalue;
					shoot=true;
					StartCoroutine (Ballistashoot ());
				}
				else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
				{
					print ("ChangePao");
					// 播放换炮声音
					m_audio.PlayOneShot (m_changeClip);
					GameObject.Find("Script").GetComponent<PaoPosManager>().changePaoDefalt(id,CurrentIndex);
				}
				if (Hooknum==0)
					print ("Hook no enough, Please order Hook!");
				break;
			case 1:
				CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
				//空格或左键或相应手柄键发射弩钩
				if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) ||(Input.GetKey (Const.MyJoystick2Button3))||(autoShoot_timer <= 0)) 
				    && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble)&&(Ready))
				{
					//print ("Shoooooooooooooooooooooot2");
					autoShoot_timer = autoShoot_timervalue;
					shoot=true;
					StartCoroutine (Ballistashoot ());
				}
				else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
				{
					print ("ChangePao");
					// 播放换炮声音
					m_audio.PlayOneShot (m_changeClip);
					GameObject.Find("Script").GetComponent<PaoPosManager>().changePaoDefalt(id,CurrentIndex);
				}
				if (Hooknum==0)
					print ("Hook no enough, Please order Hook!");
				break;
			case 2:
				CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
				//空格或左键或相应手柄键发射弩钩
				if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) ||(Input.GetKey (Const.MyJoystick3Button3))||(autoShoot_timer <= 0)) 
				    && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble)&&(Ready))
				{
					//print ("Shoooooooooooooooooooooot3");
					autoShoot_timer = autoShoot_timervalue;
					shoot=true;
					StartCoroutine (Ballistashoot ());
				}
				else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
				{
					print ("ChangePao");
					// 播放换炮声音
					m_audio.PlayOneShot (m_changeClip);
					GameObject.Find("Script").GetComponent<PaoPosManager>().changePaoDefalt(id,CurrentIndex);
				}
				if (Hooknum==0)
					print ("Hook no enough, Please order Hook!");
				break;
			case 3:
				CurrentIndex=GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentLocation[index];
				//空格或左键或相应手柄键发射弩钩
				if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) ||(Input.GetKey (Const.MyJoystick4Button3))||(autoShoot_timer <= 0)) 
				    && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble)&&(Ready))
				{
					//print ("Shoooooooooooooooooooooot4");
					autoShoot_timer = autoShoot_timervalue;
					shoot=true;
					StartCoroutine (Ballistashoot ());
				}
				else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
				{
					print ("ChangePao");
					// 播放换炮声音
					m_audio.PlayOneShot (m_changeClip);
					GameObject.Find("Script").GetComponent<PaoPosManager>().changePaoDefalt(id,CurrentIndex);
				}
				if (Hooknum==0)
					print ("Hook no enough, Please order Hook!");
				break;

			}
		
		/*
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) ||(autoShoot_timer <= 0)) 
			     && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble)&&(Ready))
			    {
				autoShoot_timer = autoShoot_timervalue;
				shoot=true;
				StartCoroutine (Ballistashoot ());
			    }

			else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
			   {
				print ("ChangePao");
				GameObject.Find("Script").GetComponent<PaoPosManager>().changePao(id);
			   }
			 if (Hooknum==0)
				print ("Hook no enough, Please order Hook!");*/
		    }
	   }

void BallistaReady()
	{ 
	//	print ("Hookgb.Count=" + Hookgb.Count);
		if (Hookgb.Count==0) {
			for (int i=0; i<Hookgunnum; i++) {
				hookgb = Instantiate (Hook, m_transform [i].position, m_transform [i].rotation) as GameObject;
				hookgb.transform.parent = this.transform;
				Hookgb.Add (hookgb);
				Hookgbsc.Add (hookgb.GetComponent<Hookbasic> ());
				//标记弩箭赋值
				hookgb.GetComponent<Hookbasic> ().id = id;
				hookgb.GetComponent<Hookbasic>().CurrentIndex=GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentLocation[id];
				print ("zidanReady" + "-----------------------------");
			      }
			Ready=true;
		   }
	   }

IEnumerator Ballistashoot()
	    {        
		if((Ready)&&(Shootenble))
		   {
			m_ani.SetBool ("fashe", true);
			print ("fashe");
			Ready = false;
			yield return new WaitForSeconds (0.01f);
			if (hookRatevalue <= 0) {
				hookRatevalue = hookRate;
				for (int i=0; i<Hookgunnum; i++) {
					    Hooknum--;
				//	print ("Hookgb"+"["+i+"]="+Hookgb[i]);
					if (Hookgb [i] != null) {
						Hookgb [i].transform.parent = null;
					}
					if (Hookgbsc [i] != null) {
						Hookgbsc [i].shoot = shoot;
					}
					GameManager.Instance.m_localmissleshootsum++;
					GameManager.Instance.AddShootNum ();
					GameManager.Instance.UpdatePlayerHooknum (id);
					GameManager.Instance.UpdateUIhooknum (id);

					GameManager.Instance.UpdateHooknumAdd (id);
				
					GameManager.Instance.UpdateUIhooknum1 (id,CurrentIndex);
				}
				Hookgb.Clear ();
				Hookgbsc.Clear ();
				shoot = false;
				// 播放射击声音
				m_audio.PlayOneShot (m_shootClip);
			}
		} else { 
			print ("Can't shoot not Ready");
		}
	}
	
public void DelBallista()
{   
	if (gameObject) {
		Destroy (gameObject);
	}
  }
}

