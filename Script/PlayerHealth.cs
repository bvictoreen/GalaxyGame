using UnityEngine.SceneManagement;
using UnityEngine;

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk me-restart scene

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("UI Components")]
    public Slider healthSlider; // Slot untuk bar nyawa Gino

    [Header("Audio Settings")]
    public AudioClip dieSFX; // Slot untuk file die.mp3

    [Header("Visual Components")]
    private SpriteRenderer spriteRenderer;
    private bool isFlashing = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setel batas maksimal bar nyawa di awal game
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log("Gino Kena Sabet! Darah tersisa: " + currentHealth);

        // Update tampilan bar nyawa saat terluka
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Picu efek berkedip merah
        if (!isFlashing && spriteRenderer != null)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashEffect()
    {
        isFlashing = true;
        Color originalColor = spriteRenderer.color;
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        isFlashing = false;
    }

    void Die()
    {
        Debug.Log("GINO MATI! GAME OVER!");

        // Memutar suara kematian Gino tepat di posisi kamera
        if (dieSFX != null)
        {
            AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);
        }

        // Jalankan jeda waktu sebelum restart map agar suara tidak terputus
        StartCoroutine(RestartDelay());
    }

    IEnumerator RestartDelay()
    {
        // Sembunyikan karakter Gino dan matikan kontrolnya dulu biar kesannya sudah mati
        GetComponent<SpriteRenderer>().enabled = false;

        // Mematikan script pergerakan agar pemain tidak bisa gerak pas mati
        PlayerMovement moveScript = GetComponent<PlayerMovement>();
        if (moveScript != null) moveScript.enabled = false;

        yield return new WaitForSeconds(0.8f); // Menunggu suara die.mp3 berputar selama 0.8 detik

        // Mengulang kembali map dari awal
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}