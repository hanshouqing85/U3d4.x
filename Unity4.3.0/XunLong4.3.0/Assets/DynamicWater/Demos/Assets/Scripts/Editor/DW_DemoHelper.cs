using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DW_DemoHelper : MonoBehaviour {
    readonly static string[] demoScenes = new string[] { "DW_Menu", "DW_Waterfall", "DW_Buoyancy", "DW_BuoyancyMobile", "DW_Pool", "DW_PoolMobile", "DW_Boat", "DW_BoatMobile" };

    static DW_DemoHelper() {
        EditorApplication.playmodeStateChanged += CheckLayers;
    }

    private static void CheckLayers() {
        if (!Application.isPlaying || !EditorApplication.isPlaying || !EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        string scene = Application.loadedLevelName;
        bool flag = false;
        foreach (string x in demoScenes) {
            if (x == scene) {
                flag = true;
                break;
            }
        }
        if (flag) {
            CheckLayer(DynamicWater.PlaneColliderLayerName);
        }
    }

    private static void CheckLayer(string layerName) {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1) {
            EditorUtility.DisplayDialog("Layer not defined",
                                        string.Format("In order to run this demo, you have to define layer {0}. " +
                                                      "Define the layer and restart the demo.\n\n" +
                                                      "(you can copy the layer name from Console)", layerName), "OK");

            Debug.LogError("Layer " + layerName + " not defined!");
        }
    }

}
