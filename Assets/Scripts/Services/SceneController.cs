using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public enum Scene // This is a mirror of the build settings, and should be updated whenever that is updated
    {
        ServicesLayer, // Contains scripts that are needed no matter what other scene is loaded (where this script sits)
        MainMenu,
        GameServices, // Contains scripts that are needed anywhere during the standard gameplay loop
        HomeArea // Contains the level layout for the home area
    }

    private List<AsyncOperation> sceneLoadingOperations = new List<AsyncOperation>();

    public bool IsLoading => sceneLoadingOperations.Count > 0;
    public UnityAction<Scene> OnSceneLoaded;


    [SerializeField] private Scene primaryScene;

    public Scene ActiveScene { get; private set; }

    private void Awake()
    {
        LoadScene(primaryScene);
    }

#if UNITY_EDITOR
    public void EditorLoadScene(Scene scene)
    {
        if (Application.isPlaying)
            return;

        primaryScene = scene;

        LoadScene(scene);
    }

    public void SafeEnterPlay()
    {
        List<Scene> allScenes = Enum.GetValues(typeof(Scene)).Cast<Scene>().ToList();

        allScenes.Remove(Scene.ServicesLayer);

        UnloadSceneListEditor(allScenes);

        EditorApplication.EnterPlaymode();
    }
#endif

    public void LoadScene(Scene scene)
    {
        // Unload any scenes that are not needed
        List<Scene> loadedScenes = GetActiveScenes();

        // If destination scene is already loaded, return out of this function
        if (IsSceneLoaded(scene))
            return;

        // Don't unload the services layer
        loadedScenes.Remove(Scene.ServicesLayer);

        // Don't unload any required scenes
        foreach (Scene requiredScene in GetRequiredScenesForScene(scene))
            loadedScenes.Remove(requiredScene);

        // Unload any remaining scenes
        if (Application.isPlaying)
            UnloadSceneList(loadedScenes);
        else
            UnloadSceneListEditor(loadedScenes);

        List<Scene> scenesToLoad = new List<Scene>();

        // Add all required scenes that haven't been loaded yet
        scenesToLoad.AddRange(GetRequiredScenesForScene(scene).Where(s => !IsSceneLoaded(s)));

        // Finally add the desired scene to load last, so that it is loaded after the required scenes have loaded
        scenesToLoad.Add(scene);

        // Load all remaining needed scenes
        if (Application.isPlaying)
            LoadSceneList(scenesToLoad);
        else
            LoadSceneListEditor(scenesToLoad);

        ActiveScene = scene;

        OnSceneLoaded?.Invoke(scene);
    }

    private void UnloadSceneList(List<Scene> scenesToUnload)
    {
        foreach (Scene unusedScene in scenesToUnload)
            sceneLoadingOperations.Add(SceneManager.UnloadSceneAsync((int)unusedScene));
    }

    private void LoadSceneList(List<Scene> scenesToLoad)
    {
        foreach (Scene sceneToLoad in scenesToLoad)
            sceneLoadingOperations.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));
    }

    private void UnloadSceneListEditor(List<Scene> scenesToUnload)
    {
        foreach (Scene unusedScene in scenesToUnload)
            //sceneLoadingOperations.Add(SceneManager.UnloadSceneAsync((int)unusedScene));
            EditorSceneManager.CloseScene(SceneManager.GetSceneByBuildIndex((int)unusedScene), true);
    }

    private void LoadSceneListEditor(List<Scene> scenesToLoad)
    {
        foreach (Scene sceneToLoad in scenesToLoad)
            //sceneLoadingOperations.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex((int)sceneToLoad), OpenSceneMode.Additive);
    }

    private List<Scene> GetRequiredScenesForScene(Scene scene)
    {
        List<Scene> requiredScenes = new List<Scene>();

        switch (scene)
        {
            case Scene.HomeArea:
                requiredScenes.Add(Scene.GameServices);
                break;
            default:
                break;
        }

        return requiredScenes;
    }

    private List<Scene> GetActiveScenes()
    {
        List<Scene> activeScenes = new List<Scene>();

        foreach (Scene scene in Enum.GetValues(typeof(Scene)))
            if (IsSceneLoaded(scene))
                activeScenes.Add(scene);

        return activeScenes;
    }

    private bool IsSceneLoaded(Scene scene) => SceneManager.GetSceneByBuildIndex((int)scene).isLoaded;

    public void MoveGameObjectToActiveScene(GameObject obj)
    {
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByBuildIndex((int)ActiveScene));
    }
}
