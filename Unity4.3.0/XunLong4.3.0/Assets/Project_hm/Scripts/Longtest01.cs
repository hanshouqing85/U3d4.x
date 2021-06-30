using UnityEngine;
using System.Collections;
[AddComponentMenu("testgame/Longtest01")]
public class Longtest01 : MonoBehaviour {
	//public GameObject gb;
	//生命
	public float m_life=2;
	//速度
	public float m_speed=1;
	//旋转速度
	public float m_rotSpeed=30;
	//变向间隔
	public float m_timer=3.0f;
	public float m_rotStep=10;
	public float m_rotStepX = 0.00f,m_rotStepY = 0.00f,m_rotStepZ = 0.00f;
	public float m_StepX = 0.00f,m_StepY = 0.00f,m_StepZ = 0.00f;

	protected Transform m_transform;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		//LeanTween.rotateAround( gameObject, Vector3.forward, 360f, 5f);
		//LeanTween.rotateAround (gameObject, Vector3.up, 360f, 7f).setOnCo	mpleteOnStart(true).setRepeat(-1);
		//LeanTween.move (gameObject, gameObject.transform.position + new Vector3 (m_StepX, m_StepY, m_StepZ), 3f).setEase (LeanTweenType.linear).setOnCompleteOnStart(true).setRepeat(-1);
		//LeanTween.move (gameObject, gameObject.transform.position + new Vector3 (-29f, 0f, 1f), 7f).setEase (LeanTweenType.linear).setOnCompleteOnStart(true).setRepeat(-1);
		//LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInBack).setOnCompleteOnStart(true).setRepeat(-1);
		//LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutQuad).setOnCompleteOnStart(true).setRepeat(-1);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuad).setOnCompleteOnStart(true).setRepeat(-1);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutQuad);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInCubic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutCubic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutCubic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuart);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutQuart);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutQuart);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuint);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutQuint);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutQuint);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInSine);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutSine);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutSine);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInExpo);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutExpo);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutExpo);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInCirc);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutCirc);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutCirc);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInBounce);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutBounce);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutBounce);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInBack);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutBack);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutBack);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInElastic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInOutElastic);
		//	LeanTween.move( gameObject, gameObject.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.punch);
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMove ();
	}
	protected virtual void UpdateMove()
	{
	//	m_timer -= Time.deltaTime;
		//if (m_timer <= 0) {
			//m_timer = 3;
			//改变方向
		//	m_rotSpeed=-m_rotSpeed;
		//	m_StepY=-m_StepY;
		//	m_StepZ=-m_StepZ;
		//}

		//旋转方向
		//m_transform.Rotate (Vector3.up,m_rotSpeed*Time.deltaTime,Space.World);
		//m_transform.Rotate (Vector3.up,m_rotStepY,Space.World);

		//m_transform.Rotate (Vector3.left,m_rotSpeed*Time.deltaTime,Space.World);
		//m_transform.Rotate (Vector3.right,m_rotSpeed*Time.deltaTime,Space.World);
		//运动
		//m_transform.Translate (new Vector3(-m_speed*Time.deltaTime,0,0));
		//m_transform.Translate (new Vector3(m_StepX,m_StepY,m_StepZ));







	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo ("Hook01") == 0) {
			print ("hit");
			print ("life="+m_life);

			Hook hook = other.GetComponent<Hook>();
			if (hook!=null)
			{
				m_life-=hook.m_power;
			  if (m_life<=0)
				{   if(gameObject){
					Destroy(this.gameObject);
					}
				}
			}
		}
	}

}
