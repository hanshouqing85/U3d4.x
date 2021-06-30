using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/LongPathtest01")]
public class LongPathtest01 : MonoBehaviour {

	//生存时间
	public float m_liveTime =15f;
	//public Transform[] trans;
	private MainScript Main;
	//private List<Transform> Path=new List<Transform>();

	LTSpline cr;

	void Awake()
	{
		Main = GameObject.Find("Script").GetComponent<MainScript>();
		//Path = Main.Path;
	}
	void OnEnable(){
		// create the path
	//	cr = new LTSpline( new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position,
		//	trans[5].position,trans[6].position} );

 cr = new LTSpline( new Vector3[] {Main.Path[0].position, Main.Path[1].position, Main.Path[2].position,Main.Path[3].position, 
			Main.Path[4].position,Main.Path[5].position,Main.Path[6].position} );
	}
	// Use this for initialization
	void Start () {
	

		// Tween automatically
		//LeanTween.moveSpline(gameObject, cr.pts, 20f).setOrientToPath(true).setRepeat(-1);
		LeanTween.moveSpline(gameObject, cr.pts, 10f).setOrientToPath(true);
	}

	// Update is called once per frame
	void Update () {
		m_liveTime -= Time.deltaTime;
		if (m_liveTime <= 0)
			Destroy (this.gameObject);
	}
	
	void OnDrawGizmos(){
		// Debug.Log("drwaing");
		if(cr!=null)
			OnEnable();
		Gizmos.color = Color.red;
		if(cr!=null)
			cr.gizmoDraw(); // To Visualize the path, use this method
	}
}
