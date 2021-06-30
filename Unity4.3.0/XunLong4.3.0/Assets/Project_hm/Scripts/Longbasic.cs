using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/Longbasic")]
public class Longbasic : MonoBehaviour {
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
	public int key;
	private float CapturedRate;
	public Transform MisslefromBallistaPos;
	public GameObject wang01;
	protected Transform m_transform;
	protected GameObject wangtp;
	public bool Captured=false;
	public GameObject Long;
	//private float DistanceDestroy;
	// 声音
	public AudioClip m_capturedClip;
	// 声音源
	protected AudioSource m_audio;
	public int CurrentIndex=-1;
//	protected float Ratenum;
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
		type=GameObject.Find ("Script").GetComponent<Bjoy.Dragonconfig> ().m_data [key].type;

	}
	
	// Update is called once per frame
	void Update () {
	
		liveTime -= Time.deltaTime;
		if (liveTime <= 0) {
		
			if (type==1)
			{
				GameManager.Instance.Longnum++;
			print ("Longnum++01");
			}
			else if (type==2)
			{
				//GameManager.Instance.Bossnum++;
				GameManager.Instance.NoBoss=true;
				GameManager.Instance.CanBoss=true;
			}
			Destroy (this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Hook01") ) {
			Hookbasic hook = other.GetComponent<Hookbasic> ();
			if ((hook != null)&&(type==1))
			{
				//hook.ArrowRate  = GameObject.Find ("Script").GetComponent<Bjoy.Catchconfig> ().GetValue(key,101);
				hook.key=key;
				CapturedRate = hook.DeathRate * hook.ArrowRate / 100f;

				print ("CapturedRate1="+CapturedRate);
				if (SelectBoolByProb (CapturedRate)){
					Captured = true;
					LeanTween.cancel (gameObject);
					//GameManager.Instance.bous +=GameManager.N- m_score;
					GameManager.Instance.bous -= m_score;
				}
				// 播放 捕获声音
				m_audio.PlayOneShot (m_capturedClip);
				if (gameObject) {
					//标记弩箭归属
					int index = 0;
					index = hook.id;
					CurrentIndex=hook.CurrentIndex;
					//获取积分和经验
					GameManager.Instance.AddScore (m_score, index);
					GameManager.Instance.AddExp (exppoint, index);
					GameManager.Instance.UpdateUIscore (index);
					GameManager.Instance.UpdateUIscore1 (index,CurrentIndex);
					this.transform.parent = GameManager.Instance.HitMissle;
					//捕获则拉回
					if( (hook.m_speed >= 0.7f)&&(hook.m_speed <= 1.5f))
						hook.m_speed = -hook.m_speed;
					else
						hook.m_speed = -1;
					if (!wangtp) {
						wangtp = Instantiate (wang01, m_transform.position, Quaternion.identity) as GameObject;
						wangtp.transform.parent = this.transform;
						
						Destroy(gameObject.GetComponent<DW_WaterSplash>());
						Destroy(gameObject.GetComponent<BuoyancyForce>());
						Destroy(gameObject.GetComponent<Rigidbody>());
						//gameObject.GetComponent<Collider>().isTrigger=false;
						//Destroy(gameObject.GetComponent<Collider>());
						StartCoroutine (timeout ());
						print ("Longnomal is Captured");
				}
			}
			}
			else 
			if ((hook != null)&&(type==2) ){

				if (m_score <= GameManager.Instance.bous) {   

					hook.key=key;
					CapturedRate = hook.DeathRate * hook.ArrowRate / 100f;
					print ("CapturedRate2="+CapturedRate);
					//Ratenum=(int)100/CapturedRate;
					//if(Random.Range(1,6)%5!=0)   80%概率
					//if(Random.Range(1,Ratenum+1)%Ratenum==0)
					if (SelectBoolByProb (CapturedRate)) {
						Captured = true;
						LeanTween.cancel (gameObject);
						print ("BossCapture=" + Captured);
						if (GameManager.Instance.bous > GameManager.min) {
							GameManager.Instance.bous -= m_score;
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
			
							Destroy(gameObject.GetComponent<DW_WaterSplash>());
							Destroy(gameObject.GetComponent<BuoyancyForce>());
							Destroy(gameObject.GetComponent<Rigidbody>());
							//gameObject.GetComponent<Collider>().isTrigger=false;
							//Destroy(gameObject.GetComponent<Collider>());
							StartCoroutine (timeout ());
							print ("Boss is Captured");
							}
						}
					}
				}
			}
		} else if (other.CompareTag ("Bound") ) {
			if (type==1)
			{
			GameManager.Instance.Longnum++;
			print ("Longnum++02");
			}
			else if (type==2)
			{
				//GameManager.Instance.Bossnum++;
				GameManager.Instance.NoBoss=true;
				GameManager.Instance.CanBoss=true;
			}
			Destroy(gameObject);
		}
	}

	IEnumerator timeout()
	{
		yield return new WaitForSeconds(2f);
		if (type == 1) {
			GameManager.Instance.Longnum++;
			print ("Longnum++03");
		}
		else if (type == 2) {
			//GameManager.Instance.Bossnum++;
			GameManager.Instance.NoBoss=true;
			GameManager.Instance.CanBoss=true;
		}
		Destroy(gameObject);

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
