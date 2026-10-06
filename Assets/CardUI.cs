using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public CardData cardData;      // 这张卡牌的数据
    public Image cardImage;        // 用来显示你画的完整卡牌图片

    // 【新增261006】初始化卡牌：直接把完整图片贴上去
    public void Setup(CardData data)
    {
        cardData = data;
        if (data != null && cardImage != null)
        {
            cardImage.sprite = data.icon; // 把数据里的图片赋给Image组件
            cardImage.preserveAspect = true; // 保持图片比例，防止变形
        }
    }

    // 鼠标点击卡牌时触发的函数
    public void OnCardClicked()
    {
        if (CardManager.Instance != null && cardData != null)
        {
            CardManager.Instance.SelectCard(cardData);
        }
    }
}