using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject peringatanText;
    public List<GameObject> SelectedBilangan = new List<GameObject>();
    public List<List<GameObject>> historyBilangan = new List<List<GameObject>>();
    public Generator gen;
    public GameObject smoke;
    public List<GameObject> listTarget;
    public int totalbenar = 0;
    public Camera cam;
    // make it selected effect

    public GameObject confetti;
    //public GameObject lifePannel;
    public Text lifeCount;
    public Animator tirai;
    public Animator skipPromt;
    public bool isComplete = false;
    private bool conffectiplayed;
    private bool isTambahPoin;

    public string nextSceneTarget;

    private string waktuKerja;
    private float startTime;

    [SerializeField] private int restartTimes;
    private int undoTimes;

    private GameLoger gl;
    private ScoreLoger sl;
    public DebugManager sendLog;
    public SoalHandler sh;
    public Timer timerv;

    //forDebugger
    public bool isDebug = false;
    public GameObject prevDiffText;
    public GameObject prevPerformanceText;
    public GameObject currDiffText;
    public GameObject targetDiffText;

    public GameObject solutionText;
    public GameObject isFoundText;

    public GameObject currentSumText;

    public GameObject sedangPause;

    public Text levelText;
    public GameObject restartBtn;

    // public float timer;


    private void Start()
    {
        
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
        isTambahPoin = true;
        // levelText.text = "Level " + gl.indexSoal.ToString();

        /* membuat timer tiap level dengan delay -3 detik untuk pergantian animasi level
        timer = -3; */

        // startTime = Time.time;
        // sl.timer_maju = sl.timer_maju -1;
        startTime = sl.timer_maju;
        timerv.StartTimer();
        gl = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        sendLog = GameObject.Find("Debuger").GetComponent<DebugManager>();
        prevDiffText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.prevDifficulty.ToString();
        prevPerformanceText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.prevPerformance.ToString();
        targetDiffText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.targetDifficulty.ToString();
        isFoundText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.isFound.ToString();
        currentSumText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.currentSum.ToString();

        if (gl.prevStatus == "SUCCESS")
        {
            prevPerformanceText.transform.GetChild(0).gameObject.GetComponent<Text>().text += " (+)";
        }
        else
        {
            prevPerformanceText.transform.GetChild(0).gameObject.GetComponent<Text>().text += " (-)";
        }

        currDiffText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.difficulty.ToString();
        solutionText.GetComponent<Text>().text = sh.solusi.ToString();

        if (isDebug)
        {
            prevDiffText.gameObject.SetActive(true);
            prevPerformanceText.gameObject.SetActive(true);
            currDiffText.gameObject.SetActive(true);
            solutionText.gameObject.SetActive(true);
            targetDiffText.gameObject.SetActive(true);
            isFoundText.gameObject.SetActive(true);
            currentSumText.gameObject.SetActive(true);
        }
        else
        {
            prevDiffText.gameObject.SetActive(false);
            prevPerformanceText.gameObject.SetActive(false);
            currDiffText.gameObject.SetActive(false);
            solutionText.gameObject.SetActive(false);
            targetDiffText.gameObject.SetActive(false);
            isFoundText.gameObject.SetActive(false);
            currentSumText.gameObject.SetActive(false);
        }

        levelText.text = "Level " + gl.indexSoal.ToString();
    }


    private void setSelectedTrue(GameObject sumber)
    {
        if (sumber.GetComponent<DataBilangan>().bilangan[0] == '+')
        {
            sumber.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else if (sumber.GetComponent<DataBilangan>().bilangan[0] == '-')
        {
            sumber.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
    }

    private void setSelectedFalse(GameObject sumber)
    {
        if (sumber.GetComponent<DataBilangan>().bilangan[0] == '+')
        {
            sumber.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else if (sumber.GetComponent<DataBilangan>().bilangan[0] == '-')
        {
            sumber.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
    }

    //disni nih--------------
    private void checkTarget(GameObject obj)
    {
        foreach (GameObject target in listTarget)
        {
            //Vector3 point = cam.ScreenToWorldPoint(target.GetComponent<RectTransform>().localPosition);
            // Debug.Log("Point: " + point);
            if (obj.GetComponent<DataBilangan>().op == target.GetComponent<DataBilangan>().op && obj.GetComponent<DataBilangan>().bilangan == target.GetComponent<DataBilangan>().bilangan)
            {
                // change transparancy 
                Image objimage = target.GetComponent<Image>();
                Color newAlpha = objimage.color;
                if (objimage.color.a == 1f)
                {
                    newAlpha.a = 0.3451f;
                    objimage.color = newAlpha;

                    GameObject partikel = target.transform.GetChild(1).gameObject;
                    partikel.SetActive(false);
                    totalbenar = totalbenar - 1;

                }
                else
                {
                    GameObject objCHild = obj.transform.GetChild(4).gameObject;
                    objCHild.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = obj.GetComponent<DataBilangan>().bilangan;
                    objCHild.SetActive(true);

                    newAlpha.a = 1f;
                    objimage.color = newAlpha;

                    GameObject partikel = target.transform.GetChild(1).gameObject;
                    partikel.SetActive(true);
                    partikel.GetComponent<ParticleSystem>().Play();


                    totalbenar = totalbenar + 1;
                }

            }
        }
    }

    private void CalculateBilangan(GameObject bil1, GameObject bil2)
    {
        Bilangan obj1 = gen.newBilangan(bil1.GetComponent<DataBilangan>().bilangan, bil1.GetComponent<DataBilangan>().op);
        Bilangan obj2 = gen.newBilangan(bil2.GetComponent<DataBilangan>().bilangan, bil2.GetComponent<DataBilangan>().op);
        List<Bilangan> bilanganList = new List<Bilangan>();
        bilanganList.Add(obj1);
        bilanganList.Add(obj2);
        Bilangan hasil = gen.Hitung(bilanganList);

        // instantiate
        GameObject newObj = gen.generateObject(hasil, bil2.transform.position);
        checkTarget(newObj); // cek apakah ada di target

        // play smoke effect
        Vector3 smokepos = newObj.transform.position;
        smoke.transform.position = smokepos;
        smoke.SetActive(true);
        smoke.GetComponent<ParticleSystem>().Play();


        // tambah ke list history
        List<GameObject> curBilangan = new List<GameObject>() { bil1, bil2, newObj };
        historyBilangan.Add(curBilangan);
    }


    public void addSelected(GameObject sumber)
    {

        if (!(SelectedBilangan.Contains(sumber))) //cek dulu sumber ini sudah ada apa belum dalem list
        {
            if (SelectedBilangan.Count >= 1)  // terus if kalo sudah ada total 2 dalam list, kalkulasi terus kosongin
            {
                setSelectedFalse(SelectedBilangan[0]);
                SelectedBilangan.Add(sumber);
                CalculateBilangan(SelectedBilangan[0], SelectedBilangan[1]);  // kalkulasi nilainya disini

                SelectedBilangan[0].SetActive(false);
                SelectedBilangan[1].SetActive(false);
                SelectedBilangan.Clear();
            }
            else
            {
                SelectedBilangan.Add(sumber);
                setSelectedTrue(sumber);

            }

        }
    }

    public void Undo()
    {
        undoTimes++;
        List<GameObject> lastHistory = new List<GameObject>();
        lastHistory = historyBilangan[historyBilangan.Count - 1];
        lastHistory[0].SetActive(true);
        checkTarget(lastHistory[0]);

        lastHistory[1].SetActive(true);
        checkTarget(lastHistory[1]);

        Destroy(lastHistory[2]);
        checkTarget(lastHistory[2]);
        historyBilangan.RemoveAt(historyBilangan.Count - 1);
    }

    public void Restart()
    {
        restartTimes++;
        foreach (List<GameObject> lastHistory in historyBilangan)
        {
            //List<GameObject> lastHistory = new List<GameObject>();
            //lastHistory = historyBilangan[historyBilangan.Count - 1];
            lastHistory[0].SetActive(true);
            lastHistory[1].SetActive(true);
            Destroy(lastHistory[2]);
        }
        foreach (GameObject target in listTarget)
        {
            // change transparancy 
            Image objimage = target.GetComponent<Image>();
            Color newAlpha = objimage.color;
            newAlpha.a = 0.3451f;
            objimage.color = newAlpha;
        }
        totalbenar = 0;
        historyBilangan.Clear();

        if (restartTimes > sh.lifeCount)
        {
            timerv.StopTimer();
            restartBtn.gameObject.SetActive(false);
            sl.hitungRataKesulitanSalah = sl.hitungRataKesulitanSalah + gl.difficulty;
            // sl.durasiBermain = sl.durasiBermain + (Time.time - startTime);

            restartTimes--;
            sl.jumlahKerjakanSoalSalah = sl.jumlahKerjakanSoalSalah + 1;
            sl.jumlahKerjakanSoal = sl.jumlahKerjakanSoal + 1;
            // sl.poin = sl.poin + ((1000 * gl.prevPerformance * (1 * gl.difficulty)/ sl.durasiBermain) + (sh.lifeCount - restartTimes));



            skipSoal();
            if (sl.kesulitanTinggiSalah == 0)
            {
                sl.kesulitanTinggiSalah = gl.difficulty;
            }
            else
            {
                if (sl.kesulitanTinggiSalah < gl.difficulty)
                {
                    sl.kesulitanTinggiSalah = gl.difficulty;
                }
            }

            if (sl.kesulitanRendahSalah == 0)
            {
                sl.kesulitanRendahSalah = gl.difficulty;
            }
            else
            {
                if (sl.kesulitanRendahSalah > gl.difficulty)
                {
                    sl.kesulitanRendahSalah = gl.difficulty;
                }
            }
        }
        else
        {
            int currentLife = sh.lifeCount - restartTimes;
            lifeCount.text = currentLife.ToString();
            // Destroy(lifePannel.transform.GetChild(0).gameObject);
        }

    }



    public void skipSoal()
    {
        timerv.StopTimer();
        // untuk versi download
        sendLog.AddLogDownloadEntry("MODE: UJIAN");
        sendLog.AddLogDownloadEntry("INDEX: " + gl.indexSoal);
        sendLog.AddLogDownloadEntry("SCHEMA: " + gl.schema);
        sendLog.AddLogDownloadEntry("Z1: " + gl.z1);
        sendLog.AddLogDownloadEntry("Z2: " + gl.z2);
        sendLog.AddLogDownloadEntry("Z3: " + gl.z3);
        sendLog.AddLogDownloadEntry("Z4: " + gl.z4);
        sendLog.AddLogDownloadEntry("SOAL: " + gl.blokSoal);
        sendLog.AddLogDownloadEntry("SOLUSI: " + gl.solusiSoal);
        sendLog.AddLogDownloadEntry("PREV DIFF: " + gl.prevDifficulty);
        sendLog.AddLogDownloadEntry("PREV PERFORMANCE: " + gl.prevPerformance);
        sendLog.AddLogDownloadEntry("TARGET DIFF: " + gl.targetDifficulty);
        sendLog.AddLogDownloadEntry("CURRENT DIFF: " + gl.difficulty);
        sendLog.AddLogDownloadEntry("isFound: " + gl.isFound);
        sendLog.AddLogDownloadEntry("STATUS: SKIPPED");
        sendLog.AddLogDownloadEntry("TOTAL UNDO: " + restartTimes);
        sendLog.AddLogDownloadEntry("TOTAL RESTART: " + undoTimes);
        sendLog.AddLogDownloadEntry("WAKTU: " + waktuKerja);
        sendLog.AddLogDownloadEntry("WAKTU GENERATE: " + gl.elespasedTime + "\n");

        sendLog.Log("MODE: UJIAN");
        sendLog.Log("INDEX: " + gl.indexSoal);
        sendLog.Log("SCHEMA: " + gl.schema);
        sendLog.Log("Z1: " + gl.z1);
        sendLog.Log("Z2: " + gl.z2);
        sendLog.Log("Z3: " + gl.z3);
        sendLog.Log("Z4: " + gl.z4);
        sendLog.Log("SOAL: " + gl.blokSoal);
        sendLog.Log("SOLUSI: " + gl.solusiSoal);
        sendLog.Log("PREV DIFF: " + gl.prevDifficulty);
        sendLog.Log("PREV PERFORMANCE: " + gl.prevPerformance);
        sendLog.Log("TARGET DIFF: " + gl.targetDifficulty);
        sendLog.Log("CURRENT DIFF: " + gl.difficulty);
        sendLog.Log("isFound: " + gl.isFound);
        sendLog.Log("STATUS: SKIPPED");
        sendLog.Log("TOTAL UNDO: "+restartTimes);
        sendLog.Log("TOTAL RESTART: "+undoTimes);
        sendLog.Log("WAKTU: " + waktuKerja);
        sendLog.Log("WAKTU GENERATE: " + gl.elespasedTime + "\n");

        // untuk versi upload
        sendLog.AddLogEntry(";MODE; " + "INDEX; " + "SCHEMA; " + "Z1; " + "Z2; " + "Z3; " + "Z4; " + "SOAL; " + "SOLUSI; " + "PREV DIFF; " + "PREV PERFORMANCE; " + "TARGET DIFF; " + "CURRENT DIFF; " + "isFound; " + "STATUS;" + "TOTAL UNDO; " + "TOTAL RESTART; " + "WAKTU; " + "WAKTU GENERATE; ");

        string logEntry = (";UJIAN;" + gl.indexSoal + ";" + gl.schema + ";" + gl.z1 + ";" + gl.z2 + ";" + gl.z3 + ";" + gl.z4 + ";" + gl.blokSoal + ";" + gl.solusiSoal + ";" + gl.prevDifficulty + ";" + gl.prevPerformance + ";" + gl.targetDifficulty + ";" + gl.difficulty + ";" + gl.isFound + ";" + "SKIPPED;" + restartTimes + ";" + undoTimes + ";" + waktuKerja + ";" + gl.elespasedTime + ";" + "\n");
        sendLog.AddLogEntry(logEntry);

        gl.prevDifficulty = gl.difficulty;
        gl.prevSum = gl.currentSum;
        gl.prevStatus = "SKIPPED";

        if (gl.prevPerformance == 0)
        {
            Debug.LogWarning("skiped with gl: " + gl.prevPerformance);
            gl.prevPerformance = 0.1f; //untuk initial difficulty kalo soal pertama dia gagal

        }
        // float durasi_max = 150 + (1- (Mathf.Round(gl.difficulty * 10)* 0.1f)) * 20;
        // float durasi_min = 30 + (1- (Mathf.Round(gl.difficulty * 10)* 0.1f)) * 10;
        // gl.prevPerformance = 0.05f * (durasi_max - Mathf.Round(timer))/(durasi_max-durasi_min) ; //biar jadi 0.0n dst...
        gl.targetDifficulty = gl.difficulty - gl.prevPerformance;

        // ketika level gagal performa pemain nantinya ditampilkan berupa angka 0 pada grafik 
        // sl.points = AddElementToArray(sl.points, 0);
        sl.points = AddElementToArray(sl.points, (Mathf.Round(gl.targetDifficulty * 1000.0f) * 0.001f));

        tirai.SetTrigger("close");
        //skipPromt.SetTrigger("close");
        timerv.StopTimer();
        StartCoroutine(nextScene(nextSceneTarget, 1f));
        // sl.timer_maju = sl.timer_maju - 1f;
    }

    private void HidePeringatanText()
    {
        peringatanText.gameObject.SetActive(false); // Sembunyikan teks peringatan
    }

    public void keluarGame()
    {
        if (sl.jumlahKerjakanSoal < 1)
        {
            peringatanText.gameObject.SetActive(true);

            // Menyembunyikan peringatan setelah beberapa detik (misalnya 2 detik)
            Invoke("HidePeringatanText", 2.0f);
        }

        else
        {

            tirai.SetTrigger("close");
            // tampilSkor.gameObject.SetActive(true);
            // sl.jumlahKerjakanSoal = sl.kerjakanBenar + sl.kerjakanSalah;

            sl.rataKesulitanBenar = sl.hitungRataKesulitanBenar / sl.jumlahKerjakanSoalBenar;
            sl.rataKesulitanSalah = sl.hitungRataKesulitanSalah / sl.jumlahKerjakanSoalSalah;

            string logData = sendLog.GetLogData();
            Application.ExternalCall("UploadLogToFirebase", logData, gl.pemain);
            timerv.StopTimer();
            StartCoroutine(nextScene("PerformanceScene", 1f));
        }
    }

    IEnumerator nextScene(string sceneTarget, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneTarget);
    }

    // fungsi untuk menyimpan nilai performa pemain tiap level guna ditampilkan di grafik
    private float[] AddElementToArray(float[] array, float element)
    {
        // Buat array baru dengan ukuran lebih besar
        float[] newArray = new float[array.Length + 1];

        // Salin elemen-elemen dari array lama ke array baru
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        // Tambahkan elemen baru
        newArray[array.Length] = element;

        return newArray;
    }

    private void Update()
    {
        int currentLife = sh.lifeCount - restartTimes;

        //timer berjalan
        if(sedangPause.gameObject.activeSelf){
            timerv.StopTimer();
        }else{
            timerv.StartTimer();
        }


        if (totalbenar == listTarget.Count)
        {
            // Debug.Log("LEVEL COMPLETE");
            isComplete = true;
        }



        if (isComplete)
        {
            timerv.StopTimer();
            // play conffeti
            confetti.SetActive(true);
            if (!conffectiplayed)
            {
                if (waktuKerja != null || waktuKerja != "" || waktuKerja != " ")
                {
                    // untuk versi download
                    sendLog.AddLogDownloadEntry("MODE: UJIAN");
                    sendLog.AddLogDownloadEntry("INDEX: " + gl.indexSoal);
                    sendLog.AddLogDownloadEntry("SCHEMA: " + gl.schema);
                    sendLog.AddLogDownloadEntry("Z1: " + gl.z1);
                    sendLog.AddLogDownloadEntry("Z2: " + gl.z2);
                    sendLog.AddLogDownloadEntry("Z3: " + gl.z3);
                    sendLog.AddLogDownloadEntry("Z4: " + gl.z4);
                    sendLog.AddLogDownloadEntry("SOAL: " + gl.blokSoal);
                    sendLog.AddLogDownloadEntry("SOLUSI: " + gl.solusiSoal);
                    sendLog.AddLogDownloadEntry("PREV DIFF: " + gl.prevDifficulty);
                    sendLog.AddLogDownloadEntry("PREV PERFORMANCE: " + gl.prevPerformance);
                    sendLog.AddLogDownloadEntry("TARGET DIFF: " + gl.targetDifficulty);
                    sendLog.AddLogDownloadEntry("CURRENT DIFF: " + gl.difficulty);
                    sendLog.AddLogDownloadEntry("isFound: " + gl.isFound);
                    sendLog.AddLogDownloadEntry("STATUS: SUCCESS");
                    sendLog.AddLogDownloadEntry("TOTAL UNDO: " + restartTimes);
                    sendLog.AddLogDownloadEntry("TOTAL RESTART: " + undoTimes);
                    sendLog.AddLogDownloadEntry("WAKTU: " + waktuKerja);
                    sendLog.AddLogDownloadEntry("WAKTU GENERATE: " + gl.elespasedTime + "\n");

                    sendLog.Log("MODE: UJIAN");
                    sendLog.Log("INDEX: " + gl.indexSoal);
                    sendLog.Log("SCHEMA: " + gl.schema);
                    sendLog.Log("Z1: " + gl.z1);
                    sendLog.Log("Z2: " + gl.z2);
                    sendLog.Log("Z3: " + gl.z3);
                    sendLog.Log("Z4: " + gl.z4);
                    sendLog.Log("SOAL: " + gl.blokSoal);
                    sendLog.Log("SOLUSI: " + gl.solusiSoal);
                    sendLog.Log("PREV DIFF: " + gl.prevDifficulty);
                    sendLog.Log("PREV PERFORMANCE: " + gl.prevPerformance);
                    sendLog.Log("TARGET DIFF: " + gl.targetDifficulty);
                    sendLog.Log("CURRENT DIFF: " + gl.difficulty);
                    sendLog.Log("isFound: " + gl.isFound);
                    sendLog.Log("STATUS: SUCCESS");
                    sendLog.Log("TOTAL UNDO: " + restartTimes);
                    sendLog.Log("TOTAL RESTART: " + undoTimes);
                    sendLog.Log("WAKTU: " + waktuKerja);
                    sendLog.Log("WAKTU GENERATE: " + gl.elespasedTime + "\n");

                    // untuk versi upload
                    sendLog.AddLogEntry(";MODE; " + "INDEX; " + "SCHEMA; " + "Z1; " + "Z2; " + "Z3; " + "Z4; " + "SOAL; " + "SOLUSI; " + "PREV DIFF; " + "PREV PERFORMANCE; " + "TARGET DIFF; " + "CURRENT DIFF; " + "isFound; " + "STATUS;" + "TOTAL UNDO; " + "TOTAL RESTART; " + "WAKTU; " + "WAKTU GENERATE; ");
                    string logEntry = (";UJIAN;" + gl.indexSoal + ";" + gl.schema + ";" + gl.z1 + ";" + gl.z2 + ";" + gl.z3 + ";" + gl.z4 + ";" + gl.blokSoal + ";" + gl.solusiSoal + ";" + gl.prevDifficulty + ";" + gl.prevPerformance + ";" + gl.targetDifficulty + ";" + gl.difficulty + ";" + gl.isFound + ";" + "SUCCESS;" + restartTimes + ";" + undoTimes + ";" + waktuKerja + ";" + gl.elespasedTime + ";" + "\n");
                    sendLog.AddLogEntry(logEntry);


                    gl.prevSuccessDifficulty = gl.difficulty;
                    gl.prevDifficulty = gl.difficulty;
                    gl.prevSum = gl.currentSum;
                    gl.prevStatus = "SUCCESS";

                    //kalkulasi performa player

                    float performance = (float)currentLife / (float)sh.lifeCount;
                    gl.prevPerformance = performance * 0.1f; //biar jadi 0.0n dst...
                    // float performance = ((float)currentLife / (float)sh.lifeCount) * (1/Mathf.Round(sl.durasiBermain));

                    // float sisa_nyawa = (float)currentLife / (float)sh.lifeCount;
                    // float durasi_max = 150 + (1- (Mathf.Round(gl.difficulty * 10)* 0.1f)) * 20;
                    // float durasi_min = 30 + (1- (Mathf.Round(gl.difficulty * 10)* 0.1f)) * 10;
                    // gl.prevPerformance = 0.05f * sisa_nyawa * (durasi_max - Mathf.Round(timer))/(durasi_max-durasi_min) ; //biar jadi 0.0n dst...

                    gl.targetDifficulty = gl.difficulty + gl.prevPerformance;
                }


                confetti.GetComponent<ParticleSystem>().Play();
                conffectiplayed = true;
            }



            tirai.SetTrigger("close");
            StartCoroutine(nextScene(nextSceneTarget, 4f));
            // sl.timer_maju = sl.timer_maju - 1f;
        }
        else
        {
            // float t = Time.time - startTime;
            float t = sl.timer_maju - startTime;
            waktuKerja = t.ToString("f1");
            
        }

        if (isTambahPoin && isComplete)
        {
            timerv.StopTimer();
            // sl.durasiBermain = sl.durasiBermain + (Time.time - startTime);
            sl.jumlahKerjakanSoalBenar = sl.jumlahKerjakanSoalBenar + 1;
            sl.jumlahKerjakanSoal = sl.jumlahKerjakanSoal + 1;
            // sl.poin = sl.poin + ((1000 * gl.prevPerformance * (1 * gl.difficulty)/ sl.durasiBermain) + (sh.lifeCount - restartTimes));

            /* // menambahkan skor ketika dapat menyelesaikan soal yang dikerjakan dengan nyawa penuh
            if (restartTimes < 1) {
                sl.poin = sl.poin + (gl.prevPerformance * gl.difficulty * 1);
            } */
            isTambahPoin = false;
            sl.hitungRataKesulitanBenar = sl.hitungRataKesulitanBenar + gl.difficulty;

            // nilai performa pemain disimpan pada array serta melakukan normalisasi jika benar dengan
            // nyawa penuh yang awalnya 0.1 menjadi 1

            // sl.points = AddElementToArray(sl.points, (Mathf.Round(gl.prevPerformance * 1000.0f) * 0.001f)/ 0.1f);
            sl.points = AddElementToArray(sl.points, (Mathf.Round(gl.targetDifficulty * 1000.0f) * 0.001f));

            if (sl.kesulitanTinggiBenar == 0)
            {
                sl.kesulitanTinggiBenar = gl.difficulty;
            }
            else
            {
                if (sl.kesulitanTinggiBenar < gl.difficulty)
                {
                    sl.kesulitanTinggiBenar = gl.difficulty;
                }
            }

            if (sl.kesulitanRendahBenar == 0)
            {
                sl.kesulitanRendahBenar = gl.difficulty;
            }
            else
            {
                if (sl.kesulitanRendahBenar > gl.difficulty)
                {
                    sl.kesulitanRendahBenar = gl.difficulty;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                // Debug.Log(hit.collider.gameObject.name);
            }
            else
            {
                setSelectedFalse(SelectedBilangan[0]);
                SelectedBilangan.Clear();
            }
        }

    }


}
