using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeAnimations : EditorWindow {

	// Will hold the object that the user has selected when the script is run
	public static Object selectedObject;

	// Will store how many animations will be created
	int numAnimations;
	// Name of the controller to be created
	string controllerName;
	// Name of each of the animations to be created
	string[] animationNames = new string[100];

	// The frame rate for each animation
	float[] clipFrameRate = new float[100];
	// The time between each animation
	float[] clipTimeBetween = new float[100];
	// What frame each animation starts at
	int[] startFrames = new int[100];
	// What frame each animation ends at
	int[] endFrames = new int[100];
	// If each animation should pingpong
	bool[] pingPong = new bool[100];
	// If each animation should loop
	bool[] loop = new bool[100];

	[MenuItem("Project Tools/2D Animations")]
	static void Init()
	{
		// Grab the active object
		selectedObject = Selection.activeObject;

		// If the object doesn't exist, do nothing
		if (selectedObject == null)
			return;

		// Otherwise, create a new window
		MakeAnimations window = (MakeAnimations)EditorWindow.GetWindow(typeof(MakeAnimations));

		// Show the window
		window.Show();
	} // end method Init

	void OnGUI()
	{
		if (selectedObject != null)
		{
			// Display the object's name that the animations will be created from
			EditorGUILayout.LabelField("Animations for " + selectedObject.name);
			// Create a space
			EditorGUILayout.Separator();
			// Get the name for the animation controller
			controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
			// Determine how many animations there will be
			numAnimations = EditorGUILayout.IntField("How many animations?", numAnimations);
			// Loop through each theoretical animation
			for (int i = 0; i < numAnimations; i++)
			{
				// Determine a name for the animation
				animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);

				// Start a section where the following items will be displayed horizontally instead of vertically
				EditorGUILayout.BeginHorizontal();
				// Determine the start frame for the animation
				startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);
				// Determine the end frame for the animation
				endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);
				// End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal();

				// Start a section where the following items will be displayed horizontally instead of vertically
				EditorGUILayout.BeginHorizontal();
				// Determine the frame rate for the animation
				clipFrameRate[i] = EditorGUILayout.FloatField("Frame Rate", clipFrameRate[i]);
				// Determine the space between each keyframe
				clipTimeBetween[i] = EditorGUILayout.FloatField("Frame Spacing", clipTimeBetween[i]);
				// End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal();
				
				// Start a section where the following items will be displayed horizontally instead of vertically
				EditorGUILayout.BeginHorizontal();
				// Create a checkbox to determine if this animation should loop
				loop[i] = EditorGUILayout.Toggle("Loop", loop[i]);
				// Create a checkbox to determine if this animation should pingpong
				pingPong[i] = EditorGUILayout.Toggle("Ping Pong", pingPong[i]);
				// End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal();
				// Create a space
				EditorGUILayout.Separator();
			} // for (int i = 0; i < numAnimations; i++)

			// Create a button with the label "Create"
			if (GUILayout.Button("Create"))
			{
				//Create an animator controller
				UnityEditor.Animations.AnimatorController controller =
					UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/" +
					controllerName + ".controller"));
				for (int i = 0; i < numAnimations; i++)
				{
					// Create animation clip
					AnimationClip tempClip = CreateClip(selectedObject, animationNames[i], startFrames[i],
						endFrames[i], clipFrameRate[i], clipTimeBetween[i], pingPong[i]);

					// Determine if the clip should loop
					if (loop[i])
					{
						// If so, capture the settings of the clip
						AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(tempClip);

						// Set the looping to true
						settings.loopTime = true;
						settings.loopBlend = true;

						// Apply the settings to the clip
						AnimationUtility.SetAnimationClipSettings(tempClip, settings);
					}

					// Add the clip to the Animator Controller
					controller.AddMotion(tempClip);
				}
			}
		}
	} // end method OnGUI

	public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame,
	                                float frameRate, float timeBetween, bool pingPong)
	{
		// Get path to the object
		string path = AssetDatabase.GetAssetPath(obj);

		// Extract the sprites
		Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

		// The following line needs to be deleted or changed:
		return new AnimationClip();
	}

	// Use this for initialization
//	void Start () {
//	
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
