using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // 子弹飞行速度
    public Vector3 moveDirection = Vector3.up; // 默认向上，之后由玩家脚本控制

    void Update()
    {
        // 让子弹沿着Y轴正上方飞
        // 让子弹沿着玩家指定的方向飞
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 飞出屏幕2秒后自动销毁，防止游戏卡死
        Destroy(gameObject, 2f);
    }
}
