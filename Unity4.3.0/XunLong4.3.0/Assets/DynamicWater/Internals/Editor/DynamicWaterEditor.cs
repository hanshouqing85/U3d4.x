using System;
using UnityEditor;
using UnityEngine;
using LostPolygon.DynamicWaterSystem;

[CustomEditor(typeof(DynamicWater))]
public class DynamicWaterEditor : UndoEditor<DynamicWater> {
    protected override void OnInspectorGUIDraw() {


        EditorGUILayout.HelpBox("Simulation", MessageType.None, true);
        _object.Quality = EditorGUILayout.IntSlider("Quality", _object.Quality, 4, 256);   

        float sizeX = Mathf.Clamp(EditorGUILayout.FloatField("Length", _object.Size.x), 0f, float.PositiveInfinity);
        float sizeY = Mathf.Clamp(EditorGUILayout.FloatField("Width", _object.Size.y), 0f, float.PositiveInfinity);
       
        _object.Size = new Vector2(sizeX, sizeY);

        _object.Damping = EditorGUILayout.Slider("Damping", _object.Damping, 0f, 1f);
        _object.Speed = EditorGUILayout.Slider("Speed", _object.Speed, 0f, _object.MaxSpeed());
        _object.Depth = Mathf.Clamp(EditorGUILayout.FloatField("Depth", _object.Depth), 0f, float.PositiveInfinity);
        _object.Density = Mathf.Clamp(EditorGUILayout.FloatField("Density", _object.Density), 0f, 10000f);
        _object.UseObstructions = EditorGUILayout.Toggle("Use obstructions", _object.UseObstructions);
        _object.UsePlaneCollider = EditorGUILayout.Toggle("Use plane collider", _object.UsePlaneCollider);
        _object.UpdateWhenNotVisible = EditorGUILayout.Toggle("Update if not visible", _object.UpdateWhenNotVisible);

        EditorGUILayout.HelpBox("Rendering", MessageType.None, true);
        _object.CalculateNormals = EditorGUILayout.Toggle("Calculate normals", _object.CalculateNormals);
        if (_object.CalculateNormals) {
            _object.UseFakeNormals = EditorGUILayout.Toggle(new GUIContent("Fast fake normals", 
                    "Indicates whether the fast approximate method of calculating the water mesh normals should be used.\n" +
                    "Enabling this could provide a huge performance boost with the cost of degraded quality. " +
                    "Especially useful on mobile devices"), _object.UseFakeNormals);
            if (_object.UseFakeNormals) {
                EditorGUILayout.BeginVertical();
                _object.NormalizeFakeNormals = EditorGUILayout.Toggle(new GUIContent("Normalize normals", 
                    "Indicates whether to normalize fast approximate normals."), _object.NormalizeFakeNormals);
                EditorGUILayout.EndVertical();
            }
        }

        _object.SetTangents = EditorGUILayout.Toggle(new GUIContent("Set tangents", "Whether the tangents must be set (usually for bump-mapped shaders)" +
                                                                                    "Enabling this may sometimes result in performance drop on high" +
                                                                                    " Quality levels. It is better to turn it off if " +
                                                                                    "your shader doesn't uses normals."), _object.SetTangents);
    }

    protected override void OnSceneGUIDraw() {
        Vector3 pos = _object.transform.position + _object.transform.right * _object.Size.x;
        Handles.DrawLine(_object.transform.position, pos);
        float sizeX = Mathf.Clamp(_object.transform.InverseTransformPoint(Handles.Slider(pos, _object.transform.right,
                                                                          HandleUtility.GetHandleSize(pos) * 0.15f, Handles.CubeCap,
                                                                          1f)).x, 0.001f, float.PositiveInfinity);

        pos = _object.transform.position + _object.transform.forward * _object.Size.y;
        Handles.DrawLine(_object.transform.position, pos);
        float sizeY = Mathf.Clamp(_object.transform.InverseTransformPoint(Handles.Slider(pos, _object.transform.forward,
                                                                          HandleUtility.GetHandleSize(pos) * 0.15f, Handles.CubeCap,
                                                                          1f)).z, 0.001f, float.PositiveInfinity);

        _object.Size = new Vector2(sizeX, sizeY);
    }

    protected override void OnSceneGUIUndo() {
        Vector2 size = _object.Size;
        _object.Size = new Vector2(0f, 0f);
        _object.Size = size;
    }
}
