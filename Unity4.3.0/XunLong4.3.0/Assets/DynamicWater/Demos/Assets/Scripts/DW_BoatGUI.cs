using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// GUI used for Buoyancy demo. 
/// </summary>
public class DW_BoatGUI : MonoBehaviour {
    public Texture2D Logo;
    private bool visible = true;

    void OnLevelWasLoaded(int level) {
        DW_CameraFade.StartAlphaFade(Color.black, true, 0.5f, 0.5f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            DW_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("DW_Menu"));

        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Return))
            visible = !visible;
    }

    void OnGUI() {
        if (!visible)
            return;
        if (DW_GUILayout.IsRuntimePlatformMobile())
            DW_GUILayout.UpdateScale();

        const float initWidth = 275f;
        const float initHeight = 185f;
        const float initItemHeight = 30f;

        DW_GUILayout.itemWidth = initWidth - 20f;
        DW_GUILayout.itemHeight = initItemHeight;
        DW_GUILayout.yPos = 0f;
        DW_GUILayout.hovered = false;

        GUILayout.BeginArea(new Rect(15f, 15f, initWidth, initHeight), "Boat Demo Scene", "Window");
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleLeft;

            DW_GUILayout.yPos = 20f;

            string text = "This demo shows how a simple boat controller can be implemented.\n\n" +
                          "Controls:\n";
            if (DW_GUILayout.IsRuntimePlatformMobile()) {
                text += "Tilt your device to control the boat.";
            } else {
                text += "Movement - Arrow keys or WSAD\n" +
                        "Camera - use wheel for zoom, use right mouse button to orbit camera";
            }
                
            DW_GUILayout.itemHeight = centeredStyle.CalcHeight(new GUIContent(text, ""), DW_GUILayout.itemWidth);
            DW_GUILayout.Label(text);
            DW_GUILayout.itemHeight = initItemHeight;

            GUI.color = new Color(1f, 0.6f, 0.6f, 1f);
            if (GUI.Button(new Rect(DW_GUILayout.paddingLeft, initHeight - 40f, DW_GUILayout.itemWidth, 30f), "Back to Main Menu")) {
                DW_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("DW_Menu"));
            }

        GUILayout.EndArea();

        GUI.color = Color.white;
        DW_GUILayout.DrawLogo(Logo);
    }

}
