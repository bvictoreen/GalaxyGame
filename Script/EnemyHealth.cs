using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk berpindah level map

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;
    public int scoreValue = 20;

    [Header("UI Components")]
    public Slider healthSlider; // Slot untuk bar nyawa Enemy

    [Header("Audio Settings")]
    public AudioClip dieSFX; // Slot untuk file die.mp3 musuh

    [Header("Level Progression")]
    public string nextLevelName = "Level2"; // Nama map tujuan selanjutnya
    public int nextLevelNumber = 2;         // Angka level tujuan selanjutnya

    [Header("Visual Components")]
    private SpriteRenderer spriteRenderer;
    private bool isFlashing = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setel batas maksimal bar nyawa musuh
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

        // Update bar nyawa musuh
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Efek kedip merah musuh
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
        Debug.Log(gameObject.name + " Berhasil Dikalahkan!");

        // Memutar suara kematian musuh
        if (dieSFX != null)
        {
            AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);
        }

        // Tambah skor koin
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // Sembunyikan tubuh musuh dan matikan fisiknya
        GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;

        EnemyAttack attackScript = GetComponent<EnemyAttack>();
        if (attackScript != null) attackScript.enabled = false;

        // Sistem absensi pengecekan musuh terakhir
        EnemyHealth[] remainingEnemies = FindObjectsOfType<EnemyHealth>();
        bool amILastEnemy = true;

        foreach (EnemyHealth enemy in remainingEnemies)
        {
            if (enemy != this && enemy.currentHealth > 0)
            {
                amILastEnemy = false;
                break;
            }
        }

        if (amILastEnemy)
        {
            Debug.Log("Semua musuh tamat! Menghitung mundur 2 detik untuk pindah level...");
            StartCoroutine(ChangeLevelDelay());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeLevelDelay()
    {
        // Menahan game selama 2.0 detik
        yield return new WaitForSeconds(2.0f);

        // Menyimpan progress level yang terbuka ke memori komputer
        int currentProgress = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (nextLevelNumber > currentProgress)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevelNumber);
            PlayerPrefs.Save();
        }

        // Pindah otomatis ke Scene berikutnya
        SceneManager.LoadScene(nextLevelName);

        Destroy(gameObject);
    }
}