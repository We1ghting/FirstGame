using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    private GameObject player;

    void Start()
    {
        // 游戏开始时找到带有PlayerController脚本的物体
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            // 计算从敌人指向玩家的方向向量
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // 沿着这个方向移动
            transform.Translate(direction * speed * Time.deltaTime);

            // 【新增】根据水平方向翻转图像（加一点容差防止正上下时鬼畜）
            if (direction.x > 0.01f)
            {
                GetComponent<SpriteRenderer>().flipX = false; // 往右走，不翻转（假设原图朝右）
            }
            else if (direction.x < -0.01f)
            {
                GetComponent<SpriteRenderer>().flipX = true;  // 往左走，翻转
            }
        }
    }
}