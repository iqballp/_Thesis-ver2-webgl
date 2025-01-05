using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class peringatanBold : MonoBehaviour
{
    public TextMeshProUGUI peringatanText;
    // Start is called before the first frame update
    void Start()
    {
        peringatanText.text = "Mohon untuk menekan <color=#FF2929><b>tombol Berhenti sebelum menutup tab atau keluar dari game</b></color> pada mode ujian agar data dapat terkirim ke server dengan benar.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
