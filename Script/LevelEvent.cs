using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEvent : MonoBehaviour
{
    public void LoadLevel(int levelId)
    {
        string levelName = "Level" + levelId;
        SceneManager.LoadScene(levelName);
    }
}