using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance; public TextMeshProUGUI scoreText; private int score;
    void Awake() { instance = this; }
    public void AddScore(int amt) { score += amt; if (scoreText != null) scoreText.text = "Score: " + score; }
}