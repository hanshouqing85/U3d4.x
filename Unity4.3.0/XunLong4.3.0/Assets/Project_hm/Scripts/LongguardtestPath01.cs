using UnityEngine;
using System.Collections;

public class LongguardtestPath01 : MonoBehaviour {
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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
