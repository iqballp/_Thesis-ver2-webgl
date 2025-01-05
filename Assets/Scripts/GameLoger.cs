using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoger : MonoBehaviour
{
    private static GameLoger instance;

    public bool isDoneTutorial;
    public bool isSetuju;

    public string pemain;
    public int indexSoal;
    public string schema;
    public float z1;
    public float z2;
    public float z3;
    public float z4;
    public float pureDiff;
    public float difficulty;
    public string status;

    public float prevDifficulty;
    public float prevSuccessDifficulty;
    public float prevPerformance;

    public float targetDifficulty;

    public string prevStatus;
    public float currentSum;
    public float prevSum;

    public string blokSoal;
    public string solusiSoal;

    public string elespasedTime; //untuk hitung waktu generate

    public bool isFound = false;

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

    public void ResetGameLoger()
    {
        // pemain = ""; nama pemain tetap tersimpan
        indexSoal = 0;
        schema = "";
        z1 = 0;
        z2 = 0;
        z3 = 0;
        z4 = 0;
        pureDiff = 0;
        difficulty = 0;
        status = "";
        prevDifficulty = 0;
        prevSuccessDifficulty = 0;
        prevPerformance = 0;
        targetDifficulty = 0;
        prevStatus = "";
        currentSum = 0;
        prevSum = 0;
        blokSoal = "";
        solusiSoal = "";
        elespasedTime = ""; // untuk hitung waktu generate
        isFound = false;
    }
}
