using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/Ballista")]
public class Ballista : MonoBehaviour {

	public delegate void KeyDown(int id);
	public event KeyDown keydown;
	
	public int id=-1; 
	//	public GameObject hookgb=null;
	public float autoShoot_timer = 10f;
	public float autoShoot_timervalue=10f;
	public float rotAngleLMin=95f;
	public float rotAngleRMax=260f;
	public float rotAngleLyouMin=30f;
	public float rotAngleRyouMax=190f;
	public float rotAngleLzuoMin=170f;
	public float rotAngleRzuoMax=330f;
	//旋转速度
	public float rotateSpeed = 20;
	//弩钩发射频率
	protected float hookRatevalue=1f;
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
	
	public List<GameObject> Hookgun = new List<GameObject>();
	public List<Transform> m_transform = new List<Transform >();
	public List<GameObject> Hookgb = new List<GameObject>();
	public List<Hookbasic> Hookgbsc = new List<Hookbasic>();
	public int Hookgunnum;
	protected int PaoPosindex;
	
	// 声音
	public AudioClip m_shootClip;
	// 声音源
	protected AudioSource m_audio;
	private bool Shootenble=true;
	public bool shoot = false;
	public bool Ready=false;
	
	private  AnimatorStateInfo stateInfo;
	
	void Awake()
	{
		//GameObject.Find ("Script").GetComponent<InterScript> ().LaserLine=LaserLine ;
	}
	
	
	
