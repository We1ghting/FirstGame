using UnityEngine;
using UnityEngine.UI; // 【新增】必须加上，不然认不出 Image

public class PlayerController : MonoBehaviour
{
    // ================== 1. 变量声明区（全部放在最上面） ==================
    public float speed = 5f; // 移动速度

    // 子弹相关变量
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;
    private float nextFireTime = 0f;

    // 血量与防御属性
    public float maxHealth = 100f; // 最大血量
    private float currentHealth;   // 当前血量
    public float physDefense = 0f; // 物理防御
    public float magDefense = 0f;  // 魔法防御
    public Image healthBarFill; // 【新增】用来存放血条的 UI

    // ================== 2. 生命周期方法区 ==================
    void Start()
    {
        Debug.Log("小狗准备好移动了！");
        currentHealth = maxHealth;
        UpdateHealthUI(); // 【新增】开局先刷新一次血条，显示满血
    }

    void Update()
    {
        // 1. 获取键盘输入（WASD 或 方向键）
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 2. 把输入组合成一个方向向量
        Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // 3. 让小狗移动
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 4. 根据水平输入方向翻转图像
        if (moveX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        // 5. 方向键射击逻辑
        float shootX = 0;
        float shootY = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) shootX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) shootX = 1;
        if (Input.GetKey(KeyCode.UpArrow)) shootY = 1;
        if (Input.GetKey(KeyCode.DownArrow)) shootY = -1;

        if ((shootX != 0 || shootY != 0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Vector3 shootDirection = new Vector3(shootX, shootY, 0).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().moveDirection = shootDirection;
        }
    }
    // 【新增】专门用来刷新血条的方法，加在类里面
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // 把当前血量转换为 0 到 1 的百分比，交给血条填充
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    // ================== 3. 自定义方法区（与 Start、Update 平级） ==================
    public void TakeDamage(float attackPower, string attackType)
    {
        float defense = (attackType == "Physical") ? physDefense : magDefense;
        float damage = attackPower * (100f / (100f + defense));
        damage = Mathf.Round(damage * 10f) / 10f;

        currentHealth -= damage;
        Debug.Log("小狗受到 " + attackType + " 伤害 " + damage + "，剩余生命: " + currentHealth);

        UpdateHealthUI(); // 【新增】受伤后立刻刷新血条

        if (currentHealth <= 0)
        {
            Debug.Log("小狗阵亡了！");

            Time.timeScale = 0f; // 【新增】让游戏时间冻结，彻底暂停！

            Destroy(gameObject); // 销毁主角
        }
    }
}