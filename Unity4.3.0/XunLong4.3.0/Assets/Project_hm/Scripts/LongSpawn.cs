using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/LongSpawn")]
public class LongSpawn : MonoBehaviour 
 {
//	public Transform[] m_long;
	public List<Transform> m_long=new List<Transform>();
	//public List<GameObject> Longlive=new List<GameObject>();
	//刷新间隔时间
	public float m_timer  = 1.0f;
	protected Transform m_transform;
	private int Longnum;
	private int Longlivenum=0;
	public int index=0;
	private GameObject  Long01;
	private float m_timerValue;
	private static float  Stoptime=1000;
	private bool NoBoss=true;
	//生存时间
	public float m_liveTime =15f;
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		Longnum = GameObject.Find ("Script").GetComponent<GameManager>().Longnum;
	   //Longlive = GameObject.Find ("Script").GetComponent<GameManager> ().Longlive;
		m_timerValue = m_liveTime / Longnum;
		//m_timer = m_timerValue;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		m_liveTime -= Time.deltaTime;
		 //  GameObject[] gbs= GameObject.FindGameObjectsWithTag("Long01");
	    //	foreach (GameObject Long01 in gbs) {
		//	Longlive.Add (Long01);
	  //	}
	//	Longlivenum = GameObject.Find ("Script").GetComponent<GameManager> ().Longlive.Count;
		//Longlivenum = Longlive.Count;
		//print ("Longlivenum="+Longlivenum);
		if ((m_timer <= 0)&&(Longlivenum<Longnum)&&NoBoss) {
		//	m_timer=Random.value*5.0f;
			if (m_timer<m_timerValue)
				m_timer =m_timerValue;
	 Long01	=   Instantiate(m_long[0],m_transform.position,Quaternion.identity) as GameObject ;
	// GameObject.Find ("Script").GetComponent<GameManager> ().Longlive.Add(Long01);
		}
		if (m_liveTime <= 0) {
			m_liveTime=15f;
		//	GameObject.Find ("Script").GetComponent<GameManager> ().Longlive.Clear();
			print ("Remove");
		}
	}
	void OnDrawGizmos()
	{  
			Gizmos.DrawIcon (transform.position, "/biaoji/A.png", true);
	}
}
