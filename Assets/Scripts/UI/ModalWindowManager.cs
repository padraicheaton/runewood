using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ModalWindowManager
{
    private static List<ModalWindow> modalWindows;

    public static void Init()
    {
        modalWindows = new List<ModalWindow>();

        InputProvider.onExitMenuButtonPressed += () => OnExitButtonPressed();
    }

    public static void AddWindowToQueue(ModalWindow window)
    {
        modalWindows.Add(window);

        InputProvider.SwitchInputState(InputProvider.ActionMaps.UI);
    }

    public static void RemoveWindowFromQueue(ModalWindow window)
    {
        if (modalWindows.Contains(window))
            modalWindows.Remove(window);

        if (modalWindows.Count == 0)
            InputProvider.SwitchInputState(InputProvider.ActionMaps.OnFoot);
    }

    public static void OnExitButtonPressed()
    {
        if (modalWindows.Count > 0)
        {
            modalWindows.Last().Hide(); // This also removes itself from the list
        }
        else
            PauseWindow.onPauseMenuRequested?.Invoke();
    }

    public static void ClearAllWindows()
    {
        while (modalWindows.Count > 0)
            modalWindows.Last().Hide();
    }
}
