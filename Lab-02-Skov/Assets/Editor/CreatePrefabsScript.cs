using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreatePrefabsScript : MonoBehaviour 
{
    [MenuItem("Project Tools/Create Prefab")]
    
    //Creates a prefab from a seleceted gameObject
    public static void CreatePrefab()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        //sets all selected gameObjects to prefabs and places them in the default assets folder
        foreach (GameObject go in selectedObjects)
        { 
            string name = go.name;
            string assetPath = "Assets/" + name + ".prefab";

            if (AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Caution", "Prefab already exists!\n" + "Do you want to overwrite?", "Yes", "No"))
                {
                    CreateNew(go, assetPath);
                }
            }
            else
            {
                CreateNew(go, assetPath);
            }
        }
    }

    //Creates a new object at a set location
    public static void CreateNew(GameObject obj, string location)
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab(location);

        PrefabUtility.ReplacePrefab(obj, prefab);
        AssetDatabase.Refresh();

        DestroyImmediate(obj);
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
    }
}
