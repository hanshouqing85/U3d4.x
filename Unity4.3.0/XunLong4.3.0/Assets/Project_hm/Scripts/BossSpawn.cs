using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/BossSpawn")]
public class BossSpawn : MonoBehaviour {
//	public List<GameObject> Longguard1=new List<GameObject>();
//	public List<GameObject> Longguard2=new List<GameObject>();
	public List<GameObject> LongBoss=new List<GameObject>();
	protected Transform m_transform;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void BossUpdate()
	{
		   Instantiate(LongBoss[0],m_transform.position,Quaternion.identity) ;
	}
	void OnDrawGizmos()
	{  
		Gizmos.DrawIcon (transform.position, "/biaoji/G.png", true);
		//		Gizmos.DrawIcon (transform.position, "/biaoji/B.png", true);
	}
}
