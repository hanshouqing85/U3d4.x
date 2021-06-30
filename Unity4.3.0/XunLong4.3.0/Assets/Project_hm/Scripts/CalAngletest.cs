using UnityEngine;
using System.Collections;
using Bjoy;
public class CalAngletest : MonoBehaviour {
	private float distance =1f;
	public Transform Cube; 
	private Quaternion r,r0,r1;
	private Vector3 f0, f1,f2;
	//旋转速度
	public float rotateSpeed = 50;
	// Use this for initialization
	void Start () {
		 r = transform.rotation;
		f0=(transform.position+(r*Vector3.forward)*distance);

		
		r0 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-80f,transform.rotation.eulerAngles.z);
		r1 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y+80f,transform.rotation.eulerAngles.z);
		f1 = (transform.position+(r0*Vector3.forward)*distance);
		f2 =(transform.position+(r1*Vector3.forward)*distance);

	}
	
	// Update is called once per frame
	void Update () {
	/*Quaternion r = transform.rotation;
		 f0=(transform.position+(r*Vector3.forward)*distance);
		Debug.DrawLine (transform.position,f0,Color.red);

		 r0 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-80f,transform.rotation.eulerAngles.z);
		 r1 = Quaternion.Euler (transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y+80f,transform.rotation.eulerAngles.z);
		 f1 = (transform.position+(r0*Vector3.forward)*distance);
		 f2 =(transform.position+(r1*Vector3.forward)*distance);

		Debug.DrawLine (transform.position,f1,Color.red);
		Debug.DrawLine (transform.position,f2,Color.red);
		Debug.DrawLine (f0,f1,Color.red);
		Debug.DrawLine (f0,f2,Color.red); */

		Debug.DrawLine (transform.position,f0,Color.red);
		Debug.DrawLine (transform.position,f1,Color.red);
		Debug.DrawLine (transform.position,f2,Color.red);
		Debug.DrawLine (f0,f1,Color.red);
		Debug.DrawLine (f0,f2,Color.red);

		//xia
		if (Input.GetKey (KeyCode.A)) {
			if (transform.rotation.y > r0.y)  {
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
				print ("rL="+transform.rotation);
			}
		}
		if (Input.GetKey (KeyCode.D)) {
			if (transform.rotation.y < r1.y)  {
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
				print ("rR="+transform.rotation);
		}
		}
		/*
     //shang
		if (Input.GetKey (KeyCode.A)) {
		if (transform.rotation.w < r0.w)  {
				this.transform.Rotate (Vector3.up * Time.deltaTime * -rotateSpeed);
			print ("rL="+transform.rotation);
			}
		}
		if (Input.GetKey (KeyCode.D)) {
			if (transform.rotation.w > r1.w)  {
				this.transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
			print ("rR="+transform.rotation);
			}
		}*/


		Vector3 point = Cube.position;

		if (isINTriangle (point, transform.position, f1, f0) || isINTriangle (point, transform.position, f2, f0)) {
			Debug.Log ("cube in this!!!");
		} else {
			Debug.Log ("cube not in this!!!");
		}

	}

	private float triangleArea(float v0x,float v0y,float v1x,float v1y,float v2x,float v2y)
	{
		return Mathf.Abs ((v0x*v1y+v1x*v2y +v2x*v0y-v1x*v0y-v2x*v1y-v0x*v2y)/2f);
	}
	bool isINTriangle(Vector3 point,Vector3 v0,Vector3 v1,Vector3 v2)
	{
		float x = point.x;
		float y = point.z;
		float v0x = v0.x;
		float v0y = v0.z;
		float v1x = v1.x;
		float v1y = v1.z;
		float v2x = v2.x;
		float v2y = v2.z;

		float t = triangleArea (v0x,v0y,v1x,v1y,v2x,v2y);
		float a = triangleArea (v0x, v0y, v1x, v1y, x, y) 
			+ triangleArea (v0x, v0y, x, y, v2x, v2y) + triangleArea (x,y,v1x,v1y,v2x,v2y);
		if (Mathf.Abs (t - a) <= 0.01f) {
			return true;
		} else {
			return false;
		}
	}
}
