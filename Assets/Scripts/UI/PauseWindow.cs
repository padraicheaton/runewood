using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseWindow : ModalWindow
{
    public static UnityAction onPauseMenuRequested;

    protected override void Start()
    {
        base.Start();

        onPauseMenuRequested += TogglePauseMenu;
    }

    private void TogglePauseMenu()
    {
        Toggle();

        // Pause??
    }

    public void ResumeBtnPressed() => ModalWindowManager.ClearAllWindows();
    public void SaveBtnPressed() => SaveGameManager.SaveData();
    public void DevToolsBtnPressed() => DevToolsWindow.onDevToolsWindowRequested?.Invoke();
}
