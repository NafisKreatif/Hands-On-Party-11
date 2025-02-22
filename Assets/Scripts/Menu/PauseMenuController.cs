using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    // Bagian Pause Menu
    // Variabel untuk melacak apakah permainan sedang pause
    public static bool paused = false;
    // Objek Canvas untuk menampilkan menu pause
    public GameObject PauseMenu;
    private BallDieController _player;
    // Fungsi yang dipanggil sekali saat game dimulai
    void Start()
    {
        Time.timeScale = 1f; // Mengatur waktu menjadi normal saat game dimulai
        FindFirstObjectByType<WinController>().WinningEvent.AddListener(OnWin);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<BallDieController>();
    }

    // Fungsi yang dipanggil setiap frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_player.isDying) // Mengecek apakah tombol Escape ditekan
        {
            if (paused)
            {
                Play(); // Melanjutkan permainan jika sedang pause
            }
            else
            {
                Stop(); // Mem-pause permainan jika tidak sedang pause
            }
        }
    }

    // Fungsi untuk mem-pause permainan
    public void Stop()
    {
        PauseMenu.SetActive(true); // Menampilkan menu pause
        Time.timeScale = 0f; // Menghentikan waktu dalam game
        paused = true; // Menandai bahwa permainan sedang pause
    }

    public void Play()
    {
        PauseMenu.SetActive(false); // Menyembunyikan menu pause
        Time.timeScale = 1f; // Melanjutkan waktu dalam game
        paused = false; // Menandai bahwa permainan tidak lagi pause
        // Debug.Log("Game resumed"); // Log untuk debugging
    }

    // Fungsi untuk berpindah ke menu utama
    public void MainMenuButton()
    {
        Time.timeScale = 1f; // Supaya transisinya bisa jalan
        SceneTransitionManager.Instance.GoToScene(0);
    }
    // Kalau sudah menang tidak bisa pause
    void OnWin()
    {
        enabled = false;
    }
}
