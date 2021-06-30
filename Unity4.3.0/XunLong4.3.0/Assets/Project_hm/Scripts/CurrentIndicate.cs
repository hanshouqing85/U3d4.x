using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;

[AddComponentMenu("game/CurrentIndicate")]
public class CurrentIndicate : MonoBehaviour {
	public List <GameObject> cardIndicatePos = new List<GameObject>();
	protected Transform m_transform;
	private int index=0;
	public int CurrentIndex,NextIndex,PreIndex;
	void Awake()
	{	
		//m_transform.position =  GetComponent<RectTransform>().position;
		cardIndicatePos=GameObject.Find("Script").GetComponent<PaoPosManager>().cardIndicatePos;
	}
	// Use this for initialization
	void Start () {
		CurrentIndex = GameObject.Find ("Script").GetComponent<PaoPosManager> ().CurrentIndex;
	}
	
	// Update is called once per frame
	void Update () {

	//	CurrentIndex=index%8;
	//	if (index >= 8)
	//		index = 0;

		// 左边手把 左键
		if (Input.GetAxis ("Horizontal") == -1) {
			if (CurrentIndex==0)
				PreIndex=7;
			else 
				PreIndex=CurrentIndex-1;

			CurrentIndex=PreIndex;
		

			print ("PreIndexPos1");
		}
		// 左边手把 右键
		if (Input.GetAxis ("Horizontal") == 1) {
			if (CurrentIndex==7)
				NextIndex=0;
			else 
				NextIndex=CurrentIndex+1;

			CurrentIndex=NextIndex;

			print ("NextIndexPos1");

		}

	}
}
