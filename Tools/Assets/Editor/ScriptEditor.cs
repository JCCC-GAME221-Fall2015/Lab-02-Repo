using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptEditor : MonoBehaviour {

	[MenuItem("Project Tools/Create Prefab")]
    public static void CreatePrefab()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach(GameObject obj in selectedObjects)
        {
            string name = obj.name;
            string assetPath = "Assets/" + name + ".prefab";

            

            if(AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)))
            {
                //Debug.Log("Asset exists");

                if (EditorUtility.DisplayDialog("CAUTION", "Prefab already exists. " +
                                    "Do you want to overwrite?", "Yes", "No"))
                {
                    Debug.Log("Overwriting");
                    CreateNew(obj, assetPath);
                }
            }

            else
            {
                //Debug.Log("Asset does not exist");
                CreateNew(obj, assetPath);
            }
            //Debug.Log("Name: " + name + " Path: " + assetPath);
        }
    }

    public static void CreateNew(GameObject obj, string location)
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab(location);
        PrefabUtility.ReplacePrefab(obj, prefab);
        AssetDatabase.Refresh();

        DestroyImmediate(obj);
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

    }
}
