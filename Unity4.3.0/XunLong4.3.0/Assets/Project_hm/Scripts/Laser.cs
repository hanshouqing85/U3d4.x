using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/Laser")]
[RequireComponent (typeof (PerFrameRaycast))]
public class Laser : MonoBehaviour {

	public float scrollSpeed=0.5f;
	public float pulseSpeed=1.5f;

	public float noiseSize=1.0f;
	public float maxWidth=0.5f;
	public float  minWidth=0.2f;

	public GameObject pointer =null;
	private LineRenderer lRenderer;
	private float aniTime=0.0f;
	private float aniDir=1.0f;
	private PerFrameRaycast raycast;

	

	
	void Start() {
		lRenderer = gameObject.GetComponent (typeof(LineRenderer) ) as LineRenderer;	
		aniTime = 0.0f;
		// Change some animation values here and there
		ChoseNewAnimationTargetCoroutine();
		raycast = GetComponent(typeof (PerFrameRaycast))  as PerFrameRaycast;
	}
	
	IEnumerator  ChoseNewAnimationTargetCoroutine () {
		while (true) {
			//aniDir = aniDir * 0.9f + Random.Range (0.5f, 1.5f) * 0.1f;
			aniDir = aniDir * 0.3f + Random.Range (0.5f, 1.5f) * 0.1f;
			yield return 1 ;
			//minWidth = minWidth * 0.8f + Random.Range (0.1f, 1.0f) * 0.2f;
			minWidth = minWidth * 0.2f + Random.Range (0.1f, 1.0f) * 0.2f;
			yield return new WaitForSeconds (1.0f + Random.value * 2.0f - 1.0f);	
		}	
	}
	
	void Update () {
		GetComponent<Renderer>().material.mainTextureOffset +=new Vector2( Time.deltaTime * aniDir * scrollSpeed,0.0f);
		GetComponent<Renderer>().material.SetTextureOffset ("_NoiseTex",new Vector2 (-Time.time * aniDir * scrollSpeed, 0.0f));
		float aniFactor  = Mathf.PingPong (Time.time * pulseSpeed, 1.0f);
		aniFactor = Mathf.Max (minWidth, aniFactor) * maxWidth;
		lRenderer.SetWidth (aniFactor, aniFactor);
		lRenderer.SetPosition (0,this.gameObject.transform.position);
		if (raycast == null) {
			Debug.Log ("raycast is null");
			return;
		}
		// Cast a ray to find out the end point of the laser
		RaycastHit hitInfo  = raycast.GetHitInfo ();
		if (hitInfo.transform) {
			//lRenderer.SetPosition (1, (hitInfo.distance * Vector3.forward));
			lRenderer.SetPosition (1, hitInfo.point );
			//renderer.material.mainTextureScale.x = 0.1f * (hitInfo.distance);
			GetComponent<Renderer>().material.mainTextureScale= new Vector2(0.1f * (hitInfo.distance),GetComponent<Renderer>().material.mainTextureScale.y);
			GetComponent<Renderer>().material.SetTextureScale ("_NoiseTex", new Vector2 (0.1f * hitInfo.distance * noiseSize, noiseSize));		

			// Use point and normal to align a nice & rough hit plane
			if (pointer) {
				pointer.GetComponent<Renderer>().enabled = true;
				pointer.transform.position = hitInfo.point + (transform.position - hitInfo.point) * 0.01f;
				pointer.transform.rotation = Quaternion.LookRotation (hitInfo.normal, transform.up);
			//	pointer.transform.eulerAngles.x = 90.0f;
				pointer.transform.eulerAngles= new Vector3(90.0f,pointer.transform.eulerAngles.y,pointer.transform.eulerAngles.z);
			}
		}
		else {
			if (pointer)
			pointer.GetComponent<Renderer>().enabled = false;		
			 float maxDist  = 10.0f;
			//lRenderer.SetPosition (1, (maxDist * Vector3.forward));
			lRenderer.SetPosition (1, (maxDist * this.transform.forward));
			//	renderer.material.mainTextureScale.x = 0.1f * (maxDist);	
			GetComponent<Renderer>().material.mainTextureScale= new Vector2(0.1f * (maxDist),GetComponent<Renderer>().material.mainTextureScale.y);	
			GetComponent<Renderer>().material.SetTextureScale ("_NoiseTex", new Vector2 (0.1f * (maxDist) * noiseSize, noiseSize));		
		}
	}

}
