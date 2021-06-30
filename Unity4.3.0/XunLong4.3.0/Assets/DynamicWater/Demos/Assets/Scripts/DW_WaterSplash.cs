using UnityEngine;
using System.Collections;

/// <summary>
/// Creates a ParticleSystem when enters or exits the water.
/// </summary>
[RequireComponent(typeof(BuoyancyForce))]
public class DW_WaterSplash : MonoBehaviour {
    /// <summary>
    /// The ParticleSystem prefab that will be instantiated on impact
    /// </summary>
    public ParticleSystem SplashPrefab;

    /// <summary>
    /// Minimum rigibody.velocity.y value to instantiate splash.
    /// </summary>
    public float SplashThreshold = 3.1f;
    private DynamicWater _water;

    /// <summary>
    /// Called when BuoyantObject enters the water.
    /// </summary>
    /// <param name="eventWater">
    /// The FluidVolume which the object has entered.
    /// </param>
    public void OnFluidVolumeEnter(FluidVolume eventWater) {
        _water = eventWater as DynamicWater;
        if (_water == null)
            return;

        if (_water.PlaneCollider != null)
            MakeSplash(SplashPrefab, _water.PlaneCollider.ClosestPointOnBounds(transform.position));
    }

    /// <summary>
    /// Called when BuoyantObject exits the water.
    /// </summary>
    /// <param name="eventWater">
    /// The FluidVolume which the object has left.
    /// </param>
    public void OnFluidVolumeExit(FluidVolume eventWater) {
        if (_water.PlaneCollider != null)
            MakeSplash(SplashPrefab, _water.PlaneCollider.ClosestPointOnBounds(transform.position));

        _water = null;
    }

    void MakeSplash(ParticleSystem SplashPrefab, Vector3 position) {
        if (SplashPrefab == null || Mathf.Abs(GetComponent<Rigidbody>().velocity.y) < SplashThreshold)
            return;

        ParticleSystem splash = Instantiate(SplashPrefab, position, Quaternion.Euler(-90f, 0f, 0f)) as ParticleSystem;
        if (splash != null) {
            Destroy(splash.gameObject, splash.duration);
        }
    }
}
