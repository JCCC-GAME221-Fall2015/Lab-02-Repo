using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class folderStructure : MonoBehaviour {

	[MenuItem("Tool Creation/Create Folder")]
	public static void CreateFolder()
	{
		AssetDatabase.CreateFolder ("Assets", "Dynamic Assets");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Resources");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Animations");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Animations", "Sources");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Animation Controllers");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Effects");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Models");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Models", "Character");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Models", "Environment");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Prefabs");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Prefabs","Common");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Sounds");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Sounds","Music");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Sounds/Music","Common");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Sounds","SFX");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Sounds/SFX","Common");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources","Textures");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Resources/Textures","Common");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Editor");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Extensions");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Gizmos");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Plugins");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets","Scripts");
		AssetDatabase.CreateFolder ("Assets/Dynamic Assets/Scripts","Common");
		AssetDatabase.CreateFolder ("Assets", "Shaders");
		AssetDatabase.CreateFolder ("Assets", "Static Assets");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Animations");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Animations", "Sources");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Animation Controllers");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Effects");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Models");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Models", "Character");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Models", "Environment");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Prefabs");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Prefabs","Common");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Sounds");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Sounds","Music");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Sounds/Music","Common");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Sounds","SFX");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Sounds/SFX","Common");
		AssetDatabase.CreateFolder ("Assets/Static Assets","Textures");
		AssetDatabase.CreateFolder ("Assets/Static Assets/Textures","Common");
		AssetDatabase.CreateFolder ("Assets", "Testing");

		System.IO.File.WriteAllText (Application.dataPath + "/folderStructure.txt", 
		                             "Assets: This folder is for storing assets.\n" +
		                             "There are four main categories\n" +
		                             "\n\tDynamic Resources" +
		                             "\n\tShaders" +
		                             "\n\tStatic Resources" +
		                             "\n\tand Testing.");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/folderStructure.txt", 
		                             "Dynamic Assets: This folder is for storing dynamic assets.\n" +
		                             "These are stored as such\n" +
		                             "\n\tDynamic Resources" +
		                             "\n\tEditor" +
		                             "\n\tExtensions" +
		                             "\n\tGizmos" +
		                             "\n\tPlugins" +
		                             "\n\tand Scripts");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Resources/folderStructure.txt", 
		                             "Resources: This folder is for storing dynamic Resources including\n" +
		                             "\n\tAnimations" +
		                             "\n\tAnimation Controllers" +
		                             "\n\tEffects" +
		                             "\n\tModels for Characters and Environments" +
		                             "\n\tPrefabs" +
		                             "\n\tSounds for Music and SFX" +
		                             "\n\tand Textures");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Editor/folderStructure.txt", 
		                             "Editor: This folder is for storing Editor Files.");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Extensions/folderStructure.txt", 
		                             "Extensions: This folder is for storing extensions.");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Gizmos/folderStructure.txt", 
		                             "Gizmos: This folder is for storing nice mogwais.\n\n" +
		                             "All others need to be placed in the round file after exposure to sunlight.");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Plugins/folderStructure.txt", 
		                             "Plugins: This folder is for storing plugins.");
		System.IO.File.WriteAllText (Application.dataPath + "/Dynamic Assets/Scripts/folderStructure.txt", 
		                             "Scripts: This folder is for storing scripts.");
		System.IO.File.WriteAllText (Application.dataPath + "/Shaders/folderStructure.txt", 
		                             "Shaders: This folder is for storing shaders.");
		System.IO.File.WriteAllText (Application.dataPath + "/Static Assets/folderStructure.txt", 
		                             "Static Assets: This folder is for storing Static Assets including\n" +
		                             "\n\tAnimations" +
		                             "\n\tAnimation Controllers" +
		                             "\n\tEffects" +
		                             "\n\tModels for Characters and Environments" +
		                             "\n\tPrefabs" +
		                             "\n\tScenes" +
		                             "\n\tSounds for Music and SFX" +
		                             "\n\tand Textures");
		System.IO.File.WriteAllText (Application.dataPath + "/Testing/folderStructure.txt", 
		                             "Testing: This folder is for storing Test files and files in need of testing.");

		AssetDatabase.Refresh ();
	}
	/*
	public static void CreateFolder()
	{
		//Debug.Log ("Creating a folder!");
		AssetDatabase.CreateFolder ("Assets", "Materials");
		AssetDatabase.CreateFolder ("Assets", "Textures");
		AssetDatabase.CreateFolder ("Assets", "Prefabs");
		AssetDatabase.CreateFolder ("Assets", "Scripts");
		AssetDatabase.CreateFolder ("Assets", "Scenes");
		AssetDatabase.CreateFolder ("Assets", "Animations");
		AssetDatabase.CreateFolder ("Assets/Animations", "AnimationControllers");

		System.IO.File.WriteAllText (Application.dataPath + "/Materials", "Materials: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Textures", "Textures: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Prefabs", "Prefabs: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Scripts", "Scripts: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Scenes", "Scenes: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Animations", "Animations: This folder is for storing materials.");
		System.IO.File.WriteAllText (Application.dataPath + "/Animations/AnimationControllers", 
		                             "AnimationControllers: This folder is for storing materials.");
	}*/
}
