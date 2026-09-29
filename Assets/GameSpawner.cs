using UnityEngine;
using TMPro; // 【新增260926】必须加上，不然认不出 TextMeshPro

public class GameSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 存放苔芙预制体
    public float spawnInterval = 2f; // 每 2 秒刷一只怪

    public TextMeshProUGUI timerText; // 【新增260926】存放 UI 文本变量
    private float survivalTime = 0f; // 【新增260926】生存时间变量

    void Start()
    {
        // 开局立刻开始刷怪，并每隔 spawnInterval 重复执行
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 找到主角当前位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // 在屏幕外的随机边缘位置生成
        float radius = 8f; // 根据你摄像机视野大小调整
        float angle = Random.Range(0f, Mathf.PI * 2f); // 0 到 2π 的随机角度

        // 【修改260929】计算刷怪位置，基于主角当前的位置
        Vector2 spawnPos = (Vector2)player.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        // 【新增260929】限制刷怪位置不能超出地图边界（根据你的地图大小 -24 到 24）
        spawnPos.x = Mathf.Clamp(spawnPos.x, -24f, 24f);
        spawnPos.y = Mathf.Clamp(spawnPos.y, -24f, 24f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void Update()
    {
        // 【新增260926】更新生存时间
        survivalTime += Time.deltaTime;

        if (timerText != null)
        {
            // 【修改260926】原来的 Debug.Log 删掉，换成下面这个
            timerText.text = "生存时间: " + survivalTime.ToString("F1") + " 秒";
        }
    }
}