﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakePrefabs : MonoBehaviour {

	[MenuItem("Project Tools/Create Prefab")]
	public static void CreatePrefab()
	{
		GameObject[] selectedObjects = Selection.gameObjects;

		foreach(GameObject go in selectedObjects)
		{
			string name = go.name;
			string assetPath = "Assets/" + name + ".prefab";

			if (AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)))
			{
				Debug.Log("Asset exists!");
				if (EditorUtility.DisplayDialog("Caution", "Prefab already exists.  " +
				                                "Do you want to overwrite?", "Yes", "No"))
				{
					CreateNew(go, assetPath);
//					Debug.Log("overwriting!");
				}
			}
			else
			{
				CreateNew(go, assetPath);
//				Debug.Log("Asset does not exist!");
			}
//			Debug.Log("Name: " + go.name + " Path: " + assetPath);
		}
	} // end method CreatePrefab

	public static void CreateNew(GameObject obj, string location)
	{
		Object prefab = PrefabUtility.CreateEmptyPrefab(location);
		PrefabUtility.ReplacePrefab(obj, prefab);
		AssetDatabase.Refresh();

		DestroyImmediate(obj);
		GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
	}

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
} // end class MakePrefabs
