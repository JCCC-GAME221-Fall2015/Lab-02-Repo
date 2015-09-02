using UnityEngine;
using System.Collections;
using UnityEditor;

public class AnimationGeneration : EditorWindow {

	public static Object selectedObject;

	int numAnimations;
	string controllerName;
	string[] animationNames = new string[100];
	float[] clipFrameRate = new float[100];
	float[] clipTimeBetween = new float[100];
	int[] startFrames = new int[100];
	int[] endFrames = new int[100];
	bool[] pingPong = new bool[100];
	bool[] loop = new bool[100];

	// Menu Item used to create 2D Animations from a selected and prepared sprite sheet
	[MenuItem("Project Tools/2D Animation Prefab")]
	public static void Init()
	{
		//Grab the active object
		selectedObject = Selection.activeObject;
		//if the object doesn't exist, do nothing
		if (selectedObject == null)
			return;
		//Otherwise, create a new window
		AnimationGeneration window = (AnimationGeneration)EditorWindow.GetWindow (typeof(AnimationGeneration));
		//show the window
		window.Show ();
	}

	// GUI Layout for 2D Animation creation function
	void OnGUI()
	{
		if (selectedObject != null) 
		{
			//Display the object's name that the animations will be created from
			EditorGUILayout.LabelField ("Animations for " + selectedObject.name);
			//create a space
			EditorGUILayout.Separator ();
			//Get the name for the animation controller
			controllerName = EditorGUILayout.TextField ("Controller Name", controllerName);
			//Determine how many animations there will be
			numAnimations = EditorGUILayout.IntField ("How many animations?", numAnimations);

			//Loop through each theoretical animation
			for (int i = 0; i< numAnimations; i++) 
			{
				//Determine a name for the animation
				animationNames [i] = EditorGUILayout.TextField ("Animation Name", animationNames [i]);

				//Start a section where the items will be displayed horizontally instead of vertically.
				EditorGUILayout.BeginHorizontal ();
				//Determine the start frame for the animation
				startFrames [i] = EditorGUILayout.IntField ("Start Frame", startFrames [i]);
				//Determine the end frame for the animation
				endFrames [i] = EditorGUILayout.IntField ("End Frame", endFrames [i]);
				//End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal ();

				//Start a section where the following items will be displayed horizontally instead of vertically.
				EditorGUILayout.BeginHorizontal ();
				//Determine the frame rate for the animation
				clipFrameRate [i] = EditorGUILayout.FloatField ("Frame Rate", clipFrameRate [i]);
				//Determine the space between each keyframe
				clipTimeBetween [i] = EditorGUILayout.FloatField ("Frame Spacing", clipTimeBetween [i]);
				//End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal ();

				//Start a section where the following items will be displayed horizontally instead of vertically.
				EditorGUILayout.BeginHorizontal ();
				//Create a checkbox to determine if this animation should loop
				loop [i] = EditorGUILayout.Toggle ("Loop", loop [i]);
				//Create a checkbox to determine if this animation should ping-pong
				pingPong [i] = EditorGUILayout.Toggle ("Ping Pong", pingPong [i]);
				//End the section where the previous items are displayed horizontally instead of vertically
				EditorGUILayout.EndHorizontal ();

				//Create a space
				EditorGUILayout.Separator ();
			}

			//Create a button with the label "Create"
			if (GUILayout.Button ("Create")) 
			{
				//if the button has been pressed
				//create an animator controller
				UnityEditor.Animations.AnimatorController controller = 
					UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath (("Assets/" + 
					controllerName + ".controller"));
				for (int j = 0; j < numAnimations; j++) 
				{
					//create animation clip
					AnimationClip tempClip = CreateClip (selectedObject, animationNames [j], startFrames [j], endFrames [j], 
				                                     clipFrameRate [j], clipTimeBetween [j], pingPong [j]);
					//determine if the clip should loop
					if (loop [j]) 
					{
						//set the loop on the clip to true
						AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings (tempClip);
						settings.loopTime = true;
						settings.loopBlend = true;
						AnimationUtility.SetAnimationClipSettings (tempClip, settings);
					}

					controller.AddMotion (tempClip);

				}
			}
		}
	}

	public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
	{
		//Get the path to the object
		string path = AssetDatabase.GetAssetPath (obj);

		//Extract the sprites
		Object[] sprites = AssetDatabase.LoadAllAssetsAtPath (path);

		//Determine how many frames, and the length of each frame
		int frameCount = endFrame - startFrame + 1;
		float frameLength = 1f / timeBetween;

		//Create a new (empty) animation clip
		AnimationClip clip = new AnimationClip ();

		//Set the framerate for the clip
		clip.frameRate = frameRate;

		//Create the new (empty) curve binding
		EditorCurveBinding curveBinding = new EditorCurveBinding ();
		//Assign it to change the sprite renderer
		curveBinding.type = typeof(SpriteRenderer);
		//Assign it to alter the sprite of the sprite renderer
		curveBinding.propertyName = "m_Sprite";
		//Create a container for all of the keyframes
		ObjectReferenceKeyframe[] keyFrames;

		//Determine how many frames there will be if we are or are not ping-pong-ing
		if (!pingPong) 
		{
			keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
		} 
		else 
		{
			keyFrames = new ObjectReferenceKeyframe[frameCount*2 + 1];
		}

		//keep track of what frame number we are on
		int frameNumber = 0;

		//Loop from start to end, incrementing frameNumber as we go
		for (int k = startFrame; k < endFrame + 1; k++, frameNumber++) 
		{
			//Create an empty keyframe
			ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
			//Assign it a time to appear in the animation
			tempKeyFrame.time = frameNumber * frameLength;
			//Assign it a sprite
			tempKeyFrame.value = sprites[k];
			//Place it into the container for all the keyframes
			keyFrames[frameNumber] = tempKeyFrame;
		}

		if(pingPong)
		{
			//Create keyframes starting at the end and going backwards
			//Continue to keep track of the frame number

			for(int l = endFrame; l >= startFrame; l--, frameNumber++)
			{
				ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
				tempKeyFrame.time = frameNumber*frameLength;
				tempKeyFrame.value = sprites[l];
				keyFrames[frameNumber] = tempKeyFrame;
			}
		}

		//Create the last sprite to stop it from switching quickly from the last frame to the first one
		ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe ();
		lastSprite.time = frameNumber * frameLength;
		lastSprite.value = sprites [startFrame];
		keyFrames [frameNumber] = lastSprite;

		//assign the name
		clip.name = clipName;
		//apply the curve
		AnimationUtility.SetObjectReferenceCurve (clip, curveBinding, keyFrames);
		//create the clip
		AssetDatabase.CreateAsset (clip, ("Assets/" + clipName + ".anim"));
		//return the clip
		return clip;
	}

	void OnFocus()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode) 
		{
			this.Close ();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
