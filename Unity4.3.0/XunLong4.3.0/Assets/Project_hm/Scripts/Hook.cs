using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("testgame/hook")]
public class Hook : MonoBehaviour {
	//弩钩飞行速度
	public float m_speed =3;
	//生存时间
	public float m_liveTime =10;
	//威力
	public float m_power=1.0f;
	protected Transform m_transform;
	public GameObject m_explosionFX;
	public GameObject Linemesh;
	protected GameObject Linegb;
	//Line生存时间
	public float Line_liveTime =10f;
	private List<GameObject> Line=new List<GameObject>();
	protected Transform ColliderPos;
	public bool Captured=false;
	public bool Bounded=false;
	private int k=0;
	public float delayTime=-1f;



	// Use this for initialization
	void Start () {
		m_transform = this.transform;


		//弹簧运动
	//	LeanTween.move( this.gameObject, this.gameObject.transform.position + new Vector3(0f, 0f, 5f), 4f).setEase(LeanTweenType.easeInQuad).setOnCompleteOnStart(true).setRepeat(-1);

	//	LeanTween.move( this.gameObject, this.gameObject.transform.position + new Vector3(3f, 0f, 5f), 4f).setEase(LeanTweenType.easeOutQuad);
		//LeanTween.moveZ( this.gameObject, this.gameObject.transform.position.z +5f, 4f).setEase(LeanTweenType.easeOutQuad);
	}
	
	// Update is called once per frame
	void Update () {
		m_liveTime -= Time.deltaTime;
		delayTime -= Time.deltaTime;
		if (m_liveTime <= 0) {
			Destroy (this.gameObject);
			//ClearPrevious ();
		}

	
		//匀速运动
		//	m_transform.Translate (new Vector3(0,0,-m_speed*Time.deltaTime));
		m_transform.Translate (new Vector3(0,0,-m_speed*Time.deltaTime));
		m_speed-=0.01f;
        if (!Captured && !Bounded) {
		//	StartCoroutine (timeout ());

		} else {
			//ClearPrevious2 ();
		}
	}

	IEnumerator timeout()
	{     
		Linegb = (GameObject)Instantiate (Linemesh, m_transform.position, m_transform.rotation);
		Linegb.SetActive (false);
		Line.Add (Linegb);
		yield return new WaitForSeconds (1f);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo ("Long01") == 0)
		{
		Captured = GameObject.Find("Script").GetComponent<MainScript>().Captured;
		if (Captured){
				for (int i=Line.Count-1;i>0;i--) 
				{
				Line[i].SetActive (true);
				}
	//	GameObject gb = Myutils.makeGameObject(m_explosionFX,m_transform);
	    //ColliderPos=GameObject.Find("Script").GetComponent<MainScript>().nowTransform;
		Destroy (this.gameObject);
				if (m_speed>0)
				{
					m_speed=-m_speed;
				}

				//Destroy (this.gameObject);
			
			}

		}
	else 
		if (other.tag.CompareTo ("Bound") == 0)
		{
			Bounded=true;
			if (m_speed>0)
			{
				m_speed=-m_speed;
			}
			print ("LineCount="+Line.Count);
			ClearPrevious ();
		Destroy (this.gameObject);

			print ("daobianle");
		}
		Bounded = false;
	}

	IEnumerator timeout1()
	{
		print ("delay");
		yield return new WaitForSeconds (1f);
		
	}

	public void ClearPrevious () {
		for (int i=0;i<Line.Count;i++) {
			Destroy (Line[i]);
		}
		Line.Clear ();
	}

	public void ClearPrevious2 () {
		for (int i=Line.Count-1;i>0;i--) {
			StartCoroutine (timeout1());
			Destroy (Line[i]);
		}
		Line.Clear ();
	}



/*void OnClollisionEnter(Collision collision)
	{   
		//hitPos = true;
		if (!this.hingeJoint) {
			print ("aaaaaa");
			HingeJoint hingej=new HingeJoint();
			if(collision.rigidbody)
			{
				this.gameObject.AddComponent(typeof(HingeJoint));
				this.hingeJoint.connectedBody=collision.rigidbody;
				print ("hhhhhh");
			}
		}
		
	}*/


}
