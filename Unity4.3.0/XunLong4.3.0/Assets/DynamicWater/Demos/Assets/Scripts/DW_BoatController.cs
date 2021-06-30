using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DW_BoatController : MonoBehaviour {

    public float MovementSpeed = 1400f;
    public float RotationSpeed = 20f;
    public float CenterOfMassY = -1f;

	void Update () {
        // The center of mass must be low to make boat stable
        GetComponent<Rigidbody>().centerOfMass = new Vector3(GetComponent<Rigidbody>().centerOfMass.x, CenterOfMassY, GetComponent<Rigidbody>().centerOfMass.z);

	    Vector3 dir = Vector3.zero;
        if (DW_GUILayout.IsRuntimePlatformMobile()) {
            dir.x = Mathf.Clamp(-Input.acceleration.y * 2f, -1f, 1f);
            dir.z = -Input.acceleration.x;

            Debug.Log(dir);
        } else {
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.z = Input.GetAxisRaw("Vertical");
        }
        
        // Move backward at 1/3 speed
        float speed = dir.z > 0f ? dir.z : -0.33f;
        // Apply movement
        GetComponent<Rigidbody>().AddForce(new Vector3(transform.forward.x, 0f, transform.forward.z) * speed * MovementSpeed * Time.deltaTime, ForceMode.Acceleration);
        // Apply rotatiom
        GetComponent<Rigidbody>().AddTorque(0f, dir.x * RotationSpeed * Time.deltaTime, 0f, ForceMode.Acceleration);
	}
}
