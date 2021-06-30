using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/PerFrameRaycast")]
public class PerFrameRaycast : MonoBehaviour {

    private    RaycastHit hitInfo;
	private Transform tr ;
	
	void Awake () {
		tr = transform;
	}
	
	void Update () {
		// Cast a ray to find out the end point of the laser
		hitInfo = new RaycastHit ();
		//Physics.Raycast (tr.position, tr.forward, out hitInfo, 10);
		Physics.Raycast (tr.position, tr.forward, out hitInfo, 10);
		//	if (Physics.Raycast (tr.position, tr.forward, out hitInfo, 1<<( LayerMask.NameToLayer("long")))) {
		//	print ("hitinfo long");}

		if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer ("Long")) {
			//GameObject.Find ("Script").GetComponent<PaoPosManager> ().BallistaOnPos [0]
		//	.GetComponent<Ballistabasic> ().Hookgun [0].GetComponent<Hookgun> ().ShowLaserLine ();
			print ("hitinfo long");
		} 
	//	else {
		//	GameObject.Find("Script").GetComponent<PaoPosManager>().BallistaOnPos[0]
		//	.GetComponent<Ballistabasic>().Hookgun[0].GetComponent<Hookgun>().NoShowLaserLine();
	//	}
		}

	
	public RaycastHit GetHitInfo ()  {
		return hitInfo;
	}
}
