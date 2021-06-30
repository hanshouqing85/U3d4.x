using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/LongSpawnTime")]
public class LongSpawnTime : MonoBehaviour {
	//public List<Transform> m_long=new List<Transform>();
	public List<GameObject> m_long=new List<GameObject>();
	//刷新间隔时间
	private float m_timer  = 1.0f;
	public List<float> InterTime=new List<float>();
	protected Transform m_transform;
	private int Longnum;
	private int Longlivenum=0;
	public int index=0;

	public float m_timerValue;
	private static float  Stoptime=1000;
	private bool NoBoss=true;
	private bool CanSpawn=true;
	
	public float rX;
	public float rY;
	public float rZ;
	//public float DeviationX =6f;
	public float DeviationY = 2f;
	public float DeviationZ = 2f;
	public float[] raX=new float[]{-8.5f,8.5f};
	public Vector3 startPosition;
	private int NumL=2;
	private int NumR=2;
	private GameObject Longgb;

	public float currentTime;
	public List<GameObject> m_longsub=new List<GameObject>();

	public float turnTime;
	public bool StartSpawn=false;
	public int m_longindex;

	// Use this for initialization
	void Start () {
		Longnum =GameManager.Instance.Longnum;
		NumL = m_long.Count;
		turnTime = GameManager.Instance.turnTime;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		currentTime +=Time.deltaTime;
		Longnum =GameManager.Instance.Longnum;
		NoBoss = GameManager.Instance.NoBoss;
		CanSpawn = GameManager.Instance.CanSpawn;
		//currentTime =Mathf.CeilToInt(Time.fixedTime);
	
	


	//	if ((m_timer < 0) && (Longnum > 0) && NoBoss && CanSpawn) {
			if ((Longnum > 0) && NoBoss && CanSpawn) {
				//	m_timer=Random.value*5.0f;
			  //if (m_timer < m_timerValue)
		     //m_timer = m_timerValue;
		
				if (currentTime>InterTime[index])
				{
				StartSpawn=true;
			     }
			else StartSpawn=false;
			  if (StartSpawn)
			{ 
				//if (m_long.Count!=0)
				//	m_longindex= index%NumL;
			
				SpawnLong();

				print ("SpawnLong");
				if (index<InterTime.Count-1)
				 index++;
				else 
					index=0;
			}


		} 
		
	}

	void SpawnLong()
	{   

		m_longsub.Clear();


		m_longsub.Add(m_long [index]);

		Longgb = Instantiate (m_longsub [0], startPosition, Quaternion.identity) as GameObject;
		StartSpawn=false;
		GameManager.Instance.Longnum--;
	}
	
	void OnDrawGizmos()
	{  
		Gizmos.DrawIcon (transform.position, "/biaoji/A.png", true);
	}
	
}
