using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        InputProvider.SwitchInputState(InputProvider.ActionMaps.UI);
    }

    public void OnPlayBtnPressed()
    {
        SceneController.Instance.LoadScene(SceneController.Scene.HomeArea);
    }

    public void OnSettingsBtnPressed()
    {

    }

    public void OnExitBtnPressed()
    {
        Application.Quit();
    }
}
