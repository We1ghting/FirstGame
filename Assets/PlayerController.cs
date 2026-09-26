using UnityEngine;
using UnityEngine.UI; // 【新增】必须加上，不然认不出 Image
using TMPro; // 【新增260926】用于属性UI文本

public class PlayerController : MonoBehaviour
{
    // ================== 1. 变量声明区（全部放在最上面） ==================
    public float speed = 5f; // 移动速度

    // 子弹相关变量
    public GameObject bulletPrefab;
    [Range(10f, 500f)] public float atkSpeed = 100f; // 【修改260926】攻速属性，两位数控制，值越大越快，默认为100
    private float nextFireTime = 0f; // 【修改260926】冷却计时器保留

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

    // ================== 新增260926：倍率变量（为了以后做属性提升道具预留的） ==================
    public float pAtkMul = 1.0f;   // 物理攻击倍率 (1.0 = 100%)
    public float pDefMul = 1.0f;   // 物理防御倍率
    public float mAtkMul = 1.0f;   // 魔法攻击倍率
    public float mDefMul = 1.0f;   // 魔法防御倍率
    public float crRMul = 1.0f;    // 暴击率倍率
    public float crDMul = 1.0f;    // 暴击伤害倍率
    public float wtMul = 1.0f;     // 重量倍率

    // 血条 UI
    public Image healthBarFill; // 【新增】用来存放血条的 UI
    public TextMeshProUGUI hpText; // 【新增260926】血条上的血量文本
    public TextMeshProUGUI statText; // 【新增260926】左侧属性面板的 UI 文本

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
            // 【修改260926】根据当前攻速，动态计算实际冷却时间 (0.45秒 是基准间隔)
            float currentFireRate = 0.45f / (atkSpeed / 100f); // 【修改260926】
            nextFireTime = Time.time + currentFireRate; // 【修改260926】

            Vector3 shootDirection = new Vector3(shootX, shootY, 0).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            BulletController bulletScript = bullet.GetComponent<BulletController>();
            bulletScript.moveDirection = shootDirection;

            // 【修改260926】根据武器自身的攻击类型，决定传递魔攻还是物攻给子弹
            if (bulletScript.attackType == "Magic") // 【新增260926】
            {
                bulletScript.shooterAtk = matk; // 【新增260926】
            }
            else // 【新增260926】
            {
                bulletScript.shooterAtk = atk; // 【新增260926】
            }

            // 【新增260926】传递主角自身的属性（用来判断本系加成），不要修改子弹自带的属性
            bulletScript.shooterVirtue = virtue1;

            // 暴击判定
            if (Random.value <= critRate)
            {
                bulletScript.damageMultiplier = critDamage;
                Debug.Log("小狗触发暴击！");
            }
            else
            {
                bulletScript.damageMultiplier = 1.0f;
            }
        }

        // 实时刷新属性面板和血量面板 // 【新增260926】
        UpdateAttributeUI(); // 【新增260926】
    }

    // 【新增】专门用来刷新血条的方法
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // 把当前血量转换为 0 到 1 的百分比，交给血条填充
            healthBarFill.fillAmount = currentHp / hp;
        }

        // 【新增260926】刷新血条上的数值文本
        if (hpText != null)
        {
            hpText.text = currentHp.ToString("F0") + " / " + hp.ToString("F0");
        }
    }

    // 新增：刷新属性UI的方法 // 【新增260926】
    void UpdateAttributeUI() // 【新增260926】
    {
        if (statText != null)
        {
            // 【修改260926】倍率为1.0时不显示，大于1.0时用"×2"的简写格式
            statText.text =
                $"PAtk {atk} {(pAtkMul == 1.0f ? "" : $"×{pAtkMul:F1}")}\n" +
                $"PDef {pdef} {(pDefMul == 1.0f ? "" : $"×{pDefMul:F1}")}\n" +
                $"MAtk {matk} {(mAtkMul == 1.0f ? "" : $"×{mAtkMul:F1}")}\n" +
                $"MDef {mdef} {(mDefMul == 1.0f ? "" : $"×{mDefMul:F1}")}\n" +
                $"CrR {critRate * 100:F0}% {(crRMul == 1.0f ? "" : $"×{crRMul:F1}")}\n" +
                $"CrD {critDamage * 100:F0}% {(crDMul == 1.0f ? "" : $"×{crDMul:F1}")}\n" +
                $"WT {wt} {(wtMul == 1.0f ? "" : $"×{wtMul:F1}")}\n" +
                $"Vir1 {virtue1}\n" +
                $"Vir2 {virtue2}";
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