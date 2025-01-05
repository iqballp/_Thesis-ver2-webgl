using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LatihanManager : MonoBehaviour
{
    public List<GameObject> SelectedBilangan = new List<GameObject>();
    public List<List<GameObject>> historyBilangan = new List<List<GameObject>>();
    public LatihanGenerator gen;
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

    public string nextSceneTarget;

    private string waktuKerja;
    private float startTime;

    private int restartTimes;
    private int undoTimes;

    private GameLoger gl;
    public DebugManager sendLog;
    public LatihanHandler sh;

    //forDebugger
    public bool isDebug = false;
    public GameObject prevDiffText;
    public GameObject prevPerformanceText;
    public GameObject currDiffText;
    public GameObject targetDiffText;

    public GameObject solutionText;
    public GameObject isFoundText;




    private void Start()
    {
        startTime = Time.time;
        gl = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        sendLog = GameObject.Find("Debuger").GetComponent<DebugManager>();
        prevDiffText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.prevDifficulty.ToString();
        prevPerformanceText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.prevPerformance.ToString();
        targetDiffText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.targetDifficulty.ToString();
        isFoundText.transform.GetChild(0).gameObject.GetComponent<Text>().text = gl.isFound.ToString();

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
        }
        else
        {
            prevDiffText.gameObject.SetActive(false);
            prevPerformanceText.gameObject.SetActive(false);
            currDiffText.gameObject.SetActive(false);
            solutionText.gameObject.SetActive(false);
            targetDiffText.gameObject.SetActive(false);
            isFoundText.gameObject.SetActive(false);
        }
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
    }



    public void skipSoal()
    {
        sendLog.Log("MODE: LATIHAN");
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
        sendLog.Log("TOTAL UNDO: " + restartTimes);
        sendLog.Log("TOTAL RESTART: " + undoTimes);
        sendLog.Log("WAKTU: " + waktuKerja);
        sendLog.Log("WAKTU GENERATE: " + gl.elespasedTime + "\n");

        gl.prevDifficulty = gl.difficulty;
        gl.prevSum = gl.currentSum;
        gl.prevStatus = "SKIPPED";

        float randomPerformance = Random.Range(0.01f, 0.1f);
        gl.prevPerformance = randomPerformance;

        gl.targetDifficulty = gl.difficulty - gl.prevPerformance;

        tirai.SetTrigger("close");
        //skipPromt.SetTrigger("close");
        StartCoroutine(nextScene(nextSceneTarget, 1f));
    }

    public void keluarGame()
    {
        tirai.SetTrigger("close");
        gl.ResetGameLoger();
        StartCoroutine(nextScene("Menu", 1f));
    }

    IEnumerator nextScene(string sceneTarget, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneTarget);
    }

    private void Update()
    {
        if (totalbenar == listTarget.Count)
        {
            // Debug.Log("LEVEL COMPLETE");
            isComplete = true;
        }

        if (isComplete)
        {
            // play conffeti
            confetti.SetActive(true);
            if (!conffectiplayed)
            {
                if (waktuKerja != null || waktuKerja != "" || waktuKerja != " ")
                {
                    sendLog.Log("MODE: LATIHAN");
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

                    gl.prevSuccessDifficulty = gl.difficulty;
                    gl.prevDifficulty = gl.difficulty;
                    gl.prevSum = gl.currentSum;
                    gl.prevStatus = "SUCCESS";

                    //kalkulasi performa player
                    int currentLife = sh.lifeCount - restartTimes;
                    float performance = (float)currentLife / (float)sh.lifeCount;

                    float randomPerformance = Random.Range(0.01f, 0.1f);
                    gl.prevPerformance = randomPerformance;
                   // gl.prevPerformance = performance * 0.1f; //biar jadi 0.0n dst...

                    gl.targetDifficulty = gl.difficulty + gl.prevPerformance;
                }


                confetti.GetComponent<ParticleSystem>().Play();
                conffectiplayed = true;
            }



            tirai.SetTrigger("close");
            StartCoroutine(nextScene(nextSceneTarget, 4f));
        }
        else
        {
            float t = Time.time - startTime;
            waktuKerja = t.ToString("f1");
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
