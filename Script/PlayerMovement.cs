using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 7f;
    public float jumpForce = 11f;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public AudioClip attackSFX;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer; // Slot khusus untuk mendeteksi layer tanah

    [Header("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator anim;

    void Start()
    {
        // Mengambil semua komponen tubuh Gino saat game dimulai
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Logika Pergerakan Horizontal (A/D atau Panah)
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Mengirimkan nilai kecepatan ke parameter "speed" di Animator
        anim.SetFloat("speed", Mathf.Abs(moveInput));

        // Logika membalikkan arah hadap sprite Gino (Flip)
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // 2. Logika Lompat (Spacebar)
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // 3. Logika Menyerang (Tombol F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetTrigger("attack");
            Attack();
        }

        // --- BARIS YANG TADI EROR SEKARANG SUDAH AMAN DI SINI ---
        anim.SetBool("isGrounded", IsGrounded());
    }

    void Attack()
    {
        // Memutar suara tebasan tepat di posisi kamera (biar terdengar jelas di 2D)
        if (attackSFX != null)
        {
            AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
        }

        // Membuat lingkaran sensor gaib tebasan sabit Gino
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Berikan damage ke setiap musuh yang terkena area sensor
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
    }

    // Fungsi otomatis untuk membalik orientasi gambar karakter kiri/kanan
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Fungsi sensor untuk mengecek apakah Gino sedang menapak di tanah
    private bool IsGrounded()
    {
        // Menembakkan kotak sensor gaib dari badan Gino ke arah bawah sepanjang 0.1f
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // Menggambarkan lingkaran merah jarak serang di jendela Scene Unity
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}