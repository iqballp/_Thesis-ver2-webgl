using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLoger : MonoBehaviour
{
    private static ScoreLoger instance;

    public float timer_maju;

    public float durasiBermain;
    public float hitungRataKesulitanBenar;
    public float hitungRataKesulitanSalah;
    public int jumlahKerjakanSoal;
    public float kesulitanTinggiBenar;
    public float kesulitanRendahBenar;
    public float kesulitanTinggiSalah;
    public float kesulitanRendahSalah;
    public float rataKesulitanBenar;
    public float rataKesulitanSalah;
    public int jumlahKerjakanSoalSalah;
    public int jumlahKerjakanSoalBenar;
    public float poin;
    public float[] points;

    // Start is called before the first frame update
    void Start()
    {


        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    // Update is called once per frame
}
