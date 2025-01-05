using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public GameObject parentObj;
    public GameObject BilanganPrefab;
    
    //untuk target
    public GameObject PanelTarget;
    public GameObject TargetPrefab;

    //untuk life
    //public GameObject panelLife;
    //public GameObject lifePrefab;
    public Text lifeCount;

    [SerializeField]
    private GameManager gm;

    //public Text kuncijawaban;



    public Bilangan newBilangan(string bilangan, string op)
    {
        Bilangan temp = new Bilangan();
        temp.bilangan = bilangan;
        temp.op = op;
        return temp;
    }


    // Utility function to create a new tree node
    public Node newNode(List<Bilangan> listBilangan, string sumber, Node parentNode)
    {
        Node temp = new Node();
        temp.listBilangan = listBilangan;
        temp.sumber = sumber;
        temp.parentNode = parentNode;
        return temp;
    }

    public Bilangan Hitung(List<Bilangan> bilangan) // max ada dua bilangan. makanya dibuat list
    {
        float a = float.Parse(bilangan[0].bilangan);
        float b = float.Parse(bilangan[1].bilangan);

        string aOp = bilangan[0].op;
        string bOp = bilangan[1].op;
        //if(a.ToString() == "0" || a.ToString() == "0.0" || b.ToString() == "0" || b.ToString() == "0.0")
        //{
        //    return newBilangan("invalid", "invalid");
        //}
        if (b.ToString() == "0" && bOp.ToString() == "/")
        {
            return newBilangan("invalid", "invalid"); //prevent biar gabisa dibagi 0
        }
        else if (bOp == "+")
        {
            if(aOp == "*")
            {
                float hasil = a + b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "*");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "*");
                }
            }
            else if (aOp == "/")
            {
                float hasil = a + b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "/");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "/");
                }
            }
            else
            {
                float hasil = a + b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "+");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "-");
                }
            }
        }
        else if (bOp == "-")
        {
            b = float.Parse(bilangan[1].bilangan.Substring(1));
            if (aOp == "*")
            {
                float hasil = a - b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "*");
                }
                else
                {
                    return newBilangan( hasil.ToString(), "*");
                }
            }
            else if (aOp == "/")
            {
                float hasil = a - b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "/");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "/");
                }
            }
            else
            {
               // Debug.Log("bilangan: "+a+" dan "+b);
                float hasil = a - b;
               // Debug.Log("hasil: " + hasil);
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "+");
                }
                else
                {
                    return newBilangan( hasil.ToString(), "-");
                }
            }
        }
        else if (bOp == "*")
        {
            if (aOp == "*")
            {
                float hasil = a * b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "*");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "*");
                }
            }
            else if (aOp == "/")
            {
                float hasil = a * b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "/");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "/");
                }
            }
            else
            {
                float hasil = a * b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "+");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "-");
                }
            }
        }
        else if (bOp == "/")
        {
            if (aOp == "*")
            {
                float hasil = a / b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "*");
                }
                else
                {
                    return newBilangan( hasil.ToString(), "*");
                }
            }
            else if (aOp == "/")
            {
                float hasil = a / b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "/");
                }
                else
                {
                    return newBilangan(hasil.ToString(), "/");
                }
            }
            else
            {
                float hasil = a / b;
                if (hasil >= 0)
                {
                    return newBilangan("+" + hasil.ToString(), "+");
                }
                else
                {
                    return newBilangan( hasil.ToString(), "-");
                }
            }
        }
        else
        {
            return newBilangan("invalid", "invalid");
        }

    }

    private bool checkList(List<Bilangan> target, List<List<Bilangan>> num_list)
    {
        //Debug.LogWarning("Check List...");
        string op1 = target[0].op;
        string op2 = target[1].op;
        int sama = 0;
        
        if (op1 == "/" || op1 == "*" || op2 == "/" || op2 == "*")
        {
            //Debug.Log("operator skip- "+target[0].bilangan.ToString() + " . " + target[1].bilangan.ToString());
            return false;
        }
        else
        {
            foreach (List<Bilangan> subList in num_list)
            {
                foreach (Bilangan item in subList)
                {
                    if (item.bilangan == target[0].bilangan )
                    {
                       // Debug.Log(item.bilangan.ToString() + " == " + target[0].bilangan.ToString() );
                        sama++;
                    }
                    else if(item.bilangan == target[1].bilangan)
                    {
                        //Debug.Log(" atau " + item.bilangan.ToString() + " == " + target[1].bilangan.ToString());
                        sama++;
                    }

                }
                if (sama == target.Count)
                {
                    //Debug.Log("COUNT SAMA");
                    return true;
                }
                sama = 0;
            }

            //Debug.Log("Target count = " + target.Count);
            //if (sama == target.Count)
            //{
            //    //Debug.Log("COUNT SAMA");
            //    return true;
            //}
            //else
            //{
            //    //Debug.Log("count tidak pas/belum ada sama");
            //    return false;
            //}
        }
        return false;


    }

    private List<List<Bilangan>> getCombination(List<Bilangan> bilangan) // fungsi ini buat ambil kombinasi 2 bilangan yang bisa di eksekusi
    {
        List<List<Bilangan>> sudah_list = new List<List<Bilangan>>();

       // Debug.LogWarning("Bilangan Count: "+bilangan.Count);
        for (int i=0; i<bilangan.Count(); i++)
        {
            for (int j=0; j < bilangan.Count(); j++)
            {
                List<Bilangan> currentList = new List<Bilangan>();
                currentList.Add(bilangan[i]);
                currentList.Add(bilangan[j]);
              // Debug.Log(currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                if (bilangan[i].bilangan == bilangan[j].bilangan)
                {
                 // Debug.LogError("SKIPPED SAMA- " + currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                    continue;
                }
                else if (checkList(currentList, sudah_list))
                {
                 // Debug.LogError("SKIPPED-sudah"+currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                    continue;
                }
                else
                {
                    sudah_list.Add(currentList);
                }
            }
        }


        //Debug.LogWarning("INI TOTALNYA: " + sudah_list.Count);
        // ini buat apaaaaa?
        //for (int i = 0; i < sudah_list.Count - 1; i++)
        //{
        //    for (int j = 0; j < sudah_list.Count; j++)
        //    {
        //        sudah_list[i].Reverse();
        //        string op1 = sudah_list[i][0].op;
        //        string op2 = sudah_list[i][1].op;

        //        if (op1 == "/" || op1 == "*" || op2 == "/" || op2 == "*")
        //        {
        //            continue;
        //        }
        //        else if (sudah_list[i][0].bilangan == sudah_list[j][0].bilangan && sudah_list[i][1].bilangan == sudah_list[j][1].bilangan)
        //        {
        //            sudah_list.RemoveAt(i);
        //        }
        //    }

        //}


        //foreach (List<Bilangan> lisbil in sudah_list)
        //{
        //    Debug.LogWarning("newComb");
        //    foreach (Bilangan bil in lisbil)
        //    {
        //        Debug.Log(bil.bilangan);
        //    }
        //}

        return sudah_list;
    }

    public void generateChildrenNodes(Node parentNode)
    {
        List<List<Bilangan>> combList = getCombination(parentNode.listBilangan);
        for(int i=0; i<combList.Count; i++)
        {
            List<Bilangan> bilanganlist = new List<Bilangan>(parentNode.listBilangan);
            foreach (Bilangan current in combList[i]) //comblist[j] = (bilangan)(bilangan) || (+2)(-4)
            {
                for(int j=0; j< bilanganlist.Count; j++) // KAMU CEK DISINI MASALAH NIH
                {
                    if(current.bilangan == bilanganlist[j].bilangan && current.op == bilanganlist[j].op)
                    {
                        bilanganlist.RemoveAt(j);
                    }
                }
                
            }

            Bilangan hasilnya = Hitung(combList[i]);
            if (hasilnya.bilangan == "invalid")
            {
                break;
            }
            bilanganlist.Add(hasilnya);
            if(bilanganlist.Count == 2)
            {
                Node aboveFinal = newNode(bilanganlist, combList[i][0].op.ToString() + combList[i][0].bilangan + " . " + combList[i][1].op.ToString() + combList[i][1].bilangan, parentNode);
                Bilangan finalKombinasi = Hitung(bilanganlist);
                if (finalKombinasi.bilangan == "invalid")
                {
                    break;
                }
                List<Bilangan> bilanganfinal = new List<Bilangan>();
                bilanganfinal.Add(finalKombinasi);

                (aboveFinal.child).Add(newNode(bilanganfinal, combList[i][0].op.ToString() + combList[i][0].bilangan + " . " + combList[i][1].op.ToString() + combList[i][1].bilangan, parentNode));
                (parentNode.child).Add(aboveFinal);
            }
            else
            {
                Node subParent = newNode(bilanganlist, combList[i][0].op.ToString() + combList[i][0].bilangan + " . " + combList[i][1].op.ToString() + combList[i][1].bilangan, parentNode);
                (parentNode.child).Add(subParent);

                generateChildrenNodes(subParent);
            }
        }
    }


     public GameObject generateObject(Bilangan bil, Vector3 pos)
    {
        GameObject childObject;
        if (pos == Vector3.zero)
        {
            float posX = Random.Range(-1.8f, 2.1f);
             childObject = Instantiate(BilanganPrefab, new Vector3(posX, 7.5f, 0), transform.rotation) as GameObject;
        }
        else
        {
             childObject = Instantiate(BilanganPrefab, pos, transform.rotation) as GameObject;
        }
       
        childObject.transform.parent = parentObj.transform;
        float size = Random.Range(0.7f, 1f);
        childObject.transform.localScale = new Vector3(size, size, size);
        childObject.GetComponent<DataBilangan>().op = bil.op;
        childObject.GetComponent<DataBilangan>().bilangan = bil.bilangan;
        childObject.name = bil.bilangan;


        if (bil.bilangan[0] == '+')
        {
            GameObject posObj = childObject.transform.GetChild(0).gameObject;
            posObj.SetActive(true);

            GameObject posObjVal = posObj.transform.GetChild(0).gameObject;
            posObjVal.GetComponent<TextMesh>().text = bil.bilangan;
            if (bil.op == "*")
            {
                childObject.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (bil.op == "/")
            {
                childObject.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        else if (bil.bilangan[0] == '-')
        {
            GameObject posObj = childObject.transform.GetChild(1).gameObject;
            posObj.SetActive(true);

            GameObject posObjVal = posObj.transform.GetChild(0).gameObject;
            posObjVal.GetComponent<TextMesh>().text = bil.bilangan;
            if (bil.op == "*")
            {
                childObject.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (bil.op == "/")
            {
                childObject.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        return childObject;
    }

    public IEnumerator instantiateforSec(List<Bilangan> bilangan, float time)
    {
        foreach (Bilangan bil in bilangan)
        {
            generateObject(bil, Vector3.zero);
            yield return new WaitForSeconds(time);
        }
    }

    public void generateTarget(Node targetNode)
    {
        foreach(Bilangan bil in targetNode.listBilangan)
        {
            
            GameObject target = Instantiate(TargetPrefab);
            gm.listTarget.Add(target);
            //target.transform.localScale = new Vector3(1f, 1f, 1f);
            target.transform.SetParent(PanelTarget.transform);
            target.GetComponent<RectTransform>().localPosition = new Vector3(target.GetComponent<RectTransform>().transform.position.x, target.GetComponent<RectTransform>().transform.position.y, 1f);
            target.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            target.transform.GetChild(0).GetComponent<Text>().text = bil.bilangan;
            target.name = bil.bilangan;
            target.GetComponent<DataBilangan>().bilangan = bil.bilangan;
            target.GetComponent<DataBilangan>().op = bil.op;

        }
    }

    public void GenerateLife(int life)
    {
        lifeCount.text = life.ToString();
        //for (int i = 0; i < life; i++)
        //{
        //    GameObject target = Instantiate(lifePrefab);
        //    target.transform.SetParent(panelLife.transform);
        //    target.GetComponent<RectTransform>().localPosition = new Vector3(target.GetComponent<RectTransform>().transform.position.x, target.GetComponent<RectTransform>().transform.position.y, 1f);
        //    //target.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        //}

    }



    //public void printSolution(Node targetLeaf)
    //{


    //   // Debug.Log(targetLeaf.sumber);
    //    if (targetLeaf.parentNode != null)
    //    {
    //        kuncijawaban.text += targetLeaf.sumber.ToString() + "\n";
    //        Node currNode = targetLeaf.parentNode;
    //        printSolution(currNode);
    //    }
    //}

    private void Start()
    {
        //List<Bilangan> bilangan = new List<Bilangan>();
        //bilangan.Add(newBilangan("+3", "+"));
        //bilangan.Add(newBilangan("+3", "*"));
        //bilangan.Add(newBilangan("+4", "/"));
        //bilangan.Add(newBilangan("+5", "+"));
        //Node root = newNode(bilangan, null, null);

        ////List<Bilangan> bilangan = new List<Bilangan>();
        ////bilangan.Add(newBilangan("+3", "*"));
        ////bilangan.Add(newBilangan("+2", "+"));
        ////bilangan.Add(newBilangan("-5", "-"));
        ////bilangan.Add(newBilangan("+11", "+"));
        ////Node root = newNode(bilangan, null, null);

        ////getCombination(root.listBilangan);
        
        //generateChildrenNodes(root);
        ////Debug.Log("Total Child: "+root.child[0].child[0].child.Count); //ENTAH YA SAYA BINUNG ANJM DISINI!!

        ////foreach (Bilangan bil in root.child[0].child[0].listBilangan)
        ////{
        ////    Debug.Log(bil.bilangan);
        ////}


        //generateTarget(root.child[2].child[2].child[0]);
        //printSolution(root.child[2].child[2].child[0]);
        //StartCoroutine(instantiateforSec(bilangan, 0.7f));

    }
}