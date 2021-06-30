//#define USE_TRIANGLE_STRIP
using UnityEngine;

namespace LostPolygon.DynamicWaterSystem {
    public class DynamicWaterMesh {
        /// <summary>
        /// Gets a value indicating whether the water mesh is dirty and must be updated.
        /// </summary>
        public bool IsDirty {
            get {
                return _isDirty;
            }
            set {
                _isDirty = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the water mesh is initilialized.
        /// </summary>
        public bool IsReady {
            get;
            private set;
        }

        /// <summary>
        /// Gets the water Mesh instance.
        /// </summary>
        public Mesh Mesh {
            get {
                return _mesh;
            }
        }

        private readonly Vector2Int _grid;
        private readonly float _nodeSize;
        private readonly Vector2 _size;
        private readonly IDynamicWaterSettings _settings;
        private readonly Mesh _mesh;
        private bool _isDirty;
        private MeshStruct _meshStruct;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicWaterMesh"/> class.
        /// </summary>
        /// <param name="settings">
        /// Interface representing the public settings properties of <see cref="DynamicWater"/> class.
        /// </param>
        public DynamicWaterMesh(IDynamicWaterSettings settings) {
            IsReady = false;

            _settings = settings;

            _grid = settings.GridSize;
            _nodeSize = settings.NodeSize;
            _size = settings.Size;

            _mesh = new Mesh {
                name = "DynamicWaterMesh"
            };
            #if !UNITY_3_5
            _mesh.MarkDynamic();
            #endif
            AllocateMeshArrays();
            CreateMeshGrid();
            AssignMesh();
            _mesh.RecalculateBounds();

            IsReady = true;
        }

        /// <summary>
        /// Frees the mesh object.
        /// </summary>
        public void FreeMesh() {
            if (_mesh != null) {
                Object.DestroyImmediate(_mesh);
            }
        }

        /// <summary>
        /// Assigns the initial mesh values generated in <see cref="CreateMeshGrid"/> to the Mesh object.
        /// </summary>
        private void AssignMesh() {
            _mesh.vertices = _meshStruct.Vertices;
            _mesh.normals = _meshStruct.Normals;
            if (_settings.SetTangents)
                _mesh.tangents = _meshStruct.Tangents;
            
            _mesh.uv = _meshStruct.UV;
            _mesh.colors32 = _meshStruct.Colors32;
            _mesh.triangles = _meshStruct.Triangles;

            _mesh.RecalculateBounds();

            // Freeing the memory
            _meshStruct.Tangents = null;
            _meshStruct.Triangles = null;
            _meshStruct.UV = null;
            _meshStruct.Colors32 = null;
        }

        /// <summary>
        /// Generates the initial values for the Mesh object.
        /// </summary>
        private void CreateMeshGrid() {
            float uvStepXInit = 1f / (_size.x / _nodeSize);
            float uvStepYInit = 1f / (_size.y / _nodeSize);
            Color32 colorOne = new Color32(255, 255, 255, 255);
            //Color32 colorZero = new Color32(0, 0, 0, 0);
            Vector3 up = Vector3.up;
            bool setTangents = _settings.SetTangents;

            //bool useObstructionData = _settings.MeshUseObstructionData;
            //bool[] fieldObstruction = null;
            //if (useObstructionData) {
            //    fieldObstruction = _settings.Solver.FieldObstruction;
            //    if (fieldObstruction == null)
            //        useObstructionData = false;
            //}

            // Tangents are not really recalculated, as that'd be horribly slow
            // for anything realtime. But in case of water it's hard to notice anyway.
            Vector4 tangent = new Vector4(1f, 0f, 0f, 1f);

            int k = 0;

            for (int j = 0; j < _grid.y; j++) {
                for (int i = 0; i < _grid.x; i++) {
                    int index = j * _grid.x + i;

                    // Set vertices
                    _meshStruct.Vertices[index].x = i * _nodeSize;
                    _meshStruct.Vertices[index].y = 0f;
                    _meshStruct.Vertices[index].z = j * _nodeSize;

                    // Set triangles
                    if (j < _grid.y - 1 && i < _grid.x - 1) {
                        _meshStruct.Triangles[k + 0] = (j * _grid.x) + i;
                        _meshStruct.Triangles[k + 1] = ((j + 1) * _grid.x) + i;
                        _meshStruct.Triangles[k + 2] = (j * _grid.x) + i + 1;

                        _meshStruct.Triangles[k + 3] = ((j + 1) * _grid.x) + i;
                        _meshStruct.Triangles[k + 4] = ((j + 1) * _grid.x) + i + 1;
                        _meshStruct.Triangles[k + 5] = (j * _grid.x) + i + 1;

                        k += 6;
                    }   

                    // Set UV
                    float uvStepX = uvStepXInit;
                    float uvStepY = uvStepYInit;

                    _meshStruct.UV[index].x = i * uvStepX;
                    _meshStruct.UV[index].y = j * uvStepY;

                    // Set colors
                    //if (useObstructionData) {
                    //    _meshStruct.Colors32[index] = fieldObstruction[index] ? colorZero : colorOne;
                    //} else {
                        _meshStruct.Colors32[index] = colorOne;
                    //}

                    // Set normals
                    _meshStruct.Normals[index] = up;

                    if (setTangents) {
                        // set tangents
                        _meshStruct.Tangents[index] = tangent;
                    }

                    // fix stretching
                    float delta;

                    if (_meshStruct.Vertices[index].x > _size.x) {
                        delta = (_size.x - _meshStruct.Vertices[index].x) / _nodeSize;
                        _meshStruct.UV[index].x -= uvStepX * delta;

                        _meshStruct.Vertices[index].x = _size.x;
                    }

                    if (_meshStruct.Vertices[index].z > _size.y) {
                        delta = (_size.y - _meshStruct.Vertices[index].z) / _nodeSize;
                        _meshStruct.UV[index].y -= uvStepY * delta;

                        _meshStruct.Vertices[index].z = _size.y;
                    }

                    if (_size.x - _meshStruct.Vertices[index].x < _nodeSize) {
                        delta = (_size.x - _meshStruct.Vertices[index].x) / _nodeSize;
                        _meshStruct.UV[index].x += uvStepX * delta;

                        _meshStruct.Vertices[index].x = _size.x;
                    }

                    if (_size.y - _meshStruct.Vertices[index].z < _nodeSize) {
                        delta = (_size.y - _meshStruct.Vertices[index].z) / _nodeSize;
                        _meshStruct.UV[index].y += uvStepY * delta;

                        _meshStruct.Vertices[index].z = _size.y;
                    }
                }
            }
        }

        /// <summary>
        /// Allocates memory for Mesh arrays.
        /// </summary>
        private void AllocateMeshArrays() {
            int numVertices = _grid.x * _grid.y;
            _meshStruct.Vertices = new Vector3[numVertices];
            _meshStruct.Normals = new Vector3[numVertices];
            if (_settings.SetTangents)
                _meshStruct.Tangents = new Vector4[numVertices];
            
            _meshStruct.Colors32 = new Color32[numVertices];
            _meshStruct.UV = new Vector2[numVertices];
            _meshStruct.Triangles = new int[((_grid.x - 1) * (_grid.y - 1)) * 2 * 3];

            _isDirty = true;
        }

        /// <summary>
        /// Updates the mesh state using the simulation state field <paramref name="field"/> 
        /// and (optinal) the obstruction field.
        /// </summary>
        /// <param name="field">
        /// The simulation state field.
        /// </param>
        /// <param name="fieldObstruction">
        /// The array of <c>bool</c> indicating whether the water is obstructed by an object.
        /// </param>
        public void UpdateMesh(ref float[] field, ref bool[] fieldObstruction) {
            if (!_isDirty || !IsReady)
                return;

            // Caching some values to avoid expensive calls
            bool isFieldObstructionNull = fieldObstruction == null;
            bool calculateNormals = _settings.CalculateNormals;
            bool useFakeNormals = _settings.UseFakeNormals;
            bool normalizeNormals = _settings.NormalizeFakeNormals;

            Vector3 up = Vector3.up;
            Vector3 normal = up;

            // Minimal node value to calculate normals for (in case of using approximate normals).
            const float normalThreshold = 0.0001f;

            // If not using fast fake normals, then all we need is to updates the vertices
            if (!useFakeNormals) {
                for (int j = 0; j < _grid.y; j++) {
                    int index = j * _grid.x;
                    for (int i = 0; i < _grid.x; i++) {
                        _meshStruct.Vertices[index].y = field[index];

                        index++;
                    }
                }
            } else {
                for (int j = 0; j < _grid.y; j++) {
                    int index = j * _grid.x;
                    for (int i = 0; i < _grid.x; i++) {
                        if (!(i == 0 || j == 0 || i >= _grid.x - 1 || j >= _grid.y - 1)) {
                            if (!isFieldObstructionNull && fieldObstruction[index]) {
                                normal = up;
                            }  else {
                                float valAbs;
                                if (field[index] > 0f) {
                                    valAbs = field[index];
                                } else {
                                    valAbs = -field[index];
                                }

                                if (valAbs > normalThreshold) {
                                    normal.x = (field[index - 1] - field[index + 1]) * 2f;
                                    normal.z = (field[index - _grid.x] - field[index + _grid.x]) * 2f;
                                    normal.y = field[index] > 1f ? field[index] : 1f;
                                } else {
                                    normal = up;
                                }
                            }

                            if (normalizeNormals) {
                                // Fast approximate Vector3 normalization
                                // See also: LostPolygon.DynamicWaterNS.FastFunctions.FastInvSqrt()
                                float invLength = normal.x * normal.x + normal.y * normal.y + normal.z * normal.z;
                                FastFunctions.FloatIntUnion u;
                                u.i = 0;
                                u.f = invLength;
                                float xhalf = 0.5f * invLength;
                                u.i = 0x5f3759df - (u.i >> 1);
                                invLength = u.f * (1.5f - xhalf * u.f * u.f);

                                normal.x *= invLength;
                                normal.y *= invLength;
                                normal.z *= invLength;
                            }
                        } else {
                            normal = up;
                        }
                        _meshStruct.Normals[index] = normal;

                        _meshStruct.Vertices[index].y = field[index];
                        index++;
                    }
                }
            }

            // Actually updating the mesh
            _mesh.vertices = _meshStruct.Vertices;
            if (calculateNormals) {
                if (useFakeNormals) {
                    _mesh.normals = _meshStruct.Normals;
                } else {
                    _mesh.RecalculateNormals();
                }
            }

            _isDirty = false;
        }

    }
}
