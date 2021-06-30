using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/MainScript")]
public class MainScript : MonoBehaviour {
	public List<Transform> Path=new List<Transform>();
	public List <GameObject> Paolei = new List<GameObject>();
	public Transform[] PaoPos;
	//public List <Player> Player = new List<Player>();
	public Transform nowTransform;
	public bool Captured=false;
	public bool CapturedDes=false;
	//public int m_Hooknum;
	//public List <GameObject>  m_Hook = new List<GameObject>();
	public GameObject start;
	public GameObject End;
	public Transform HitMissle;
	public bool Hitshoot=false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	             
	}
}
