using UnityEngine;
using System.Collections;
using System.Collections.Generic;


enum JinengState{
	              ZUOKAN,
	              YOUKAN,
	            // shangpi
                         };

[AddComponentMenu("PocketRPG/Blade Master")]
public class xuanfengzhan : MonoBehaviour {

// This is the PocketRPG Blade Master class stripped down to just a bit of animation... it's just for an example of how to use the PocketRPG weapon trails.
// This code was written by Evan Greenwood (of Free Lives) and used in the game PocketRPG by Tasty Poison Games.
// But you can use this how you please... Make great games... that's what this is about.
// This code is provided as is. Please discuss this on the Unity Forums if you need help.


 
	//  ---------------------------------------------------------------  
	#region Inspector Assigned	
	//
	public WeaponTrail leftSwipe;
	public WeaponTrail rightSwipe;
	//     
	#endregion
	//  ---------------------------------------------------------------  
	#region Temporary

	protected float t = 0.033f;
	#endregion
	//
	#region Internal
	protected AnimationController animationController;
	#endregion
	//  ---------------------------------------------------------------  
	#region Logic
	//
	protected float thinkTime = 0.0f;
	protected int thinkState = 0;
	//
	#endregion
	//  ---------------------------------------------------------------  
	#region Animation
	//
	private AnimationState animationAttack1Anticipation;
	private AnimationState animationAttack1;
	private AnimationState animationAttack2Anticipation;
	private AnimationState animationAttack2;
	//private AnimationState animationAttack4Anticipation;
	//private AnimationState animationAttack4;
	
	private AnimationState animationAttack3;	
	private AnimationState animationWhirlwind;
	//
	private AnimationState animationIdle;
	//private AnimationState animationRespawn;
	//
	protected float timeScale = 1; // This is here for personal time distortion... like freeze spells that slow enemies... (changing this affects the animation rate)
	protected float facingAngle = 0;
	//
	#endregion
	//
	
  	bool i=false ;  
	bool j=false ;
    bool k=false;
	bool h=false ;
	
	bool m=false;
 
    float   currentTime=0;
    //float	nextCutTime=-0.5f;
   // float	reloadTime=2.8f;
	#region Initialisation
	//
	
	   JinengState  jinengstate=JinengState.ZUOKAN;
	protected void Awake ()
	{
		animationController = GetComponent<AnimationController> ();
		transform.position = Vector3.zero;
	}
	protected void Start ()
	{
		// 允许多点触屏
//		Input.multiTouchEnabled=true;
//		Input.multiTouchEnabled=false;
		
		animationController.AddTrail (leftSwipe); // Adds the trails to the animationController which will run them
		animationController.AddTrail (rightSwipe);
		//
		Initialise ();
		//
		// This is just making him jump at the start... normally you would just hit PlayAnimation(idle)...		
		//
		//thinkTime = 1.5f;
		//animationController.PlayAnimation (animationRespawn);
		//leftSwipe.SetTime (1.5f,0f, 1f);
		//rightSwipe.SetTime (1.5f, 0, 1);			
		//
	}

