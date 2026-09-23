using UnityEngine;
using TMPro; // 【新增1】必须加上这一行

public class GameSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 存放苔芙预制体
    public float spawnInterval = 2f; // 每 2 秒刷一只怪

    public TextMeshProUGUI timerText; // 【新增1】存放 UI 文本变量

    void Start()
    {
        // 开局立刻开始刷怪，并每隔 spawnInterval 重复执行
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        // 在屏幕外的随机边缘位置生成
        // 简单写法：在距中心固定半径的圆上随机取点
        float radius = 8f; // 根据你摄像机视野大小调整
        float angle = Random.Range(0f, Mathf.PI * 2f); // 0 到 2π 的随机角度
        Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    private float survivalTime = 0f;

    void Update()
    {
        survivalTime += Time.deltaTime;

        // 【修改1】原来的 Debug.Log 删掉，换成下面这个
        if (timerText != null)
        {
            timerText.text = "生存时间: " + survivalTime.ToString("F1") + " 秒";
        }
    }
}
