using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/Hookgun")]
public class Hookgun : MonoBehaviour {
//	public float hookRate =0.2f;
	//public GameObject Hook;
	//protected int m_Hooknum;
	protected Transform m_transform;
	public GameObject HookgunParent;
	private Ballistabasic BallistaScript;
	private int id;
	public GameObject LaserLine;
	// 声音
//	public AudioClip m_shootClip;
	
	// 声音源
//	protected AudioSource m_audio;

	// Use this for initialization
	void Start () {
		//openFire ();
		m_transform = this.transform;
	//	m_audio = this.audio;
		//m_Hooknum = this.gameObject.GetComponentInParent<BallistaAdd> ().Hooknum;
		BallistaScript = HookgunParent.GetComponent<Ballistabasic> ();
		id = BallistaScript.id;
	}
	
	// Update is called once per frame
	void Update () {
	//	BallistaScript.OnlyRope.GetComponent<OnlyRope>().Rope.GetComponent<UltimateRope>().RopeSegmentSides;
	//	HookgunParent.GetComponent<Ballistabasic> ().BallistaHit();
		BallistaScript.BallistaHit(id);
	//	BallistaScript.OnlyRope.GetComponent<OnlyRope>().UpdateRopeMove();
	}

	public void ShowLaserLine()
	{
	// LaserLine =	Instantiate(LaserLine,m_transform.position,m_transform.rotation) as GameObject;
	//	LaserLine.SetActive (true);
	}
	public void NoShowLaserLine()
	{
		// Alpha Out, and destroy
	//	LeanTween.alpha(LaserLine, 0f, 0.6f).setDelay(1f).setDestroyOnComplete(true).setOnComplete(
	//		()=>{
	//		Destroy(LaserLine); // destroying parent as well
	//	}
	//	);
		//LaserLine.SetActive (false);
	}




/*	public void fire(){
		GameObject gb = Myutils.makeGameObject (Hook,transform);

	}
	public void openFire(){
		InvokeRepeating ("fire",1,gunrate);
	}
*/

/*	void BallistaHit(){
		hookRate -= Time.deltaTime;
		if (hookRate <= 0) {
			hookRate = 1f;
		
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) && (m_Hooknum != 0)) {
				Instantiate(Hook,m_transform.position,m_transform.rotation);

				// 播放射击声音
				m_audio.PlayOneShot(m_shootClip);
				//GameObject gb = Myutils.makeGameObject (Hook, m_transform);
				m_Hooknum--;
			} else if (m_Hooknum == 0) {
				print ("Please order Hook!");
			}
		}
	}
	*/
}
