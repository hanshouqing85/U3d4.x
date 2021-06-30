using UnityEngine;
using System.Collections;

public class Myutils : MonoBehaviour {
	
	public static GameObject makeGameObject(GameObject gb,Transform pos){
		GameObject gbb=Instantiate(gb,pos.position,pos.rotation) as GameObject;
		gbb.transform.parent=pos.parent;
		gbb.transform.localScale=pos.localScale;
	    return gbb;       
	}
	 public static string getConfigPath()
    {
        string url = string.Empty;
#if UNITY_EDITOR
        url = "file://" + Application.dataPath + "/../" + "Config/";
#endif
#if UNITY_WEBPLAYER && !UNITY_EDITOR
        url = "../AssetBundle/";
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        url = "jar:file://" + Application.dataPath + "/Config/";
#endif
#if (UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
        url = "file://" + Application.dataPath + "/" + "Config/";
#endif
        /*
        if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            url = "../Config/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            url = "file://" + Application.dataPath + "/../" + "Config/";
        }
        else if (Application.platform == RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
        {
            url = "jar:file://" + Application.dataPath + "/Config/";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.OSXEditor)
        {
            url = "file://" + Application.dataPath + "/" + "Config/";
        }
        */
        return url;
    }
	public static string getDataPath()
    {
        string url = string.Empty;
#if UNITY_EDITOR
        url = "file://" + Application.dataPath + "/../" + "";
#endif
#if UNITY_WEBPLAYER && !UNITY_EDITOR
        url = "../AssetBundle/";
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        url = "jar:file://" + Application.dataPath + "/";
#endif
#if (UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
        url = "file://" + Application.dataPath + "/" + "";
#endif
        /*
        if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            url = "../img/";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            url = "file://" + Application.dataPath + "/../" + "";
        }
        else if (Application.platform == RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor)
        {
            url = "jar:file://" + Application.dataPath + "/";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.OSXEditor)
        {
            url = "file://" + Application.dataPath + "/" + "";  
        }
        */
        return url;
    }
}
