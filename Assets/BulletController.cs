using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // 子弹飞行速度
    public Vector3 moveDirection = Vector3.up; // 默认向上，之后由玩家脚本控制
    public float attackPower = 40f; // 魔法攻击力

    void Update()
    {
        // 让子弹沿着Y轴正上方飞
        // 让子弹沿着玩家指定的方向飞
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 飞出屏幕2秒后自动销毁，防止游戏卡死
        Destroy(gameObject, 2f);
    }
    // 在BulletController类里面加这个方法
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 调用敌人的受伤方法，攻击类型是魔法
            other.GetComponent<EnemyController>().TakeDamage(attackPower, "Magic");
            Destroy(gameObject); // 子弹消失
        }
    }
}
