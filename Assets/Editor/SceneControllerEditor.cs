using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneController))]
public class SceneControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneController sceneController = (SceneController)target;

        EditorGUILayout.LabelField("");

        EditorGUILayout.LabelField("Scene Switch Buttons", EditorStyles.boldLabel);

        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();

        float btnWidth = (Screen.width / 2f) - 5f;

        if (GUILayout.Button("Home Area", GUILayout.Width(btnWidth)))
            sceneController.EditorLoadScene(SceneController.Scene.HomeArea);
        if (GUILayout.Button("Main Menu", GUILayout.Width(btnWidth)))
            sceneController.EditorLoadScene(SceneController.Scene.MainMenu);

        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();

        if (GUILayout.Button("Safe Play"))
            sceneController.SafeEnterPlay();

    }
}
