using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Pengaturan Serangan")]
    public int damageAmount = 30;
    public float moveSpeed = 2.5f;
    public float detectionRadius = 6f;
    public float attackRadius = 1.8f; // Default dinaikkan ke 1.8 agar lebih mudah mengenai Player
    public float attackCooldown = 1.5f;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sprite;
    private float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // RESTRUKTURISASI: Mencari objek yang memiliki Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            // Radar otomatis jika Tag Player belum dipasang di Inspector
            Debug.LogWarning("⚠️ RADAR: Enemy tidak bisa menemukan Gino! Pastikan objek Gino di Hierarchy sudah kamu ubah TAG-nya menjadi 'Player' di pojok kanan atas Inspector.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Hitung jarak antara koordinat Dokter dan Gino
        float distance = Vector2.Distance(transform.position, player.position);

        // KONDISI 1: JIKA GINO MASUK RADIUS SERANG
        if (distance <= attackRadius)
        {
            anim.SetBool("isWalking", false);

            if (Time.time >= nextAttackTime)
            {
                Debug.Log("⚔️ RADAR: Jarak pas! Enemy memicu animasi 'attack' dan menyerang Gino.");

                // Panggil Trigger "attack" di Animator
                if (anim != null) anim.SetTrigger("attack");

                // Jalankan fungsi pengurang darah di script PlayerHealth
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
                else
                {
                    Debug.LogError("❌ ERROR: Gino sudah ketemu, tapi kamu BELUM memasang script 'PlayerHealth' di dalam objek Player!");
                }

                nextAttackTime = Time.time + attackCooldown;
            }
        }
        // KONDISI 2: GINO JAUH TAPI MASUK RADIUS DETEKSI (MENGEJAR)
        else if (distance <= detectionRadius)
        {
            anim.SetBool("isWalking", true);
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (player.position.x > transform.position.x)
            {
                sprite.flipX = false; // <--- Diubah jadi false
            }
            else
            {
                sprite.flipX = true;  // <--- Diubah jadi true
            }
        }
        // KONDISI 3: GINO DI LUAR JANGKAUAN (DIAM)
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    // Menampilkan visual lingkaran radius kuning (deteksi) & merah (serang) di layar Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}