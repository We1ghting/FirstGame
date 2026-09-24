using UnityEngine;
using UnityEngine.UI; // 【新增】必须加上，不然认不出 Image

public class PlayerController : MonoBehaviour
{
    // ================== 1. 变量声明区（全部放在最上面） ==================
    public float speed = 5f; // 移动速度

    // 子弹相关变量
    public GameObject bulletPrefab;
    public float fireRate = 0.3f; // 射速
    private float nextFireTime = 0f;

    // ================== 基础数值 ==================
    public float hp = 100f;        // 最大血量
    private float currentHp;       // 当前血量 (统一用 currentHp)
    public float pdef = 0f;        // 物理防御
    public float mdef = 0f;        // 魔法防御

    // 攻击属性
    public float atk = 20f;        // 物理攻击力（改成20了）
    public float matk = 50f;       // 魔法攻击力（主角目前都用这个）

    // ================== 新增：暴击、重量、属性 ==================
    [Range(0f, 1f)] public float critRate = 0.02f;   // 暴击率 (0% - 100%)
    [Range(0f, 3f)] public float critDamage = 1.1f;  // 暴击伤害 (0% - 300%，1.1就是110%)
    [Range(0f, 21f)] public float wt = 2f;           // 重量 (0 - 21，21代表霸体)
    public string virtue1 = "贞洁";                  // 第一美德属性（待实装克制）
    public string virtue2 = "无";                    // 第二美德属性

    // 血条 UI
    public Image healthBarFill; // 【新增】用来存放血条的 UI

    // ================== 2. 生命周期方法区 ==================
    void Start()
    {
        currentHp = hp; // 【修复】开局满血，统一用 currentHp
        Debug.Log("小狗准备好移动了！");
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

            // ========== 核心修改：子弹接收主角的属性 ==========
            BulletController bulletScript = bullet.GetComponent<BulletController>();
            bulletScript.moveDirection = shootDirection;

            // 1. 把主角当前的魔攻打在子弹身上
            bulletScript.shooterAtk = matk;

            // 2. 掷骰子判定暴击，结果打在子弹身上
            if (Random.value <= critRate)
            {
                bulletScript.damageMultiplier = critDamage; // 暴击！
                Debug.Log("小狗触发暴击！");
            }
            else
            {
                bulletScript.damageMultiplier = 1.0f; // 普通伤害
            }
        }
    }

    // 【新增】专门用来刷新血条的方法
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // 把当前血量转换为 0 到 1 的百分比，交给血条填充
            healthBarFill.fillAmount = currentHp / hp;
        }
    }

    // ================== 3. 自定义方法区 ==================
    // 新的 TakeDamage 方法：接收对方的攻击力和技能威力
    public void TakeDamage(float enemyAtk, float skillPower, string attackType)
    {
        // 1. 计算攻击方的基础伤害
        float baseDamage = (enemyAtk / 50f) * skillPower;

        // 2. 获取自身防御力
        float defense = (attackType == "Physical") ? pdef : mdef;

        // 3. 计算防御减免（乘除法）
        float defenseMultiplier = 100f / (100f + defense);

        // 4. 最终伤害
        float finalDamage = baseDamage * defenseMultiplier;

        // 5. 扣血
        currentHp -= finalDamage;
        Debug.Log("小狗受到 " + finalDamage.ToString("F1") + " 伤害，剩余血量: " + currentHp.ToString("F1"));

        UpdateHealthUI(); // 【新增】受伤后立刻刷新血条

        if (currentHp <= 0)
        {
            Debug.Log("小狗阵亡了！");
            Time.timeScale = 0f; // 【新增】让游戏时间冻结，彻底暂停！
            Destroy(gameObject); // 销毁主角
        }
    }
}