	protected void Initialise ()
	{
		// The Animation Controller feeds on AnimationStates. You've got to assign your animations to variables so that you can call them from the controller
		//    
		animation["Attack1"].speed = 2.0f;
		animation["Attack2"].speed = 2.0f;
		//animation["Attack4"].speed = 2.0f;
		//
		//
		animationAttack1Anticipation = animation["Attack1Anticipation"];
		animationAttack1 = animation["Attack1"];
		//
		animationAttack2Anticipation = animation["Attack2Anticipation"];
		animationAttack2 = animation["Attack2"];
		
	//	nimationAttack4Anticipation = animation["Attack4Anticipation"];
	//	animationAttack4 = animation["Attack4"];
		
		animationAttack3 = animation["WhirlwindAttack"];
		//		
		animationWhirlwind = animation["Whirlwind"];		
		animationIdle = animation["Idle"];
		
		animationIdle.speed = 0.4f;		
		//animationRespawn = animation["Resurection"];
		
		//animationRespawn.speed = 0.8f;
		//animationRespawn.speed = 3.8f;
		
		animationAttack3.speed = 0.8f;
		//	
		
		
		
		
		
		leftSwipe.SetTime (0.0f, 0, 1);
		rightSwipe.SetTime (0.0f, 0, 1);
		//		
	}
	//
	#endregion
	// ------------------------------------------------------------------------------------------------------------------- 
	
	
	void Func1()
	{
			if(k==false)
			{
				currentTime=Time.time;
			    k=true;
				j=true;
				h=false;
				//print ("k="+k);
				//print ("j="+j);
			}
			
			if(Time.time>currentTime+0.1f&&Time.time<currentTime+0.8f)
			    // if(thinkState>=3)
			{ j=true;
			  i=true;
			  h=true;
			  jinengstate=JinengState.YOUKAN;
			}
			
//			if(Time.time>currentTime+0.2f&&Time.time<currentTime+0.8f)
//			    // if(thinkState>=3)
//			{ j=true;
//			  i=true;
//			 // h=true;
//			  jinengstate=JinengState.shangpi;
//			}
			
			   if(Time.time>currentTime+0.8f&&h==false)
			{
				//print ("h="+h);
				j=true;
				jinengstate=JinengState.ZUOKAN;
				currentTime=Time.time;
			}
			
//			if(Time.time>currentTime+0.1f&&Time.time<currentTime+0.8f)
//			    // if(thinkState>=3)
//			{ j=true;
//			  i=true;
//			  //h=true;
//			  jinengstate=JinengState.YOUKAN;
//			}
		
	}
	
