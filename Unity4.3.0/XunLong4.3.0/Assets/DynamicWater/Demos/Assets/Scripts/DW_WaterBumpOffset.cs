using UnityEngine;

/// <summary>
/// Sets up transformation matrices to scale and scroll water waves.
/// </summary>
public class DW_WaterBumpOffset : MonoBehaviour {
    public Vector2 waveSpeed;
    public float waveScale;

    private float t;
    private Material mat;
    private Vector2 offsetClamped;
    private Vector3 scale;
    private Matrix4x4 scrollMatrix;

    void Start() {
        mat = GetComponent<Renderer>().sharedMaterial;
    }

	void Update()
	{
        t = Time.time / 20.0f;
		
		Vector4 offset4 = waveSpeed * (t * waveScale);

        offsetClamped.x = Mathf.Repeat(offset4.x, 1.0f);
        offsetClamped.y = Mathf.Repeat(offset4.y, 1.0f);
		//mat.SetVector( "_WaveOffset", offsetClamped );
        mat.SetTextureOffset("_BumpMap", offsetClamped);

	    scale.x = 1.0f/waveScale;
	    scale.y = 1.0f/waveScale;
	    scale.z = 1f;
	}
}
