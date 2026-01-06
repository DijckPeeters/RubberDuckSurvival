using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            collision.GetComponent<ExperienceGem>().StartFollowing(transform);
        }
    }
}