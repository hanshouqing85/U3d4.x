using UnityEngine;
using System.Collections;

public class MouseObrit_hm : MonoBehaviour {

	public Transform target;
	public Transform postarget;
	private float distance =10.0f;

	private float xSpeed = 250.0f;
	private float ySpeed = 120.0f;
	
	private float yMinLimit = -20;
	private float yMaxLimit = 80;

	private bool isSleep=false;
	private float x = 0;
	private float y = 0;
	
	private Vector3 oldTarget;
	private bool isStart=true; 
	private float _initX=0;
	private float _initY=0;
	 
	private bool _isSleep=false;
	private bool _isOne=true;
	private float _dx = 0;
	private float _dy = 0;
	private float _sdx = 0;
	private float _sdy = 0;
	private float _ease=0.05f;
	public bool _isAutoRota=false;
	private float _autoRota=0.1f;
	private float _autoRotaX=0.1f; 
	private bool _autoAdd=true; 
	private bool _autoMoveX=false; 
	private bool _autoMoveY=false;

	private bool _isMouseDown=false;      
	private Vector3 _oldPos=new Vector3(); 
	private Vector3 tempPos;
	private Quaternion tempRot;

	// Use this for initialization
	void Start () {

		distance=Vector3.Distance(postarget.position,target.position);
		//SendMessage("setDistance",distance);
		Vector3 angles = transform.eulerAngles;       
		x = angles.y;     
		y = angles.x;      		
		oldTarget=target.position;			
		
		// Make the rigid body not change rotation		
		if (GetComponent<Rigidbody>())	
			GetComponent<Rigidbody>().freezeRotation = true;	
		
		_isMouseDown=true;				
		isStart=false;  				
		_initX=Input.mousePosition.x;		
		_initY=Input.mousePosition.y;
		if (_isMouseDown) {  			
			//  if(Win7Input.GetTouch(0).phase==Win7TouchPhase.Moved) 
			if(!isSleep){	
				float dx= (Input.mousePosition.x-_initX) * xSpeed * 0.001f;	
				if(dx!=0){	
					_dx=dx;	
				}
				//x+=_dx;     				
				float dy= (Input.mousePosition.y-_initY) * ySpeed * 0.001f; 	
				if(dy!=0)	
					_dy=dy;		
				//y-=_dy; 		
				
				if(Mathf.Abs(_dx)>Mathf.Abs(_dy)){
					_dy=0;
				}else{
					_dx=0;   
				}
				if(Mathf.Abs(dx-_sdx)>0.1f){  		
					_sdx=_sdx+(dx-_sdx)*_ease;   	       	
				}				
				
				if(Mathf.Abs(dy-_sdy)>0.1f){  			
					_sdy=_sdy+(dy-_sdy)*_ease;   	       	
				}		
				y-=_sdy;     	
				x+=_sdx;         				
			}
			
			//x += Input.GetAxis("Mouse X") * xSpeed * 0.02;    		 	
			//y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;  		  	
			y = ClampAngle(y, yMinLimit, yMaxLimit);     
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

			transform.rotation = rotation;
			transform.position = position; 
			_oldPos=transform.position;
			oldTarget=target.position; 
			_initX=Input.mousePosition.x;		
			_initY=Input.mousePosition.y;	
		}
	
	}
	public void  UpdateDistance(){
		distance=Vector3.Distance(transform.position,target.position);			
		//print(distance);  		
	}

	// Update is called once per frame
	void Update () {
	
	}

