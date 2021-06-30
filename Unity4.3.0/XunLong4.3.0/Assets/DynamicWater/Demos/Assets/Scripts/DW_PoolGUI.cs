using UnityEngine;
using LostPolygon.DynamicWaterSystem;

/// <summary>
/// GUI used for demos. 
/// </summary>
public class DW_PoolGUI : MonoBehaviour {

    public Transform BuoyantCrate;
    public DynamicWater Water = null;

    public SplashZone RainZone;
    public SplashZone WaterfallSplashZone;
    public ParticleSystem WaterfallParticleSystem;
    public Texture2D Logo;
    private bool visible = true;


    private string _sceneName;

    void OnLevelWasLoaded(int level) {
        DW_CameraFade.StartAlphaFade(Color.black, true, 0.5f, 0.5f);
    }

    void Start() {
        _sceneName = Application.loadedLevelName;
        if ((_sceneName == "DW_Pool" || _sceneName == "DW_Waterfall") && DW_GUILayout.IsRuntimePlatformMobile())
            Water.Quality = 48;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            DW_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("DW_Menu"));

        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Return))
            visible = !visible;
    }

    void OnGUI() {
        if (!visible || Water == null)
            return;

        if (DW_GUILayout.IsRuntimePlatformMobile())
            DW_GUILayout.UpdateScale();

        // Initializing depending on platform and scene
        string text;
        bool toggleChanged;

        float initWidth = 275f;
        float initHeight = 610f;
        float initItemHeight = 30f;
        switch (_sceneName) {
            case "DW_PoolMobile":
                initHeight = 550f;
                break;
            case "DW_Waterfall":
                initHeight = 540f;
                break;
        }
        
        if (DW_GUILayout.IsRuntimePlatformMobile()) {
            initHeight = 480f - 15f * 2f;

            switch (_sceneName) {
                case "DW_PoolMobile":
                    initItemHeight = 31f;
                    break;
                case "DW_Waterfall":
                    initItemHeight = 31f;
                    break;
                default:
                    initItemHeight = 36f;
                    break;
            }
        }

        DW_GUILayout.itemWidth = initWidth - 20f;
        DW_GUILayout.itemHeight = initItemHeight;
        DW_GUILayout.yPos = 0f;
        DW_GUILayout.hovered = false;

        switch (_sceneName) {
            case "DW_PoolMobile":
                text = "Pool Scene (Mobile Optimized)";
                break;
            case "DW_Waterfall":
                text = "Waterfall Scene";
                break;
            case "DW_Pool":
            default:
                text = "Pool Scene";
                break;
        }

        GUILayout.BeginArea(new Rect(15f, 15f, initWidth, initHeight), text, "Window");
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleLeft;

            DW_GUILayout.yPos = 20f;
            
            switch (_sceneName) {
                case "DW_Pool":
                    text = "This scene demonstrates some basic abilities of Dynamic Water:\n" +
                                  " - interacting with water surface;\n" +
                                  " - static obstructions;\n" +
                                  " - buoyant objects that make splashes on water;\n" + 
                                  " - usage of SplashZone component.\n" +
                                  "You can also create some crates and move objects around.";
                    if (DW_GUILayout.IsRuntimePlatformMobile())
                        text = "This scene demonstrates some basic abilities of Dynamic Water.";
                    break;
                case "DW_PoolMobile":
                    text = "This is the same scene as Pool, but optimized for low-end " +
                            "mobile GPUs. " +
                            "Try \"Use fast fake normals\" and disable GUI " +
                           "(Enter/menu button) for more performance!";
                    break;
                case "DW_Waterfall":
                    text = "This demo demonstrates a practical use of SplashZone " +
                           "component for creating a waterfall effect. Note how you " +
                           "can use multiple SplashZones on a single water instance, " +
                           "like for the rain and for waterfall.";
                    break;
                default:
                    text = "";
                    break;
            }

            DW_GUILayout.itemHeight = centeredStyle.CalcHeight(new GUIContent(text, ""), DW_GUILayout.itemWidth);
            DW_GUILayout.Label(text);
            DW_GUILayout.itemHeight = initItemHeight;
            
            DW_GUILayout.Space(5);
            DW_GUILayout.Box("Simulation");

            if (BuoyantCrate != null) {
                DW_GUILayout.tooltip = "Drops a crate into water. You can drag it around to see how it makes splashes when going in and out of water.";
                if (DW_GUILayout.Button("Drop a box!", 180f)) {
                    Bounds bounds = Water.GetComponent<Collider>().bounds;
                    Instantiate(BuoyantCrate, new Vector3(Random.Range(bounds.min.x, bounds.max.x), 10f, Random.Range(bounds.min.z, bounds.max.z)),
                                Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)));
                }
                DW_GUILayout.Space(5);
            }

            if (_sceneName == "DW_Waterfall" && WaterfallSplashZone != null && WaterfallParticleSystem != null) {
                DW_GUILayout.tooltip = "Enables or disables waterfall. Waves create by waterfall are controlled by SplashZone component.";
                WaterfallSplashZone.IsRaining = DW_GUILayout.Toggle(WaterfallSplashZone.IsRaining, "Waterfall", out toggleChanged);
                #if UNITY_3_5
                WaterfallParticleSystem.gameObject.active = WaterfallSplashZone.IsRaining;
                #else
                WaterfallParticleSystem.gameObject.SetActive(WaterfallSplashZone.IsRaining);
                #endif
                DW_GUILayout.Space(10);
            }

            DW_GUILayout.tooltip = "Enables or disables raindrops. Raindrops are controlled by SplashZone component.";
            RainZone.IsRaining = DW_GUILayout.Toggle(RainZone.IsRaining, "Raindrops", out toggleChanged);
            DW_GUILayout.Space(10);

            DW_GUILayout.tooltip = "The damping defines how fast waves dissipate. The bigger the value, the faster they will go away.\n" +
                                   "  0 - waves never dissipate (some odd behaviour may occur);\n  1 - waves dissipate instantly.";
            Water.Damping = DW_GUILayout.Slider(Water.Damping, "Damping: ", 0f, 1f);

            DW_GUILayout.Space(10);
            DW_GUILayout.tooltip = "Controls the speed of wave propagation. Maximal value is limited by Quality.";
            Water.Speed = DW_GUILayout.Slider(Water.Speed, "Speed: ", 0f, Water.MaxSpeed());

            DW_GUILayout.Space(10);
            DW_GUILayout.Box("Quality");
            DW_GUILayout.tooltip = string.Format("Simulation quality. Larger values result in more smooth visuals.\n\nGrid:{0}x{1}, {2} vertices, {3} triangles.",
                                                Water.GridSize.x, Water.GridSize.y, Water.GridSize.x * Water.GridSize.y, Water.GridSize.x * Water.GridSize.y * 2);
            Water.Quality = (int)DW_GUILayout.Slider(Water.Quality, "Quality: ", 25f, _sceneName == "DW_PoolMobile" ? 120 : 256);

            DW_GUILayout.tooltip = string.Format("Simulation quality. Larger values result in more smooth visuals.\n\nGrid: {0}x{1}, {2} vertices, {3} triangles.",
                                Water.GridSize.x, Water.GridSize.y, Water.GridSize.x * Water.GridSize.y, Water.GridSize.x * Water.GridSize.y * 2);

            DW_GUILayout.tooltip = "This option enables another method of mesh normals calculation. " +
                                   "It can massively increase performance (up to 6x), especially on high Quality levels " +
                                   "and mobile devices (with the cost of lighting quality degradation).\n";
            Water.UseFakeNormals = DW_GUILayout.Toggle(Water.UseFakeNormals, "Use fast fake normals:", out toggleChanged);

            centeredStyle.alignment = TextAnchor.UpperLeft;

            GUI.color = Color.cyan;
            if (DW_GUILayout.hovered) {
                DW_GUILayout.Space(10);
                DW_GUILayout.itemHeight = 200f;
                DW_GUILayout.Label(DW_GUILayout.tooltipActive);
                DW_GUILayout.itemHeight = initItemHeight;
            }

            GUI.color = new Color(1f, 0.6f, 0.6f, 1f);
            if (GUI.Button(new Rect(DW_GUILayout.paddingLeft, initHeight - 40f, DW_GUILayout.itemWidth, 30f), "Back to Main Menu")) {
                DW_CameraFade.StartAlphaFade(Color.black, false, 0.5f, 0f, () => Application.LoadLevel("DW_Menu"));
            }

        GUILayout.EndArea();

        GUI.color = Color.white;
        DW_GUILayout.DrawLogo(Logo);
    }

}
