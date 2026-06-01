using UnityEngine;

public class Diamond : MonoBehaviour
{
    public int scoreValue = 10; // 1 berlian = 20 skor sesuai rikues terbaru

    [Header("Audio Settings")]
    public AudioClip coinSFX; // Slot untuk file coin.mp3

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek jika yang menabrak adalah Gino (ber-Tag Player)
        if (collision.CompareTag("Player"))
        {
            // Memutar suara koin terambil secara instan di koordinat kamera
            if (coinSFX != null)
            {
                AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
            }

            // Tambahkan skor otomatis lewat ScoreManager
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(scoreValue);
            }

            Debug.Log("Berlian diambil! Skor bertambah 10");

            // Langsung hancurkan objek koin ini
            Destroy(gameObject);
        }
    }
}