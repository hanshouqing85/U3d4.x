using UnityEngine;
using System.Collections;
[AddComponentMenu("game/Hookbasic")]
public class Hookbasic : MonoBehaviour {
	//弩钩飞行速度
	public float m_speed =1;
	//生存时间
	public float HookliveTime =10;
	//威力
	public float m_power=1.0f;
	protected Transform m_transform;
	//public Transform HookPos;
	//Line生存时间
	public float Line_liveTime =10f;
	protected Transform ColliderPos;
	public bool Captured=false;
	public bool Bounded=false;
	public float ArrowRate;
	public float DeathRate=1f;
	public bool shoot=false;
	public int id=-1;
	public int CurrentIndex=-1;
	public int Arrowtype=101;
	public int key=1;
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		//gameObject.GetComponent<Collider>().isTrigger=false;
		ArrowRate = GameObject.Find ("Script").GetComponent<Bjoy.Catchconfig> ().GetValue(key,Arrowtype);
	}
	
	// Update is called once per frame
	void Update () {
		ArrowRate = GameObject.Find ("Script").GetComponent<Bjoy.Catchconfig> ().GetValue(key,Arrowtype);
		if (shoot) {
		//	gameObject.GetComponent<Collider>().isTrigger=true;
			gameObject.GetComponent<Collider>().enabled=true;
		HookliveTime -= Time.deltaTime;
		if ( HookliveTime <= 0) {
			Destroy (this.gameObject);
		}
			m_transform.Translate (new Vector3 (0, 0, -m_speed * Time.deltaTime));
			//test code
			m_speed -= 0.1f;
			//test code end
		}
	}


	void OnTriggerEnter(Collider other)
	{
		//if (other.tag.CompareTo ("Long01") == 0) {
			if (other.gameObject.CompareTag("Long01")) {
		//Captured = GameObject.Find ("Script").GetComponent<MainScript> ().Captured;
		//	print ("HookCaptured="+Captured);
		//	if (Captured) {
				//gameObject.GetComponent<Rigidbody>().isKinematic=false;
				//	GameObject gb = Myutils.makeGameObject(m_explosionFX,m_transform);
				//ColliderPos=GameObject.Find("Script").GetComponent<MainScript>().nowTransform;
				//	Destroy (this.gameObject);
				//	if (m_speed>0)
				//	{
				//			m_speed=-m_speed;
				//	}
			GameManager.Instance.HitMissle=this.transform;
			gameObject.GetComponent<MeshRenderer>().enabled=false;
			print ("hit");

        // GameObject.Find("Script").GetComponent<PaoPosManager>().CurrentBall.GetComponent<Ballistabasic>().Hookgun[0].GetComponent<Hookgun>().ShowLaserLine();
		//	GameObject.Find("Script").GetComponent<PaoPosManager>().BallistaOnPos[0]
		//	.GetComponent<Ballistabasic>().Hookgun[0].GetComponent<Hookgun>().ShowLaserLine();
				//Destroy (this.gameObject);
			//}

		} else {
			if (other.gameObject.CompareTag ("Bound")) {
				Bounded = true;
				//	if (m_speed>0)
				//	{
				//		m_speed=-m_speed;
				//		}
			//	gameObject.GetComponent<Rigidbody>().isKinematic=false;
				//gameObject.GetComponent<Rigidbody>().useGravity=true;
				m_speed=0;
				gameObject.GetComponent<Collider>().isTrigger=false;
				StartCoroutine(timeout());
				//Destroy (this.gameObject);
		//		print ("daobianle");
			}
			
		}
	}
	IEnumerator timeout()
	{
		yield return new WaitForSeconds(0.4f);
		Destroy (gameObject);
	}
}