	void Func2()
	{
				if(j==true)
		{
			
			 
			// while(Time .time>nextCutTime+reloadTime)
			 // {
			
	 		switch(jinengstate)
	 	  {
		 	case    JinengState.ZUOKAN:
				
			      animationAttack1Anticipation.layer=2;
		          animation["Attack1"].layer = 2;
		          animationIdle.layer=2; 
		
		           animationAttack2Anticipation.layer=1;
		           animation["Attack2"].layer = 1;		
				 
				  // animationAttack4Anticipation.layer=1;
		          // animation["Attack4"].layer = 1;	
				
				  thinkTime -= t;
 		          if (thinkTime < 0) {
 			       switch (thinkState) {			
		    	   case 0:
				// START ATTACK 1
 				   animationController.CrossfadeAnimation (animationAttack1Anticipation, 0.1f);
 				   thinkState++;
 			       thinkTime = 0.1f;
					//	print (222); 
 				// facingAngle = -180 + Random.value * 360;			
				 break;
			    case 1:
					// animationController.CrossfadeAnimation (animationAttack1, 0.1f);	
 				   animationController.PlayAnimation (animationAttack1);
 				  thinkTime = 0.2f;
 				  thinkState++;
				  rightSwipe.StartTrail(0.3f, 0.0f); // Fades the trail in		
 				//rightSwipe.StartTrail(0.5f, 0.4f); // Fades the trail in				
//				facingAngle += -40 + Random.value * 80;			
				break;
			case 2:				
 				 thinkState++;
 			 	 thinkTime = 0.1f;
 		 	      rightSwipe.FadeOut(0.1f); // Fades the trail out
				break;
			case 3:
				// BACK TO IDLE
				
 				  animationController.CrossfadeAnimation (animationIdle, 0.2f);
					 
 		    	  thinkState++;
 				 thinkTime = 0.3f;	
 				 rightSwipe.ClearTrail(); // Forces the trail to clear			
				break;
			case 4:
				     j=false;	
					 i=true;			 
					jinengstate=JinengState.YOUKAN;
				break;	
				
		    	}
			}
	            // StartCoroutine("zuokan");
			     
	 		break;		
					
				case JinengState.YOUKAN:
				
				 animationAttack2Anticipation.layer=2;
		          animation["Attack2"].layer = 2;
		          animationIdle.layer=2; 
		
		           animationAttack1Anticipation.layer=1;
		           animation["Attack1"].layer = 1;		
				 
				  // animationAttack4Anticipation.layer=1;
		          // animation["Attack4"].layer = 1;	
				
				 
				    
				     thinkTime -= t;
 		             if (thinkTime < 0) {
 			 switch (thinkState) {					
			 case 0:
				// START ATTACK 2
 				 animationController.CrossfadeAnimation (animationAttack2Anticipation, 0.1f);
 				 thinkState++;
 				 thinkTime = 0.1f;
//				facingAngle = -180 + Random.value *360;
 		
				 break;
			case 1:
 				animationController.PlayAnimation (animationAttack2);
 				thinkState++;
 				thinkTime = 0.2f;
    			leftSwipe.StartTrail(0.3f, 0.0f);  // Fades the trail in				
//				facingAngle += -40 + Random.value * 80;
					//print (111);
				break;
 			case 2:				
 				thinkState++;
 				thinkTime = 0.1f;
 				leftSwipe.FadeOut(0.1f); // Fades the trail out
 				break;
 			case 3:
//				// BACK TO IDLE
					
					// animationIdle.layer=2;
 				animationController.CrossfadeAnimation (animationIdle, 0.2f);
 				thinkState++;
					 				 
 				thinkTime = 0.3f;
 				leftSwipe.ClearTrail(); // Forces the trail to clear			
 				break;
 			case 4:
				     j=false;	
					 i=true;
					 k=false ;	
				jinengstate=JinengState.ZUOKAN;
				break;
						
//				case JinengState.shangpi:
//				
//				 animationAttack4Anticipation.layer=2;
//		          animation["Attack4"].layer = 2;
//		          animationIdle.layer=2; 
//		
//		           animationAttack1Anticipation.layer=1;
//		           animation["Attack1"].layer = 1;		
//				 
//				   animationAttack2Anticipation.layer=1;
//		           animation["Attack2"].layer = 1;	
//				
//				    
//				     thinkTime -= t;
// 		             if (thinkTime < 0) {
// 			 switch (thinkState) {					
//			 case 0:
//				// START ATTACK 2
// 				 animationController.CrossfadeAnimation (animationAttack4Anticipation, 0.1f);
// 				 thinkState++;
// 				 thinkTime = 0.1f;
////				facingAngle = -180 + Random.value *360;
// 		
//				 break;
//			case 1:
// 				animationController.PlayAnimation (animationAttack4);
// 				thinkState++;
// 				thinkTime = 0.2f;
//    			leftSwipe.StartTrail(0.3f, 0.2f);  // Fades the trail in				
////				facingAngle += -40 + Random.value * 80;
//					//print (111);
//				break;
// 			case 2:				
// 				thinkState++;
// 				thinkTime = 0.3f;
// 				leftSwipe.FadeOut(0.2f); // Fades the trail out
// 				break;
// 			case 3:
////				// BACK TO IDLE
//					
//					// animationIdle.layer=2;
// 				animationController.CrossfadeAnimation (animationIdle, 0.1f);
// 				thinkState++;
//					 				 
// 				thinkTime = 0.1f;
// 				leftSwipe.ClearTrail(); // Forces the trail to clear			
// 				break;
// 			case 4:
//				     j=false;	
//					 i=true;
//					 k=false ;	
//				jinengstate=JinengState.ZUOKAN;
//				break;		
				}
			}
				
				// StartCoroutine("youkan");							
			break;
		    }			  
 				// nextCutTime=Time .time;

			//}
		 }
				
 			if(m==true)
		{
			      thinkTime -= t;
		        if (thinkTime < 0) {
		 	 switch (thinkState) {					
					case 0:
						// START THE SPIN ATTACK
						animationController.CrossfadeAnimation (animationAttack1Anticipation, 0.1f);
						thinkState++;
						thinkTime = 0.1f;
						//facingAngle = -180 + Random.value * 360;
					
						break;
					case 1:
						animationController.CrossfadeAnimation (animationWhirlwind, 0.2f);
						thinkState++;
						thinkTime = 0.8f;
						leftSwipe.StartTrail(0.9f, 1.4f); // Fades both trais in				
						rightSwipe.StartTrail(0.9f, 1.4f);					
						break;
					case 2:
						// Checking for a specific place in the animation from which to start the next animation
						//
						if (Mathf.Repeat(animationWhirlwind.normalizedTime, 1) < 0.93f + t*1f && Mathf.Repeat(animationWhirlwind.normalizedTime, 1) > 0.93f-t*1.2f){				
							animationController.CrossfadeAnimation (animationAttack3, 0.05f* animationWhirlwind.length);
							thinkState++;			
							thinkTime = 0.3f;
						}
						break;
					case 3:				
						thinkState++;
						leftSwipe.FadeOut(0.1f); // Fades both trails out			
						rightSwipe.FadeOut(0.1f);
						thinkTime = 0.3f;
						break;
					case 4:
						// BACK TO IDLE (for a second)
						animationController.CrossfadeAnimation (animationIdle, 0.2f);
						thinkState++;
						thinkTime =0.1f;
						leftSwipe.ClearTrail();  // Forces both trails to clear		
						rightSwipe.ClearTrail();		  
 						break;
					case 5:
				        m=false ;
						i=true;
				break;
 		 
 					}
 				}
		//
		// ** THIS IS A REALLY TERRIBLE EXAMPLE OF THE CHARACTER MOVING ... (It moves forward when executing a attack)
//	 	if (thinkState == 3  || thinkState == 7 ){
//			transform.position += transform.TransformDirection( Vector3.forward) * t*3;
//		} else if (thinkState == 11 || thinkState == 10){
//			transform.position += transform.TransformDirection( Vector3.forward) * t*0.5f;
//		}
		//
		// This rotates the character a based on "facingAngle". It's an experiment to show that the animationController works
		// transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle( transform.eulerAngles.y, facingAngle, 12 *t), transform.eulerAngles.z);
	//}
	//#endregion	
     // }
         }
		
		
			
		
		            if(i==true)
					{
						thinkState=0;
						i=false;
					}
	}
	
