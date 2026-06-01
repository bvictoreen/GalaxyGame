using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextLevelName;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) SceneManager.LoadScene(nextLevelName);
    }
}