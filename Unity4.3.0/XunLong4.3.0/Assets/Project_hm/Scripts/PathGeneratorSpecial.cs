using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/PathGeneratorSpecial")]

public class PathGeneratorSpecial : MonoBehaviour {
	//生存时间
	public float m_liveTime =15f;
	//public Transform[] trans;
	private MainScript Main;
	//private List<Transform> Path=new List<Transform>();
	//飞行时间
	public float m_flyTime= 15f;

	//public GameObject Path;
	public List<GameObject> Path=new List<GameObject>();
	private int Num;
	private LTSpline cr;
	private PathEditor_hm PathScript;
	public GameObject Pathtp;
	void Awake()
	{
		Num = Path.Count;
		//cr = Path.GetComponent<PathEditor_hm> ().cr;
		Pathtp = Path [Random.Range (0, Num)];

		PathScript = Pathtp.GetComponent<PathEditor_hm> ();
		cr=PathScript.cr;
		//cr=Path[Random.Range(0,Num)].GetComponent<PathEditor_hm> ().cr;
	
	}
	void OnEnable(){
		// create the path
		//	cr = new LTSpline( new Vector3[] {trans[0].position, trans[1].position, trans[2].position, trans[3].position, trans[4].position,
		//	trans[5].position,trans[6].position} );
	   // PathEditor_hm.GetPath("test");
	}

	// Use this for initialization
	void Start () {
		m_flyTime = PathScript.flyTime;
		 //Tween automatically
		//LeanTween.moveSpline(gameObject, cr.pts, 20f).setOrientToPath(true).setRepeat(-1);
		LeanTween.moveSpline (gameObject, cr.pts, m_flyTime).setOrientToPath (true);
	}
	
	// Update is called once per frame
	void Update () 
	   {
		m_liveTime -= Time.deltaTime;
		if (m_liveTime <= 0)
		Destroy (this.gameObject);
	   }
}
