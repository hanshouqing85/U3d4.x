using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/Longguardtest01")]
public class Longguardtest01 : MonoBehaviour {
	protected Transform m_transform;
	//飞行时间
	public float m_flyTime= 30f;
	//旋转速度
	public float m_rotSpeed=30;
	public float m_speed=1;

	public int pointCount = 3;
	public float pathLength = 10;
	public float pointDeviation =1f;
	public float pointDeviationY = 0.3f;
	//public Vector3[] path = null;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	private Vector3 rootPosition;
	private LTSpline cr;
	public GameObject PathNode01;
	public GameObject prefabAvatar;
	public float R=1.0f;
	public int N=2;
	public bool debugluxian=false;
	void Awake()
	{
		GeneratePath ();
	}

	void OnEnable(){
		// create the path
		//	cr = new LTSpline( new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position,
		//	trans[5].position,trans[6].position} );
		
		cr = new LTSpline( path );
	}
	// Use this for initialization
	void Start () {
		m_transform = this.transform;

	//	LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
		LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
		//	LeanTween.rotateAround (gameObject, Vector3.up, 360f, 4f).setOnCompleteOnStart(true).setRepeat(-1);
		//LeanTween.rotateAround( gameObject, Vector3.forward, 360f, 5f);
	}
	


	//圆形路线
	void GeneratePath(){
		rootPosition = transform.position;
		path = new Vector3[N*pointCount+3];
		float pointGap = pathLength/pointCount;
		path[0]=rootPosition;
		float period1 = LeanTween.tau/( pointCount);
		//float newZ = rootPosition.z+R*Mathf.Sin(period1) ;
		//float newX = rootPosition.x + R*Mathf.Cos(period1) ;
		//path[pointCount+1]=new Vector3(rootPosition.x ,rootPosition.y,rootPosition.z);
		path[N*pointCount+2]=new Vector3(rootPosition.x ,rootPosition.y,rootPosition.z);
	//	float red   = Mathf.Sin(period + LeanTween.tau*0f/3f) * 0.5f + 0.5f;
		for (int i = 1; i <N*pointCount+2; i++) {
			//正负控制顺逆时针方向负为顺，正为逆 LeanTween.tau=2pi
			float period =- LeanTween.tau/( pointCount)*i;
			float newZ = rootPosition.z+R*Mathf.Sin(period) ;
			float newY = rootPosition.y ;
			float newX = rootPosition.x + R*Mathf.Cos(period) ;
			path[i]=new Vector3(newX,newY,newZ);
			if (debugluxian) {
				Instantiate(PathNode01,path[i],Quaternion.identity);
			}
		}
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
