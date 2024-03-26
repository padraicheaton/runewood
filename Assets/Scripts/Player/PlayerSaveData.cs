using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSaveData : MonoBehaviour
{
    public static UnityAction<PlayerData> onPlayerDataLoaded;

    private void Start()
    {
        // Assuming that the save data has already been loaded
        onPlayerDataLoaded?.Invoke(SaveGameManager.CurrentSaveData.playerData);
    }

    private void OnEnable()
    {
        SaveLoad.OnSaveGame += OnSaveGame;
    }

    private void OnDisable()
    {
        SaveLoad.OnSaveGame -= OnSaveGame;
    }

    private void OnSaveGame()
    {
        SaveGameManager.CurrentSaveData.playerData.position = transform.position;
        SaveGameManager.CurrentSaveData.playerData.rotation = transform.rotation;
    }
}

[System.Serializable]
public struct PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
}