
using UnityEngine;

/// <summary>
/// Automates drawing ripples with mouse or touch.
/// </summary>
class DW_MouseSplash : MonoBehaviour {

    public DynamicWater Water = null;
    public float SplashForce = 10f;
    public float SplashRadius = 0.25f;

    private Vector3 prevPoint;
    private RaycastHit hitInfo;

    // Updating the splash generation
    void FixedUpdate() {
        if (Water == null)
            return;

        // Creating a ray from camera to world
        Ray ray;
        if (DW_GUILayout.IsRuntimePlatformMobile()) {
            ray = Input.touchCount == 0
                      ? Camera.main.ScreenPointToRay(Vector2.zero)
                      : Camera.main.ScreenPointToRay(Input.touches[0].position);
        } else {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        // Checking for collision
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity,
                        1 << LayerMask.NameToLayer(DynamicWater.PlaneColliderLayerName));

        // Creating a splash line between previous position and current
        if (GUIUtility.hotControl == 0 && (Input.GetMouseButton(0) || Input.touchCount > 0)) {
            if (hitInfo.transform != null && Water != null && prevPoint != Vector3.zero) {
                Water.CreateSplash(prevPoint, hitInfo.point, SplashRadius, -SplashForce * Time.deltaTime);
            }
        }

        prevPoint = hitInfo.transform != null ? hitInfo.point : Vector3.zero;
    }

}

