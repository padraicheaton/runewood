using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ModalWindow : MonoBehaviour
{
    private CanvasGroup cg;
    public bool shown { get; private set; }

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
    }

    public void Hide()
    {
        if (!shown)
            return;

        ModalWindowManager.RemoveWindowFromQueue(this);

        cg.alpha = 0f;
        cg.interactable = cg.blocksRaycasts = false;

        shown = false;
    }

    public void Toggle()
    {
        if (shown)
            Hide();
        else
            Show();
    }
}
