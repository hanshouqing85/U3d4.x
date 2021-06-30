﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/LongqunSpawn")]
public class LongqunSpawn : MonoBehaviour {
	public List<Transform> m_long=new List<Transform>();
	public float m_timer  = 3;
	public float m_distance =1;
	protected Transform m_transform;
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		m_timer -= Time.deltaTime;
		if (m_timer <= 0) {
			m_timer=Random.value*5.0f;
			if (m_timer<3)
				m_timer =3;
			Instantiate(m_long[0],m_transform.position,Quaternion.identity);
			//Instantiate(m_long[0],m_transform.position+new Vector3(m_distance*0.5f,m_distance*0.85f,0),Quaternion.identity);
			//Instantiate(m_long[0],m_transform.position+new Vector3(-m_distance*0.5f,m_distance*0.85f,0),Quaternion.identity);
	}
}
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position,"/biaoji/C.png",true);		
	}

}
