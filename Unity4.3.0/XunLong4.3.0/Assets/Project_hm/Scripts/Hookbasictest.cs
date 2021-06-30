using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/Hookbasictest")]
public class Hookbasictest : MonoBehaviour {
	//弩钩飞行速度
		public float m_speed =1;
	//生存时间
	public float m_liveTime =10f;
	//威力
	public float m_power=1.0f;
	protected Transform m_transform;
	//public Transform HookPos;
	//Line生存时间
	public float Fly_liveTime =2f;
	protected Transform ColliderPos;
	public bool Captured=false;
	public bool Bounded=false;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		m_liveTime -= Time.deltaTime;
		Fly_liveTime -= Time.deltaTime;
		if (m_liveTime <= 0) {
			Destroy (this.gameObject);
	}
		if (Fly_liveTime>0)
		m_transform.Translate (new Vector3(0,0,-m_speed*Time.deltaTime));
}
}
