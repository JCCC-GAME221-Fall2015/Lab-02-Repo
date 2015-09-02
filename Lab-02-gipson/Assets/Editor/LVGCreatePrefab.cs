using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Author: Matt Gipson
/// Contact: Deadwynn@gmail.com
/// Domain: www.livingvalkyrie.com
/// 
/// Description: CreatePrefab 
/// </summary>
public class LVGCreatePrefab : MonoBehaviour {
    #region Fields

    #endregion

    [MenuItem("Project Tools/Create Prefab")]
    public static void CreatePrefab() {
        //get all selected game objects
        GameObject[] selectedObjects = Selection.gameObjects;

        //loop through all objects
        foreach (GameObject selectedObject in selectedObjects) {
            string name = selectedObject.name;
            string assetPath = "Assets/" + name + ".prefab";

            //test if prefab exist
            if (AssetDatabase.LoadAssetAtPath(assetPath, typeof (GameObject))) {
                print("Asset exist");
                if (EditorUtility.DisplayDialog("Caution!",
                                                "Prefab already exist. " +
                                                "Do you want to overwrite?",
                                                "Yes",
                                                "No")) {
                    CreateNew(selectedObject, assetPath);
                }
            }
            else {
                CreateNew( selectedObject, assetPath );
            }

            //print out 
           // print("Name: " + name + " Path: " + assetPath);
        }
    }

    public static void CreateNew(GameObject selected, string path) {
        Object prefab = PrefabUtility.CreateEmptyPrefab(path);
        PrefabUtility.ReplacePrefab(selected, prefab);
        AssetDatabase.Refresh();
        
        DestroyImmediate(selected);
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;


    }

}