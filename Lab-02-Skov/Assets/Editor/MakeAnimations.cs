using UnityEngine;
using System.Collections;
using UnityEditor;
public class MakeAnimations : EditorWindow
{

    public static Object selectedObject;

    int numAnimations;

    string controllerName;
    string[] animationNames = new string[100];

    //Frame rate modifiers
    float[] clipFrameRate = new float[100];
    float[] clipTimeBetween = new float[100];

    //Animation length modifiers
    int[] startFrames = new int[100];
    int[] endFrames = new int[100];

    //Animation behaviour modifiers
    bool[] pingPong = new bool[100];
    bool[] loop = new bool[100];

    [MenuItem("Project Tools/Create Animations #%u")]

    //initalises the GUI
    static void Init()
    {
        selectedObject = Selection.activeObject;

        if (selectedObject == null)
        {
            return;
        }
        MakeAnimations window = (MakeAnimations)EditorWindow.GetWindow(typeof(MakeAnimations));

        window.Show();
    }

    //Defines Gui Features
    void OnGUI()
    {
        if (selectedObject != null)
        {
            EditorGUILayout.LabelField("Animations for " + selectedObject.name);
            EditorGUILayout.Separator();

            controllerName = EditorGUILayout.TextField("Controller Name", controllerName);
            numAnimations = EditorGUILayout.IntField("How many animations", numAnimations);

            for (int i = 0; i < numAnimations; i++)
            {
                //Allows the user to name the animation
                animationNames[i] = EditorGUILayout.TextField("Animation Name", animationNames[i]);

                //Animation length modifiers for GUI
                EditorGUILayout.BeginHorizontal();
                startFrames[i] = EditorGUILayout.IntField("Start Frame", startFrames[i]);
                endFrames[i] = EditorGUILayout.IntField("End Frame", endFrames[i]);
                EditorGUILayout.EndHorizontal();

                //Frame Rate modifiers for GUI
                EditorGUILayout.BeginHorizontal();
                clipFrameRate[i] = EditorGUILayout.FloatField("Frame Rate", clipFrameRate[i]);
                clipTimeBetween[i] = EditorGUILayout.FloatField("Frame Spacing", clipTimeBetween[i]);
                EditorGUILayout.EndHorizontal();

                //Animation behaviour modifiers for GUI
                EditorGUILayout.BeginHorizontal();
                loop[i] = EditorGUILayout.Toggle("Loop", loop[i]);
                pingPong[i] = EditorGUILayout.Toggle("Ping Pong", pingPong[i]);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();
            }

            //if the user presses the create button
            if (GUILayout.Button("Create"))
            {

                UnityEditor.Animations.AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(
                    ("Assets/" + controllerName + ".controller"));

                for (int i = 0; i < numAnimations; i++)
                {
                    //apply changes to the Animation
                    AnimationClip tempClip = CreateClip(selectedObject, animationNames[i], startFrames[i], endFrames[i],
                    clipFrameRate[i], clipTimeBetween[i], pingPong[i]);

                    //Sets up loop behaviour
                    if (loop[i])
                    {

                        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(tempClip);

                        settings.loopTime = true;
                        settings.loopBlend = true;

                        AnimationUtility.SetAnimationClipSettings(tempClip, settings);
                    }

                    controller.AddMotion(tempClip);
                }

                //Creates a prefab of the given Ojbect
                PrefabUtility.InstantiatePrefab(selectedObject);
                Instantiate(selectedObject);

                //Refreshs the AssetDatabase
                AssetDatabase.Refresh();
            }
        }
    }

    //Creates the animation
    public AnimationClip CreateClip(Object obj, string clipName, int startFrame, int endFrame, float frameRate, float timeBetween, bool pingPong)
    {

        string path = AssetDatabase.GetAssetPath(obj);

        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);

        int frameCount = endFrame - startFrame + 1;
        float frameLength = 1f / timeBetween;

        AnimationClip clip = new AnimationClip();

        clip.frameRate = frameRate;

        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] keyFrames;

        if (!pingPong)
        {
            keyFrames = new ObjectReferenceKeyframe[frameCount + 1];
        }
        else
        {
            keyFrames = new ObjectReferenceKeyframe[frameCount * 2 + 1];
        }

        int frameNumber = 0;

        for (int i = startFrame; i < endFrame + 1; i++, frameNumber++)
        {
            ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
            tempKeyFrame.time = frameNumber * frameLength;
            tempKeyFrame.value = sprites[i];
            keyFrames[frameNumber] = tempKeyFrame;
        }

        //sets up ping pong behaviour
        if (pingPong)
        {
            for (int i = endFrame; i >= startFrame; i--, frameNumber++)
            {
                ObjectReferenceKeyframe tempKeyFrame = new ObjectReferenceKeyframe();
                tempKeyFrame.time = frameNumber * frameLength;
                tempKeyFrame.value = sprites[i];
                keyFrames[frameNumber] = tempKeyFrame;
            }
        }

        ObjectReferenceKeyframe lastSprite = new ObjectReferenceKeyframe();
        lastSprite.time = frameNumber * frameLength;
        lastSprite.value = sprites[startFrame];
        keyFrames[frameNumber] = lastSprite;

        clip.name = clipName;

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

        //Creates the animation asset inside of the Animations folder
        AssetDatabase.CreateAsset(clip, ("Assets/" + clipName + ".anim"));

        return clip;
    }

    //Sets the program to close if the user enters or exits the player
    void OnFocus()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            this.Close();
        }
    }
}
