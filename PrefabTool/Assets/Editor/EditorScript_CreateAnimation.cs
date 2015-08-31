using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorScript_CreateAnimation : EditorWindow
{
    public static Object selectedObject;

    static int numAnimations;
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
        {

        }
        else
        {
            //Play Current Windows sounds Beep sound (Asterisk, Hand, Question, Exclamation are the other sounds availible)
            System.Media.SystemSounds.Beep.Play();
            //Create window
            EditorScript_CreateAnimation window = (EditorScript_CreateAnimation)EditorWindow.GetWindow(typeof(EditorScript_CreateAnimation));
            window.Show();
        }
    }

    void OnFocus()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode)
        {
            this.Close();
        }
    }

    void OnGUI()
    {
        if (selectedObject != null)
        {
            //Display the name of the object the animations will be created from
            EditorGUILayout.LabelField("Animations for " + selectedObject.name);
            //Create a space
            EditorGUILayout.Separator();
            //Get the name of the Animation Controller
            controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
            //Determine how many animations there will be
            numAnimations = EditorGUILayout.IntField("How many animations?", numAnimations);
            //Loop through all the theoretical animations
            for (int i = 0; i < numAnimations; i++)
            {
                //Get the name of the animation
                animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);
                //Start a section where the elements will be listed horizontally
                EditorGUILayout.BeginHorizontal();
                //Determine the start frame of the animation
                startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);
                //Determine the end frame of the animation
                endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);
                //Change to the next row of elements listed horizontally
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                //Create a checkbox to see if the animation should loop
                loop[i] = EditorGUILayout.Toggle("Loop", loop[i]);
                //Create a checkbox to see if the animation should pingpong
                pingPong[i] = EditorGUILayout.Toggle("Ping Pong", pingPong[i]);
                //End the horizontal display section
                EditorGUILayout.EndHorizontal();

                //Create a space
                EditorGUILayout.Separator();
            }

            //Create a button labeled "Create"
            if (GUILayout.Button("Create"))
            {
                //If the button is pressed...
                UnityEditor.Animations.AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/"
                    + controllerName + ".controller"));

                for (int i = 0; i < numAnimations; i++)
                {
                    AnimationClip tempClip = new AnimationClip();

                    tempClip = CreateClip(selectedObject, animationNames[i], startFrames[i], endFrames[i], clipFrameRate[i], clipTimeBetween[i], pingPong[i]);

                    if (tempClip == null)
                    {
                        //Play error sound and abort animation creation
                        System.Media.SystemSounds.Exclamation.Play();
                        i = numAnimations;
                    }
                    else
                    {
                        if (loop[i])
                        {
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
    }

    public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
    {
        //Get the path to the object
        string path = AssetDatabase.GetAssetPath(obj);

        //Extract the sprites
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        if (endFrame <= sprites.Length & startFrame <= endFrame)
        {
            //Determine number of frames and frame length
            int frameCount = endFrame - startFrame + 1;
            float frameLength = 1f / timeBetween;

            //Create new clip and initialize frame rate
            AnimationClip clip = new AnimationClip();
            clip.frameRate = frameRate;

            //Create the new curve binding and set it to change the sprite of the sprite renderer
            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.propertyName = "m_Sprite";

            //Create a container for the key frames
            ObjectReferenceKeyframe[] keyFrames;

            //Determine if we are ping ponging or not
            if (pingPong)
            {
                keyFrames = new ObjectReferenceKeyframe[frameCount * 2 + 1];
            }
            else
            {
                keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
            }

            //Keep track of what frame number we are on
            int frameNumber = 0;

            //Loop from start to finish of the animation
            for (int i = startFrame; i < endFrame; i++, frameNumber++)
            {
                //Create a new keyframe
                ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();

                //Assign it a time to appear in the animation
                tempKeyFrame.time = frameNumber * frameLength;

                //Assign it a sprite
                tempKeyFrame.value = sprites[i];

                //Place it in the container for all the keyframes
                keyFrames[frameNumber] = tempKeyFrame;
            }

            //If we are pingponging
            if (pingPong)
            {
                //Create keyframes starting at the end and going backwards
                //Continue to keep track of the frame number
                for (int i = endFrame; i > startFrame; i--, frameNumber++)
                {
                    //Create a new keyframe
                    ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();

                    //Assign it a time to appear in the animation
                    tempKeyFrame.time = frameNumber * frameLength;

                    //Assign it a sprite
                    tempKeyFrame.value = sprites[i];

                    //Place it in the container for all the keyframes
                    keyFrames[frameNumber] = tempKeyFrame;
                }
            }

            //Create the last keyframe to stop the loop from not really displaying the final frame
            ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe();
            lastSprite.time = frameNumber * frameLength;
            lastSprite.value = sprites[startFrame];
            keyFrames[frameNumber] = lastSprite;

            //Assign the name
            clip.name = clipName;

            //Apply the curve
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

            //Create the clip
            AssetDatabase.CreateAsset(clip, (@"Assets/" + clipName + ".anim"));
            return clip;
        }
        else
        {
            return null;
        }
    }

}
