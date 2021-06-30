using System;
using UnityEditor;
using UnityEngine;
using LostPolygon.DynamicWaterSystem;

[CustomEditor(typeof(FluidVolume))]
public class DynamicWaterFluidVolumeEditor : UndoEditor<FluidVolume> {

    protected override void OnInspectorGUIDraw() {
        EditorGUILayout.HelpBox("Simulation", MessageType.None, true);
        float sizeX = Mathf.Clamp(EditorGUILayout.FloatField("Length", _object.Size.x), 0f, float.PositiveInfinity);
        float sizeY = Mathf.Clamp(EditorGUILayout.FloatField("Width", _object.Size.y), 0f, float.PositiveInfinity);

        _object.Size = new Vector2(sizeX, sizeY);

        _object.Depth = Mathf.Clamp(EditorGUILayout.FloatField("Height", _object.Depth), 0f, float.PositiveInfinity);
        _object.Density = Mathf.Clamp(EditorGUILayout.FloatField("Density", _object.Density), 0f, 10000f);
    }

    protected override void OnSceneGUIDraw() {
        Vector3 pos = _object.transform.position + _object.transform.right * _object.Size.x;
        Handles.DrawLine(_object.transform.position, pos);
        float sizeX = _object.transform.InverseTransformPoint(Handles.Slider(pos, _object.transform.right,
                                                                      HandleUtility.GetHandleSize(pos) * 0.15f, Handles.CubeCap,
                                                                      1f)).x;

        pos = _object.transform.position + _object.transform.forward * _object.Size.y;
        Handles.DrawLine(_object.transform.position, pos);
        float sizeY = _object.transform.InverseTransformPoint(Handles.Slider(pos, _object.transform.forward,
                                                                      HandleUtility.GetHandleSize(pos) * 0.15f, Handles.CubeCap,
                                                                      1f)).z;

        _object.Size = new Vector2(sizeX, sizeY);
    }

    protected override void OnSceneGUIUndo() {
        Vector2 size = _object.Size;
        _object.Size = new Vector2(0f, 0f);
        _object.Size = size;
    }
}
