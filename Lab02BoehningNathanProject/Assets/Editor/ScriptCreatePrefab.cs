using UnityEngine;
using System.Collections;
using UnityEditor;

// Code gotten and modified from lab 2
// Author: Tiffany Fischer
// Modifier: Nathan Boehning

public class ScriptCreatePrefab : MonoBehaviour {

    // Create a menu button to create a prefab in the project tools dropdown
	[MenuItem("Project Tools/Create Prefab")]
	public static void CreatePrefab()
	{
        // Create array to hold selected objects that you are making prefabs of
		GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through all selected objects
		foreach(GameObject curObject in selectedObjects)
		{
            // Get the name of the object
			string name = curObject.name;
            // set the asset path of the selected object
            string assetPath = "Assets/" + name + ".prefab";

            // Check to see if asset already exists at location
			if(AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)))
			{
				//Debug.Log ("Asset exists!");

                // Create a dialogue box prompting user if they want to overwrite preexisting prefab
				if(EditorUtility.DisplayDialog("Caution", "Prefab already exists. " + 
				                               "Do you want to overwrite?",
				                               "Yes", "No"))
				{
                    // If user says yes create the prefab at designated path.
					CreateNew(curObject, assetPath);
				}
			}
			else
			{
                // Prefab doesn't exist already, create a prefab
				CreateNew(curObject, assetPath);
			}
		}
	}

    // Function to create the prefab.
	public static void CreateNew(GameObject obj, string location)
	{
        // Create an empty prefab at inputted location
		Object prefab = PrefabUtility.CreateEmptyPrefab(location);

        // Replace the empty prefab with the inputted object
		PrefabUtility.ReplacePrefab(obj, prefab);
        
        // Refresh the assets
		AssetDatabase.Refresh();

        // Destroy the empty prefab
		DestroyImmediate(obj);

        // Instantiate the prefab
		PrefabUtility.InstantiatePrefab(prefab);
	}
}
