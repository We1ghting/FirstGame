using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // 移动速度，可以在Unity界面里直接调

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
    }
}
