using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GeneratorManual : MonoBehaviour
{
    public GameObject parentObj;
    public GameObject BilanganPrefab;

    //untuk target
    public GameObject PanelTarget;
    public GameObject TargetPrefab;

    [SerializeField]
    private TutorialManager gm;


    public Bilangan newBilangan(string bilangan, string op)
    {
        Bilangan temp = new Bilangan();
        temp.bilangan = bilangan;
        temp.op = op;
        return temp;
    }


    // Utility function to create a new tree node
    static Node newNode(List<Bilangan> listBilangan, string sumber, Node parentNode)
    {
        Node temp = new Node();
        temp.listBilangan = listBilangan;
        temp.sumber = sumber;
        temp.parentNode = parentNode;
        return temp;
    }

    public Bilangan Hitung(List<Bilangan> bilangan)
    {
        float a = float.Parse(bilangan[0].bilangan);
        float b = float.Parse(bilangan[1].bilangan);

        string aOp = bilangan[0].op;
        string bOp = bilangan[1].op;

        if (a.ToString() == "0" || a.ToString() == "0.0" || b.ToString() == "0" || b.ToString() == "0.0")
        {
            return newBilangan("invalid", "invalid");
        }
        else if (bOp == "+")
        {
            if (aOp == "*")
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
                    return newBilangan(hasil.ToString(), "*");
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
                    return newBilangan(hasil.ToString(), "-");
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
                    return newBilangan(hasil.ToString(), "*");
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
                    return newBilangan(hasil.ToString(), "-");
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
                    if (item.bilangan == target[0].bilangan || item.bilangan == target[1].bilangan)
                    {
                        //Debug.Log(item.bilangan.ToString() + " == " + target[0].bilangan.ToString() + " atau " + item.bilangan.ToString() + " == " + target[1].bilangan.ToString());
                        sama++;
                    }

                }
            }

            //Debug.Log("Target count = " + target.Count);
            if (sama == target.Count)
            {
                //Debug.Log("COUNT SAMA");
                return true;
            }
            else
            {
                //Debug.Log("count tidak pas/belum ada sama");
                return false;
            }
        }


    }

    private List<List<Bilangan>> getCombination(List<Bilangan> bilangan)
    {
        List<List<Bilangan>> sudah_list = new List<List<Bilangan>>();

        //Debug.Log(bilangan.Count);
        for (int i = 0; i < bilangan.Count(); i++)
        {
            for (int j = 0; j < bilangan.Count(); j++)
            {
                List<Bilangan> currentList = new List<Bilangan>();
                currentList.Add(bilangan[i]);
                currentList.Add(bilangan[j]);
                //Debug.Log(currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                if (bilangan[i].bilangan == bilangan[j].bilangan)
                {
                    //Debug.LogError("SKIPPED SAMA- " + currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                    continue;
                }
                else if (checkList(currentList, sudah_list))
                {
                    //Debug.LogError("SKIPPED- "+currentList[0].bilangan.ToString() + " . " + currentList[1].bilangan.ToString());
                    continue;
                }
                else
                {
                    sudah_list.Add(currentList);
                }
            }
        }

        for (int i = 0; i < sudah_list.Count - 1; i++)
        {
            for (int j = 0; j < sudah_list.Count; j++)
            {
                sudah_list[i].Reverse();
                string op1 = sudah_list[i][0].op;
                string op2 = sudah_list[i][1].op;

                if (op1 == "/" || op1 == "*" || op2 == "/" || op2 == "*")
                {
                    continue;
                }
                else if (sudah_list[i][0].bilangan == sudah_list[j][0].bilangan && sudah_list[i][1].bilangan == sudah_list[j][1].bilangan)
                {
                    sudah_list.RemoveAt(i);
                }
            }

        }

        return sudah_list;
    }

    private void generateChildrenNodes(Node parentNode)
    {
        List<List<Bilangan>> combList = getCombination(parentNode.listBilangan);
        for (int i = 0; i < combList.Count; i++)
        {
            List<Bilangan> bilanganlist = new List<Bilangan>(parentNode.listBilangan);
            foreach (Bilangan current in combList[i]) //comblist[j] = (bilangan)(bilangan)
            {
                for (int j = 0; j < bilanganlist.Count; j++) // KAMU CEK DISINI MASALAH NIH
                {
                    if ((current.bilangan == combList[j][0].bilangan && current.op == combList[j][0].op) || (current.bilangan == combList[j][1].bilangan && current.op == combList[j][1].op))
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
            if (bilanganlist.Count == 2)
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

    private void printSolution(Node targetLeaf)
    {
        Debug.Log(targetLeaf.sumber);
        if (targetLeaf.parentNode != null)
        {
            Node currNode = targetLeaf.parentNode;
            printSolution(currNode);
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

    IEnumerator instantiateforSec(List<Bilangan> bilangan, float time)
    {
        foreach (Bilangan bil in bilangan)
        {
            generateObject(bil, Vector3.zero);
            yield return new WaitForSeconds(time);
        }
    }

    private void generateTarget(Node targetNode)
    {
        foreach (Bilangan bil in targetNode.listBilangan)
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

    private void Start()
    {


    }
}