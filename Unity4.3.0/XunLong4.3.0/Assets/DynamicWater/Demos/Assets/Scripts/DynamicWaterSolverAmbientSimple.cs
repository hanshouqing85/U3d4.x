using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// Ancestor of DynamicWaterSolverSimulation with simple sine ambient waves over the simulation.
/// </summary>
[AddComponentMenu("Lost Polygon/Dynamic Water System/Simple Ambient Wave Solver")]
public class DynamicWaterSolverAmbientSimple : DynamicWaterSolverSimulation {
    /// <summary>
    /// The ambient wave height.
    /// </summary>
    public float AmbientWaveHeight = 0.3f;

    /// <summary>
    /// The ambient wave frequency.
    /// </summary>
    public float AmbientWaveFrequency = 1f;

    /// <summary>
    /// The ambient wave simulation speed factor.
    /// </summary>
    public float AmbientWaveSpeed = 1f;

    /// <summary>
    /// Value indicating whether only ambient waves have to be simulated.
    /// </summary>
    public bool OnlyAmbient = true;

    /// <summary>
    /// The resulting simulation state. Calculated as the sum of ambient waves and dynamic simulated waves.
    /// </summary>
    private float[] _fieldSum;

    /// <summary>
    /// The current ambient wave simulation time.
    /// </summary>
    private float _time;

    /// <summary>
    /// The initialization method. Called by <see cref="DynamicWater"/>.
    /// </summary>
    /// <param name="settings">
    /// The interface to the settings provider (usually <see cref="DynamicWater"/>)
    /// </param>
    /// <seealso cref="DynamicWater"/>
    public override void Initialize(IDynamicWaterSettings settings) {
        base.Initialize(settings);

        _fieldSum = new float[_grid.x * _grid.y];
    }

    /// <summary>
    /// Executes the simulation step.
    /// </summary>
    /// <param name="speed">
    /// The overall simulation speed factor.
    /// </param>
    /// <param name="damping">
    /// The damping value (range 0-1).
    /// </param>
    public override void StepSimulation(float speed, float damping) {
        if (!OnlyAmbient)
            base.StepSimulation(speed, damping);

        // Ambient wave time
        _time += Time.deltaTime * AmbientWaveSpeed;

        // Caching to avoid many divisions
        Vector2 invGrid = new Vector2(1f / _grid.x, 1f / _grid.y);
        for (int i = 0; i < _grid.x; i++) {
            for (int j = 0; j < _grid.y; j++) {
                int index = j * _grid.x + i;

                // Normalizing the coordinates
                float normX = i * invGrid.x;
                float normY = j * invGrid.y;

                // Calculating new node value
                float val = (normX + normY) * FastFunctions.DoublePi;
                if (OnlyAmbient)
                    _fieldSum[index] = FastFunctions.FastSin(val * AmbientWaveFrequency + _time) * AmbientWaveHeight;
                else
                    _fieldSum[index] = FastFunctions.FastSin(val * AmbientWaveFrequency + _time) * AmbientWaveHeight + FieldSimNew[index];
            }
        }

        // Updating the field
        _field = _fieldSum;

        _isDirty = true;
    }

    /// <summary>
    /// Creates a circular drop splash on the fluid surface.
    /// </summary>
    /// <param name="center">
    /// The center of the splash in simulation grid space.
    /// </param>
    /// <param name="radius">
    /// The radius of the splash.
    /// </param>
    /// <param name="force">
    /// The amount of force applied to create the splash.
    /// </param>
    public override void CreateSplashNormalized(Vector2 center, float radius, float force) {
        if (!OnlyAmbient)
            CreateSplashNormalized(center, radius, force, ref FieldSim);
    }

    /// <summary>
    /// Returns water level at the given position in simulation grid space.
    /// </summary>
    /// <param name="x">
    /// The x coordinate.
    /// </param>
    /// <param name="z">
    /// The z coordinate.
    /// </param>
    /// <returns>
    /// The water level at the given position in simulation grid space.
    /// </returns>
    public override float GetWaterLevel(float x, float z) {
        if (x <= 0 || z <= 0 || x >= _grid.x || z >= _grid.y) {
            return float.NegativeInfinity;
        }

        return _fieldSum[(int)z * _grid.x + (int)x];
    }
}
