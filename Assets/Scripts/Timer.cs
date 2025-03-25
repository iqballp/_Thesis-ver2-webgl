using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public ScoreLoger sl;
    public Text timerText; // Referensi ke UI Text untuk menampilkan waktu
    // private float elapsedTime = 0f; // Waktu yang sudah berlalu
    private bool isRunning = false; // Status timer
    // Start is called before the first frame update
    void Start()
    {
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
    }
    public void StartTimer()
    {
        isRunning = true;
    }

    // Fungsi untuk menghentikan timer
    public void StopTimer()
    {
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            // Tambahkan waktu berdasarkan deltaTime
            sl.timer_maju += Time.deltaTime;

            // Konversi waktu ke detik (bulat)
            int seconds = Mathf.FloorToInt(sl.timer_maju);

            // Perbarui teks timer
            timerText.text = seconds.ToString();
        }
    }
}
