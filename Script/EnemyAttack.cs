using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    [Header("Target Settings")]
    public LayerMask playerLayers;

    [Header("Audio Settings")]
    public AudioClip attackSFX; // Slot untuk file attack.mp3 musuh

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Otomatis menyerang berkala jika cooldown selesai
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        // Memicu visual gerakan menyerang musuh di Animator
        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        // Memutar suara tebasan sabit musuh
        if (attackSFX != null)
        {
            AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
        }

        // Sensor area gaib sabetan musuh
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}