using UnityEngine;
using TMPro; // Wajib diisi untuk mengatur TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText; // Slot untuk memasukkan ScoreText UI
    private int score = 0;

    void Awake()
    {
        // Membuat sistem singleton agar gampang dipanggil dari script lain
        if (instance == null) instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI(); // Update tulisan skor di layar
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}