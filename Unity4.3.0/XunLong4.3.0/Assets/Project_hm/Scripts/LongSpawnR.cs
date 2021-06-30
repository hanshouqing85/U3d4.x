using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/LongSpawnR")]
public class LongSpawnR : MonoBehaviour {
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
	public float m_timerValue;
	private static float  Stoptime=1000;
	private bool NoBoss=true;
	//生存时间
//	public float m_liveTime =15f;
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		Longnum =GameManager.Instance.Longnum;
	
	//	m_timerValue = m_liveTime / Longnum;
		//m_timer = m_timerValue;
	}

	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
	//	m_liveTime -= Time.deltaTime;
		Longnum =GameManager.Instance.Longnum;
		//print ("Longnum="+Longnum);
		if ((m_timer < 0) && (Longnum > 0) && NoBoss) {
			//	m_timer=Random.value*5.0f;
			if (m_timer < m_timerValue)
			m_timer = m_timerValue;
			Long01 = Instantiate (m_long [0], m_transform.position, Quaternion.identity) as GameObject;
			GameManager.Instance.Longnum--;
		} 
		
	
	}
	void OnDrawGizmos()
	{  
		Gizmos.DrawIcon (transform.position, "/biaoji/B.png", true);
	}

}
