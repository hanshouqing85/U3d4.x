using UnityEngine;
using System.Collections;

public class _test : MonoBehaviour {
      public    WeaponTrail  trail;
	  public    AnimationState state;
	  public    AnimationController  ac;
	// Use this for initialization
	void Start () {
		
		ac=gameObject .GetComponent<AnimationController>();
		ac.AddTrail(trail);
		state =animation ["attack"];
	
	}
	
	// Update is called once per frame
	void Update () {
	  if(Input .GetKeyDown(KeyCode .A))
		{
			ac.PlayAnimation(state );
			trail.StartTrail(0.5f,0.4f);
		}
	}
}
