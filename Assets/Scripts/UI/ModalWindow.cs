using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class ModalWindow : MonoBehaviour
{
    private CanvasGroup cg;
    public bool shown { get; private set; }

    public UnityAction OnModalWindowOpened;
    public UnityAction OnModalWindowClosed;

    protected virtual void Start()
    {
        cg = GetComponent<CanvasGroup>();

        shown = true;
        Hide();
    }

    public void Show()
    {
        if (shown)
            return;

        InputProvider.SwitchInputState(InputProvider.ActionMaps.UI);
        ModalWindowManager.AddWindowToQueue(this);

        cg.alpha = 1f;
        cg.interactable = cg.blocksRaycasts = true;

        shown = true;

        OnModalWindowOpened?.Invoke();
    }

    public void Hide()
    {
        if (!shown)
            return;

        ModalWindowManager.RemoveWindowFromQueue(this);

        cg.alpha = 0f;
        cg.interactable = cg.blocksRaycasts = false;

        shown = false;

        OnModalWindowClosed?.Invoke();
    }

    public void Toggle()
    {
        if (shown)
            Hide();
        else
            Show();
    }
}
