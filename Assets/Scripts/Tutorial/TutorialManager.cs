using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public List<GameObject> SelectedBilangan = new List<GameObject>();
    public List<List<GameObject>> historyBilangan = new List<List<GameObject>>();
    public GeneratorManual gen;
    public GameObject smoke;
    public List<GameObject> listTarget;
    public int totalbenar = 0;
    public Camera cam;
    // dibawah ini baru
    public GameObject confetti;
    public Animator tirai;
    public Animator skipPromt;
    public bool isComplete = false;
    private bool conffectiplayed;

    public string nextSceneTarget;

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
    IEnumerator nextScene(string sceneTarget, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneTarget);
    }

    public void skipSoal()
    {
        tirai.SetTrigger("close");
        skipPromt.SetTrigger("close");
        StartCoroutine(nextScene(nextSceneTarget, 1f));
    }

    private void Update()
    {
        if (totalbenar == listTarget.Count)
        {
            //Debug.Log("LEVEL COMPLETE");
            isComplete = true;
        }

        if (isComplete)
        {
            // play conffeti
            confetti.SetActive(true);
            if (!conffectiplayed)
            {

                confetti.GetComponent<ParticleSystem>().Play();
                conffectiplayed = true;
            }
            tirai.SetTrigger("close");
            StartCoroutine(nextScene(nextSceneTarget, 4f));
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

    public void keluarGame()
    {

        StartCoroutine(nextScene("Menu", 1f));

    }


}
