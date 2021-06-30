using UnityEngine;
using System.Collections;

public class HuochonglongPath : MonoBehaviour {
	//飞行时间
	public float m_flyTime= 10f;
	//生命
	//public float m_life=2;
	
	public int pointCount = 2;
	public float pathLength = 10;
	public float pointDeviationZ =0.5f;
	public float pointDeviationY = 0.3f;
	//public Vector3[] path = null;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	private Vector3 rootPosition;
	private Vector3 startPosition;
	private LTSpline cr;
	public GameObject PathNode01;

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
		LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GenerateRandomPath(){
		rootPosition = transform.position;
		path = new Vector3[pointCount+2];
		float pointGap = pathLength/pointCount;
		startPosition.y =rootPosition.y + Random.Range (-pointDeviationY,pointDeviationY);
		startPosition.z = rootPosition.z + Random.Range (-pointDeviationZ,pointDeviationZ);
		path[0]=startPosition;
		path[pointCount+1]=new Vector3(rootPosition.x+(pathLength+pointGap),rootPosition.y,rootPosition.z);
		for (int i = 1; i < pointCount+1; i++) {
			//float randomZ = rootPosition.z + Random.Range(-pointDeviation,pointDeviation);
			//float randomY = rootPosition.y + Random.Range(-pointDeviationY,pointDeviationY);

			float newX = rootPosition.x + (pointGap*i);
			float newY = startPosition.y;
			float newZ = 	startPosition.z;
			path[i]=new Vector3(newX,newY,newZ);
			Instantiate(PathNode01,path[i],Quaternion.identity);
		}
	}
}
