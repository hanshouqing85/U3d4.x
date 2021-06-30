using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/PathNode02")]
public class PathNode02 : MonoBehaviour {
	//生存时间
	public float m_liveTime =1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_liveTime -= Time.deltaTime;
		if (m_liveTime <= 0)
			Destroy (this.gameObject);
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position,"Node.tif",true);
		
	}
}
