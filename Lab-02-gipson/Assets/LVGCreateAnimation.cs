using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Author: Matt Gipson
/// Contact: Deadwynn@gmail.com
/// Domain: www.livingvalkyrie.com
/// 
/// Description: LVGCreateAnimation 
/// </summary>
public class LVGCreateAnimation : EditorWindow {
    #region Fields

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

    #endregion

    [MenuItem("Project Tools/Create Animation")]
    public static void Init() {
        selectedObject = Selection.activeObject;
        if (selectedObject == null) {
            return;
        }

        LVGCreateAnimation window = (LVGCreateAnimation) EditorWindow.GetWindow(typeof (LVGCreateAnimation));
        window.Show();
    }

    void OnGUI() {
        EditorGUILayout.LabelField("Animations for " + selectedObject.name);
        EditorGUILayout.Separator();
        controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
        numAnimations = EditorGUILayout.IntField("How many animations?", numAnimations);

        for (int i = 0; i < numAnimations; i++) {
            //Determine a name for the animation 
            animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);

            //Start a section where the following items will be displayed horizontally instead of vertically 
            EditorGUILayout.BeginHorizontal();

            //Determine the start frame for the animation 
            startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);

            //Determine the end frame for the animation 
            endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);

            //End the section where the previous items are displayed horitontally instead of vertically 
            EditorGUILayout.EndHorizontal();

            //Start a section where the following items will be displayed horizontally instead of vertically 
            EditorGUILayout.BeginHorizontal();

            //Determine the frame rate for the animation 
            clipFrameRate[i] = EditorGUILayout.FloatField("Frame Rate", clipFrameRate[i]);

            //Determine the space between each keyframe 
            clipTimeBetween[i] = EditorGUILayout.FloatField("Frame Spacing", clipTimeBetween[i]);

            //End the section where the previous items are displayed horitontally instead of vertically 
            EditorGUILayout.EndHorizontal();

            //Start a section where the following items will be displayed horizontally instead of vertically 
            EditorGUILayout.BeginHorizontal();

            //Create a checkbox to determine if this animation should loop 
            loop[i] = EditorGUILayout.Toggle("Loop", loop[i]);

            //Create a checkbox to determine if this animation should pingpong 
            pingPong[i] = EditorGUILayout.Toggle("Ping Pong", pingPong[i]);

            //End the section where the previous items are displayed horitontally instead of vertically 
            EditorGUILayout.EndHorizontal();

            //Create a space 
            EditorGUILayout.Separator();
        }

        //Create a button with the label "Create" 
        if (GUILayout.Button("Create")) {
            UnityEditor.Animations.AnimatorController controller =
                UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/" +
                                                                                          controllerName + ".controller"));
            for (int i = 0; i < numAnimations; i++) {
                //Create animation clip 
                AnimationClip tempClip = CreateAnimationClip( selectedObject, animationNames[i], startFrames[i], endFrames[i],
                                         clipFrameRate[i], clipTimeBetween[i], pingPong[i] );
                if (loop[i]) {
                    AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(tempClip);
                    settings.loopTime = true;
                    settings.loopBlend = true;
                    AnimationUtility.SetAnimationClipSettings(tempClip, settings);
                }
                controller.AddMotion(tempClip);
            }
        }
    }

    public AnimationClip CreateAnimationClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate,
                                         float timeBetween, bool pingPong) {
        //Get the path to the object 
        string path = AssetDatabase.GetAssetPath(obj);

        //Extract the sprites 
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        //Determine how many frames, and the length of each frame 
        int frameCount = endFrame - startFrame + 1;
        float frameLength = 1f / timeBetween;

        //Create a new (empty) animation clip 
        AnimationClip clip = new AnimationClip();

        //Set the framerate for the clip 
        clip.frameRate = frameRate;

        //Create the new (empty) curve binding 
        EditorCurveBinding curveBinding = new EditorCurveBinding(); //Assign it to change the sprite renderer 
        curveBinding.type = typeof (SpriteRenderer); //Assign it to alter the sprite of the sprite renderer 
        curveBinding.propertyName = "m_Sprite";

        //Create a container for all of the keyframes 
        ObjectReferenceKeyframe[] keyFrames;

        //Determine how many frames there will be if we are or are not pingponging 
        if (!pingPong) {
            keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
        }
        else {
            keyFrames = new ObjectReferenceKeyframe[frameCount * 2 + 1];
        }

        //Keep track of what frame number we are on 
        int frameNumber = 0;

        //Loop from start to end, incrementing frameNumber as we go 
        for (int i = startFrame; i < endFrame + 1; i++, frameNumber++) {
            //Create an empty keyframe 
            ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe(); //Assign it a time to appear in the animation 
            tempKeyFrame.time = frameNumber * frameLength; //Assign it a sprite 
            tempKeyFrame.value = sprites[i]; //Place it into the container for all the keyframes 
            keyFrames[frameNumber] = tempKeyFrame;
        }

        //If we are pingponging this animation 
        if (pingPong) {
            //Create keyframes starting at the end and going backwards             //Continue to keep track of the frame number 
            for (int i = endFrame; i >= startFrame; i--, frameNumber++) {
                ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
                tempKeyFrame.time = frameNumber * frameLength;
                tempKeyFrame.value = sprites[i];
                keyFrames[frameNumber] = tempKeyFrame;
            }
        }

        //Create the last sprite to stop it from switching quickly from the last frame to the first one 
        ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe();
        lastSprite.time = frameNumber * frameLength;
        lastSprite.value = sprites[startFrame];
        keyFrames[frameNumber] = lastSprite;

        //Assign the name 
        clip.name = clipName;

        //Apply the curve 
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

        //Creat the clip 
        AssetDatabase.CreateAsset(clip, ("Assets/" + clipName + ".anim")); //return the clip 
        return clip;
    }

    void OnFocus() {
        if ( EditorApplication.isPlayingOrWillChangePlaymode )
            this.Close();
    }

}