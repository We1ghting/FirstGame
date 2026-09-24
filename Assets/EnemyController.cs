using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ================== 1. 变量声明区 ==================
    public float speed = 2f;
    private GameObject player;

    // ================== 基础数值 ==================
    public float hp = 54f;         // 血量（刚好三下死）
    private float currentHp;
    public float pdef = 10f;       // 物理防御
    public float mdef = 10f;       // 魔法防御
    public float patk = 50f;       // 物理攻击力
    public float matk = 40f;       // 魔法攻击力
    public float collisionPower = 10f; // 碰撞技能威力

    // ================== 新增：重量与属性 ==================
    [Range(0f, 21f)] public float wt = 1f; // 重量
    public string sin1 = "嫉妒";            // 第一罪孽属性
    public string sin2 = "无";              // 第二罪孽属性

    // ================== 2. 生命周期方法区 ==================
    void Start()
    {
        currentHp = hp;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            if (direction.x > 0.01f) GetComponent<SpriteRenderer>().flipX = false;
            else if (direction.x < -0.01f) GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // ================== 3. 自定义方法区 ==================
    // 敌人受到的伤害（接收子弹传来的数据）
    public void TakeDamage(float shooterAtk, float skillPower, string attackType, float critMultiplier)
    {
        // 1. 基础伤害
        float baseDamage = (shooterAtk / 50f) * skillPower;

        // 2. 获取自身防御力
        float defense = (attackType == "Physical") ? pdef : mdef;

        // 3. 防御减免
        float defenseMultiplier = 100f / (100f + defense);

        // 4. 最终伤害（乘上暴击倍率）
        float finalDamage = baseDamage * defenseMultiplier * critMultiplier;

        // 5. 扣血
        currentHp -= finalDamage;
        Debug.Log("苔芙受到 " + finalDamage.ToString("F1") + " 伤害，剩余血量: " + currentHp.ToString("F1"));

        if (currentHp <= 0)
        {
            Debug.Log("苔芙阵亡了！");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(patk, collisionPower, "Physical");
        }
    }
}