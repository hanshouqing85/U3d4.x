using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/rouerduolongPath")]
public class ruoerduolongPath : MonoBehaviour {
	//生存时间
	//	public float m_liveTime =1;
	//飞行时间
	public float m_flyTime= 7f;
	public float m_flyTime1= 3f;

	public float m_timer =2f;
//	public float m_timer_second =2f;
	//生命
	//public float m_life=2;
	
//	public int pointCount = 3;
	public int pathnum = 2;
	//private List<float> pointGap=new List<float>();

	public List<int> pointCount = new List<int> ();
	//public float pathLength = 5;
	public List<float> pathLength=new List<float>();
	public float pointDeviationZ =0.5f;
	public float pointDeviationY = 0.3f;
	//public Vector3[] path = null;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	public Vector3[] path1={new Vector3(0,0,0),new Vector3(2,2,2)};

	private Vector3 rootPosition;
	private LTSpline cr;
	private LTSpline cr1;
	public float staytime=2f;
	//private List< LTSpline> cr=new  List< LTSpline>() ;
	//public List<Transform> m_long=new List<Transform>();
	//private LTSpline cr1;
	//private LTSpline cr2;
	public GameObject PathNode01;
	public GameObject PathNode02;
	private float randomY=0.000000f;
	private float randomZ=0.000000f;
	private float newX=0.000000f;
	private float randomZ1=0.000000f;
	private float randomY1=0.000000f;
	private float newX1=0.000000f;

	public bool debug=false;

	void Awake()
	{
		GenerateRandomPath ();
	}
	
	void OnEnable(){
		// create the path
		//	cr = new LTSpline( new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position,
		//	trans[5].position,trans[6].position} );
		cr = new LTSpline( path );
		//cr = new  List< LTSpline>(path);
		cr1 = new LTSpline (path1);

	}

	// Use this for initialization
	void Start () {
		LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);

	//	StartCoroutine(timeout());
		//LeanTween.moveSpline (gameObject, cr1.pts, m_flyTime).setOrientToPath (true);
		//LeanTween.delayedCall(5, loopPause);
		//LeanTween.delayedCall(8, loopResume);
	}

	void Update()
	{ 
		m_timer -= Time.deltaTime;
	   if (m_timer <= 0) {
			LeanTween.pause (gameObject);
			staytime-=Time.deltaTime;
			if(staytime<=0)
			{
				LeanTween.resume(gameObject);
			}
		}
	}


	IEnumerator timeout()
	{
	   yield return new WaitForSeconds(m_flyTime+staytime);
		//LeanTween.rotateAround (gameObject, Vector3.up, 360f, m_flyTime);
		LeanTween.move (gameObject, path1[0], 1f);
		// transform.position = Vector3.Lerp(path[pointCount[0]], path1[0], 3f);
		StartCoroutine(timeout1());
		//  LeanTween.moveSpline (gameObject, cr1.pts, m_flyTime).setOrientToPath (true);
	}
	IEnumerator timeout1()
	{
		yield return new WaitForSeconds(2f);
		//LeanTween.rotateAround (gameObject, Vector3.up, 360f, m_flyTime);

	
	
		 LeanTween.moveSpline (gameObject, cr1.pts, m_flyTime).setOrientToPath (true);
	}

	void GenerateRandomPath(){
		rootPosition = transform.position;
			path = new Vector3[pointCount [0] + 2];
			//float pointGap = pathLength / pointCount ;
			float pointGap = pathLength[0] / pointCount [0];
			path [0] = rootPosition;
			path [pointCount [0] + 1] = new Vector3 (rootPosition.x + (pathLength[0] + pointGap), rootPosition.y, rootPosition.z);
			for (int i = 1; i < pointCount[0]+1; i++) {
				 randomZ = rootPosition.z + Random.Range (-pointDeviationZ, pointDeviationZ);
				 randomY = rootPosition.y + Random.Range (-pointDeviationY, pointDeviationY);
				 newX = rootPosition.x + (pointGap * i);
				path [i] = new Vector3 (newX, randomY, randomZ);
				Instantiate (PathNode01, path [i], Quaternion.identity);
			}

		Debug.Log ("path["+pointCount[0]+"]="+path[pointCount[0]].ToString("f4"));

		path1 = new Vector3[pointCount [1] + 2];
		float pointGap1 = pathLength [1] / pointCount [1];
		path1[0]=path[pointCount[0]];

		Debug.Log ("path1[0]="+path1[0].ToString("f4"));

		path1 [pointCount [1] + 1] = new Vector3 (path[pointCount[0]].x+(pathLength[1]+pointGap1),path[pointCount[0]].y,path[pointCount[0]].z);
		for (int i=1; i<pointCount[1]+1; i++) {
			 randomZ1=path[pointCount[0]].z+Random.Range (-pointDeviationZ, pointDeviationZ);
			 randomY1=path[pointCount[0]].y+Random.Range (-pointDeviationY, pointDeviationY);
			 newX1 = path[pointCount[0]].x + (pointGap1 * (i-1));
			path1 [i] = new Vector3 (newX1, randomY1, randomZ1);
			Instantiate (PathNode02, path1 [i], Quaternion.identity);
		}
	}
}
