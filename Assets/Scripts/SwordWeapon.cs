using UnityEngine;
using System.Collections.Generic; // Gebruikt voor de List om data van geraakte vijanden bij te houden.

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
    private List<Enemy> enemiesHitThisAttack = new List<Enemy>(); // Voorkomt dat één zwaai een vijand meerdere keren raakt.

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            Attack();
            timer = 0;
        }

        // Houdt de hitbox een specifiek aantal seconden open om de animatie visueel te volgen.
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
        enemiesHitThisAttack.Clear(); // Maakt de lijst leeg voor elke nieuwe aanval.
        GameObject currentAttack = strikeRight ? rightAttackObject : leftAttackObject;

        currentAttack.SetActive(true);
        Animator anim = currentAttack.GetComponent<Animator>();
        // Reset de animatie naar frame 0 om vertraging in de visualisatie te voorkomen.
        if (anim != null) anim.Play("Slash_Anim", -1, 0f);

        isHitboxActive = true;
        activeTimer = hitboxDuration;
        strikeRight = !strikeRight; // Wisselt de aanvalsrichting af (links/rechts).
    }

    void CheckHitContinuous()
    {
        // Bepaalt de positie van de onzichtbare 'hit-box' op basis van de huidige aanvalsrichting.
        Vector3 pos = !strikeRight ? rightAttackObject.transform.position : leftAttackObject.transform.position;

        // Physics2D.OverlapBoxAll scant een gebied en geeft alle colliders binnen die box terug.
        Collider2D[] hits = Physics2D.OverlapBoxAll(pos, hitboxSize, 0);
        foreach (Collider2D hit in hits)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();
            if (enemyScript == null) enemyScript = hit.GetComponentInParent<Enemy>();

            // Controleert of het object een vijand is en of deze niet al geraakt is tijdens deze specifieke zwaai.
            if (enemyScript != null && !enemiesHitThisAttack.Contains(enemyScript))
            {
                enemyScript.TakeDamage(damage);
                enemiesHitThisAttack.Add(enemyScript); // Registreert de vijand in de lijst van geraakte doelen.
            }
        }
    }

    void DisableAttacks()
    {
        rightAttackObject.SetActive(false);
        leftAttackObject.SetActive(false);
    }

    // Tekent visuele hulplijnen in de Unity Editor (Scene view) om de grootte van de hitbox te debuggen.
    private void OnDrawGizmos()
    {
        Gizmos.color = isHitboxActive ? Color.green : Color.red;
        if (rightAttackObject) Gizmos.DrawWireCube(rightAttackObject.transform.position, hitboxSize);
        if (leftAttackObject) Gizmos.DrawWireCube(leftAttackObject.transform.position, hitboxSize);
    }
}