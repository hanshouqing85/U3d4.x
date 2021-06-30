using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/LongSpawnNomal")]
public class LongSpawnNomal : MonoBehaviour {
	//public List<Transform> m_long=new List<Transform>();
	public List<GameObject> m_long=new List<GameObject>();
	public List<GameObject> m_longR=new List<GameObject>();
	public List<GameObject> SpLong=new List<GameObject>();
	public List<GameObject> Boss=new List<GameObject>();
	//刷新间隔时间
	private float m_timer  = 1.0f;
	private float m_timerboss  = 20f;
	//public List<float> InterTime=new List<float>();
	protected Transform m_transform;
	private int Longnum;
	private int Bossnum;
	private int Longlivenum=0;
	public int index=0;
//	private GameObject  Long01;
	public float m_timerValue=1f;
	public float m_timerbossValue=20f;
	private static float  Stoptime=1000f;
	private bool NoBoss=true;
	private bool CanSpawn=true;
	private bool CanBoss=true;

	public float rX;
	public float rY;
	public float rZ;
	//public float DeviationX =6f;
	public float DeviationY = 2f;
	public float DeviationZ = 2f;
	public float[] raX=new float[]{-8.5f,8.5f};
	public Vector3 startPosition;
	public Vector3 BossPosition;
	private int NumL=2;
	private int NumR=2;
	private int NumBoss=1;
	private int NumSpLong=1;

	private GameObject Longgb;
	private GameObject Longgbtp;
	private GameObject LongBossgb;
	private GameObject LongBossgbtp;
	private GameObject SpLonggb;
	private GameObject SpLonggbtp;
	private GameObject LastLong;
	private GameObject NewLong;
	// Use this for initialization
	void Start () {
		Longnum =GameManager.Instance.Longnum;
		Bossnum = GameManager.Instance.Bossnum;
		NumL = m_long.Count;
		NumR = m_longR.Count;
		NumSpLong = SpLong.Count;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		m_timerboss -= Time.deltaTime;
		Longnum =GameManager.Instance.Longnum;
		Bossnum = GameManager.Instance.Bossnum;
		NoBoss = GameManager.Instance.NoBoss;
		CanSpawn = GameManager.Instance.CanSpawn;
		CanBoss = GameManager.Instance.CanBoss;

		if ((m_timer < 0) && (Longnum > 0) && NoBoss && CanSpawn) {
			//	m_timer=Random.value*5.0f;
			if (m_timer < m_timerValue)
				m_timer = m_timerValue;
			rX = raX [Random.Range (0, 2)];
			//rY=Random.Range(-DeviationY ,DeviationY );
			rZ = Random.Range (-DeviationZ, DeviationZ);
			//rZ =Random.Range(-1 ,-4);
			//	startPosition = new Vector3(rX,rY,rZ);
			startPosition = new Vector3 (rX, DeviationY, rZ);

			if (rX < 0) {	
				Longgbtp = m_long [Random.Range (0, NumL)];
				Longgb = Instantiate (Longgbtp, startPosition, Quaternion.identity) as GameObject;

			} else if (rX > 0) {
				Longgbtp = m_longR [Random.Range (0, NumR)];
				Longgb = Instantiate (Longgbtp, startPosition, Quaternion.identity) as GameObject;
			}
			GameManager.Instance.Longnum--;

			if ((m_timerboss < 0)) {
				if (m_timerboss < m_timerbossValue)
					m_timerboss = m_timerbossValue;
				SpLonggbtp = SpLong [Random.Range (0, NumSpLong)];
				SpLonggb = Instantiate (SpLonggbtp, startPosition, Quaternion.identity) as GameObject;
				print ("SpLong");
			//	GameManager.Instance.Longnum--;
			}
		} else if (!NoBoss) {
			GeneratorBoss();
		}
	}


	public void GeneratorBoss()
	{
		   LongBossgbtp=Boss[0];
	       if (!LongBossgb) {
			LongBossgb = Instantiate (LongBossgbtp, BossPosition, Quaternion.identity) as GameObject;
			//GameManager.Instance.Bossnum--;
			//GameManager.Instance.NoBoss=true;
		}
			
		}



	IEnumerator Bosstimeout()
	{
		yield return new WaitForSeconds (1f);
		GameManager.Instance.CanBoss=false;
	}

	void OnDrawGizmos()
	{  
		Gizmos.DrawIcon (transform.position, "/biaoji/A.png", true);
	}

}
