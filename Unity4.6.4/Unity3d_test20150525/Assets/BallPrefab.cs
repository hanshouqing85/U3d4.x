using UnityEngine;
using System.Collections;

public class BallPrefab : MonoBehaviour {

	public int i;
	public int j;
	public Rigidbody BallPrefabs;
	public float x, y, z;
	public float k;
	public int n;
	
	public int count = 0;
	
	private Rigidbody[] BP;
	
	// Use this for initialization
	void Start () {
		BP=new Rigidbody[10];
		count = 0;
		for (i = 0; i <= n; ++i)
		{
			for (int j = 0; j < i; ++j)
			{
				BP[count++] = (Rigidbody)Instantiate(BallPrefabs, new Vector3(x - 2.0F * k * i + 4 * j * k, 2.0F, z - 2 * 1.75F * k * i), BallPrefabs.rotation);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
