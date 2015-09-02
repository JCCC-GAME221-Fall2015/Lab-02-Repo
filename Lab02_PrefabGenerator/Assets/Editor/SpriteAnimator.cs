using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;

public class SpriteAnimator : EditorWindow
{
    // Holds the object the user has selected when this script is run
    public static Object oSelected;

    #region Animation Parameters
    int numAnimations; // How many animations will be created
    string controllerName; // Name of the controller at its creation
    string[] animationNames = new string[100]; // Name of animations at their creations
    float[] clipFrameRate = new float[100]; // Frame rates for each animation
    float[] clipTimeBetween = new float[100]; // Time between frames for each animation
    int[] startFrames = new int[100]; // Starting frame of each animation
    int[] endFrames = new int[100]; // Ending frame of each animation
    bool[] pingPong = new bool[100]; // Flag that tells if an animation should ping-pong
    bool[] loop = new bool[100]; // Flag that tells if an animation should loop
    #endregion

    int finalFrame = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(oSelected)).Length - 1;

    [MenuItem("Project Tools/Create Sprite Animation")]
    public static void Init()
    {
        // Get active object
        oSelected = Selection.activeObject;
        // Do nothing if no object selected
        if (oSelected == null)
            return;
        // Create and show the animations window
        SpriteAnimator window = (SpriteAnimator)EditorWindow.GetWindow(typeof(SpriteAnimator));
        window.Show();
    }

    void OnGUI()
    {
        if (oSelected != null)
        {
            // Display object name animations come from
            EditorGUILayout.LabelField("Animations for " + oSelected.name);
            EditorGUILayout.Separator();

            // Get animation controller name
            controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
            // Get number of animations
            numAnimations = EditorGUILayout.IntField("Number of animations: ", numAnimations);

            for (int i = 0; i < numAnimations; i++)
            {
                animationNames[i] = EditorGUILayout.TextField("Animation name: ", animationNames[i]);
                // Start and end frame info
                EditorGUILayout.BeginHorizontal();
                startFrames[i] = EditorGUILayout.IntField("Start frame: ", startFrames[i]);
                endFrames[i] = EditorGUILayout.IntField("End frame: ", endFrames[i]);
                EditorGUILayout.EndHorizontal();
                // Frame rate info
                EditorGUILayout.BeginHorizontal();
                clipFrameRate[i] = EditorGUILayout.FloatField("Frame rate: ", clipFrameRate[i]);
                clipTimeBetween[i] = EditorGUILayout.FloatField("Frame spacing: ", clipTimeBetween[i]);
                EditorGUILayout.EndHorizontal();
                // Animation special info (loop/ping-pong)
                EditorGUILayout.BeginHorizontal();
                loop[i] = EditorGUILayout.Toggle("Loop? ", loop[i]);
                pingPong[i] = EditorGUILayout.Toggle("Ping-Pong? ", pingPong[i]);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }

            
            if (GUILayout.Button("Create"))
            {
                AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/" + controllerName + ".controller");
                for (int i = 0; i < numAnimations; i++)
                {
                        // Create a temporary animation clip
                        AnimationClip tempClip = CreateClip(oSelected, animationNames[i], startFrames[i], endFrames[i],
                                                            clipFrameRate[i], clipTimeBetween[i], pingPong[i]);
                        // Set the animation clip loop settings to true if looping should occur
                        if (loop[i])
                        {
                            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(tempClip);
                            settings.loopTime = true;
                            settings.loopBlend = true;
                            AnimationUtility.SetAnimationClipSettings(tempClip, settings);
                        }
                        // Add the clip to the animation controller
                        controller.AddMotion(tempClip);
                }
            }
        }
    }

    /**
     * OnFocus closes the Animations window when Unity enters or exits play mode.
     */
    void OnFocus()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) this.Close();
    }

    /**
     * AnimationClip creates a 2D sprite animation.
     * 
     * @param obj: Spritesheet to build animation from
     * @param slipName: animation name
     * @param startFrame: first frame of animation
     * @param endFrame: last frame of animation
     * @param frameRate: time to transition between frames
     * @param timeBetweem: delay time for frame transitions
     * @param pingPong: flag for determining if animation will ping-pong
     * 
     * @return: Returns a 2D sprite animation clip
     */
    public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
    {
        // Get animation information: animation location (via its filepath) and all sprites
        string path = AssetDatabase.GetAssetPath(obj);
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        // Determine frame information
        int frameCount = endFrame - startFrame + 1;
        float frameLength = 1f / timeBetween;


        AnimationClip clip = new AnimationClip();
        clip.frameRate = frameRate;

        // Create a curve binding
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        // Make the binding change the sprite renderer
        curveBinding.type = typeof(SpriteRenderer); // This code seems obsolete in Unity 5; sprites fail to render in Editor.
        curveBinding.propertyName = "m_Sprite";
        // Create an animation keyframes container
        ObjectReferenceKeyframe[] keyframes;
        // Determine number of keyframes based on ping-ponging
        if (!pingPong)
            keyframes = new ObjectReferenceKeyframe[frameCount + 1];
        else
            keyframes = new ObjectReferenceKeyframe[frameCount * 2 + 1];

        // Initialize frameNumber
        int frameNumber = 0;

        // Add keyframes
        for (int i = startFrame; i < endFrame + 1; i++, frameNumber++)
        {
            ObjectReferenceKeyframe tempFrame = new ObjectReferenceKeyframe();
            tempFrame.time = frameNumber * frameLength;
            tempFrame.value = sprites[i];
            keyframes[frameNumber] = tempFrame;
        }
        // Double the frames if ping-ponging
        if (pingPong)
        {
            for (int i = endFrame; i >= startFrame; i--, frameNumber--)
            {
                ObjectReferenceKeyframe tempFrame = new ObjectReferenceKeyframe();
                tempFrame.time = frameNumber * frameLength;
                tempFrame.value = sprites[i];
                keyframes[frameNumber] = tempFrame;
            }
        }

        // Create the final frame
        ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe();
        lastSprite.time = frameNumber * frameLength;
        lastSprite.value = sprites[startFrame];
        keyframes[frameNumber] = lastSprite;

        // Finalize clip properties
        clip.name = clipName;
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
        // Create and return the animation clip
        AssetDatabase.CreateAsset(clip, ("Assets/" + clipName + ".anim"));
        return clip;
    }
}