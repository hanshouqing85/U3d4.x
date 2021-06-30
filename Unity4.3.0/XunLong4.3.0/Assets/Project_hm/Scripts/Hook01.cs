using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/hook")]
public class Hook01 : MonoBehaviour {
	//弩钩飞行速度
	public float m_speed =2;
	//生存时间
	public float m_liveTime =10;
	//威力
	public float m_power=1.0f;
	protected Transform m_transform;
	
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		m_liveTime -= Time.deltaTime;
		if (m_liveTime <= 0)
			Destroy (this.gameObject);
		m_transform.Translate (new Vector3(0,0,m_speed*Time.deltaTime));
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo ("Long01") != 0)
			return;
		Destroy (this.gameObject);
	}
}
