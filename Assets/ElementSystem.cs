using UnityEngine;

public static class ElementSystem
{
    // 环形克制链：美德1 -> 罪1 -> 美德2 -> 罪2 ...
    // 注：顺序不能乱，这是你笔记里设计的闭环克制环。
    private static readonly string[] Cycle = {
        "谦逊", "嫉妒", "宽容", "暴怒", "耐心", "懒惰",
        "勤勉", "贪婪", "慷慨", "暴食", "节制", "色欲",
        "贞洁", "傲慢"
    };

    /// <summary>
    /// 计算属性克制倍率
    /// </summary>
    /// <param name="attackerAttr">攻击方属性</param>
    /// <param name="defenderAttr">防御方属性</param>
    /// <returns>克制倍率：1.5(克制), 0.5(被克), 1.0(无关系/双属性抵消)</returns>
    public static float GetElementMultiplier(string attackerAttr, string defenderAttr)
    {
        if (string.IsNullOrEmpty(attackerAttr) || string.IsNullOrEmpty(defenderAttr) || defenderAttr == "无")
            return 1.0f;

        int attackIndex = System.Array.IndexOf(Cycle, attackerAttr);
        int defendIndex = System.Array.IndexOf(Cycle, defenderAttr);

        // 如果找不到属性（比如填了错别字），返回1.0
        if (attackIndex == -1 || defendIndex == -1) return 1.0f;

        // 计算环形距离（14种属性，正向+1是克制，反向-1是被克）
        int diff = defendIndex - attackIndex;
        if (diff == 1 || diff == -13) return 1.5f; // 克制
        if (diff == -1 || diff == 13) return 0.5f; // 被克
        return 1.0f;
    }
}