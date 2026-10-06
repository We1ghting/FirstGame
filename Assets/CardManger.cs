using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance; // 【新增261006】单例，方便主角调用

    [Header("UI 引用")]
    public GameObject cardSelectionPanel; // 【新增261006】整个面板
    public Transform cardContainer;       // 【新增261006】卡牌父节点
    public GameObject cardUIPrefab;       // 【新增261006】卡牌预制体

    [Header("卡牌数据池")]
    public CardData[] allCards;           // 【新增261006】把 Project/Cards 里的卡牌全拖进来

    void Awake()
    {
        Instance = this;
        cardSelectionPanel.SetActive(false); // 【新增261006】开局隐藏面板
    }

    // 【新增261006】供主角升级时调用的方法
    public void OpenCardSelection()
    {
        cardSelectionPanel.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏

        // 清理上一次的卡牌
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // 筛选卡池
        List<CardData> freePool = new List<CardData>();
        List<CardData> shopPool = new List<CardData>();

        foreach (var card in allCards)
        {
            if (card.isShopCard) shopPool.Add(card);
            else freePool.Add(card);
        }

        List<CardData> selectedCards = new List<CardData>();

        // 【修改261006】严格按顺序：2张免费 + 2张商店
        // 抽2张免费
        for (int i = 0; i < 2; i++)
        {
            if (freePool.Count > 0)
            {
                int randIndex = Random.Range(0, freePool.Count);
                selectedCards.Add(freePool[randIndex]);
                freePool.RemoveAt(randIndex);
            }
        }

        // 抽2张商店
        for (int i = 0; i < 2; i++)
        {
            if (shopPool.Count > 0)
            {
                int randIndex = Random.Range(0, shopPool.Count);
                selectedCards.Add(shopPool[randIndex]);
                shopPool.RemoveAt(randIndex);
            }
        }

        // 【修改261006】删除了生成第5张卡的代码，现在总共只生成4张卡

        // 生成4张卡牌
        foreach (CardData data in selectedCards)
        {
            GameObject cardObj = Instantiate(cardUIPrefab, cardContainer);
            CardUI cardUI = cardObj.GetComponent<CardUI>();
            if (cardUI != null)
            {
                cardUI.Setup(data);
            }
        }
    }
    // 【新增261006】供卡牌点击时调用的方法
    public void SelectCard(CardData selectedData)
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (selectedData.isShopCard)
        {
            // 如果是商店卡，检查美德币够不够
            if (player.currentVirtue >= selectedData.price)
            {
                player.currentVirtue -= selectedData.price;
                Debug.Log($"购买成功！消耗 {selectedData.price} 美德币");
            }
            else
            {
                Debug.Log("美德币不足，无法购买！");
                return; // 钱不够，直接返回，不关闭面板
            }
        }

        // 【修改261006】把属性应用到主角身上
        player.ApplyCardEffect(selectedData);

        Debug.Log($"获得了卡牌: {selectedData.cardName}");

        cardSelectionPanel.SetActive(false);
        Time.timeScale = 1f; // 恢复游戏
    }
}