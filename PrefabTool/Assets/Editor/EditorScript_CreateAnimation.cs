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
            EditorScript_CreateAnimation window = (EditorScript_CreateAnimation)EditorWindow.GetWindow(typeof(EditorScript_CreateAnimation));
            window.Show();
        }
    }

    void OnGUI()
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
        for(int i = 0; i < numAnimations; i++)
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
        if(GUILayout.Button("Create"))
        {
            //If the button has been pressed...
            UnityEditor.Animations.AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(("Assets/"
                + controllerName + ".controller"));

            for(int i = 0; i < numAnimations; i++)
            {
                AnimationClip tempClip = new AnimationClip();

                if(loop[i])
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

    public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
    {
        //Get the path to the object
        string path = AssetDatabase.GetAssetPath(obj);

        //Extract the sprites
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        int frameCount = endFrame - startFrame + 1;
        float frameLength = 1f / timeBetween;
    }

}
