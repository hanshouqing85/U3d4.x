using UnityEngine;
using System.Collections;
[AddComponentMenu("game/PathGeneratorAccelerationR")]
public class PathGeneratorAccelerationR : MonoBehaviour 
   {
	//飞行时间
	public float m_flyTime= 15f;
	public int pointCount = 3;
	public float pathLength = 15;
	public float pointDeviationZ =1f;
	public float pointDeviationY = 0.3f;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	private Vector3 rootPosition;
	private LTSpline cr;
	//public GameObject PathNode01;
	public bool debug=false;
	public bool debugluxian=false;
	public float pointOffset = 1f;
	public bool pathVisible = true;
	public Color pathColor = Color.cyan;

	void Awake()
	{
		GenerateRandomPathAcceleration ();
	}
	void OnEnable(){
	
		cr = new LTSpline( path );
	}
	// Use this for initialization
	void Start () {
		if (debug) {
		
		} 
		else {
			LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
		}
	}
	
	void GenerateRandomPathAcceleration()
	{
		rootPosition = transform.position;
		path = new Vector3[pointCount+2];
		float pointGap = pathLength / pointCount;
		path[0]=rootPosition;
		path[pointCount+1]=new Vector3(rootPosition.x-(pathLength+pointGap),rootPosition.y,rootPosition.z);
		for (int i = 1; i < pointCount+1; i++) {
			float randomZ = rootPosition.z + Random.Range(-pointDeviationZ,pointDeviationZ);
			float randomY = rootPosition.y + Random.Range(-pointDeviationY,pointDeviationY);
			float newX = rootPosition.x - (pointGap*i)-Random.Range (-pointOffset,pointOffset);
			path[i]=new Vector3(newX,randomY,randomZ);
		
		}
	}

	void OnDrawGizmos()
	{ 
		if (debugluxian) {
			for (int i=1; i<path.Length-1; i++)
				Gizmos.DrawIcon (path [i], "pathNode.tif", true);
		}
		if(pathVisible){
			if(cr!=null)
				OnEnable();
			Gizmos.color = pathColor;
			if(cr!=null)
				cr.gizmoDraw(); // To Visualize the path, use this method
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
}