using UnityEngine;
using System.Collections.Generic;

public class SwordWeapon : MonoBehaviour
{
    [Header("Objects")]
    public GameObject attackVisual; // Sleep hier je 'SwordSlash' object in.

    [Header("Settings")]
    public float damage = 20f;
    public float hitboxDuration = 0.5f;
    public Vector2 hitboxSize = new Vector2(1.5f, 1f);
    public float hitboxOffsetX = 0.8f;

    private bool isHitboxActive = false;
    private float activeTimer;
    private List<Enemy> enemiesHitThisAttack = new List<Enemy>();

    void Update()
    {
        // Arcade input check
        if (Input.GetButtonDown("Fire1") && !isHitboxActive)
        {
            Attack();
        }

        if (isHitboxActive)
        {
            CheckHitContinuous();
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                isHitboxActive = false;
                attackVisual.SetActive(false);
            }
        }
    }

    void Attack()
    {
        enemiesHitThisAttack.Clear();
        if (attackVisual != null)
        {
            attackVisual.SetActive(true);
            Animator anim = attackVisual.GetComponent<Animator>();
            if (anim != null) anim.Play("Slash_Anim", -1, 0f);
        }

        isHitboxActive = true;
        activeTimer = hitboxDuration;
    }

    void CheckHitContinuous()
    {
        // We gebruiken de positie van de visual. Omdat de Player flipt, 
        // flipt de 'right' van dit object automatisch mee!
        Vector3 hitboxPos = attackVisual.transform.position + attackVisual.transform.right * hitboxOffsetX;

        Collider2D[] hits = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0);
        foreach (Collider2D hit in hits)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();
            if (enemyScript == null) enemyScript = hit.GetComponentInParent<Enemy>();

            if (enemyScript != null && !enemiesHitThisAttack.Contains(enemyScript))
            {
                enemyScript.TakeDamage(damage);
                enemiesHitThisAttack.Add(enemyScript);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackVisual == null) return;
        Gizmos.color = isHitboxActive ? Color.green : Color.red;
        Vector3 hitboxPos = attackVisual.transform.position + attackVisual.transform.right * hitboxOffsetX;
        Gizmos.DrawWireCube(hitboxPos, hitboxSize);
    }
}