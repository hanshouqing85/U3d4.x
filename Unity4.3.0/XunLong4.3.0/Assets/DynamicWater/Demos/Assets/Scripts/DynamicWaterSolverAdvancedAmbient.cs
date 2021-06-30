using System;
using System.Collections;
using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// Ancestor of DynamicWaterSolverSimulation capable of doing some very crude ocean-like waves.
/// </summary>
/// <remarks>
/// This Solver is not very efficient performance-wise, so it's probably better to avoid using it on mobile.
/// </remarks>
[AddComponentMenu("Lost Polygon/Dynamic Water System/Advanced Ambient Wave Solver")]
public class DynamicWaterSolverAdvancedAmbient : DynamicWaterSolverSimulation {

    [Serializable]
    public class Wave {
        public float Amplitude;
        public float Angle;
        public float Frequency;
        public float Velocity;
        public int Steepness;
        public bool Excluded;
        [HideInInspector]
        public Vector2 Direction;
    }

    public Wave[] Waves;

    /// <summary>
    /// Value indicating whether only ambient waves have to be simulated.
    /// </summary>
    public bool OnlyAmbient = true;

    /// <summary>
    /// The resulting simulation state. Calculated as the sum of ambient waves and dynamic simulated waves.
    /// </summary>
    private float[] _fieldSum;
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

        // Initial state
        _field = _fieldSum;
    }

    public override void CreateSplashNormalized(Vector2 center, float radius, float force) {
        if (OnlyAmbient)
            return;

        CreateSplashNormalized(center, radius, force, ref FieldSim);
    }

    public override float GetWaterLevel(float x, float z) {
        if (x <= 0 || z <= 0 || x >= _grid.x || z >= _grid.y) {
            return float.NegativeInfinity;
        }

        return _fieldSum[(int)z * _grid.x + (int)x];
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
        _time += Time.deltaTime;
        // Recalculating direction
        foreach (Wave wave in Waves) {
            wave.Direction = new Vector2(Mathf.Cos(wave.Angle * FastFunctions.Deg2Rad), Mathf.Sin(wave.Angle * FastFunctions.Deg2Rad));
        }

        // Caching to avoid many divisions
        Vector2 invGrid = new Vector2(1f / _grid.x, 1f / _grid.y);
        for (int i = 0; i < _grid.x; i++) {
            for (int j = 0; j < _grid.y; j++) {
                int index = j * _grid.x + i;

                // Normalizing the coordinates
                float normX = i * invGrid.x * FastFunctions.DoublePi;
                float normY = j * invGrid.y * FastFunctions.DoublePi;
                _fieldSum[index] = 0f;
                foreach (Wave wave in Waves) {
                    // Calculating new node value
                    if (wave.Excluded) continue;
                    float val = (wave.Direction.x * normX + wave.Direction.y * normY) * wave.Frequency + _time * wave.Velocity;
                     _fieldSum[index] += FastFunctions.FastPow((FastFunctions.FastSin(val) + 1f) * 0.5f, wave.Steepness) * wave.Amplitude;
                }
                if (!OnlyAmbient) _fieldSum[index] += FieldSimNew[index];
            }
        }

        // Updating the field
        _field = _fieldSum;

        _isDirty = true;
    }
}
