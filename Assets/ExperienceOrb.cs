using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public int expValue = 1; // 新增260927 这个经验球给多少经验

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 新增260927 让主角获得经验
            other.GetComponent<PlayerController>().GainExp(expValue);

            Destroy(gameObject); // 经验球消失
        }
    }
}