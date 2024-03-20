using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSaveData : MonoBehaviour
{
    public static UnityAction<PlayerData> onPlayerDataLoaded;

    private void OnEnable()
    {
        SaveLoad.OnSaveGame += OnSaveGame;
        SaveGameManager.OnGameSuccessfullyLoaded += OnLoadGame;
    }

    private void OnDisable()
    {
        SaveLoad.OnSaveGame -= OnSaveGame;
        SaveGameManager.OnGameSuccessfullyLoaded -= OnLoadGame;
    }

    private void OnSaveGame()
    {
        SaveGameManager.CurrentSaveData.playerData.position = transform.position;
        SaveGameManager.CurrentSaveData.playerData.rotation = transform.rotation;
    }

    private void OnLoadGame(SaveData data)
    {
        onPlayerDataLoaded?.Invoke(data.playerData);
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
}