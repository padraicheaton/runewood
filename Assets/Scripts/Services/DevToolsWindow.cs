using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DevToolsWindow : ModalWindow
{
    public static UnityAction onDevToolsWindowRequested;

    protected override void Start()
    {
        base.Start();

        onDevToolsWindowRequested += Toggle;
    }

    public void OnDeleteSaveDataPressed() => SaveLoad.DeleteSaveData();
    public void OnLoadSaveBtnPressed() => SaveGameManager.TryLoadData();

    public void OnGiveAllItemsBtnPressed()
    {
        foreach (InventoryItemData item in ItemManager.GetDatabase().GetAllItems())
            PlayerInventoryHolder.OnAddToPlayerInventoryRequested(item, 1);
    }
}
