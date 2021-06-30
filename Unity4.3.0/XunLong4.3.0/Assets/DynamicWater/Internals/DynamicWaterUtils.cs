using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LostPolygon.DynamicWaterSystem {

    /// <summary>
    /// Simple 2-dimensional integer vector
    /// </summary>
    public struct Vector2Int {
        public int x;
        public int y;

        public Vector2Int(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public Vector2Int(Vector2 value) {
            x = (int) value.x;
            y = (int) value.y;
        }

        public static implicit operator Vector2Int(Vector2 value) {
            return new Vector2Int((int) value.x, (int) value.y);
        }

        public override string ToString() {
            return "{" + x + ", " + y + "}";
        }
    }

    /// <summary>
    /// Simple 3-dimensional integer vector
    /// </summary>
    public struct Vector3Int {
        public int x;
        public int y;
        public int z;

        public Vector3Int(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3Int(Vector3 value) {
            return new Vector3Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y), Mathf.RoundToInt(value.z));
        }

        public override string ToString() {
            return "{" + x + ", " + y + ", " + z + "}";
        }
    }

    /// <summary>
    /// Interface representing the state of fluid simulation in simulation grid space.
    /// </summary>
    public interface IDynamicWaterSolverFieldState {
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
        float GetWaterLevel(float x, float z);
    }

    /// <summary>
    /// Interface representing the state of fluid simulation.
    /// </summary>
    public interface IDynamicWaterFieldState {
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
        /// The water level at the given position in world space.
        /// </returns>
        float GetWaterLevel(float x, float z);
    }

    /// <summary>
    /// Interface representing generic fluid volume physical properties.
    /// </summary>
    public interface IDynamicWaterFluidVolumeSettings : IDynamicWaterFieldState {
        /// <summary>
        /// Gets or sets the size of simulation field in world units.
        /// </summary>
        Vector2 Size { get; set; }
        /// <summary>
        /// Gets the fluid density in kg/m^3.
        /// </summary>
        float Density { get; }
        /// <summary>
        /// Gets the fluid volume BoxCollider
        /// </summary>
        BoxCollider Collider { get; }
        /// <summary>
        /// Creates a circular drop splash on the fluid surface (if available).
        /// </summary>
        /// <param name="center">
        /// The center of the splash in world space coordinates.
        /// </param>
        /// <param name="radius">
        /// The radius of the splash in world space units.
        /// </param>
        /// <param name="force">
        /// The amount of force applied to create the splash.
        /// </param>
        void CreateSplash(Vector3 center, float radius, float force);
    }

    /// <summary>
    /// Interface representing the public settings properties of <see cref="DynamicWater"/> class.
    /// </summary>
    public interface IDynamicWaterSettings : IDynamicWaterFluidVolumeSettings {
        /// <summary>
        /// Gets the actual simulation grid resolution.
        /// </summary>
        Vector2Int GridSize { get; }
        /// <summary>
        /// Gets the size of single simulation grid node in world space.
        /// </summary>
        float NodeSize { get; }
        /// <summary>
        /// Gets the current DynamicWaterSolver component instance.
        /// </summary>
        DynamicWaterSolver Solver { get; }
        /// <summary>
        /// Gets a value indicating whether the water mesh normals should be calculated.
        /// </summary>
        bool CalculateNormals { get; }
        /// <summary>
        /// Gets a value indicating whether the fast approximate method of calculating the
        /// water mesh normals should be used instead of <c>Mesh.RecalculateNormals()</c>.
        /// </summary>
        /// <remarks>
        /// Enabling this could provide a huge performance boost with the cost of degraded quality.
        /// Especially useful on mobile devices.
        /// </remarks>
        bool UseFakeNormals { get; }
        /// <summary>
        /// Gets a value indicating whether to normalize the fast approximate normals.
        /// </summary>
        /// <remarks>
        /// This value must generally be <c>true</c>, but you may disable the normalization 
        /// in case you want to normalize the normal in the shader.
        /// </remarks>
        bool NormalizeFakeNormals { get; }
        /// <summary>
        /// Gets a value indicating whether the tangents must be set (usually for bump-mapped shaders).
        /// </summary>
        /// <remarks>
        /// Enabling this may sometimes result in performance drop on high Quality levels. It is better to
        /// turn it off if your shader doesn't uses normals.
        /// </remarks>
        bool SetTangents { get; }
        /// <summary>
        /// Gets a value indicating whether to calculate where the simulation field is obstructed with
        /// GameObjects with tag <c>DynamicWaterObstruction</c>
        /// </summary>
        /// <remarks>
        /// As the simulation field can only be of rectangular shape for now, 
        /// this can be used to simulate complex shapes such as pond banks.
        /// </remarks>
        bool UseObstructions { get; }
        //bool MeshUseObstructionData { get; }
        /// <summary>
        /// Returns maximum resolution (along largest side) possible for this water plane dimensions.
        /// </summary>
        /// <returns>
        /// Maximum resolution (along largest side) possible for this water plane dimensions.
        /// </returns>
        int MaxResolution();
        /// <summary>
        /// Returns maximum wave propagation speed possible for this quality level.
        /// </summary>
        /// <returns>
        /// Maximum wave propagation speed possible for this quality level.
        /// </returns>
        float MaxSpeed();
    }

    /// <summary>
    /// A structure representing the properties of Mesh object.
    /// </summary>
    struct MeshStruct {
        public int[]     Triangles;
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector4[] Tangents;
        public Vector2[] UV;
        public Color32[] Colors32;
    }

    /// <summary>
    /// A collection of functions that work with colliders.
    /// </summary>
    public static class ColliderTools {
        /// <summary>
        /// Checks whether the point is inside a collider.
        /// </summary>
        /// <remarks>
        /// This method can check against all types of colliders, 
        /// including <c>TerrainCollider</c> and concave <c>MeshCollider</c>.
        /// </remarks>
        /// <param name="collider">
        /// The collider to check against.
        /// </param>
        /// <param name="point">
        /// The point being checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="point"/> is inside the <paramref name="collider"/>, 
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool IsPointInsideCollider(Collider collider, Vector3 point) {
            RaycastHit hit;
            #if !UNITY_FLASH
            if (collider is TerrainCollider) {
                if (!collider.Raycast(new Ray(point, Vector3.up), out hit, collider.bounds.size.y)) {
                    return false;
                }
            } else 
            #endif
            if (collider is MeshCollider && !((MeshCollider) collider).convex) {
                if (!IsPointInsideMeshCollider(collider, point)) {
                    return false;
                }
            } else {
                Vector3 direction = collider.bounds.center - point;
                float directionMagnitude = direction.magnitude;
                if (directionMagnitude > 0.01f &&
                    collider.Raycast(new Ray(point, direction.normalized), out hit, directionMagnitude)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the point is inside a MeshCollider.
        /// </summary>
        /// <param name="collider">
        /// Collider to check against.
        /// </param>
        /// <param name="point">
        /// Point being checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="point"/> is inside the <paramref name="collider"/>, 
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool IsPointInsideMeshCollider(Collider collider, Vector3 point) {
            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            foreach (var ray in directions) {
                RaycastHit hit;
                if (collider.Raycast(new Ray(point - ray * 1000f, ray), out hit, 1000f) == false) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if point is at most <paramref name="tolerance"/> away from the collider edge.
        /// </summary>
        /// <param name="collider">
        /// The collider to check against.
        /// </param>
        /// <param name="point">
        /// Point being checked.
        /// </param>
        /// <param name="tolerance">
        /// Maximal distance
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="point"/> is inside the <paramref name="collider"/> 
        /// and at most <paramref name="tolerance"/> away from its edge, 
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool IsPointAtColliderEdge(Collider collider, Vector3 point, float tolerance) {
            RaycastHit hit;

            tolerance *= 0.71f; // Approximately 1/sqrt(2)
            Vector3 direction = collider.bounds.center - point;
            Vector3 directionNormalized = direction.normalized;

            bool result = direction != Vector3.zero &&
                          collider.Raycast(new Ray(point - directionNormalized * tolerance, directionNormalized),
                                           out hit, tolerance);

            return result;
        }
    }

    /// <summary>
    /// A collection of fast implementations of mathematical functions.
    /// </summary>
    public static class FastFunctions {
        public const float DoublePi = Mathf.PI * 2f;
        public const float InvDoublePi = 1f / DoublePi;
        public const float Deg2Rad = 1f / 180f * Mathf.PI;

        /// <summary>
        /// The union of float and int sharing the same location in memory.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct FloatIntUnion {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int i;
        }

        /// <summary>
        /// Calculates the approximate value of sine function at a given angle.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Sin()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// The angle in radians.
        /// </param>
        /// <returns>
        /// The approximate value of sine function at angle <paramref name="x"/>.
        /// </returns>
        /// <seealso href="http://lab.polygonal.de/?p=205"/>
        public static float FastSin(float x) {
            x = x - DoublePi * Mathf.Floor((x + Mathf.PI) * InvDoublePi);

            if (x < 0)
                x = 1.27323954f * x + 0.405284735f * x * x;
            else
                x = 1.27323954f * x - 0.405284735f * x * x;

            return x;
        }

        /// <summary>
        /// Calculates the approximate inverse square root of a given value.
        /// </summary>
        /// <remarks>
        /// This function is much faster than calling <c>1/Mathf.Sqrt(x)</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// The input value.
        /// </param>
        /// <returns>
        /// The approximate value of inverse square root of <paramref name="x"/>.
        /// </returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Fast_inverse_square_root"/>
        public static float FastInvSqrt(float x) {
            FloatIntUnion u;
            u.i = 0;
            u.f = x;
            float xhalf = 0.5f * x;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f;
        }

        /// <summary>
        /// Calculates the approximate square root of a given value.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Sqrt()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// Input value.
        /// </param>
        /// <returns>
        /// The approximate value of square root of <paramref name="x"/>.
        /// </returns>
        public static float FastSqrt(float x) {
            FloatIntUnion u;
            u.i = 0;
            u.f = x;
            float xhalf = 0.5f * x;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * x;
        }

        /// <summary>
        /// Calculates the approximate log2 of a given value.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Log()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// Input value.
        /// </param>
        /// <returns>
        /// The approximate value of log2 of <paramref name="x"/>.
        /// </returns>
        /// <seealso cref="http://code.google.com/p/fastapprox"/>
        public static float FastLog2(float x) {
            FloatIntUnion union;
            union.i = 0;
            union.f = x;

            float y = union.i;
            y *= 1.1920928955078125e-7f;
            return y - 126.94269504f;
        }

        /// <summary>
        /// Calculates the approximate value of 2^<paramref name="x"/>.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Pow()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// Input value.
        /// </param>
        /// <returns>
        /// The approximate value of 2^<paramref name="x"/>.
        /// </returns>
        /// <seealso cref="http://code.google.com/p/fastapprox"/>
        public static float FastPow2(float x) {
            float clip = (x < -126f) ? -126.0f : x;

            FloatIntUnion union;
            union.f = 0;
            union.i = (int) ((1 << 23) * (clip + 126.94269504f));

            return union.f;
        }

        /// <summary>
        /// Calculates the approximate value of <paramref name="a"/>^<paramref name="b"/>.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Mathf.Pow()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="a">
        /// The vase value.
        /// </param>
        /// <param name="b">
        /// The power value.
        /// </param>
        /// <returns>
        /// The approximate value of <paramref name="a"/>^<paramref name="b"/>.
        /// </returns>
        /// <seealso cref="http://code.google.com/p/fastapprox"/>
        public static float FastPow(float a, float b) {
            //FastPow2(b * FastLog2(a));

            FloatIntUnion union;
            union.i = 0;
            union.f = a;

            float y = union.i;
            y *= 1.1920928955078125e-7f;
            union.f = b * (y - 126.94269504f);

            float clip = (union.f < -126f) ? -126.0f : union.f;
            union.i = (int)((1 << 23) * (clip + 126.94269504f));

            return union.f;
        }

        /// <summary>
        /// Calculates the approximate magnitude of <c>Vector3</c>.
        /// </summary>
        /// <remarks>
        /// This function is much faster than <c>Vector3.magnitude</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="vector">
        /// Input vector.
        /// </param>
        /// <returns>
        /// The approximate magnitude of <see cref="vector"/>.
        /// </returns>
        public static float FastVector3Magnitude(Vector3 vector) {
            float magnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;

            FloatIntUnion u;
            u.i = 0;
            u.f = magnitude;
            float xhalf = 0.5f * magnitude;
            u.i = 0x5f375a86 - (u.i >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * magnitude;
        }

        /// <summary>
        /// Checks whether the float value is NaN.
        /// </summary>
        /// <remarks>
        /// This function is much faster than calling <c>Math.IsNaN()</c>, especially on mobile devices.
        /// </remarks>
        /// <param name="x">
        /// The input value to be checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="x"/> is NaN, <c>false</c> otherwise.
        /// </returns>
        /// <seealso href="http://stackoverflow.com/questions/639010/how-can-i-compare-a-float-to-nan-if-comparisons-to-nan-always-return-false"/>
        public static bool FastIsNaN(float x) {
            FloatIntUnion union;
            union.i = 0;
            union.f = x;

            return ((union.i & 0x7F800000) == 0x7F800000) && ((union.i & 0x007FFFFF) != 0);
        }
    }

    /// <summary>
    /// Wrapper for the linear wave equeation solver. This is the core of interactive simulation.
    /// </summary>
    public static class LinearWaveEqueationSolver {
        /// <summary>
        /// Represents the maximal time delta for wave simulation to not diverge.
        /// </summary>
        /// <remarks>
        /// The <c>timeDelta</c> parameter of <see cref="Solve"/> method must always be clamped to this value, otherwise undesired behaviour will occur, producing massive artifacts.
        /// </remarks>
        /// <seealso cref="Solve"/>
        public const float MaxDt = 1.412f;

        /// <summary>
        /// Performs the wave simulation step.
        /// </summary>
        /// <param name="field">
        /// Represents the current simulation state.
        /// </param>
        /// <param name="fieldNew">
        /// Represents the updated simulation state.
        /// </param>
        /// <param name="fieldSpeed">
        /// Represents the simulation state difference.
        /// </param>
        /// <param name="fieldObstruction">
        /// Array of <c>bool</c> indicating whether the water is obstructed by an object. \n
        /// <c>true</c> means the grid node is obstructed by an object, so the simulation is not updated;
        /// <c>false</c> means the grid node is not obstructed, and the simulation can proceed freely.
        /// </param>
        /// <param name="gridSize">
        /// Actual simulation grid resolution.
        /// </param>
        /// <param name="timeDelta">
        /// Time delta in seconds.
        /// </param>
        /// <param name="damping">
        /// Damping value. Must be clamped to the 0-1 range. 
        /// </param>
        /// <param name="maxValue">
        /// Value representing maximal absolute wave height.
        /// </param>
        public static void Solve(ref float[] field, ref float[] fieldNew, ref float[] fieldSpeed, bool[] fieldObstruction, Vector2Int gridSize, float timeDelta, float damping, out float maxValue) {
            maxValue = float.NegativeInfinity;

            bool isFieldObstructionNull = fieldObstruction == null;
            for (int j = 0; j < gridSize.y; j++) {
                int index = j * gridSize.x;
                for (int i = 0; i < gridSize.x; i++) {
                    // Not updating borders and obstructions
                    if (!(i <= 0 || j <= 0 || i >= gridSize.x - 1 || j >= gridSize.y - 1 || (!isFieldObstructionNull && fieldObstruction[index]))) {
                        float laplasian = (field[index - 1] +
                                           field[index + 1] +
                                           field[index + gridSize.x] +
                                           field[index - gridSize.x]) * 0.25f -
                                           field[index];

                        fieldSpeed[index] += laplasian * timeDelta;
                        fieldNew[index] = (field[index] + fieldSpeed[index]) * damping;

                        float valueAbs;
                        if (fieldNew[index] > 0f) {
                            valueAbs = fieldNew[index];
                        } else {
                            valueAbs = -fieldNew[index];
                        }
                        if (valueAbs > maxValue) maxValue = valueAbs;
                    }

                    index++;
                }
            }
        }
    }

}