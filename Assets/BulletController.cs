using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // 子弹飞行速度
    public Vector3 moveDirection = Vector3.up; // 默认向上，之后由玩家脚本控制

    // ================== 新增：子弹自身的属性 ==================
    public float bulletPower = 20f; // 子弹自身的技能威力（写死在子弹身上，而不是主角身上）
    public float shooterAtk = 50f;  // 记录发射这颗子弹时，主角的攻击力
    public float damageMultiplier = 1.0f; // 伤害倍率（暴击判定结果，默认1.0）

    void Update()
    {
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
            // 把子弹自带的攻击力、子弹威力、攻击类型，以及伤害倍率一起传给敌人
            other.GetComponent<EnemyController>().TakeDamage(shooterAtk, bulletPower, "Magic", damageMultiplier);

            Destroy(gameObject); // 子弹消失
        }
    }
}