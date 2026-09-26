using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f; // 子弹飞行速度
    public Vector3 moveDirection = Vector3.up; // 默认向上，之后由玩家脚本控制
    public float bulletPower = 20f; // 子弹自身的技能威力
    public float shooterAtk = 50f;  // 记录发射这颗子弹时，主角的攻击力
    public float damageMultiplier = 1.0f; // 暴击倍率
    public string attackerAttribute = "贞洁"; // 【修改260926】子弹/技能自带的属性，根据你的设计，普攻就是贞洁
    public string shooterVirtue = "无"; // 【新增260926】记录发射这颗子弹时，主角自身的属性，用来判断本系加成
    public string attackType = "Magic"; // 【新增260926】这把武器（子弹）的攻击类型，物理还是魔法

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
            // 把攻击方属性、攻击力、威力、暴击倍率、发射者属性传给敌人
            other.GetComponent<EnemyController>().TakeDamage(shooterAtk, bulletPower, attackType, damageMultiplier, attackerAttribute, shooterVirtue);

            Destroy(gameObject); // 子弹消失
        }
    }
}