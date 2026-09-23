using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ================== 1. 变量声明区 ==================
    public float speed = 2f;
    private GameObject player;

    // 血量与属性
    public float maxHealth = 90f;
    private float currentHealth;
    public float physDefense = 5f;   // 物理防御
    public float magDefense = 10f;   // 魔法防御
    public float attackPower = 10f;  // 物理攻击力

    // ================== 2. 生命周期方法区 ==================
    void Start()
    {
        currentHealth = maxHealth;
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
    // 这就是报错缺失的方法！
    public void TakeDamage(float attackPower, string attackType)
    {
        float defense = (attackType == "Physical") ? physDefense : magDefense;
        float damage = attackPower * (100f / (100f + defense));
        damage = Mathf.Round(damage * 10f) / 10f; // 保留一位小数

        currentHealth -= damage;
        Debug.Log("苔芙受到 " + attackType + " 伤害 " + damage + "，剩余生命: " + currentHealth);

        if (currentHealth <= 0)
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
            other.GetComponent<PlayerController>().TakeDamage(attackPower, "Physical");
        }
    }
}