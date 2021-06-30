using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/PathNode")]
public class PathNode : MonoBehaviour {
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
		Gizmos.DrawIcon (transform.position,"PathNode.tif",true);
		
	}
}
