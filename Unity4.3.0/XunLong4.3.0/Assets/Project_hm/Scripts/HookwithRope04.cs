using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/HookwithRope04")]
public class HookwithRope04 : MonoBehaviour {
	protected Transform m_transform;
	//public GameObject m_explosionFX;
	public Transform HookPos;
	//Line生存时间
	public float Line_liveTime =3f;
	private List<GameObject> Line=new List<GameObject>();
	protected Transform ColliderPos;

	public bool Bounded=false;
	//public UltimateRope Rope;
	//public GameObject Hookpos;
	public Hookbasic Hook;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Line_liveTime -= Time.deltaTime;

	

		
	

		if (Line_liveTime <= 0) {
			Destroy(gameObject);
		}
	
	}


	
}
