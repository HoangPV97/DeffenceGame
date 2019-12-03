using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEditor.SceneManagement;
[ExecuteInEditMode]
public class CustomTool : EditorWindow
{
    [SerializeField] string lastScene = "";
    [SerializeField] int targetScene = 0;
    [SerializeField] string waitScene = null;
    //	[SerializeField] bool hasPlayed = false;

    [MenuItem("Tools/Play From Scene %`")]
    public static void Run()
    {
        EditorWindow.GetWindow<CustomTool>();
    }

    static string[] sceneNames;
    static EditorBuildSettingsScene[] scenes;

    void OnEnable()
    {
        scenes = EditorBuildSettings.scenes;
        sceneNames = scenes.Select(x => AsSpacedCamelCase(Path.GetFileNameWithoutExtension(x.path))).ToArray();
        //  sceneNames = { 'Splash','Demo2' };

    }

    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            if (null == waitScene && !string.IsNullOrEmpty(lastScene))
            {
                EditorSceneManager.OpenScene(lastScene);
                lastScene = null;
            }
        }
    }

    void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            if (EditorApplication.currentScene == waitScene)
            {
                waitScene = null;
            }
            return;
        }

        if (EditorApplication.currentScene == waitScene)
        {
            EditorApplication.isPlaying = true;
        }
        if (null == sceneNames)
            return;
        targetScene = EditorGUILayout.Popup(targetScene, sceneNames);
        if (GUILayout.Button("Play"))
        {
            lastScene = EditorApplication.currentScene;
            waitScene = scenes[targetScene].path;
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(waitScene);
        }
    }

    public string AsSpacedCamelCase(string text)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder(text.Length * 2);
        sb.Append(char.ToUpper(text[0]));
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                sb.Append(' ');
            sb.Append(text[i]);
        }
        return sb.ToString();
    }

    [MenuItem("Tools/Cleanup Missing Scripts")]
    static void CleanupMissingScripts()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            var gameObject = Selection.gameObjects[i];

            // We must use the GetComponents array to actually detect missing components
            var components = gameObject.GetComponents<Component>();

            // Create a serialized object so that we can edit the component list
            var serializedObject = new SerializedObject(gameObject);
            // Find the component list property
            var prop = serializedObject.FindProperty("m_Component");

            // Track how many components we've removed
            int r = 0;

            // Iterate over all components
            for (int j = 0; j < components.Length; j++)
            {
                // Check if the ref is null
                if (components[j] == null)
                {
                    // If so, remove from the serialized component array
                    prop.DeleteArrayElementAtIndex(j - r);
                    // Increment removed count
                    r++;
                }
            }

            // Apply our changes to the game object
            serializedObject.ApplyModifiedProperties();
        }
    }

    [MenuItem("Tools/Build AssetBundles Android")]
    static void BuildAllAssetBundlesAndroid()
    {
        string assetBundleDirectory = "Assets/AssetBundlesAndroi";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("Tools/Build AssetBundles ios")]
    static void BuildAllAssetBundlesios()
    {
        string assetBundleDirectory = "Assets/AssetBundlesios";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    [MenuItem("Tools/Clear Data")]
    static void Clear()
    {
        PlayerPrefs.DeleteAll();
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        foreach (string filePath in filePaths)
            if (filePath.Contains(".dat"))
                File.Delete(filePath);
    }
}
