using UnityEngine;
using System.Collections;
using Bjoy;
[AddComponentMenu("testgame/PathGenerator")]
public class PathGenerator : MonoBehaviour {

	public bool pathVisible = true;
	//飞行时间
	public float m_flyTime= 15f;
	public int pointCount = 3;
	public float pathLength = 15;
	public float pointDeviation =1f;
	public float pointDeviationY = 0.3f;
	//public Vector3[] path = null;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	private Vector3 rootPosition;
	private LTSpline cr;
	public GameObject PathNode01;
	public bool debug=false;
	public bool debugluxian=false;
	public Color pathColor = Color.cyan;

	void Awake()
	{

		GenerateRandomPath ();
	}

	void OnEnable(){
		// create the path
		//	cr = new LTSpline( new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position,
		//	trans[5].position,trans[6].position} );

		cr = new LTSpline( path );
	}
	// Use this for initialization
	void Start () {
		if (debug) {

		} 
		else {
			LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
			//LeanTween.moveSpline (gameObject, cr.pts, m_flyTime);
		}
	}

	void GenerateRandomPath()
	{
		rootPosition = transform.position;
		path = new Vector3[pointCount+2];
		float pointGap = pathLength/pointCount;
		path[0]=rootPosition;
	    path[pointCount+1]=new Vector3(rootPosition.x+(pathLength+pointGap),rootPosition.y,rootPosition.z);
		for (int i = 1; i < pointCount+1; i++) {
			float randomZ = rootPosition.z + Random.Range(-pointDeviation,pointDeviation);
			float randomY = rootPosition.y + Random.Range(-pointDeviationY,pointDeviationY);
			float newX = rootPosition.x + (pointGap*i);
			path[i]=new Vector3(newX,randomY,randomZ);
     //    if (debugluxian) {
	//		Instantiate(PathNode01,path[i],Quaternion.identity);
		//	}
		}
	}

	void OnDrawGizmos()
	{ 
		if (debugluxian) {
			for (int i=1; i<path.Length-1; i++)
				Gizmos.DrawIcon (path [i], "pathNode.tif", true);
		}
	}
	void OnDrawGizmosSelected(){
		if(pathVisible){
			if(cr!=null)
				OnEnable();
			Gizmos.color = pathColor;
			if(cr!=null)
				cr.gizmoDraw(); // To Visualize the path, use this method
		}
	}
/*	void OnDrawGizmos(){
		// Debug.Log("drwaing");
		if(cr!=null)
			OnEnable();
		Gizmos.color = Color.red;
		if(cr!=null)
			cr.gizmoDraw(); // To Visualize the path, use this method
	}*/



}
