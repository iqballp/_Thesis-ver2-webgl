using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public ScreenshotCapture sc;
    private GameLoger gl;
    private ScoreLoger sl;
    private DebugManager sendLog;
    public string statusLog;

    public GameObject namaPemainText;
    public TMP_Text uploadStatusText;
    public GameObject scoreText;
    public GameObject durasiText;
    public GameObject jumlahSoalText;
    public GameObject soalBenarText;
    public GameObject soalSalahText;
    public GameObject rataBenarText;
    public GameObject rataSalahText;
    public GameObject kesRenBenarText;
    public GameObject kesTingBenarText;
    public GameObject kesRenSalahText;
    public GameObject kesTingSalahText;

    public GameObject unggahBtn;

    // Start is called before the first frame update
    void Start()
    {
        gl = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
        sendLog = GameObject.Find("Debuger").GetComponent<DebugManager>();
        sl.poin = 0;

        sl.durasiBermain = sl.timer_maju;
        if (sl.jumlahKerjakanSoalBenar > 0)
        {
            sl.rataKesulitanBenar = (Mathf.Round(sl.rataKesulitanBenar * 1000) * 0.001f);
        }
        else
        {
            sl.rataKesulitanBenar = 0;
        }

        if(sl.jumlahKerjakanSoalSalah > 0)
        {
            sl.rataKesulitanSalah = (Mathf.Round(sl.rataKesulitanSalah * 1000) * 0.001f);
        }
        else
        {
            sl.rataKesulitanSalah = 0;
        }
        if (sl.jumlahKerjakanSoalBenar > 0)
        {
            sl.poin = Mathf.Round(sl.rataKesulitanBenar * 100);
        }
        sl.kesulitanRendahBenar = (Mathf.Round(sl.kesulitanRendahBenar * 1000) * 0.001f);
        sl.kesulitanRendahSalah = (Mathf.Round(sl.kesulitanRendahSalah * 1000) * 0.001f);
        sl.kesulitanTinggiBenar = (Mathf.Round(sl.kesulitanTinggiBenar * 1000) * 0.001f);
        sl.kesulitanTinggiSalah = (Mathf.Round(sl.kesulitanTinggiSalah * 1000) * 0.001f);
        scoreText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.poin.ToString();
        durasiText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.durasiBermain.ToString("#") + " detik";
        jumlahSoalText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.jumlahKerjakanSoal.ToString();
        namaPemainText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = gl.pemain.ToString();
        soalBenarText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.jumlahKerjakanSoalBenar.ToString();
        soalSalahText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.jumlahKerjakanSoalSalah.ToString();
        rataBenarText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.rataKesulitanBenar.ToString();
        rataSalahText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.rataKesulitanSalah.ToString();
        kesRenBenarText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.kesulitanRendahBenar.ToString();
        kesTingBenarText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.kesulitanTinggiBenar.ToString();
        kesRenSalahText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.kesulitanRendahSalah.ToString();
        kesTingSalahText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = sl.kesulitanTinggiSalah.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainLagi()
    {
        gl.pemain = "";
        gl.indexSoal = 0;
        gl.schema = "";
        gl.z1 = 0;
        gl.z2 = 0;
        gl.z3 = 0;
        gl.z4 = 0;
        gl.pureDiff = 0;
        gl.difficulty = 0;
        gl.status = "";
        gl.prevDifficulty = 0;
        gl.prevSuccessDifficulty = 0;
        gl.prevPerformance = 0;
        gl.targetDifficulty = 0;
        gl.prevStatus = "";
        gl.currentSum = 0;
        gl.prevSum = 0;
        gl.blokSoal = "";
        gl.solusiSoal = "";
        gl.elespasedTime = ""; //untuk hitung waktu generate
        gl.isFound = false;

        sl.poin = 0;
        sl.durasiBermain = 0;
        sl.rataKesulitanBenar = 0;
        sl.rataKesulitanSalah = 0;
        sl.kesulitanRendahBenar = 0;
        sl.kesulitanRendahSalah = 0;
        sl.kesulitanTinggiBenar = 0;
        sl.kesulitanTinggiSalah = 0;
        sl.hitungRataKesulitanBenar = 0;
        sl.hitungRataKesulitanSalah = 0;
        sl.jumlahKerjakanSoal = 0;
        sl.jumlahKerjakanSoalBenar = 0;
        sl.jumlahKerjakanSoalSalah = 0;
        sl.points = new float[0];


        if (sendLog != null)
        {
            Destroy(sendLog);
        }

        StartCoroutine(nextScene("Menu", 1f));
    }

    public void DownloadLogFile()
    {
        string logData = sendLog.GetLogDownloadData();

        Application.ExternalCall("DownloadLogFile", logData, gl.pemain);
    }

    public void OnUploadLogResult(string uploadStatus)
    {
        statusLog = uploadStatus;
        // PlayerPrefs.SetString("uploadLogResult", uploadStatus);
        // PlayerPrefs.Save();

        DisplayUploadStatus(uploadStatus);

        if (statusLog == "success")
        {
            uploadStatusText.text = "Log berhasil dikirim";
            unggahBtn.gameObject.SetActive(false);
        }
        else if (statusLog == "failed")
        {
            uploadStatusText.text = "Gagal mengirim Log";
            
        }
        else
        {
            uploadStatusText.text = "Error";
        }
    }

    void DisplayUploadStatus(string status)
    {
        // Implementasi menampilkan status di UI, misalnya dengan Text
        Debug.Log("Upload status: " + status);
    }

    public void UploadToFirebase()
    {
        string logData = sendLog.GetLogData();

        Application.ExternalCall("UploadLogToFirebase", logData, gl.pemain);
        StartCoroutine(sc.CaptureScreenshot());
    }

    public void UploadImageToFirebase()
    {
        StartCoroutine(sc.CaptureScreenshot());
    }

    IEnumerator nextScene(string sceneTarget, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneTarget);
    }
}
