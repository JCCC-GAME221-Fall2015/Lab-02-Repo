using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeAnimations : EditorWindow {

    //This is static because the menu function is also static*
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

    /// <summary>
    /// store the selected objects and open a popup window
    /// </summary>
    [MenuItem("Project Tools/2D Animations")]
    static void Init()
    {
        selectedObject = Selection.activeObject;

        if(selectedObject == null)
        {
            return;
        }

        MakeAnimations window = (MakeAnimations)EditorWindow.GetWindow(typeof(MakeAnimations));

        window.Show();
    }

    void OnGUI()
    {
        if(selectedObject != null)
        {
            EditorGUILayout.LabelField("Animations for " + selectedObject.name);
            EditorGUILayout.Separator();

            controllerName = EditorGUILayout.TextField("Controller Name", controllerName);

            numAnimations = EditorGUILayout.IntField("How many animations?", numAnimations);

            for (int i = 0; i < numAnimations; i++)
            {
                //Get the animation name
                animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);

                //Start a section where the items are displayed horizontally
                EditorGUILayout.BeginHorizontal();

                startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);

                //get end frame
                endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);

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

                EditorGUILayout.Separator();

            }

            if (GUILayout.Button("Create"))
            {
                UnityEditor.Animations.AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/" + controllerName + ".controller"));

                for(int i = 0; i< numAnimations; i++)
                {
                    AnimationClip tempClip = CreateClip(selectedObject, animationNames[i], startFrames[i], endFrames[i], clipFrameRate[i], clipTimeBetween[i], pingPong[i]);

                    if (loop[i])
                    {
                        //set the loop on the clip to true
                        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(tempClip);
                        settings.loopTime = true;
                        settings.loopBlend = true;
                        AnimationUtility.SetAnimationClipSettings(tempClip, settings);
                    }

                    controller.AddMotion(tempClip);
                }
            }

        }
    }

    void OnFocus()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            this.Close();
        }
    }

    public AnimationClip CreateClip(
        Object obj, 
        string clipName, 
        int startFrame, 
        int endFrame, 
        float frameRate, 
        float timeBetween, 
        bool pingPong)
    {
        string path = AssetDatabase.GetAssetPath(obj);
        
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        //check end frame 
        if (endFrame < sprites.Length)
        {

            //Determine how many frames, and the length of each frame
            int frameCount = endFrame - startFrame + 1;

            float frameLength = 1f / timeBetween;

            //Create a new (empty animation clip
            AnimationClip clip = new AnimationClip();

            clip.frameRate = frameRate;

            EditorCurveBinding curveBinding = new EditorCurveBinding();

            //Assing it to change the sprite renderer

            curveBinding.type = typeof(SpriteRenderer);
            //assign it to the sprite renderer
            curveBinding.propertyName = "m_Sprite";

            //Create a container for all of the keyframes
            ObjectReferenceKeyframe[] keyFrames;

            //Determine how many frames if ping ponging

            if (!pingPong)
            {
                keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
            }
            else
            {
                keyFrames = new ObjectReferenceKeyframe[frameCount * 2 + 1];
            }

            //frame counter
            int frameNumber = 0;

            //loop from start to end
            for (int i = startFrame; i < endFrame + 1; i++, frameNumber++)
            {
                //create empty keyframe
                ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
                //assign it a time to appear in the anim
                tempKeyFrame.time = frameNumber * frameLength;
                //assign it a sprite
                tempKeyFrame.value = sprites[i];
                //place it into the container for all the keyframes
                keyFrames[frameNumber] = tempKeyFrame;
            }

            //If we are pinponging this aimation
            if (pingPong)
            {
                //create keyframes starting at the end and going backwards
                //continue to keep track of the frame number
                for (int i = endFrame; i >= startFrame; i--, frameNumber++)
                {
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

            //apply the curve
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

            //create the clip
            AssetDatabase.CreateAsset(clip, ("Assets/" + clipName + ".anim"));

            //return the clip
            return clip;
        }
        else
        {
            Debug.Log("endFrame is greater than amount of slices the sprite has.");
            return null;
        }
    }
}
