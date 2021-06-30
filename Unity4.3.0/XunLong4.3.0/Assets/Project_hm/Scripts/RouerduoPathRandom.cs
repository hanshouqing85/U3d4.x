using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/RouerduoPathRandom")]
public class RouerduoPathRandom : MonoBehaviour {
	
	//飞行时间
	public float m_flyTime= 7f;
	public float start_timer =5f;
	public float staytime=2f;
	//生命
	//public float m_life=2;
	
	public int pointCount = 6;
	public float pathLength = 10;
	public float pointDeviationZ =1f;
	public float pointDeviationY = 1f;
	//public Vector3[] path = null;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	
	private Vector3 rootPosition;
	private LTSpline cr;
	public GameObject PathNode01;
	public bool debugluxian=false;
	protected Transform m_transform;
	public float red_rotSpeed=30;
	private float newX;
	void Awake()
	{   
		m_transform = this.transform;
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
		start_timer -= Time.deltaTime;
		if (start_timer <= 0) {
			LeanTween.pause (gameObject);
			//m_transform.Rotate(Vector3.up,red_rotSpeed*Time.deltaTime,Space.World);
			staytime-=Time.deltaTime;
			if(staytime<=0)
			{
				LeanTween.resume(gameObject);
			}
		}
	}
	void GenerateRandomPath(){
		rootPosition = transform.position;
		path = new Vector3[pointCount + 2];
		float pointGap = pathLength / pointCount ;
		path [0] = rootPosition;
		path [pointCount  + 1] = new Vector3 (rootPosition.x - (pathLength + pointGap), rootPosition.y, rootPosition.z);
		for (int i = 1; i < pointCount+1; i++) {
			float 	randomZ = rootPosition.z + Random.Range (-pointDeviationZ, pointDeviationZ);
			float	randomY = rootPosition.y + Random.Range (-pointDeviationY, pointDeviationY);
			if (SelectBoolByProb(0.5f))
			{
				newX = rootPosition.x - (pointGap * i);
			}
			else 
				newX=rootPosition.x+(pointGap*i);
			path [i] = new Vector3 (newX, randomY, randomZ);
			
			if (debugluxian) {
				Instantiate(PathNode01,path[i],Quaternion.identity);
			}
		}
		
	}
	
	public  bool SelectBoolByProb(float p)
	{
		if(p>=1)
			return true;
		if(p<=0)
			return false;
		int P=(int)(p*1000+0.5); 
		int randNum =Random.Range(0,1000);
		if(randNum<P)
			return true;
		return false;
	}
	
}
