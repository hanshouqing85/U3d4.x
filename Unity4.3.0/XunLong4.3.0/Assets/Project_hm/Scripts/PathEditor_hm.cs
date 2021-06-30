using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu("game/PathEditor_hm")]
public class PathEditor_hm : MonoBehaviour {
	public string pathName ="";
	public Color pathColor = Color.cyan;
	public Vector3[] path ={new Vector3(0,0,0),new Vector3(2,2,2)};
	public static Dictionary<string, PathEditor_hm> paths = new Dictionary<string, PathEditor_hm>();
	public bool pathVisible = true;
	public LTSpline cr;
	public bool debugluxian=false;
	//public GameObject Dragon;
	public float flyTime=15f;
	//public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	//public int nodeCount;
	//public bool initialized = false;
	//public string initialName = "";
	public int pathID=-1;

	void OnEnable(){
		if(!paths.ContainsKey(pathName)){
			paths.Add(pathName.ToLower(), this);
		}
		cr = new LTSpline(path);
	}

	void OnDisable(){
		paths.Remove(pathName.ToLower());
	}

	void OnDrawGizmosSelected(){
		if(pathVisible){
				if(cr!=null)
				OnEnable();
				Gizmos.color = pathColor;
				if(cr!=null)
				cr.gizmoDraw(); // To Visualize the path, use this method
		}
	}

	public static Vector3[] GetPath(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
			//return paths[requestedName].nodes.ToArray();
			return paths[requestedName].path;
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}

	public static Vector3[] GetPathReversed(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
			//List<Vector3>  revNodes = paths[requestedName].nodes.GetRange(0,paths[requestedName].nodes.Count);
			List<Vector3>  revNodes = new List<Vector3>( paths[requestedName].path);
			revNodes.Reverse();
			return revNodes.ToArray();
		}else{
			Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
			return null;
		}
	}


	void OnDrawGizmos()
	{ 
		if (debugluxian) {
			for (int i=1; i<path.Length-1; i++)
				Gizmos.DrawIcon (path [i], "pathNode.tif", true);
		}
	}
}

