using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    [Header("Card Slots")]
    public Transform[] cardSlots;  // 8个固定卡槽

    [Header("Available Cards for this Level")]
    public List<GameObject> cardPrefabs; // 关卡要用的卡片Prefab列表（直接放CardPeaShooter等）

    private List<GameObject> cardInstances = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateCards();
    }

    // 生成卡片
    public void GenerateCards()
    {
        ClearCards();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i < cardPrefabs.Count)
            {
                GameObject card = Instantiate(cardPrefabs[i], cardSlots[i]);
                cardInstances.Add(card);
            }
        }
    }

    // 清理卡片
    private void ClearCards()
    {
        foreach (var card in cardInstances)
        {
            Destroy(card);
        }
        cardInstances.Clear();
    }
}
