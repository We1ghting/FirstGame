using UnityEngine;

// 【新增261006】创建卡牌数据资产，不需要挂载在物体上
[CreateAssetMenu(fileName = "New Card", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("基础信息")]
    public string cardName;        // 卡牌名字
    public string description;     // 卡牌效果描述
    public Sprite icon;            // 卡牌图标的 Sprite

    [Header("稀有度与商店")]
    public int rarityLevel;        // 稀有度等级：1-6
    public int price = 0;          // 购买价格
    public bool isShopCard = false; // 是否为商店卡

    [Header("特殊词条")]
    public bool isUnique = false;   // 唯一
    public bool isHoly = false;     // 神圣
    public bool isCurse = false;    // 诅咒

    // ================== 基础属性的独立加值与倍率 ==================
    [Header("物理攻击")]
    public float pAtkAdd = 0f;      // 物理攻击 加值
    public float pAtkMulAdd = 0f;   // 物理攻击 倍率加成（填0.5代表乘1.5）

    [Header("魔法攻击")]
    public float mAtkAdd = 0f;      // 魔法攻击 加值
    public float mAtkMulAdd = 0f;   // 魔法攻击 倍率加成

    [Header("生命上限")]
    public float hpAdd = 0f;        // 生命上限 加值
    public float hpMulAdd = 0f;     // 【新增261006】生命上限 倍率加成

    [Header("物理防御")]
    public float pDefAdd = 0f;      // 物理防御 加值
    public float pDefMulAdd = 0f;   // 【新增261006】物理防御 倍率加成

    [Header("魔法防御")]
    public float mDefAdd = 0f;      // 魔法防御 加值
    public float mDefMulAdd = 0f;   // 【新增261006】魔法防御 倍率加成

    [Header("暴击率")]
    public float crRAdd = 0f;       // 暴击率 加值
    public float crRMulAdd = 0f;    // 【新增261006】暴击率 倍率加成

    [Header("暴击伤害")]
    public float crDAdd = 0f;       // 暴击伤害 加值
    public float crDMulAdd = 0f;    // 【新增261006】暴击伤害 倍率加成

    [Header("移动速度")]
    public float spdAdd = 0f;       // 移动速度 加值
    public float spdMulAdd = 0f;    // 【新增261006】移动速度 倍率加成

    [Header("攻击速度")]
    public float atkSpeedAdd = 0f;  // 攻速加成
    public float atkSpeedMulAdd = 0f;// 【新增261006】攻速倍率加成

    [Header("射程")]
    public float rangeAdd = 0f;     // 射程加成
    public float rangeMulAdd = 0f;  // 【新增261006】射程倍率加成
}