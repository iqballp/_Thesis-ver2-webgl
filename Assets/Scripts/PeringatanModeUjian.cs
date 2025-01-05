using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeringatanModeUjian : MonoBehaviour
{
    public TextMeshProUGUI peringatanUjian;
    // Start is called before the first frame update
    void Start()
    {
        peringatanUjian.text = "Pada mode ini <color=#FF2929>tidak ada batas level akhir</color>. \n<color=#FF2929>Permainan selesai saat pemain menekan tombol Berhenti</color> pada menu <color=#FF2929>Pause</color>. Tingkat kesulitan level akan meningkat ketika berhasil menyelesaikan level serta menurun ketika gagal menyelesaikan level";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