	//#region Update
	//
	protected void Update ()
	{
		
		
		 
	 
		
		t = Mathf.Clamp (Time.deltaTime * timeScale, 0, 0.066f);
		//
		animationController.SetDeltaTime (t); // Sets the delta time that the animationController uses.
		//
		//
		//  This is just some sample code to show you how you can use the animation controller component along with the trails 
		//
		
//		 thinkTime -= t;
//		 if (thinkTime < 0) {
//			switch (thinkState) {
			//case 0:
				//animationController.PlayAnimation (animationRespawn);
 			//	 animationController.CrossfadeAnimation (animationIdle, 0.2f);
 			//	 thinkState++;
 			//	thinkTime = 1.0f;
			//	break;
		
		// 1个手指触摸屏幕，开始触屏
        if(Input.GetKeyDown(KeyCode.Q))//||(Application.platform==RuntimePlatform.Android && Input.touchCount==1 && Input.GetTouch(0).phase==TouchPhase.Began))
		{

			Func1();
			
		}
		
		// 2个手指触摸屏幕，开始触屏
		if(Input.GetKeyDown(KeyCode.Q))//||(Application.platform!=RuntimePlatform.Android && Input.touchCount==2 && Input.GetTouch(0).phase==TouchPhase.Began))
 		{
			 m=true;
		}
//		m=false;
//		thinkState=0;
		Func2();

		
	}
 

						
	
//	IEnumerator  zuokan()
//	{
//		       
//		  yield return null ;
//	}
//	
//	   IEnumerator   youkan()
//	{                
//		             
//		 yield return null ;
//	}
	
}	
	