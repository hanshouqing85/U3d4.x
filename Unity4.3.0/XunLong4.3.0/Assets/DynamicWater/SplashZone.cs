using System.Collections;
using UnityEngine;

[AddComponentMenu("Lost Polygon/Dynamic Water System/Splash Zone")]
[RequireComponent(typeof(BoxCollider))]
public class SplashZone : MonoBehaviour {
    public DynamicWater Water;
    public float DropsPerSecond = 10f;
    public float RadiusMin = 0.1f;
    public float RadiusMax = 0.2f;
    public float ForceMin = 0.3f;
    public float ForceMax = 0.8f;
    public bool AutoStart = true;

    private bool _isRaining;
    private BoxCollider _collider;

    public bool IsRaining {
        get { return _isRaining; }
        set {
            if (_isRaining != value) {
                _isRaining = value;

                if (_isRaining) {
                    StartRain();
                } else {
                    StopRain();
                }
            }
        }
    }

    private void StartRain() {
        if (Water == null)
            return;

        StopRain();
        StartCoroutine("DoMakeSplash");
        _isRaining = true;
    }

    private void StopRain() {
        if (Water == null)
            return;

        StopCoroutine("DoMakeSplash");
        _isRaining = false;
    }

	void Start () {
        if (Water == null) {
            Debug.LogError("Water field is not set, SplashZone disabled", this);
            enabled = false;
            return;
        }

        _collider = (BoxCollider) GetComponent<Collider>();
	    _collider.isTrigger = true;

        if (AutoStart)
            StartRain();
	}

    IEnumerator DoMakeSplash() {
        while (true) {
            if (Water == null)
                break;

            Vector3 point = new Vector3(Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
                                        Water.transform.position.y,
                                        Random.Range(_collider.bounds.min.z, _collider.bounds.max.z));

            Water.CreateSplash(point, Random.Range(RadiusMin, RadiusMax), Random.Range(ForceMin, ForceMax));

            yield return new WaitForSeconds(1f / Mathf.Clamp(DropsPerSecond, 0f, 100f));
        }
    }

    void OnDrawGizmos() {
        if (!Application.isEditor) {
            return;
        }

        Gizmos.DrawIcon(transform.position, "DynamicWater/SplashZone.png");

        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        if (GetComponent<Collider>() != null)
            Gizmos.DrawCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);
    }
}