	// Use this for initialization
	void Start () {
		//	transform = this.transform;
		//	transform.Rotate (Vector3.left,10,Space.World);
		//获取组件
		m_ani = this.GetComponent<Animator>();
		PaoPosindex = GameObject.Find("Script").GetComponent<PaoPosManager>().PaoPosindex;
		//Hooknum=GameObject.Find ("Script").GetComponent<PaoPosManager> ().Hooknum;
		//	Hooknum=GameManager.Instance.Players
		m_audio = this.GetComponent<AudioSource>();
		Hookgunnum = Hookgun.Count;
		for (int i=0; i<Hookgun.Count; i++) {
			m_transform.Add(Hookgun[i].transform);
		}
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		Shootenble = GameManager.Instance.Shootenble;
		//获取当前动画状态
		stateInfo = m_ani.GetCurrentAnimatorStateInfo (0);
		/*	//如果处于闲置
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Idle") && !m_ani.IsInTransition (0)) {
			m_ani.SetBool("Idle",false);
		}
		// 闲置一段时间
		m_timer -= Time.deltaTime;
		if (m_timer>0)
			return;
		if ((Hooknum >=Hookgunnum)&&(Hooknum!=0)&&(Shootenble))
		{
			m_timer=1;
			m_ani.SetBool("shangxian",true);
		}
		//如果处于上弦状态
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.shangxian") && !m_ani.IsInTransition (0)) {
			m_ani.SetBool("shangxian",false);
		}*/
		
		//按A键分中右左三类位置讨论
		if (Input.GetKey (KeyCode.A)) {
			if ((PaoPosindex<4)&&(this.transform.eulerAngles.y>=rotAngleLMin))	
			{  
				//LeanTween.rotateAround( gameObject, Vector3.up, -10f, 0.01f);
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*-rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
			if (((PaoPosindex>=4)&&(PaoPosindex<=5))&&(this.transform.eulerAngles.y>=rotAngleLyouMin)){
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*-rotateSpeed);
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
			if ((PaoPosindex<4)&&(this.transform.eulerAngles.y<=rotAngleRMax))
			{
				this.transform.Rotate(Vector3.up*Time.deltaTime*rotateSpeed);
				//print ("RotateAnglesD="+Vector3.up*Time.deltaTime*rotateSpeed);
				//print ("euleranglesright="+this.transform.eulerAngles);
			}
			if (((PaoPosindex>=4)&&(PaoPosindex<=5))&&(this.transform.eulerAngles.y<=rotAngleRyouMax)){
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
			if (((PaoPosindex>=6)&&(PaoPosindex<=7))&&(this.transform.eulerAngles.y<=rotAngleRzuoMax)){
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				//print ("RotateAnglesA="+Vector3.up*Time.deltaTime*rotateSpeed);
				//print ("euleranglesleft="+this.transform.eulerAngles);
			}
		}
		
		/*	if (hookRate <= 0) {
			hookRate=0.5f;
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) && (m_Hooknum!=0)) {
				//Instantiate(m_Hook,m_transform.position,m_transform.rotation);
			GameObject	gb =Myutils.makeGameObject (m_Hook,m_transform);
				m_Hooknum--;
			}
			else if (m_Hooknum==0)
			{
				print ("No Hook!Please order Hook!");
			}
		}*/
		//BallistaHit ();
	}
	
	public void DelBallista()
	{   
		if (gameObject) {
			Destroy (gameObject);
		}
	}
	
	public void BallistaHit()
	{
		shangxianRatevalue -= Time.deltaTime;
		hookRatevalue -= Time.deltaTime;
		autoShoot_timer -= Time.deltaTime;
		
		//如果处于闲置
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Idle") && !m_ani.IsInTransition (0)) 
		{
			m_ani.SetBool ("Idle", false);
			// 闲置一段时间
			//		m_timer -= Time.deltaTime;
			//		if (m_timer > 0)
			//			return;
			
			if ((Hooknum >= Hookgunnum) && (Hooknum > 0) && (Shootenble))
			{
				m_timer = 1;
				m_ani.SetBool ("shangxian", true);
				if (!Ready){
					BallistaReady();
					Ready=true;
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
				m_timer = 1;
			}
		}
		
		if (shangxianRatevalue <= 0) {
			shangxianRatevalue = shangxianRate;
			
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0) || (autoShoot_timer <= 0)) 
			    && (Hooknum %Hookgunnum>= 0) &&(Hooknum>=Hookgunnum)&& (Hooknum > 0) && (Shootenble))
			{
				autoShoot_timer = autoShoot_timervalue;
				shoot=true;
				
				//播放射击动画
				m_ani.SetBool ("fashe", true);
				//如果处于发射状态
				if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.fashe") && !m_ani.IsInTransition (0)) {
					m_ani.SetBool ("fashe", false);
					//如果发射动画播完，重新进入闲置
					if (stateInfo.normalizedTime >= 1.0f) {
						m_ani.SetBool ("Idle", true);
						//重置定时器
						m_timer = 1;
					}
				}
				
				//如果处于闲置
				if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Idle") && !m_ani.IsInTransition (0)) {
					m_ani.SetBool ("Idle", false);
					
					// 闲置一段时间
					m_timer -= Time.deltaTime;
					if (m_timer > 0)
						return;
					
					if ((Hooknum >= Hookgunnum) && (Hooknum != 0) && (Shootenble)) {
						m_timer = 1;
						m_ani.SetBool ("shangxian", true);
						if (!Ready){
							BallistaReady();
							Ready=true;
						}
					}
					
				}
				//如果处于上弦状态
				if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.shangxian") && !m_ani.IsInTransition (0))
				{
					m_ani.SetBool("shangxian",false);
				}
				StartCoroutine (Ballistashoot ());
				
				if (!Ready){
					BallistaReady();
					Ready=true;
				}
			}
			else if ((Hooknum<Hookgunnum)&&(Hooknum > 0))
			{
				print ("ChangePao");
				//GameObject.Find("Script").GetComponent<PaoPosManager>().changePao(id);
				
			}
			if (Hooknum==0)
				print ("Hook no enough, Please order Hook!");
		}
	}
	
	
	void BallistaReady()
	{
		//if (hookRatevalue <= 0) {
		//hookRatevalue = hookRate;
		for (int i=0; i<Hookgunnum; i++) {
			GameObject hookgb =Instantiate (Hook, m_transform [i].position, m_transform [i].rotation) as GameObject;
			Hookgb.Add (hookgb );
			hookgb.transform.parent=this.transform;
			Hookgbsc.Add( hookgb.GetComponent<HookwithRope04> ().Hook.GetComponent<Hookbasic> ());
			//标记弩箭赋值
			hookgb.GetComponent<HookwithRope04> ().Hook.GetComponent<Hookbasic> ().id = id;
			print ("zidanReady" + "-----------------------------");
		}
		//	}
		
	}
	
	IEnumerator Ballistashoot()
	{         Ready=false;
		yield return new WaitForSeconds (1f);
		
		if (hookRatevalue <= 0) {
			hookRatevalue = hookRate;
			
			for (int i=0; i<Hookgunnum; i++) {
				Hooknum--;
				Hookgbsc[i].shoot=shoot;
				Hookgb[i].transform.parent=null;
				GameManager.Instance.m_localmissleshootsum++;
				GameManager.Instance.AddShootNum ();
				GameManager.Instance.UpdatePlayerHooknum(id);
				GameManager.Instance.UpdateUIhooknum(id);
				//	print ("zidanfashed"+"-----------------------------");
				
			}
			shoot=false;
			// 播放射击声音
			m_audio.PlayOneShot (m_shootClip);
			Hookgb.Clear();
			Hookgbsc.Clear();
			//GameObject gb = Myutils.makeGameObject (Hook, m_transform);
			//GameManager.Instance.UpdatePlayerHooknum(id,Hookgunnum);
		}
	}
	
}
/*protected virtual void BallistaHit()
	{  
		hookRate -= Time.deltaTime;
		if (hookRate <= 0) {
			hookRate=0.5f;
	
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) && (m_Hooknum!=0)) {
				//Instantiate(m_Hook,m_transform.position,m_transform.rotation);
				GameObject	gb =Myutils.makeGameObject (m_Hook,m_transform);
				m_Hooknum--;
			}
			else if (m_Hooknum==0)
			{
				print ("Please order Hook!");
			}
		}
	}*/
/*	IEnumerator Ballistashoot()
		{   
			yield return new WaitForSeconds (1f);
			if (hookRatevalue <= 0) {
				hookRatevalue = hookRate;
				for (int i=0; i<Hookgunnum; i++) {
					hookgb =Instantiate (Hook, m_transform [i].position, m_transform [i].rotation) as GameObject;
					Hooknum--;
					Hookbasic hook=hookgb.GetComponent<HookwithRope04>().Hook.GetComponent<Hookbasic>();
					//标记弩箭赋值
					hook.id=id;
					GameManager.Instance.m_localmissleshootsum++;
					GameManager.Instance.AddShootNum ();
					GameManager.Instance.UpdatePlayerHooknum(id);
					GameManager.Instance.UpdateUIhooknum(id);
					//GameObject.Find ("Script").GetComponent<MainScript> ().Hitshoot = true;
					print ("zidanfashed"+"-----------------------------");
				}
				// 播放射击声音
				m_audio.PlayOneShot (m_shootClip);
				//GameObject gb = Myutils.makeGameObject (Hook, m_transform);
				//GameManager.Instance.UpdatePlayerHooknum(id,Hookgunnum);
			}
		}*/