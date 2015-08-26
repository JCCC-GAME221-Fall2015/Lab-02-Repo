using UnityEngine;
using System.Collections;
using UnityEditor;


public class Script2DSpriteAutomation : EditorWindow 
{
	public static Object selectedObject;

	int numAnimations;
	string controllerName;
	string[] animationNames = new string[100];
	float[] clipFrameRate = new float[100];
	float[] cliopTimeBetween = new float[100];
	int[] startFrames = new int[100];
	int[] endFrames = new int[100];
	bool[] pingPong = new bool[100];
	bool[] loop = new bool[100];


	[MenuItem("Project Tools/2D Animations")]
	public static void Init()
	{
		selectedObject = Selection.activeObject;

		if(selectedObject == null)
		{
			return;
		}

		Script2DSpriteAutomation window = (Script2DSpriteAutomation)EditorWindow.GetWindow (typeof(Script2DSpriteAutomation));
		window.Show();
	}
}