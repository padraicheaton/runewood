using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData CurrentSaveData;

    public static UnityAction<SaveData> OnGameSuccessfullyLoaded;

    private void Awake()
    {
        CurrentSaveData = new SaveData();

        SaveLoad.OnLoadGame += LoadData;
    }

    public void DeleteSaveData()
    {
        SaveLoad.DeleteSaveData();
    }

    public static void SaveData()
    {
        SaveData saveDataCopy = CurrentSaveData;

        SaveLoad.Save(saveDataCopy);
    }

    private static void LoadData(SaveData _data)
    {
        CurrentSaveData = _data;

        // Refresh the item database
        ItemManager.GetDatabase().SetItemIDs();

        OnGameSuccessfullyLoaded?.Invoke(CurrentSaveData);
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
