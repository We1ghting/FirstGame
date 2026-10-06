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
    public float range = 24f; // 【新增261006】子弹的最大飞行距离
    private Vector3 startPosition; // 【新增261006】记录子弹发射的起点

    void Update()
    {
        // 让子弹沿着玩家指定的方向飞
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 【修改261006】新增：计算子弹已经飞了多远，如果超出射程就销毁
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
            return; // 销毁后直接退出Update，防止后续代码报错
        }

        // 飞出屏幕2秒后自动销毁，防止游戏卡死
        Destroy(gameObject, 2f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 把攻击方属性、攻击力、威力、暴击倍率、发射者属性传给敌人
            other.GetComponent<EnemyController>().TakeDamage(shooterAtk, bulletPower, attackType, damageMultiplier, attackerAttribute, shooterVirtue);

            Destroy(gameObject); // 子弹消失
        }
    }
    void Start()
    {
        startPosition = transform.position; // 【新增261006】开局记录当前位置
    }
}