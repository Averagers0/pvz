using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> level1Cards;

    private void Start()
    {
        LoadLevel1();
    }

    public void LoadLevel1()
    {
        if (CardManager.Instance != null)
        {
            CardManager.Instance.cardPrefabs = level1Cards;
            CardManager.Instance.GenerateCards();
        }
        else
        {
            Debug.LogError("CardManager.Instance is null! 请确认场景中有 CardManager 组件.");
        }
    }
}


