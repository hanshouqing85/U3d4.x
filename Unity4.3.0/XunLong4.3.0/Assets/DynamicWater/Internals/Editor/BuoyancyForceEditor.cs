using System;
using UnityEditor;
using UnityEngine;
using LostPolygon.DynamicWaterSystem;

[CustomEditor(typeof(BuoyancyForce))]
public class BuoyancyForceEditor : UndoEditor<BuoyancyForce> {
    protected override void OnInspectorGUIDraw() {
        _object.UseDensity = EditorGUILayout.Toggle("Use density", _object.UseDensity);
        if (_object.UseDensity) {
            _object.Density = Mathf.Clamp(EditorGUILayout.FloatField("Density", _object.Density), 0.1f, float.PositiveInfinity);
            _object.SetMassByDensity = EditorGUILayout.Toggle("Set mass by density", _object.SetMassByDensity);
        }
            
        _object.Resolution = EditorGUILayout.IntSlider("Quality", _object.Resolution, 1, 15);
        _object.ProcessChildren = EditorGUILayout.Toggle("Process children", _object.ProcessChildren);
        _object.DragInFluid = Mathf.Clamp(EditorGUILayout.FloatField("Drag in fluid", _object.DragInFluid), 0f, float.PositiveInfinity);
        _object.AngularDragInFluid = Mathf.Clamp(EditorGUILayout.FloatField("Angular drag in fluid", _object.AngularDragInFluid), 0f, float.PositiveInfinity);
        _object.SplashForceFactor = EditorGUILayout.Slider("Splash force factor", _object.SplashForceFactor, 0f, 200f);
        _object.MaxSplashForce = EditorGUILayout.Slider("Max splash force", _object.MaxSplashForce, 0f, 200f);
    }

    protected override void OnSceneGUIDraw() {
        //
    }

    protected override void OnSceneGUIUndo() {
        //
    }
}
