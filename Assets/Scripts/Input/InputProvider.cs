using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InputProvider
{
    public enum ActionMaps
    {
        OnFoot,
        UI
    }
    public static ActionMaps activeActionMap { get; private set; }

    // Input Class References
    private static PlayerInput playerInput;
    private static PlayerInput.OnFootActions onFoot;
    private static PlayerInput.UIActions uiActions;
    private static PlayerInput.MenusActions menusActions;

    public static void OnEnable()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;
        uiActions = playerInput.UI;
        menusActions = playerInput.Menus;

        menusActions.Enable();

        SetupInputEvents();

        SwitchInputState(ActionMaps.OnFoot);
    }

    public static void OnDisable()
    {
        onFoot.Disable();
        uiActions.Disable();
        menusActions.Disable();
    }

    public static void SwitchInputState(ActionMaps actionMapToSwitchTo)
    {
        onFoot.Disable();
        uiActions.Disable();

        switch (actionMapToSwitchTo)
        {
            case ActionMaps.OnFoot:
            default:
                onFoot.Enable();
                break;
            case ActionMaps.UI:
                uiActions.Enable();
                break;
        }

        activeActionMap = actionMapToSwitchTo;

        SetCursorState(activeActionMap == ActionMaps.UI);
    }

    private static void SetCursorState(bool active)
    {
        Cursor.visible = active;
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private static void SetupInputEvents()
    {
        menusActions.Backpack.performed += ctxt => onBackpackButtonPressed?.Invoke();
        menusActions.Exit.performed += ctxt => onExitMenuButtonPressed?.Invoke();

        onFoot.Interact.performed += ctxt => onInteractButtonPressed?.Invoke();
        onFoot.Jump.performed += ctxt => onJumpButtonPressed?.Invoke();

        onFoot.UseSlot1.performed += ctxt => onUseItemButtonPressed?.Invoke(0);
        onFoot.UseSlot2.performed += ctxt => onUseItemButtonPressed?.Invoke(1);
        onFoot.UseSlot3.performed += ctxt => onUseItemButtonPressed?.Invoke(2);
        onFoot.UseSlot4.performed += ctxt => onUseItemButtonPressed?.Invoke(3);
        onFoot.UseSlot5.performed += ctxt => onUseItemButtonPressed?.Invoke(4);
    }

    // Public Methods
    public static Vector2 MovementInput => onFoot.Movement.ReadValue<Vector2>();
    public static bool SprintPressed => onFoot.Sprint.IsPressed();
    public static bool SplitItemPressed => uiActions.SplitItem.IsPressed();

    // Events
    public static UnityAction onBackpackButtonPressed;
    public static UnityAction onExitMenuButtonPressed;
    public static UnityAction onInteractButtonPressed;
    public static UnityAction onJumpButtonPressed;
    public static UnityAction<int> onUseItemButtonPressed;
}
