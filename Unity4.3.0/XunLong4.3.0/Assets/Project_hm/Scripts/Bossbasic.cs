using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/Bossbasic")]
public class Bossbasic : MonoBehaviour {
	//生存时间
	public float liveTime =30f;
	//生命
	public float life=1;
	//积分值
	public int m_score = 10;
	//经验值
	public int exppoint=1;
	public int length=100;
	public int loot;
	public int type;
	private float CapturedRate;
	public Transform MisslefromBallistaPos;
	public GameObject wang01;
	protected Transform m_transform;
	protected GameObject wangtp;
	public bool Captured=false;
 //public GameObject Long;

	// 声音
	public AudioClip m_capturedClip;
	// 声音源
	protected AudioSource m_audio;
	public float f=0;

	//protected float Ratenum;
	//private bool CapturedDes=false;
	//public Transform nowTransform;
	//protected float LongtoPaodis;
	//private List<Vector3> Line=new List<Vector3>();
	//protected int Linecount;
	//public float LineStep=0.5f;
	//public GameObject Linemesh;
	//protected Vector3 startPos;
	//public Material lineMat;
	//public float lineWidth=1f;
	//List<GameObject> lastRender = new List<GameObject>();
	
	// Use this for initialization
	void  Start () {
		m_transform = this.transform;
		m_audio = this.GetComponent<AudioSource>();
		m_score=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [1].score;
		exppoint=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [1].exp;
		length=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [1].length;
		loot=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [1].loot;
		type=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [1].type;
		
	}
	
	// Update is called once per frame
	void Update () {
		liveTime -= Time.deltaTime;
		if (liveTime <= 0) {
			GameManager.Instance.Longnum++;
			Destroy (this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Hook01") ) {
			
			Hookbasic hook = other.GetComponent<Hookbasic> ();
			if (hook != null) {
				if (m_score <= GameManager.Instance.bous) {   
					CapturedRate = hook.DeathRate * hook.ArrowRate / 100f;
					print ("CapturedRate=" + CapturedRate);
					//Ratenum=(int)100/CapturedRate;
					//if(Random.Range(1,6)%5!=0)   80%概率
					//if(Random.Range(1,Ratenum+1)%Ratenum==0)
					if (SelectBoolByProb (CapturedRate)) {
						Captured = true;
						//	GameObject.Find ("Script").GetComponent<MainScript> ().Captured = Captured;
						//	GameManager.Instance.Captured = Captured;
						
						LeanTween.cancel (gameObject);
						print ("hitLong1Capture=" + Captured);
						if (GameManager.Instance.bous > GameManager.min) {
							GameManager.Instance.bous +=GameManager.N- m_score;
						}
						// 播放 捕获声音
						m_audio.PlayOneShot (m_capturedClip);
						if (gameObject) {
							//捕获则换运动学
							//	other.GetComponent<Rigidbody>().isKinematic=false;
							//标记弩箭归属
							int index = 0;
							index = hook.id;
							//获取积分和经验
							GameManager.Instance.AddScore (m_score, index);
							GameManager.Instance.AddExp (exppoint, index);
							GameManager.Instance.UpdateUIscore (index);
							//Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);
							
							this.transform.parent = GameManager.Instance.HitMissle;
							//捕获则拉回
							if( (hook.m_speed >= 1)&&(hook.m_speed <= 2))
								hook.m_speed = -hook.m_speed;
							else
								hook.m_speed = -1;
							if (!wangtp) {
								wangtp = Instantiate (wang01, m_transform.position, Quaternion.identity) as GameObject;
								wangtp.transform.parent = this.transform;
								//this.transform.parent=GameObject.Find("Script").GetComponent<MainScript>().HitMissle;
								//GameObject.Find("Script").GetComponent<MainScript>().HitMissle.parent=this.transform;
								//nowTransform= this.transform;
								//	GameObject.Find("Script").GetComponent<MainScript>().nowTransform = this.transform;
								//m_transform.Translate(nowTransform.position);
								//GameObject.Find ("Script").GetComponent<InterScript> ().LaserLine[i].SetActive();
								//MisslefromBallistaPos = GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentPos;
								//LeanTween.move( gameObject,  MisslefromBallistaPos.position, 3f);
								//gameObject.collider.isTrigger=false;
								
								Destroy(gameObject.GetComponent<DW_WaterSplash>());
								Destroy(gameObject.GetComponent<BuoyancyForce>());
								Destroy(gameObject.GetComponent<Rigidbody>());
								//gameObject.GetComponent<Collider>().isTrigger=false;
								//Destroy(gameObject.GetComponent<Collider>());
								StartCoroutine (timeout ());
								print ("Long is Captured");
							}
						}
					}
				}
			}
		} else if (other.CompareTag ("Bound") ) {
			GameManager.Instance.Longnum++;
			Destroy(gameObject);
		}
	}
	
	IEnumerator timeout()
	{
		yield return new WaitForSeconds(2f);
		//GameObject.Find ("Script").GetComponent<GameManager> ().Longlive.Remove (gameObject);
		//	GameObject.Find("Script").GetComponent<PaoPosManager>().BallistaOnPos[0]
		//	.GetComponent<Ballistabasic>().Hookgun[0].GetComponent<Hookgun>().NoShowLaserLine();
		GameManager.Instance.Longnum++;
		Destroy(this.gameObject);
		
	}
	
	public  bool SelectBoolByProb(float p)
	{
		if(p>=1)
			return true;
		if(p<=0)
			return false;
		//	Debug.Log ("px="+p);
		int P=(int)(p*1000+0.5); 
		//	Debug.Log ("P="+P);
		//	int randNum = GetRand(0,1000);
		int randNum =Random.Range(0,1000);
		//print ("randNum="+randNum );
		if(randNum<P)
			return true;
		return false;
	}
	
}