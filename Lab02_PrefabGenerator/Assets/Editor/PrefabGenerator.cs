using UnityEngine;
using System.Collections;
using UnityEditor;

public class PrefabGenerator : MonoBehaviour 
{
    [MenuItem("Project Tools/Create Prefab")]
    public static void CreatePrefab()
    {
        GameObject[] selected = Selection.gameObjects;
        foreach (GameObject obj in selected)
        {
            string name = obj.name;
            string assetPath = "Assets/" + name + ".prefab";

            /** 
             * Determine if prefab exists.
             * If not, create a new prefab.
             * If it does, warn the user and ask if they want to overwrite.
             */
            if (AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Caution", "Prefab already exists." + "\nDo you want to overwrite?", "Yes", "No"))
                    CreateNew(obj, assetPath);
            }
            else
                CreateNew(obj, assetPath);
        }
    }

    /**
     * CreateNew creates the prefab object.
     */
    public static void CreateNew(GameObject obj, string location)
    {
        // Create the prefab
        Object prefab = PrefabUtility.CreateEmptyPrefab(location);
        PrefabUtility.ReplacePrefab(obj, prefab);
        AssetDatabase.Refresh();

        // Destroy the original GameObject
        DestroyImmediate(obj);
        // GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
    }
}
