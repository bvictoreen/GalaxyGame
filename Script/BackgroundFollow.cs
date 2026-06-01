using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    private Transform kameraUtama;
    private Vector3 posisiTerakhirKamera;

    void Start()
    {
        // Otomatis mencari posisi Kamera Utama
        kameraUtama = Camera.main.transform;
        posisiTerakhirKamera = kameraUtama.position;
    }

    void LateUpdate()
    {
        // Menghitung seberapa jauh kamera sudah bergerak
        Vector3 pergerakanKamera = kameraUtama.position - posisiTerakhirKamera;

        // Menggeser background mengikuti kamera 
        // (Ubah angka 1f menjadi 0.8f jika ingin background bergerak sedikit lebih lambat/efek parallax)
        transform.position += new Vector3(pergerakanKamera.x * 1f, pergerakanKamera.y * 1f, 0);

        posisiTerakhirKamera = kameraUtama.position;
    }
}