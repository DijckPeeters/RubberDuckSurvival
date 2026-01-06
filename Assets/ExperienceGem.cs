using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    public int expAmount = 10;
    public float moveSpeed = 8f;
    private Transform target;
    private bool isFollowing = false;

    public void StartFollowing(Transform player)
    {
        target = player;
        isFollowing = true;
    }

    void Update()
    {
        if (isFollowing && target != null)
        {
            // Vlieg naar de player toe
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Als we heel dichtbij zijn: verdwijnen en XP geven
            if (Vector3.Distance(transform.position, target.position) < 0.2f)
            {
                // We roepen dadelijk de GainExperience functie aan op de player
                target.GetComponent<Player>().GainExperience(expAmount);
                Destroy(gameObject);
            }
        }
    }
}