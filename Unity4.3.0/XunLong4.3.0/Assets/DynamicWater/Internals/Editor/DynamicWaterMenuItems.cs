using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DynamicWaterMenuItems : MonoBehaviour {

    private static GameObject CreateGameObject<T>() where T : Component {
        Undo.RegisterSceneUndo("Create_" + typeof(T).Name);
        GameObject go = new GameObject(typeof(T).Name);
        go.AddComponent<T>();
        Selection.activeGameObject = go;

        return go;
    }

    [MenuItem("GameObject/Create Other/Dynamic Water")]
    static void CreateDynamicWater() {
        CreateGameObject<DynamicWater>();
    }

    [MenuItem("GameObject/Create Other/Fluid Volume")]
    static void CreateFluidVolume() {
        CreateGameObject<FluidVolume>();
    }

    [MenuItem("GameObject/Create Other/Splash Zone")]
    static void CreateSplashZone() {
        CreateGameObject<SplashZone>();
    }

    [MenuItem("Component/Lost Polygon/Dynamic Water System/Open Online Documentation")]
    static void OpenOnlineDocumentation() {
        Application.OpenURL("http://docs.zimm.biz/dynamicwater/");
    }

}
