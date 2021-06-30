using System;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// The main dynamic water class.
/// </summary>
/// <remarks>
/// Most interaction is done with this class. 
/// It differs from <see cref="FluidVolume"/> by having the wave simulation 
/// and by having the visual representation.
/// </remarks>
[AddComponentMenu("Lost Polygon/Dynamic Water System/Dynamic Water")]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class DynamicWater : FluidVolume, IDynamicWaterSettings {
    /// <summary>
    /// The tag and layer name used by clickable plane collider.
    /// </summary>
    public const string PlaneColliderLayerName = "DynamicWaterPlaneCollider";

    /// <summary>
    /// The tag name used to define obstruction geometry.
    /// </summary>
    public const string DynamicWaterObstructionTag = "DynamicWaterObstruction";

    /// <summary>
    /// Value indicating whether to generate the clickable static collider on initialization.
    /// </summary>
    public bool UsePlaneCollider = true;

    /// <summary>
    /// Value indicating whether to update the simulation when the mesh is not visible.
    /// </summary>
    public bool UpdateWhenNotVisible = false;

    /// <summary>
    /// Gets or sets the simulation grid resolution.
    /// </summary>
    /// <remarks>
    /// This value is normalized, so that it doesn't depends on the size of water plane.
    /// To get the actual grid size, see <see cref="GridSize"/>.
    /// </remarks>
    /// <value>This value to the range 4-256</value>
    public int Quality {
        get {
            return _quality; 
        }
        set {
            if (_quality != value) {
                _quality = Mathf.Clamp(value, 4, 256);

                _resolution = Mathf.Clamp(Mathf.RoundToInt(_quality * MaxResolution() / 256f), 4, MaxResolution());

                // Making sure resolution is even
                if (_resolution % 2 == 1)
                    _resolution++;

                PropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets the actual simulation grid resolution.
    /// </summary>
    public Vector2Int GridSize {
        get { return _grid; }
    }

    /// <summary>
    /// Gets the size of single simulation grid node in world space.
    /// </summary>
    public float NodeSize {
        get { return _nodeSize; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the water mesh normals should be calculated.
    /// </summary>
    public bool CalculateNormals {
        get { return _calculateNormals; }
        set { _calculateNormals = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the fast approximate method of calculating the
    /// water mesh normals should be used instead of <c>Mesh.RecalculateNormals()</c>.
    /// </summary>
    /// <remarks>
    /// Enabling this could provide a huge performance boost with the cost of degraded quality.
    /// Especially useful on mobile devices.
    /// </remarks>
    public bool UseFakeNormals {
        get { return _useFakeNormals; }
        set { _useFakeNormals = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to normalize the fast approximate normals.
    /// </summary>
    /// <remarks>
    /// This value must generally be <c>true</c>, but you may disable the normalization 
    /// in case you want to normalize the normal in the shader.
    /// </remarks>
    public bool NormalizeFakeNormals {
        get { return _normalizeFakeNormals; }
        set { _normalizeFakeNormals = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the tangents must be set (usually for bump-mapped shaders).
    /// </summary>
    /// <remarks>
    /// Enabling this may sometimes result in performance drop on high Quality levels. It is better to
    /// turn it off if your shader doesn't uses normals.
    /// </remarks>
    public bool SetTangents {
        get { return _setTangents; }
        set { _setTangents = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to calculate where the simulation field is obstructed with
    /// GameObjects with tag <c>DynamicWaterObstruction</c>
    /// </summary>
    /// <remarks>
    /// As the simulation field can only be of rectangular shape for now, 
    /// this can be used to simulate complex shapes such as pond banks.
    /// </remarks>
    public bool UseObstructions {
        get { return _useObstructions; }
        set { _useObstructions = value; }
    }

    //private bool _meshUseObstructionData = true;
    //public bool MeshUseObstructionData {
    //    get { return _meshUseObstructionData; }
    //    set { _meshUseObstructionData = value; }
    //}

    /// <summary>
    /// Gets or sets the speed.
    /// </summary>
    public float Speed {
        get { return _speed; }
        set {
            _speed = Mathf.Clamp(value, 0f, MaxSpeed());
        }
    }

    /// <summary>
    /// Gets or sets the wave damping value.
    /// </summary>
    /// <value>
    /// The value is clamped to the 0-1 range. The higher the value, the higher the damping. 
    /// Value of zero correspond to absence of any damping, which could lead to simulation instability
    /// when too much force was applied to the system.
    /// </value>
    /// <example>
    /// Optimal value for water is around 0.05.
    /// </example>
    public float Damping {
        get { return 1f - _damping; }
        set { _damping = 1f - Mathf.Clamp01(value); }
    }

    /// <summary>
    /// Gets the current DynamicWaterSolver component instance.
    /// </summary>
    public DynamicWaterSolver Solver {
        get { return _waterSolver; }
    }

    /// <summary>
    /// Gets the water plane BoxCollider
    /// </summary>
    public virtual BoxCollider PlaneCollider {
        get {
            return _planeCollider;
        }
    }

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private DynamicWaterSolver _waterSolver;
    private bool _prevIsDirty = true;
    private DynamicWaterMesh _waterMesh;
    private DynamicWaterMesh _waterMeshSimple;
    private Vector2 _nodeSizeNormalized;
    private BoxCollider _planeCollider;
    private Vector3 _lossyScale;

    /* Property fields */

    [SerializeField]
    private float _damping = 0.97f;
    [SerializeField]
    private float _speed = 45f;
    [SerializeField]
    private bool _calculateNormals = true;
    [SerializeField]
    private bool _useFakeNormals = false;
    [SerializeField]
    private bool _normalizeFakeNormals = true;
    [SerializeField]
    private bool _useObstructions = false;
    [SerializeField]
    private bool _setTangents = false;
    [SerializeField]
    private int _quality = 64;

    private int _resolution = 64;
    private Vector2Int _grid;
    private float _nodeSize;

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
    public override void CreateSplash(Vector3 center, float radius, float force) {
        if (_waterSolver == null || !_collider.bounds.Contains(center))
            return;

        center = _transform.InverseTransformPoint(center);
        center = new Vector2(center.x * _nodeSizeNormalized.x, center.z * _nodeSizeNormalized.y);
        CreateSplashNormalized(center, radius, force);
    }

    /// <summary>
    /// Creates a circular drop splashes on the fluid surface across the line.
    /// </summary>
    /// <param name="start">
    /// The start point in world space coordinates.
    /// </param>
    /// <param name="end">
    /// The end point in world space coordinates.
    /// </param>
    /// <param name="radius">
    /// The radius of the splash in world space units.
    /// </param>
    /// <param name="force">
    /// The amount of force applied to create the splash.
    /// </param>
    public void CreateSplash(Vector3 start, Vector3 end, float radius, float force) {
        if (_waterSolver == null || !(_collider.bounds.Contains(start) || _collider.bounds.Contains(end)))
            return;

        start = _transform.InverseTransformPoint(start);
        end = _transform.InverseTransformPoint(end);

        start = new Vector2(start.x * _nodeSizeNormalized.x, start.z * _nodeSizeNormalized.y);
        end = new Vector2(end.x * _nodeSizeNormalized.x, end.z * _nodeSizeNormalized.y);

        CreateSplashLine(new Vector2Int(start), new Vector2Int(end), radius, force);
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
    /// The water level at the given position in world space.
    /// </returns>
    public override float GetWaterLevel(float x, float z) {
        if (_waterSolver == null)
            return 0f;

        Vector3 projected;
        projected.x = x;
        projected.y = 0f;
        projected.z = z;
        projected = _transform.InverseTransformPoint(projected);

        projected.x = projected.x * _nodeSizeNormalized.x;
        projected.z = projected.z * _nodeSizeNormalized.y;
        projected.y = _waterSolver.GetWaterLevel(projected.x, projected.z);
        if (projected.y != float.NegativeInfinity)
            projected.y = _transform.position.y + _lossyScale.y * projected.y; // FIXME?

        return projected.y;

        // For some little speed boost
        /*Vector3 lossyScale = _transform.lossyScale;
        Vector3 position = _transform.position;
        Vector3 projected;
        projected.x = (x - position.x * lossyScale.x) * _nodeSizeNormalized.x;
        projected.z = (z - position.z * lossyScale.z) * _nodeSizeNormalized.y;
        projected.y = _waterSolver.GetWaterLevel(projected.x, projected.z);

        if (projected.y != float.NegativeInfinity)
            projected.y = _transform.position.y + _transform.lossyScale.y * projected.y; // FIXME?
        return projected.y;*/
    }

    /// <summary>
    /// Returns maximum resolution (along largest side) possible for this water plane dimensions.
    /// </summary>
    /// <returns>
    /// Maximum resolution (along largest side) possible for this water plane dimensions.
    /// </returns>
    public int MaxResolution() {
        const int maxSide = 65000;

        // Evil formula for calculating the max grid side resolution
        float maxResolution = 0.5f * (Mathf.Sqrt(4f * maxSide * _size.x * _size.y
                                                 + _size.x * _size.x + _size.y * _size.y
                                                 - 2f * _size.x * _size.y)
                                                 - _size.x - _size.y);

        maxResolution /= Mathf.Min(_size.x, _size.y);
        int maxResolutionInt = (int)maxResolution - 1;

        // Ugly hack. FIXME
        while (true) {
            Vector2Int grid = SizeToGridResolution(Size, maxResolutionInt);
            if (grid.x * grid.y > maxSide)
                maxResolutionInt--;
            else {
                break;
            }
        }

        return maxResolutionInt;
    }

    /// <summary>
    /// Returns maximum wave propagation speed possible for this quality level.
    /// </summary>
    /// <returns>
    /// Maximum wave propagation speed possible for this quality level.
    /// </returns>
    public float MaxSpeed() {
        Vector2Int grid = SizeToGridResolution(Size, _resolution);
        float speedCoeff = (grid.x + grid.y) / 128f;
        float maxSpeed = LinearWaveEqueationSolver.MaxDt / (Time.fixedDeltaTime * speedCoeff);

        return maxSpeed;
    }

    /// <summary>
    /// Recalculate the static obstructions.
    /// </summary>
    public void RecalculateObstructions() {
        bool[] fieldObstruction = new bool[_grid.x * _grid.y];

        Vector2 invGrid = new Vector2(1f / _grid.x * _collider.size.x, 1f / _grid.y * _collider.size.z);
        GameObject[] obstructions = GetGameObjectsWithTagInBounds(DynamicWaterObstructionTag, _collider.bounds);
        //GameObject[] obstructionsInverted = GetGameObjectsWithTagInBounds("DynamicWaterObstructionInverted", _collider.bounds);

        for (int j = 0; j < _grid.y; j++) {
            for (int i = 0; i < _grid.x; i++) {
                int index = j * _grid.x + i;

                float normX = i * invGrid.x;
                float normZ = j * invGrid.y;

                // Usual obstructions
                foreach (GameObject obstruction in obstructions) {
                    Collider oCollider = obstruction.GetComponent<Collider>();
                    if (oCollider != null) {
                        Vector3 point = _transform.TransformPoint(normX, 0f, normZ);

                        if (oCollider.bounds.Contains(point) && ColliderTools.IsPointInsideCollider(oCollider, point)) {
                            fieldObstruction[index] = true;
                            break;
                        }
                    }
                }

                // Inverted obstructions
                /*
                foreach (GameObject obstruction in obstructionsInverted) {
                    Collider oCollider = obstruction.collider;
                    if (oCollider != null) {
                        Vector3 point = _transform.TransformPoint(normX, 0f, normZ);

                        if (!ColliderTools.IsPointInsideCollider(oCollider, point)) {
                            fieldObstruction[index] = true;
                            break;
                        }
                    }
                }
                */
            }
        }


        _waterSolver.FieldObstruction = fieldObstruction;
    }

    /// <summary>
    /// Returns GameObjects with tag <param name="searchTag"/> within Bounds <param name="bounds"></param>.
    /// </summary>
    /// <param name="searchTag">
    /// The tag to find GameObjects with.
    /// </param>
    /// <param name="bounds">
    /// The bounds to search within.
    /// </param>
    /// <returns>
    /// The GameObject array <see cref="GameObject[]"/>.
    /// </returns>
    private GameObject[] GetGameObjectsWithTagInBounds(string searchTag, Bounds bounds) {
        GameObject[] obstructionsTemp = GameObject.FindGameObjectsWithTag(searchTag);
        List<GameObject> obstructionList = new List<GameObject>(obstructionsTemp.Length);
        foreach (GameObject obstruction in obstructionsTemp) {
            if (obstruction.GetComponent<Collider>() != null && bounds.Intersects(obstruction.GetComponent<Collider>().bounds)) {
                obstructionList.Add(obstruction);
            }
        }

        return obstructionList.ToArray();
    }

    /// <summary>
    /// Initializes the required components and creates initial Mesh.
    /// </summary>
    protected override void Initialize() {
        base.Initialize();

        UpdateComponents();
        UpdateProperties();
    }

    /// <summary>
    /// Called when any property connected with the mesh is changed.
    /// </summary>
    protected override void PropertyChanged() {
        UpdateProperties();
    }

    /// <summary>
    /// Update the collider boundaries.
    /// </summary>
    protected override void UpdateCollider() {
        Vector2 sizeScaled = new Vector2(_size.x * transform.lossyScale.x, _size.y * transform.lossyScale.y);

        // We don't want the collider to obstruct view in Editor mode
        if (Application.isPlaying) {
            _collider.center = new Vector3(sizeScaled.x / 2f, 0f, sizeScaled.y / 2f);
            _collider.size = new Vector3(sizeScaled.x, _depth * 2f, sizeScaled.y);
        } else {
            _collider.center = new Vector3(sizeScaled.x / 2f, -_depth / 2f - 0.1f, sizeScaled.y / 2f);
            _collider.size = new Vector3(sizeScaled.x, _depth - 0.1f, sizeScaled.y);
        }

        _collider.isTrigger = true;
    }

    /// <summary>
    /// Create the child gameObject for interacting with water plane.
    /// </summary>
    protected void CreatePlaneCollider() {
        GameObject planeColliderObject = null;

        // Trying to find an existing collider
        foreach (Transform child in transform) {
            if (child.CompareTag(PlaneColliderLayerName)) {
                planeColliderObject = child.gameObject;
                break;
            }
        }

        // If we haven't found any - creating a new one
        if (planeColliderObject == null) {
            planeColliderObject = new GameObject(PlaneColliderLayerName);
            planeColliderObject.tag = PlaneColliderLayerName;
            planeColliderObject.layer = LayerMask.NameToLayer(PlaneColliderLayerName);
            planeColliderObject.transform.parent = _transform;
            planeColliderObject.transform.rotation = transform.rotation;
        }

        // Attaching and setting up the BoxCollider
        BoxCollider bc = planeColliderObject.GetComponent<BoxCollider>();
        if (bc == null) bc = planeColliderObject.AddComponent<BoxCollider>();

        bc.size = new Vector3(_collider.size.x, 0f, _collider.size.z);
        bc.center = _collider.center;
        bc.isTrigger = true;

        _planeCollider = bc;

        planeColliderObject.transform.localPosition = Vector3.zero;
    }

    private void Start() {
        Initialize();
    }

    private void FixedUpdate() {
        if (!Application.isPlaying)
            return;

        if (Application.isEditor)
            UpdateComponents();

        StepSimulation();
    }

    private void OnDestroy() {
        ClearMeshes();
    }

    private void StepSimulation() {
        if (_waterSolver == null || (!_meshRenderer.isVisible && !UpdateWhenNotVisible))
            return;

        // Cache for GetWaterLevel
        _lossyScale = _transform.lossyScale;

        // Update the field
        _waterSolver.StepSimulation(_speed, _damping);

        // Update the Mesh, if needed
        if (_waterSolver.IsDirty) {
            _waterMesh.IsDirty = _waterSolver.IsDirty;
            if (_meshRenderer.isVisible) {
                float[] field = _waterSolver.Field;
                bool[] fieldObstruction = _waterSolver.FieldObstruction;
                _waterMesh.UpdateMesh(ref field, ref fieldObstruction);
            }

        }

        //if (!_meshUseObstructionData && (_prevIsDirty != _waterSolver.IsDirty)) {
        // Switch to simple mesh, if we can
        if (_prevIsDirty != _waterSolver.IsDirty) {
            _meshFilter.mesh = _waterSolver.IsDirty ? _waterMesh.Mesh : _waterMeshSimple.Mesh;
        }

        _prevIsDirty = _waterSolver.IsDirty;
    }

    private void UpdateComponents() {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Called when any property that defines the water simulation, changes.
    /// </summary>
    private void UpdateProperties() {
        // Creating the simple mesh substitute for when Solver is not in dirty state
        ClearMeshes();

        _meshFilter = gameObject.GetComponent<MeshFilter>();

        const int simpleMeshResolution = 6;
        _nodeSize = Mathf.Max(Size.x, Size.y) / simpleMeshResolution;
        _grid.x = Mathf.RoundToInt(Size.x / _nodeSize) + 1;
        _grid.y = Mathf.RoundToInt(Size.y / _nodeSize) + 1;
        _waterMeshSimple = new DynamicWaterMesh(this);

        // Creating the actual water mesh
        _resolution = Mathf.RoundToInt(_quality * MaxResolution() / 256f);

        _nodeSize = Mathf.Max(Size.x, Size.y) / _resolution;
        _grid.x = Mathf.RoundToInt(Size.x / _nodeSize) + 1;
        _grid.y = Mathf.RoundToInt(Size.y / _nodeSize) + 1;
        // Some failsafe
        if (_size.x == 0f || _size.y == 0f) {
            _grid = new Vector2Int(1, 1);
        }

        _nodeSizeNormalized.x = 1f / Size.x * _grid.x;
        _nodeSizeNormalized.y = 1f / Size.y * _grid.y;

        _waterMesh = new DynamicWaterMesh(this);
        _meshFilter.mesh = _waterMesh.Mesh;

        // Update the collider bound
        UpdateCollider();

        // Create the child gameObject for interacting with water plane
        if (Application.isPlaying && UsePlaneCollider) {
            CreatePlaneCollider();
        }

        // Determining what Solver instance do we have to use
        // and destroy other instances
        if (Application.isPlaying) {
            _waterSolver = null;

            DynamicWaterSolver[] solverComponents = GetComponents<DynamicWaterSolver>();
            foreach (DynamicWaterSolver solverComponent in solverComponents) {
                if (_waterSolver == null) {
                    _waterSolver = solverComponent;
                } else {
                    Destroy(solverComponent);
                }
            }

            if (solverComponents.Length > 1) {
                Debug.LogWarning(
                    "More than one DynamicWaterSolver component present. Using the first one, others are destroyed", this);
            }

            if (_waterSolver != null) {
                _waterSolver.Initialize(this);

                // Recalculate the static obstruction objects
                if (_useObstructions) {
                    RecalculateObstructions();
                    // Update initial mesh obstruction state
                    //float[] field = _waterSolver.Field;
                    //bool[] fieldObstruction = _waterSolver.FieldObstruction;
                    //_waterMesh.UpdateMesh(ref field, ref fieldObstruction);
                }
                    
            } else {
                Debug.LogError("No DynamicWaterSolver component attached!", this);
            }
        }

        _prevIsDirty = true;
    }

    /// <summary>
    /// The size to grid resolution.
    /// </summary>
    /// <param name="size">
    /// The water plane dimensions.
    /// </param>
    /// <param name="resolution">
    /// The resolution.
    /// </param>
    /// <returns>
    /// The <see cref="Vector2Int"/>.
    /// </returns>
    private Vector2Int SizeToGridResolution(Vector2 size, int resolution) {
        float nodeSize = Mathf.Max(size.x, size.y) / resolution;
        Vector2Int grid;
        grid.x = Mathf.RoundToInt(size.x / nodeSize) + 1;
        grid.y = Mathf.RoundToInt(size.y / nodeSize) + 1;

        return grid;
    }

    /// <summary>
    /// Frees the memory from the meshes.
    /// </summary>
    private void ClearMeshes() {
        if (_meshFilter != null) {
            DestroyImmediate(_meshFilter.sharedMesh);
        }

        if (_waterMeshSimple != null) {
            _waterMeshSimple.FreeMesh();
            _waterMeshSimple = null;
        }

        if (_waterMesh != null) {
            _waterMesh.FreeMesh();
            _waterMesh = null;
        }
    }

    /// <summary>
    /// Creates a circular drop splash on the fluid surface (if available).
    /// </summary>
    /// <param name="center">
    /// The center of the splash in simulation field space coordinates.
    /// </param>
    /// <param name="radius">
    /// The radius of the splash in simulation field space space units.
    /// </param>
    /// <param name="force">
    /// The amount of force applied to create the splash.
    /// </param>
    private void CreateSplashNormalized(Vector2 center, float radius, float force) {
        radius = _size.x < _size.y ? radius / _size.x * _grid.x : radius / _size.y * _grid.y;

        _waterSolver.CreateSplashNormalized(center, radius, force);
    }

    private void CreateSplashLine(Vector2Int start, Vector2Int end, float radius, float force) {
        int dx = Math.Abs(end.x - start.x);
        int dy = Math.Abs(end.y - start.y);

        int sx, sy;

        if (start.x < end.x) sx = 1; else sx = -1;
        if (start.y < end.y) sy = 1; else sy = -1;
        
        int err = dx - dy;
        bool splashMade = false;
        while (true) {
            if (start.x == end.x && start.y == end.y)
                break;

            Vector2 startf;
            startf.x = start.x;
            startf.y = start.y;
            CreateSplashNormalized(startf, radius, force);

            splashMade = true;

            int e2 = 2 * err;

            if (e2 > -dy) {
                err = err - dy;
                start.x = start.x + sx;
            }

            if (e2 < dx) {
                err = err + dx;
                start.y = start.y + sy;
            }
        }
        // Make sure we have made at least one splash
        if (!splashMade) {
            CreateSplashNormalized(new Vector2(start.x, start.y), radius, force);
        }
    }
}