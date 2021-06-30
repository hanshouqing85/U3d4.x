using UnityEngine;
using System.Collections;

public class EaseMove : MonoBehaviour {
	
	private float maxY;
	private float minY;
	public float m_timer=1f;
	public float addY=1f;
	void Start () 
	{
	//	maxY=transform.position.y+Random.value*0.5f;			
	//	minX=transform.position.y+Random.value*-0.5f;
	//	addY=addY*Random.value*0.1f;	
	}
	
	void Update () {
		m_timer-= Time.deltaTime;
		if (m_timer <= 0) {
			m_timer = 1f;
			addY*=-1f;
	//	if(transform.position.y>maxY)
//			addY*=-1f;
	//	if(transform.position.y<minY)
	//		addY*=-1f;
//	transform.position+=new Vector3(0,addY,0);
		LeanTween.moveY (gameObject, gameObject.transform.position.y + addY, 1f);
	
	}
  }
}