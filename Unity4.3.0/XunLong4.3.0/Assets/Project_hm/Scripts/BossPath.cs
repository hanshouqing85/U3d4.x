using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/BossPath")]
public class BossPath : MonoBehaviour {
	protected Transform m_transform;
	//飞行时间
	public float m_flyTime= 10f;
	//旋转速度
	public float m_rotSpeed=30;
	public float m_speed=1;
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	LeanTween.rotateAround (gameObject, Vector3.up, 360f, 7f).setOnCompleteOnStart(true).setRepeat(-1);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMove ();
	}
	protected  void UpdateMove()
	{
		//m_transform.Rotate (Vector3.up,m_rotSpeed*Time.deltaTime,Space.World);
	//m_transform.Translate (new Vector3(-m_speed*Time.deltaTime,0,0));
	}
}
