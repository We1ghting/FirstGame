using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ================== 1. 变量声明区 ==================
    public float speed = 2f;
    private GameObject player;

    // ================== 基础数值 ==================
    public float hp = 65f;         // 【修改】血量提升到65，配合本系加成刚好三下死
    private float currentHp;
    public float pdef = 10f;       // 物理防御
    public float mdef = 10f;       // 魔法防御
    public float patk = 50f;       // 物理攻击力
    public float matk = 40f;       // 魔法攻击力（改成了40）
    public float collisionPower = 10f; // 碰撞技能威力（把碰撞当作它的攻击技能）

    // ================== 新增：重量与属性 ==================
    [Range(0f, 21f)] public float wt = 1f; // 重量 (0 - 21，21代表霸体)
    public string sin1 = "嫉妒";            // 第一罪孽属性
    public string sin2 = "无";              // 第二罪孽属性

    // ================== 2. 生命周期方法区 ==================
    void Start()
    {
        currentHp = hp;
        // 找到主角
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            // 1. 计算指向主角的方向向量
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // 2. 朝主角移动
            transform.Translate(direction * speed * Time.deltaTime);

            // 3. 根据水平方向翻转图像
            if (direction.x > 0.01f)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction.x < -0.01f)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    // ================== 3. 自定义方法区 ==================
    // 敌人受到的伤害 // 【新增260926】参数增加
    public void TakeDamage(float shooterAtk, float skillPower, string attackType, float critMultiplier, string attackerAttr, string shooterVirtue) // 【新增260926】
    {
        // 1. 基础伤害
        float baseDamage = (shooterAtk / 50f) * skillPower;

        // 2. 获取自身防御力
        float defense = (attackType == "Physical") ? pdef : mdef;

        // 3. 防御减免
        float defenseMultiplier = 100f / (100f + defense);

        // 4. 属性克制计算（用子弹/技能属性 对抗 怪物属性） // 【新增260926】
        float elementMultiplier = ElementSystem.GetElementMultiplier(attackerAttr, sin1); // 【新增260926】

        // 5. 本系加成判定（子弹属性 是否等于 主角自身属性） // 【新增260926】
        float stabMultiplier = (attackerAttr == shooterVirtue) ? 1.2f : 1.0f; // 【新增260926】

        // 6. 最终伤害
        float finalDamage = baseDamage * defenseMultiplier * critMultiplier * elementMultiplier * stabMultiplier; // 【新增260926】

        // 7. 扣血
        currentHp -= finalDamage;
        Debug.Log($"苔芙受到 {finalDamage.ToString("F1")} 伤害 (克制:{elementMultiplier}, 本系:{stabMultiplier})，剩余: {currentHp.ToString("F1")}"); // 【新增260926】

        if (currentHp <= 0)
        {
            Debug.Log("苔芙阵亡了！");
            Destroy(gameObject);
        }
    }

    // 碰到主角造成伤害
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(patk, collisionPower, "Physical");
        }
    }
}