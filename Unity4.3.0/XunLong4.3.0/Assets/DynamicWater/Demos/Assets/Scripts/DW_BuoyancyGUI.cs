using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// GUI used for Buoyancy demo. 
/// </summary>
public class DW_BuoyancyGUI : MonoBehaviour {
    public Texture2D Logo;
    private bool visible = true;
    private string _sceneName;

    void OnLevelWasLoaded(int level) {
        DW_CameraFade.StartAlphaFade(Color.black, true, 0.5f, 0.5f);
    }

    void Start() {
        _sceneName = Application.loadedLevelName;
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
        const float initHeight = 395f;
        const float initItemHeight = 30f;

        DW_GUILayout.itemWidth = initWidth - 20f;
        DW_GUILayout.itemHeight = initItemHeight;
        DW_GUILayout.yPos = 0f;
        DW_GUILayout.hovered = false;

        GUILayout.BeginArea(new Rect(15f, 15f, initWidth, initHeight), "Buoyancy Demo Scene", "Window");
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleLeft;

            DW_GUILayout.yPos = 20f;

            string text;
            if (_sceneName == "DW_BuoyancyMobile") {
                text = "This demo shows how buoyancy force can be applied to objects of any shape and configuration." +
                             "Note that the boat actually consists of multiple child objects with BoxCollider, and with \"Process children\" enabled on parent.\n" +
                             "This demo also shows how to create and modify wave functions to get the desired effect " +
                             "(see DynamicWaterSolverAmbientSimple.cs for example).\n" +
                             "Also note that huge waves like the ones in this demo can be effectively created using " +
                             "very small quality level (this demo uses 20x20 grid, total 441 vertices, 800 triangles). " +
                             "However, actual wave simulation is disabled in this demo, as it doesn't scales well for large surfaces.\n\n" +
                             "You can also drag objects to see how buoyancy is applied.";
            }
            else {
                 text = "This demo shows how buoyancy force can be applied to objects of any shape and configuration. " +
                              "Note that the boat actually consists of three child objects with \"Process children\" enabled on parent.\n\n" +
                              "This demo also shows how to create and modify wave functions to get the desired effect " +
                              "(see DynamicWaterSolverAmbientSimple.cs for example).\n" +
                              "Also note that huge waves like in this demo can be effectively created using" +
                              "very small quality level (this demo uses 20x20 grid, total 441 vertices, 800 triangles). " +
                              "However, actual wave simulation is disabled in this demo, as it doesn't scales well for large surfaces.\n\n" +
                              "You can also drag objects to see how buoyancy is applied.";
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
