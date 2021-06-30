using UnityEngine;
using System.Collections;

public class SuperBallista01 : Ballistabasic {
	protected Transform m_transform2;

	// Use this for initialization
	void Start () {
	//	m_transform = this.transform;

	}
	
	// Update is called once per frame
	void Update () {
		//BallistaHit ();
	}
/*	protected override void BallistaHit ()
	{
		//base.BallistaHit ();
		hookRate -= Time.deltaTime;
		if (hookRate <= 0) {
			hookRate=0.2f;
			
			//空格或左键发射弩钩
			if ((Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) && (m_Hooknum!=0)) {
				//Instantiate(m_Hook,m_transform.position,m_transform.rotation);
				GameObject	gb =Myutils.makeGameObject (m_Hook,m_transform);
				m_transform2.localEulerAngles = new Vector3 (0,30,0);
				//m_transform2.position =m_transform.position+new Vector3(1,0,0);

				Instantiate(m_Hook,m_transform.position,m_transform2.rotation);
				//GameObject	gb1=Myutils.makeGameObject (m_Hook,m_transform2);

				//gb1 = Instantiate (Tip, newPos, luxian [i].rotation) as GameObject;


				m_Hooknum--;
			}
			else if (m_Hooknum==0)
			{
				print ("Please order Hook!");
			}
		}
	}*/
}
