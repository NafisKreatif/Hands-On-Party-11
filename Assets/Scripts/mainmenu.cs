using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class mainmenu : MonoBehaviour
{
    // Bagian Main menu
    public void play() // Fungsi mengatur tombol play
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Lanjut ke scene selanjutnya setelah mencet play
    }
    public void quit() //Fungsi untuk mengatur quit game
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }
    //Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume musik
    [SerializeField] private Slider musicSlider; // Slider untuk mengatur volume musik

    // Fungsi untuk mengatur volume musik berdasarkan slider
    public void SetMusicVloume()
    {
        float volume = musicSlider.value; // Mengambil nilai dari slider
        myMixer.SetFloat("music", Mathf.Log10(volume)*20); // Mengubah nilai slider menjadi decibel dan mengatur volume musik
    }

    //Bagian Gyroscope
    private Gyroscope gyro; // Objek untuk mengakses gyroscope
    private bool isGyroEnabled = false; // Menandai apakah gyroscope sedang aktif

    // Fungsi untuk mengaktifkan atau menonaktifkan gyroscope menggunakan toggle
    public void ToggleGyroscope(bool isOn)
    {
        if (gyro == null) // Mengecek apakah perangkat mendukung gyroscope
        {
            Debug.LogWarning("Gyroscope is not supported, cannot toggle.");

            // Gyroscope tidak didukung, tapi tidak ada elemen status untuk diperbarui
            return;
        }

        if (isOn)
        {
            EnableGyroscope(); // Mengaktifkan gyroscope
        }
        else
        {
            DisableGyroscope(); // Menonaktifkan gyroscope
        }
    }

    // Fungsi untuk mengaktifkan gyroscope
    private void EnableGyroscope()
    {
        if (!isGyroEnabled) // Mengecek apakah gyroscope belum aktif
        {
            gyro.enabled = true; // Mengaktifkan gyroscope
            isGyroEnabled = true; // Menandai bahwa gyroscope aktif
                                  // Tidak ada teks untuk diperbarui
        }
    }

    // Fungsi untuk menonaktifkan gyroscope
    private void DisableGyroscope()
    {
        if (isGyroEnabled) // Mengecek apakah gyroscope sedang aktif
        {
            gyro.enabled = false; // Menonaktifkan gyroscope
            isGyroEnabled = false; // Menandai bahwa gyroscope tidak aktif
                                   // Tidak ada teks untuk diperbarui
        }
    }
}
