using UnityEngine;
using System.Collections.Generic; // Nodig voor de lijst

public class SwordWeapon : MonoBehaviour
{
    [Header("Objects")]
    public GameObject rightAttackObject;
    public GameObject leftAttackObject;

    [Header("Settings")]
    public float damage = 20f;
    public float attackSpeed = 1.5f;
    public float hitboxDuration = 0.5f;
    public Vector2 hitboxSize = new Vector2(3f, 1f);

    private float timer;
    private bool strikeRight = true;
    private bool isHitboxActive = false;
    private float activeTimer;
    private List<Enemy> enemiesHitThisAttack = new List<Enemy>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            Attack();
            timer = 0;
        }

        // Blijf checken zolang de hitbox aan staat
        if (isHitboxActive)
        {
            CheckHitContinuous();
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                isHitboxActive = false;
                DisableAttacks();
            }
        }
    }

    void Attack()
    {
        enemiesHitThisAttack.Clear(); // Reset de lijst voor de nieuwe klap
        GameObject currentAttack = strikeRight ? rightAttackObject : leftAttackObject;

        currentAttack.SetActive(true);
        Animator anim = currentAttack.GetComponent<Animator>();
        if (anim != null) anim.Play("Slash_Anim", -1, 0f);

        isHitboxActive = true;
        activeTimer = hitboxDuration;
        strikeRight = !strikeRight;
    }

    void CheckHitContinuous()
    {
        // Bepaal welke kant we nu checken
        Vector3 pos = !strikeRight ? rightAttackObject.transform.position : leftAttackObject.transform.position;

        Collider2D[] hits = Physics2D.OverlapBoxAll(pos, hitboxSize, 0);
        foreach (Collider2D hit in hits)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();
            if (enemyScript == null) enemyScript = hit.GetComponentInParent<Enemy>();

            // Alleen hitten als het een Enemy is EN we hem deze klap nog niet geraakt hebben
            if (enemyScript != null && !enemiesHitThisAttack.Contains(enemyScript))
            {
                enemyScript.TakeDamage(damage);
                enemiesHitThisAttack.Add(enemyScript); // Voeg toe aan lijst zodat hij niet dubbel geraakt wordt
            }
        }
    }

    void DisableAttacks()
    {
        rightAttackObject.SetActive(false);
        leftAttackObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isHitboxActive ? Color.green : Color.red;
        if (rightAttackObject) Gizmos.DrawWireCube(rightAttackObject.transform.position, hitboxSize);
        if (leftAttackObject) Gizmos.DrawWireCube(leftAttackObject.transform.position, hitboxSize);
    }
}