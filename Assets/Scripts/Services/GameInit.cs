using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void OnEnable()
    {
        InputProvider.OnEnable();
        ModalWindowManager.Init();
    }

    private void OnDisable()
    {
        InputProvider.OnDisable();
    }

    private void Start()
    {
        SaveGameManager.TryLoadData();
    }
}
