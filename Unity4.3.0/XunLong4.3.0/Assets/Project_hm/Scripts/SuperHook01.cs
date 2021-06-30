using UnityEngine;
using System.Collections;
[AddComponentMenu("game/SuperHook01")]
public class SuperHook01 : Hookbasic {

	// Use this for initialization
	void Start () {
		m_transform = this.transform;

	
	//	ArrowRate = GameObject.Find ("Script").GetComponent<Bjoy.Catchconfig> ().GetValue (key, Arrowtype);
		print ("ArrowRate="+ArrowRate);
	}
	

}