	void  LateUpdate ()
	{
		if(distance<400){
			distance=400;
		}	
		if(distance>1615.409f){     		   
			distance=1615.409f;     				    				  
		}
		
		if(_isAutoRota){
			if(_autoMoveX&&_autoMoveY){
				if(_autoAdd){
					_autoRotaX+=0.006f;
					distance-=1.8f;
					_autoRota+=0.003f;
					if(_autoRotaX>1.5f){  			
						_autoAdd=false;
					}			
				}else{
					_autoRotaX-=0.002f;	
					_autoRota-=0.001f;			
					distance+=0.6f;  	   
					if(_autoRotaX<0.1f){	   
						_autoAdd=true;
					}
				}
			}
			if(!_autoMoveX){
				if(Mathf.Abs(distance-800)>10){ 
					if(distance>800){
						distance=distance-0.5f;
					}
					if(distance<800){
						distance=distance+0.5f;         
					}
				}else{_autoMoveX=true;}
			}
			if(!_autoMoveY){
				if(Mathf.Abs(y-25)>1){ 		
					
					if(y>25){
						y=y-0.1f;
					}
					if(y<25){
						y=y+0.1f;                         
					} 
				} else{_autoMoveY=true;}		
			}
			x+=_autoRota;		  
			//y-=_autoRota;   
			Quaternion rotation2 = Quaternion.Euler(y, x, 0);		  										
			//var rotation2 = Quaternion.Euler(y, x, 0);
			Vector3 position2 = rotation2 * new Vector3(0.0f, 0.0f, -distance) + target.position;


			transform.rotation = rotation2;							
			transform.position = position2; 
			_oldPos=transform.position;
			oldTarget=target.position; 
			_initX=Input.mousePosition.x;				
			_initY=Input.mousePosition.y;
		}

		
		if(_isSleep)
			return;
		Vector2 pos=Input.mousePosition;
		if(Input.GetMouseButtonDown(0)){		
			_isMouseDown=true;				
			isStart=false;  				
			_initX=Input.mousePosition.x;		
			_initY=Input.mousePosition.y;		
		}
		if(Input.GetMouseButtonUp(0)){		
			_isMouseDown=false;   		
			transform.position=_oldPos;  		 
		}                
		//if(Win7Input.touchCount==1){   
		if (_isMouseDown) {  			
			//  if(Win7Input.GetTouch(0).phase==Win7TouchPhase.Moved) 
			if(y <10)
			{
				y =10;
				return;
			}
			if(!isSleep){		
				float dx= (Input.mousePosition.x-_initX) * xSpeed * 0.001f;	
				if(dx!=0){	
					_dx=dx;	
				}
				//x+=_dx;     				
				float dy= (Input.mousePosition.y-_initY) * ySpeed * 0.001f; 	
				if(dy!=0)	
					_dy=dy;		
				//y-=_dy; 		
				
				if(Mathf.Abs(_dx)>Mathf.Abs(_dy)){
					_dy=0;
				}else{
					_dx=0;   
				}
				if(Mathf.Abs(dx-_sdx)>0.1f){  		
					_sdx=_sdx+(dx-_sdx)*_ease;   	       	
				}				
				
				if(Mathf.Abs(dy-_sdy)>0.1f){  			
					_sdy=_sdy+(dy-_sdy)*_ease;   	       	
				}		
				y-=_sdy;     	
				x+=_sdx;         				
			}
			
			//x += Input.GetAxis("Mouse X") * xSpeed * 0.02;    		 	
			//y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;  		  	
			y = ClampAngle(y, yMinLimit, yMaxLimit);     

			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;


			transform.rotation = rotation;
			transform.position = position; 
			_oldPos=transform.position;
			oldTarget=target.position; 
			_initX=Input.mousePosition.x;		
			_initY=Input.mousePosition.y;		
			//}          
		}else{			
			if(y <10)
			{
				y =10;
				return;
			}
			_sdx=0;			
			_sdy=0;					
			if(Mathf.Abs(_dx)>0.2||Mathf.Abs(_dy)>0.2){		
				_dx=_dx-_dx*_ease;			
				x+=_dx;		
				if(y>yMaxLimit||y<yMinLimit){		
					_dy=0;		
				}
				_dy=_dy-_dy*_ease;
				y-=_dy;		
				Quaternion rotation1 = Quaternion.Euler(y, x, 0);
				Vector3 position1 = rotation1 * new Vector3(0.0f, 0.0f, -distance) + target.position;


				transform.rotation = rotation1;
				transform.position = position1; 
				_oldPos=transform.position;
				oldTarget=target.position; 
				_initX=Input.mousePosition.x;				
				_initY=Input.mousePosition.y;
			}
		}				
		if(!_isMouseDown){     
			Vector3 addPos=target.position-oldTarget;              
			if(oldTarget!=target.position){   		
				float x=target.position.x-oldTarget.x;        		
				float y=target.position.y-oldTarget.y;       	
				float z=target.position.z-oldTarget.z;  		
				float ay=_oldPos.y+y;		
				//if(ay>100)		
				//	ay=100;			
				Vector3 pos1=new Vector3(_oldPos.x+x,ay,_oldPos.z+z);            
				_oldPos=pos1;                       
			}         
			//print(addPos);    
			transform.position+=addPos;     			      	    
			oldTarget=target.position; 				
		}	
		
	}


	public void cSetRota(float rota){		
		_autoRota=rota;	
	}

	public void cSetDistance(float dis){
		distance=dis;			
	}		
	public void jsSetAuto(bool isAuto){
		_isAutoRota=isAuto;		  
		if(!isAuto){
			_autoMoveX=false;		
			_autoMoveY=false;		
		}
	}
	public void jsSetAutoTwo(bool isAuto){
		_isSleep=true;	
		_isAutoRota=isAuto;		
		//SendMessage("jsCallsetAutoTwo",distance);
	}  
	
	public void jsSetSleepTwo(bool isSleep){
		_isSleep=isSleep;
	}	
	
	
	public void  jsSetSleep(bool isSleep){
		if(_isOne){
			var angles = transform.eulerAngles;       
			x = angles.y;     
			y = angles.x;      
			oldTarget=target.position;					
		}		
		isSleep=false;
		isSleep=false;			
		_isSleep=false;					
	}			
	public void jsSetDistance(float dis){			
		//if(Mathf.Abs(dis-distance)>10){			
		//	SendMessage("setDistance",distance);	   		
		//	return;    	
		//}				
		if(dis>0){
			distance=dis;					
			isSleep=true;		
		}			
	}
	public void jsSetActiveT(){
		_isSleep=false;
	}
	public void jsSetActiveTrue(){			
		_isSleep=true;		
	}
	public void jsSetActive(){			
		isSleep=false;			
	}

	public static  float ClampAngle ( float angle,float min,float max) {
		if (angle < -360)
			angle += 360;             
		if (angle > 360)      
			angle -= 360;      
		return Mathf.Clamp (angle, min, max);
	}

}
