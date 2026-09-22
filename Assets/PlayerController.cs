using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // 【补回来了！】移动速度，可以在Unity界面里调

    // 子弹相关变量
    public GameObject bulletPrefab; // 存放子弹预制体
    public float fireRate = 0.3f;   // 发射间隔
    private float nextFireTime = 0f;

    void Start()
    {
        Debug.Log("小狗准备好移动了！");
    }

    void Update()
    {
        // 1. 获取键盘输入（WASD 或 方向键）
        float moveX = Input.GetAxisRaw("Horizontal"); // 左右输入
        float moveY = Input.GetAxisRaw("Vertical");   // 上下输入

        // 2. 把输入组合成一个方向向量，normalized 是为了防止斜着走速度变快
        Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // 3. 让小狗移动，乘以 Time.deltaTime 是为了保证不同帧率下速度一致
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // 4. 根据水平输入方向翻转图像（左转/右转）
        if (moveX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // 往右走，不翻转（保持原图向右）
        }
        else if (moveX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; // 往左走，翻转（让脸朝左）
        }

        // 5. 方向键射击逻辑（WASD移动，方向键射击）
        float shootX = 0;
        float shootY = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) shootX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) shootX = 1;
        if (Input.GetKey(KeyCode.UpArrow)) shootY = 1;
        if (Input.GetKey(KeyCode.DownArrow)) shootY = -1;

        // 如果按了方向键，并且冷却时间到了，就发射
        if ((shootX != 0 || shootY != 0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            // 组合方向，如果同时按上、左，就是45度左上
            Vector3 shootDirection = new Vector3(shootX, shootY, 0).normalized;

            // 生成子弹，并把方向传给子弹
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletController>().moveDirection = shootDirection;
        }
    }
}