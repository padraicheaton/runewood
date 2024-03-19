using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData CurrentSaveData;

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
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}
