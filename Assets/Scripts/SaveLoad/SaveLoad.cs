using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadGame;

    private static string SaveDirectory = "/SaveData/";
    private static string FileName = "SaveGame.sav";

    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string dir = Application.persistentDataPath + SaveDirectory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(dir + FileName, json);

        GUIUtility.systemCopyBuffer = dir; // This just copies the save folder location to the clipboard for ease of navigation

        return true;
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        SaveData temporaryData = new SaveData();

        bool saveFileExists = File.Exists(fullPath);

        if (saveFileExists)
        {
            string json = File.ReadAllText(fullPath);

            temporaryData = JsonUtility.FromJson<SaveData>(json);

            OnLoadGame?.Invoke(temporaryData);
        }

        return temporaryData;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + SaveDirectory + FileName;

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        Load(); // This will create a fresh save file and load it into the game
    }
}
