using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float delay = 0.5f; // Hoe lang de explosie duurt

    void Start()
    {
        // Vernietig dit object na de ingestelde tijd
        Destroy(gameObject, delay);
    }
}