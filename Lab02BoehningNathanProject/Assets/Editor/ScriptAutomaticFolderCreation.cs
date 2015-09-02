using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ScriptAutomaticFolderCreation : MonoBehaviour {
	
	[MenuItem("Tool Creation/Create folder")]
	public static void CreateFolder()
	{
		AssetDatabase.CreateFolder("Assets", "Dynamic Assets");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets", "Resources");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Animations");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Animations", "Sources");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Animation Controllers");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Effects");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Models");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Models", "Character");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Models", "Environment");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Prefabs");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Prefabs", "Common");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Sounds");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Sounds", "Music");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Sounds/Music", "Common");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Sounds", "SFX");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Sounds/SFX", "Common");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources", "Textures");
        AssetDatabase.CreateFolder("Assets/Dynamic Assets/Resources/Textures", "Common");
        AssetDatabase.CreateFolder("Assets", "Editor");
		AssetDatabase.CreateFolder("Assets", "Extensions");
		AssetDatabase.CreateFolder("Assets", "Gizmos");
		AssetDatabase.CreateFolder("Assets", "Plugins");
		AssetDatabase.CreateFolder("Assets", "Scripts");
        AssetDatabase.CreateFolder("Assets/Scripts", "Common");
        AssetDatabase.CreateFolder("Assets", "Shaders");
	    AssetDatabase.CreateFolder("Assets", "Static Assets");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Animations");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Animations", "Sources");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Animation Controllers");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Effects");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Models");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Models", "Character");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Models", "Environment");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Prefabs");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Prefabs", "Common");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Scenes");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Sounds");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Sounds", "Music");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Sounds/Music", "Common");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Sounds", "SFX");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Sounds/SFX", "Common");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources", "Textures");
        AssetDatabase.CreateFolder("Assets/Static Assets/Resources/Textures", "Common");
        AssetDatabase.CreateFolder("Assets", "Testing");

		System.IO.File.WriteAllText(Application.dataPath + "/Dynamic Assets/Resources/folderStructure.txt", "This folder is for storing Resources in the dynamic assets folder");
        System.IO.File.WriteAllText(Application.dataPath + "/Editor/folderStructure.txt", "This folder is for storing editor altering files");
        System.IO.File.WriteAllText(Application.dataPath + "/Extensions/folderStructure.txt", "This folder is for storing extensions");
		System.IO.File.WriteAllText(Application.dataPath + "/Gizmos/folderStructure.txt", "This folder is for storing gizmos");
		System.IO.File.WriteAllText(Application.dataPath + "/Plugins/folderStructure.txt", "This folder is for storing plugins");
		System.IO.File.WriteAllText(Application.dataPath + "/Scripts/folderStructure.txt", "This folder is for storing scripts");
		System.IO.File.WriteAllText(Application.dataPath + "/Shaders/folderStructure.txt", "This folder is for storing shaders");
        System.IO.File.WriteAllText(Application.dataPath + "/Dynamic Assets/Resources/folderStructure.txt", "This folder is for storing Resources in the static assets folder");
        System.IO.File.WriteAllText(Application.dataPath + "/Testing/folderStructure.txt", "This folder is for testing purposes");

        AssetDatabase.Refresh();
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
