using UnityEngine;
using System.Collections;
using UnityEditor;

/* A tool for creating sprite sheet animations quickly and easily
*  @author Mike Dobson
*/

public class CreateAnimations : EditorWindow {

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
	
    [MenuItem("Project Tools/2D Animations")]
    public static void Init()
    {
        selectedObject = Selection.activeObject;

        if (selectedObject == null)
            return;

        CreateAnimations window = (CreateAnimations)EditorWindow.GetWindow(typeof(CreateAnimations));
        window.Show();
    }

    void OnGUI()
    {
        if (selectedObject != null)
        {
            //Displays the object name that the animations are being created from
            EditorGUILayout.LabelField("Animations for " + selectedObject.name);
            //Seperator
            EditorGUILayout.Separator();

            //Get the name of the controller for the animations
            controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
            //Get the number of animations
            numAnimations = EditorGUILayout.IntField("How Many Animations?", numAnimations);

            //Loop through each animation
            for (int i = 0; i < numAnimations; i++)
            {
                //Determine the name of the animation
                animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);

                //Start a section where the following items will be displayed horozontally instead of vertically
                EditorGUILayout.BeginHorizontal();
                //Determine the start frame of the animation
                startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);
                //Determine the end frame of the animation
                endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);
                //End the section where the previous items are displayed horozontally instead of vertically
                EditorGUILayout.EndHorizontal();

                //Start a section where the items will be displayed horozontally instead of vertically
                EditorGUILayout.BeginHorizontal();
                //Determine the frame rate of the animation
                clipFrameRate[i] = EditorGUILayout.FloatField("Frame Rate", clipFrameRate[i]);
                //Determine the space between each keyframe
                clipTimeBetween[i] = EditorGUILayout.FloatField("Frame Spacing", clipTimeBetween[i]);
                //End the section where the previous items are displayed horozontally instead of vertically
                EditorGUILayout.EndHorizontal();

                //Start a section where the items will be displayed horozontally instead of vertically
                EditorGUILayout.BeginHorizontal();
                //Create a checkbox to determine if this animation should loop
                loop[i] = EditorGUILayout.Toggle("Loop", loop[i]);
                //Create a checkbox to determine if this animation should pingpong
                pingPong[i] = EditorGUILayout.Toggle("Ping Pong", pingPong[i]);
                //End the section where items are displayed horozontally instead of vertically
                EditorGUILayout.EndHorizontal();

                //Seperator
                EditorGUILayout.Separator();
            }

            //Create a button with the label Create
            if (GUILayout.Button("Create"))
            {
                //if the button has been pressed
                UnityEditor.Animations.AnimatorController controller =
                    UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/" + controllerName + ".controller"));

                for (int i = 0; i < numAnimations; i++)
                {
                    //Create animation clip
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
    public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
    {
        //Get the path to the object
        string path = AssetDatabase.GetAssetPath(obj);

        //Extract the sprites at the path
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        //Determine how many frames and the length of each frame
        int frameCount = endFrame - startFrame + 1;
        float frameLength = 1f / timeBetween;

        //create the new empty animation clip
        AnimationClip clip = new AnimationClip();

        //set the framerate of the clip
        clip.frameRate = frameRate;

        //create the new empty curve binding
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        //assign it to change the sprite renderer
        curveBinding.type = typeof(SpriteRenderer);
        //assign it to alter the sprite of the sprite renderer
        curveBinding.propertyName = "m_Sprite";

        //Create a container for all the keyframes
        ObjectReferenceKeyframe[] keyFrames;

        //Determine how many frames there will be if we are or are not pingponging
        if(!pingPong)
        {
            keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
        }
        else
        {
            keyFrames = new ObjectReferenceKeyframe[frameCount * 2 + 1];
        }

        //track what frame number we are on
        int frameNumber = 0;

        //loop from start to end, incrementing frameNumber as we go
        for (int i = startFrame; i < endFrame + 1; i++, frameNumber++)
        {
            //create an empty keyframe
            ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
            //Assign i a time to appear in the animation
            tempKeyFrame.time = frameNumber * frameLength;
            //assign it a sprite
            tempKeyFrame.value = sprites[i];
            //Place it into the container fro all the keyframes
            keyFrames[frameNumber] = tempKeyFrame;
        }

        if(pingPong)
        {
            //create the keyframes starting at the end and going backwards
            //continue to keep track of the frame number
            for(int i = endFrame; i>= startFrame; i--, frameNumber++)
            {
                ObjectReferenceKeyframe tempkeyframe = new ObjectReferenceKeyframe();
                tempkeyframe.time = frameNumber * frameLength;
                tempkeyframe.value = sprites[i];
                keyFrames[frameNumber] = tempkeyframe;
            }
        }

        //Create the last sprite to stop it from switching to the first frame from the last immediately
        ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe();
        lastSprite.time = frameNumber * frameLength;
        lastSprite.value = sprites[startFrame];
        keyFrames[frameNumber] = lastSprite;

        //assign the name to the clip
        clip.name = clipName;

        //apply the curve
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        //create the clip
        AssetDatabase.CreateAsset(clip, ("Assets/" + clipName + ".anim"));
        //return the clip
        return clip;
    }
}
