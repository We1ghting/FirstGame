using UnityEngine;

public class VirtueCoin : MonoBehaviour
{
    public int virtueValue = 1; // 【新增260930】每个货币增加多少美德

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 【新增260930】让主角获得美德
            other.GetComponent<PlayerController>().GainVirtue(virtueValue);

            Destroy(gameObject); // 货币消失
        }
    }
}