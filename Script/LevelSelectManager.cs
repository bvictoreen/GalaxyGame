using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Untuk teks TextMeshPro pop-up

public class LevelSelectManager : MonoBehaviour
{
    [Header("UI Level Buttons")]
    public Button[] levelButtons; // Slot tombol angka 1-3

    [Header("Popup System UI")]
    public GameObject popupPanel; // Slot Panel Pop-up
    public TextMeshProUGUI popupText; // Slot Teks Pop-up (TMP)

    [Header("Rules System")]
    public GameObject rulesPanel; // Slot Panel Aturan Main

    private int unlockedLevel;

    void Start()
    {
        // Mengambil data progress level
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Sembunyikan semua panel saat awal game mulai
        if (popupPanel != null) popupPanel.SetActive(false);
        if (rulesPanel != null) rulesPanel.SetActive(false);

        // Mengatur visual tombol yang masih terkunci
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            if (levelNum > unlockedLevel)
            {
                ColorBlock cb = levelButtons[i].colors;
                cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                levelButtons[i].colors = cb;
            }
        }
    }

    // ?? Fungsi 1: Dipanggil oleh Tombol Level 1, 2, 3 di Main Menu
    public void ClickLevel(int levelNum)
    {
        if (levelNum <= unlockedLevel)
        {
            SceneManager.LoadScene("Level" + levelNum);
        }
        else
        {
            if (popupPanel != null && popupText != null)
            {
                popupPanel.SetActive(true);
                popupText.text = "Mainkan / selesaikan Level " + (levelNum - 1) + " terlebih dahulu!";
            }
        }
    }

    // ============================================================
    // ?? FUNGSI 2: INI DIA YANG KITA MASUKKAN LAGI BIAR KEMBALI MUNCUL!
    //              (Dipanggil oleh tombol Kembali di WinScene)
    // ============================================================
    public void GoToLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // ?? Fungsi 3: Untuk menutup Pop-up
    public void ClosePopup()
    {
        if (popupPanel != null) popupPanel.SetActive(false);
    }

    // ?? Fungsi 4: Untuk membuka Aturan Main
    public void OpenRules()
    {
        if (rulesPanel != null) rulesPanel.SetActive(true);
    }

    // ?? Fungsi 5: Untuk menutup Aturan Main
    public void CloseRules()
    {
        if (rulesPanel != null) rulesPanel.SetActive(false);
    }

    // ?? Fungsi 6: Untuk meriset progress game
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}