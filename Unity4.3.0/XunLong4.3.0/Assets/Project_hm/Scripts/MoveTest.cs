using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour {
	public  GameObject m_transform;
	// Use this for initialization
	void Start () {
	//	m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//m_transform.Translate (new Vector3 (0, 0, -1 * Time.deltaTime));
		//LeanTween.move( gameObject, m_transform.position , 2f).setEase(LeanTweenType.easeInQuad);
	//	LeanTween.moveX(gameObject, m_transform.transform.position.x , 2f).setEase(LeanTweenType.easeOutBounce);
	//	LeanTween.moveY( gameObject, m_transform.transform.position.y , 1f).setEase(LeanTweenType.easeInOutQuad).setOnCompleteOnStart(true).setRepeat(-1);
		//LeanTween.rotateAround (gameObject, Vector3.up, 360f, 7f).setOnCompleteOnStart(true).setRepeat(-1);
		LeanTween.moveY( gameObject, gameObject.transform.position.y + 1f, 1f).setEase(LeanTweenType.easeInBack).setRepeat(-1);
	}
}
